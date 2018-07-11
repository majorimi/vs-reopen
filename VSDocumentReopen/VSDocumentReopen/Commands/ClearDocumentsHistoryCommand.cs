using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Documents;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.Commands
{
	internal sealed class ClearDocumentsHistoryCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0102;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("d968b4de-3a69-4eb1-b676-942055da9dfd");

		private readonly AsyncPackage _package;

		private ClearDocumentsHistoryCommand(AsyncPackage package, OleMenuCommandService commandService)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			var menuCommandId = new CommandID(CommandSet, CommandId);
			var menuItem = new MenuCommand(Execute, menuCommandId);
			commandService.AddCommand(menuItem);
		}

		public static ClearDocumentsHistoryCommand Instance
		{
			get;
			private set;
		}

		private IAsyncServiceProvider ServiceProvider => _package;

		public static async Task InitializeAsync(AsyncPackage package)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new ClearDocumentsHistoryCommand(package, commandService);
		}

		private void Execute(object sender, EventArgs e)
		{
			DocumentHistory.Instance.Clear();
		}
	}
}