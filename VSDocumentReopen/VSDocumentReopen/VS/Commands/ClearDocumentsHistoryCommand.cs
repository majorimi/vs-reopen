using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.VS.Commands
{
	public sealed class ClearDocumentsHistoryCommand
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
		private readonly IHistoryCommand _clearHistoryCommand;

		private ClearDocumentsHistoryCommand(AsyncPackage package, OleMenuCommandService commandService, IHistoryCommand clearHistoryCommand)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));
			_clearHistoryCommand = clearHistoryCommand ?? throw new ArgumentNullException(nameof(clearHistoryCommand));
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

		public static async Task InitializeAsync(AsyncPackage package, IHistoryCommand clearHistoryCommand)
		{
			var commandService = package == null
				? null
				: await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			Instance = new ClearDocumentsHistoryCommand(package, commandService, clearHistoryCommand);
		}

		private void Execute(object sender, EventArgs e)
		{
			_clearHistoryCommand.Execute();
		}
	}
}