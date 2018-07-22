using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Infrastructure.HistoryCommands;
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
		private readonly IHistoryCommand _historyCommand;

		private ReopenClosedDocumentsCommand(AsyncPackage package, OleMenuCommandService commandService, IHistoryCommand historyCommand)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
			_historyCommand = historyCommand ?? throw new ArgumentNullException(nameof(historyCommand));

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

		public static async Task InitializeAsync(AsyncPackage package, IHistoryCommand historyCommand)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new ReopenClosedDocumentsCommand(package, commandService, historyCommand);
		}

		private void Execute(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			_historyCommand.Execute();
		}
	}
}