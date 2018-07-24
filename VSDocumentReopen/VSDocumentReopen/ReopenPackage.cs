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
using VSDocumentReopen.Infrastructure.DocumentCommands;
using VSDocumentReopen.Infrastructure.DocumentTracking;
using VSDocumentReopen.Infrastructure.FileIcons;
using VSDocumentReopen.Infrastructure.Helpers;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.VS.Commands;
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

		private readonly IHistoryCommand _reopenLastClosdCommand;
		private readonly IHistoryCommandFactory _reopenSomeDocumentsCommandFactory;
		private readonly IHistoryCommandFactory _removeSomeDocumentsCommandFactory;
		private readonly IHistoryCommand _clearHistoryCommand;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReopenClosedDocumentsCommand"/> class.
		/// </summary>
		public ReopenPackage()
		{
			//DI
			_dte = GetGlobalService(typeof(DTE)) as DTE2 ?? throw new NullReferenceException($"Unable to get service {nameof(DTE2)}");

			IDocumentHistoryManager documentHistory = new DocumentHistoryManager();
			_documentHistoryCommands = documentHistory;
			_documentHistoryQueries = documentHistory;
			//Commands
			_reopenLastClosdCommand = new RemoveLastCommand(_documentHistoryCommands,
				new ReopenDocumentCommandFactory(_dte));
			_reopenSomeDocumentsCommandFactory = new HistoryCommandFactory<RemoveSomeCommand>(_documentHistoryCommands,
				new ReopenDocumentCommandFactory(_dte));
			_removeSomeDocumentsCommandFactory = new HistoryCommandFactory<RemoveSomeCommand>(_documentHistoryCommands,
				new DoNothingDocumentCommandFactory());
			_clearHistoryCommand = new ClearHistoryCommand(_documentHistoryCommands);

			_documentTracker = new DocumentEventsTracker(_dte,
				documentHistory,
				new JsonHistoryRepositoryFactory(new ServiceStackJsonSerializer()));
		}

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

			//Init Commands with DI
			await ReopenClosedDocumentsCommand.InitializeAsync(this, _reopenLastClosdCommand);
			await ClearDocumentsHistoryCommand.InitializeAsync(this, _clearHistoryCommand);
			await DocumentsHistoryCommand.InitializeAsync(this, _documentHistoryQueries, _reopenSomeDocumentsCommandFactory);

			var imageService = (IVsImageService2)Package.GetGlobalService(typeof(SVsImageService));

			//Init ToolWindow Commands with DI
			await ShowDocumentsHIstoryCommand.InitializeAsync(this);
			await ClosedDocumentsHistory.InitializeAsync(_documentHistoryQueries,
				_reopenLastClosdCommand,
				_reopenSomeDocumentsCommandFactory,
				_removeSomeDocumentsCommandFactory,
				_clearHistoryCommand,
				new CachedFileExtensionIconResolver(
					new VisualStudioFileExtensionIconResolver(imageService)));

			EnforceKeyBinding();
		}

		/// <summary>
		/// Enforcing the Ctrl+Shift+T because if it was assigned to another command then the default does not work.
		/// </summary>
		private void EnforceKeyBinding()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			var commandsGuid = ReopenClosedDocumentsCommand.CommandSet.ToString("B").ToUpper();
			var reopenCommandBinding = ConfigurationManager.Config.ReopenCommandBinding;
			var showMoreCommandBinding = ConfigurationManager.Config.ShowMoreCommandBinding;

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
			Bind(ShowDocumentsHIstoryCommand.CommandId, showMoreCommandBinding);

			void Bind(int commandId, string keyBinding)
			{
				var command = myCommands.SingleOrDefault(x => x.ID == commandId);
				if (command != null)
				{
					command.Bindings = keyBinding;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			_documentTracker.Dispose();
			base.Dispose(disposing);
		}
	}
}