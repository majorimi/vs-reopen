using System;
using System.ComponentModel.Design;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.Commands
{
	internal sealed class ShowMore
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0110;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("d968b4de-3a69-4eb1-b676-942055da9dfd");

		private readonly AsyncPackage _package;
		private readonly DTE2 _dte;

		private ShowMore(AsyncPackage package, OleMenuCommandService commandService, DTE2 dte)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
			_dte = dte ?? throw new ArgumentNullException(nameof(dte));

			var menuCommandId = new CommandID(CommandSet, CommandId);
			var menuItem = new MenuCommand(Execute, menuCommandId);
			commandService.AddCommand(menuItem);
		}

		public static ShowMore Instance
		{
			get;
			private set;
		}

		private IAsyncServiceProvider ServiceProvider => _package;

		public static async Task InitializeAsync(AsyncPackage package, DTE2 dte)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new ShowMore(package, commandService, dte);
		}

		private void Execute(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
		}
	}
}