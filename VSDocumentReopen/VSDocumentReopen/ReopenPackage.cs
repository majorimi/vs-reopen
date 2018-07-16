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
using VSDocumentReopen.Infrastructure.ClosedDocument;
using VSDocumentReopen.Infrastructure.Helpers;
using VSDocumentReopen.VS.Commands;
using VSDocumentReopen.VS.ToolWindows;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	[Guid(ReopenPackage.PackageGuidString)]
	[ProvideAutoLoad(UIContextGuids.NoSolution)]
	[ProvideToolWindow(typeof(ClosedDocumentsHistory))]
	public sealed class ReopenPackage : AsyncPackage
	{
		/// <summary>
		/// ReopenPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "b30147a1-6fbc-4b94-bf01-123d837c4fe2";

		private readonly DTE2 _dte;
		private readonly DocumentTracker _documentTracker;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReopenClosedDocumentsCommand"/> class.
		/// </summary>
		public ReopenPackage()
		{
			_dte = GetGlobalService(typeof(DTE)) as DTE2 ?? throw new NullReferenceException($"Unable to get service {nameof(DTE2)}");

			_documentTracker = new DocumentTracker(_dte,
				new JsonHistoryRepositoryFactory(new ServiceStackJsonSerializer()));
		}

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

			//Init Commands
			await ReopenClosedDocumentsCommand.InitializeAsync(this, _dte);
			await ClearDocumentsHistoryCommand.InitializeAsync(this);
			await DocumentsHistoryCommand.InitializeAsync(this, _dte);

			//Init ToolWindow Commands
			await ShowDocumentsHIstoryCommand.InitializeAsync(this, _dte);

			EnforceKeyBinding();
		}

		/// <summary>
		/// Enforcing the Ctrl+Shift+T because if it was assigned to another command then the default does not work.
		/// </summary>
		private void EnforceKeyBinding()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			var commandsGuid = ReopenClosedDocumentsCommand.CommandSet.ToString("B").ToUpper();
			var reopenCommandBinding = Infrastructure.ConfigurationManager.Config.ReopenCommandBinding;
			var showMoreCommandBinding = Infrastructure.ConfigurationManager.Config.ShowMoreCommandBinding;

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
	}
}