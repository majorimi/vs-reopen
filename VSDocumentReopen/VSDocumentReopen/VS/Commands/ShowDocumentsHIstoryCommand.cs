using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSDocumentReopen.VS.ToolWindows;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.VS.Commands
{
	public sealed class ShowDocumentsHIstoryCommand
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

		private ShowDocumentsHIstoryCommand(AsyncPackage package, OleMenuCommandService commandService)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			var menuCommandId = new CommandID(CommandSet, CommandId);
			var menuItem = new MenuCommand(Execute, menuCommandId);
			commandService.AddCommand(menuItem);
		}

		public static ShowDocumentsHIstoryCommand Instance
		{
			get;
			private set;
		}

		public static async Task InitializeAsync(AsyncPackage package)
		{
			var commandService = package == null
				? null
				: await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			Instance = new ShowDocumentsHIstoryCommand(package, commandService);
		}

		private void Execute(object sender, EventArgs e)
		{
			ToolWindowPane window = _package.FindToolWindow(typeof(ClosedDocumentsHistory), 0, true);
			if ((null == window) || (null == window.Frame))
			{
				throw new NotSupportedException("Cannot create tool window");
			}

			IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
			Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
		}
	}
}