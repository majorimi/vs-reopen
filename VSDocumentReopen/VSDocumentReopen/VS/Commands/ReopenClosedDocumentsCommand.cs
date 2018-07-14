using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Infrastructure.ClosedDocument;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.VS.Commands
{
	internal sealed class ReopenClosedDocumentsCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0101;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("d968b4de-3a69-4eb1-b676-942055da9dfd");

		private readonly AsyncPackage _package;
		private readonly _DTE _dte;

		private ReopenClosedDocumentsCommand(AsyncPackage package, OleMenuCommandService commandService, _DTE dte)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
			_dte = dte ?? throw new ArgumentNullException(nameof(dte));

			var menuCommandId = new CommandID(CommandSet, CommandId);
			var menuItem = new MenuCommand(Execute, menuCommandId);
			commandService.AddCommand(menuItem);
		}

		public static ReopenClosedDocumentsCommand Instance
		{
			get;
			private set;
		}

		private IAsyncServiceProvider ServiceProvider => _package;

		public static async Task InitializeAsync(AsyncPackage package, _DTE dte)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new ReopenClosedDocumentsCommand(package, commandService, dte);
		}

		private void Execute(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			var document = DocumentHistory.Instance.GetLastClosed();
			if (document.IsValid())
			{
				_dte.ItemOperations.OpenFile(document.FullName, document.Kind);
			}
		}
	}
}