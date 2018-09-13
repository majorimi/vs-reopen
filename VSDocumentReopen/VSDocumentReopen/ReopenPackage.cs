using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.Document.Factories;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.Infrastructure.FileIcons;
using VSDocumentReopen.Infrastructure.Helpers;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.Infrastructure.Logging;
using VSDocumentReopen.Infrastructure.Logging.Logentries;
using VSDocumentReopen.Infrastructure.Version;
using VSDocumentReopen.VS.Commands;
using VSDocumentReopen.VS.MessageBox;
using VSDocumentReopen.VS.ToolWindows;
using ConfigurationManager = VSDocumentReopen.Infrastructure.ConfigurationManager;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	[Guid(PackageGuidString)]
	[ProvideAutoLoad(UIContextGuids.NoSolution)]
	[ProvideToolWindow(typeof(ClosedDocumentsHistory))]
	[ExcludeFromCodeCoverage]
	public sealed class ReopenPackage : AsyncPackage
	{
		/// <summary>
		/// ReopenPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "b30147a1-6fbc-4b94-bf01-123d837c4fe2";

		private readonly _DTE _dte;
		private readonly DocumentEventsTracker _documentTracker;
		private readonly IDocumentHistoryCommands _documentHistoryCommands;
		private readonly IDocumentHistoryQueries _documentHistoryQueries;

		private readonly IHistoryCommand _reopenLastClosedCommand;
		private readonly IHistoryCommand _removeLastClosedCommand;
		private readonly IHistoryCommandFactory _reopenSomeDocumentsCommandFactory;
		private readonly IHistoryCommandFactory _removeSomeDocumentsCommandFactory;
		private readonly IHistoryCommand _clearHistoryCommand;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReopenClosedDocumentsCommand"/> class.
		/// </summary>
		public ReopenPackage()
		{
			_dte = GetGlobalService(typeof(DTE)) as DTE2 ?? throw new NullReferenceException($"Unable to get service {nameof(DTE2)}");

			//Log context and Serilog enricher
			VsVersionContext.Current = new VsDteVersionContext(_dte);
			LoggerContext.Current = new LogentriesSerilogLoggerContext();

			LoggerContext.Current.Logger.Info($"{nameof(ReopenPackage)} started to load. Initializing dependencies...");

			//DI
			IDocumentHistoryManager documentHistory = new DocumentHistoryManager();
			_documentHistoryCommands = documentHistory;
			_documentHistoryQueries = documentHistory;

			//Commands
			_reopenLastClosedCommand = new RemoveLastCommand(_documentHistoryCommands,
				new ReopenDocumentCommandFactory(_dte));
			_removeLastClosedCommand = new RemoveLastCommand(_documentHistoryCommands,
				new DoNothingDocumentCommandFactory());
			_reopenSomeDocumentsCommandFactory = new HistoryCommandFactory<RemoveSomeCommand>(_documentHistoryCommands,
				new ReopenDocumentCommandFactory(_dte));
			_removeSomeDocumentsCommandFactory = new HistoryCommandFactory<RemoveSomeCommand>(_documentHistoryCommands,
				new DoNothingDocumentCommandFactory());
			_clearHistoryCommand = new ClearHistoryCommand(_documentHistoryCommands);

			_documentTracker = new DocumentEventsTracker(_dte,
				documentHistory,
				new JsonHistoryRepositoryFactory(new ServiceStackJsonSerializer()),
				new VSMessageBox(this));
		}

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			try
			{
				LoggerContext.Current.Logger.Info($"{nameof(ReopenPackage)} initializing VS commands...");

				await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

				//Init Commands with DI
				await ReopenClosedDocumentsCommand.InitializeAsync(this, _reopenLastClosedCommand);
				await RemoveClosedDocumentsCommand.InitializeAsync(this, _removeLastClosedCommand);
				await ClearDocumentsHistoryCommand.InitializeAsync(this, _clearHistoryCommand);
				await DocumentsHistoryCommand.InitializeAsync(this, _documentHistoryQueries, _reopenSomeDocumentsCommandFactory);

				var imageService = (IVsImageService2)Package.GetGlobalService(typeof(SVsImageService));

				//Init ToolWindow Commands with DI
				await ShowDocumentsHIstoryCommand.InitializeAsync(this);
				await ClosedDocumentsHistory.InitializeAsync(_documentHistoryQueries,
					_reopenLastClosedCommand,
					_reopenSomeDocumentsCommandFactory,
					_removeSomeDocumentsCommandFactory,
					_clearHistoryCommand,
					new CachedFileExtensionIconResolver(
						new VisualStudioFileExtensionIconResolver(imageService)),
					new JsonIHistoryToolWindowRepositoryFactory(new ServiceStackJsonSerializer()));

				EnforceKeyBinding();

				LoggerContext.Current.Logger.Info($"{nameof(ReopenPackage)} VS commands initialization was successfully finished...");
			}
			catch (Exception ex)
			{
				LoggerContext.Current.Logger.Error($"Failed to Initialize {nameof(ReopenPackage)} VS Extension", ex);
			}
		}

		/// <summary>
		/// Enforcing all shortcut key bindings because if it was assigned to another command then the default does not work.
		/// </summary>
		private void EnforceKeyBinding()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			var globalKeyBindingValue = GetLocalizedGlobalKeyBindings();

			var commandsGuid = ReopenClosedDocumentsCommand.CommandSet.ToString("B").ToUpper();
			var reopenCommandBinding = globalKeyBindingValue + ConfigurationManager.Current.Config.ReopenCommandBinding;
			var removeCommandBinding = globalKeyBindingValue + ConfigurationManager.Current.Config.RemoveCommandBinding;
			var showMoreCommandBinding = globalKeyBindingValue + ConfigurationManager.Current.Config.ShowMoreCommandBinding;

			var myCommands = new List<Command>();
			foreach (Command command in _dte.Commands)
			{
				if (command.Guid == commandsGuid)
				{
					myCommands.Add(command);
				}

				if (command.Bindings is object[] bindings && bindings.Length > 0)
				{
					var bind = string.Join(" ", bindings.Select(x => (string)x));
					if (bind.Contains(reopenCommandBinding) || bind.Contains(showMoreCommandBinding))
					{
						command.Bindings = new object[0];
					}
				}
			}

			Bind(ReopenClosedDocumentsCommand.CommandId, reopenCommandBinding);
			Bind(RemoveClosedDocumentsCommand.CommandId, removeCommandBinding);
			Bind(ShowDocumentsHIstoryCommand.CommandId, showMoreCommandBinding);

			void Bind(int commandId, string keyBinding)
			{
				try
				{
					var command = myCommands.SingleOrDefault(x => x.ID == commandId);
					if (command != null)
					{
						command.Bindings = new object[] { keyBinding };
					}
				}
				catch (Exception ex)
				{
					LoggerContext.Current.Logger.Error($"Error occurred during Key biding CommandId: {commandId}, HotKey: {keyBinding}.", ex);
				}
			}
		}

		/// <summary>
		/// Read localized "Global" keyword
		/// https://github.com/jaredpar/VsVim/blob/master/Src/VsVimShared/Implementation/Misc/ScopeData.cs
		/// </summary>
		private string GetLocalizedGlobalKeyBindings()
		{
			const string defaultGlobalKey = "Global";

			try
			{
				var shell = GetGlobalService(typeof(SVsShell)) as IVsShell ?? throw new NullReferenceException($"Unable to get service {nameof(SVsShell)}");

				using (var rootKey = VSRegistry.RegistryRoot(__VsLocalRegistryType.RegType_Configuration, writable: false))
				{
					using (var keyBindingsKey = rootKey.OpenSubKey("KeyBindingTables"))
					{
						using (var subKey = keyBindingsKey.OpenSubKey("{5efc7975-14bc-11cf-9b2b-00aa00573819}", writable: false))
						{
							var package = Guid.Parse((string)subKey.GetValue("Package"));

							uint globalKeybindingScopeNameId = 13018;
							string globalKeybindingScopeName;
							int res = shell.LoadPackageString(ref package, globalKeybindingScopeNameId, out globalKeybindingScopeName);

							LoggerContext.Current.Logger.Info($"{nameof(GetLocalizedGlobalKeyBindings)}() was executed Global key binding value: {globalKeybindingScopeName}");
							return globalKeybindingScopeName ?? defaultGlobalKey;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LoggerContext.Current.Logger.Error($"Unexpected error while try to get Global Key binding.", ex);
				return defaultGlobalKey;
			}
		}

		protected override void Dispose(bool disposing)
		{
			_documentTracker.Dispose();
			base.Dispose(disposing);
		}
	}
}