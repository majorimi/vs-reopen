using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure;
using VSDocumentReopen.Infrastructure.ClosedDocument;
using VSDocumentReopen.Infrastructure.Helpers;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.VS.Commands
{
	internal sealed class DocumentsHistoryCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0103;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("d968b4de-3a69-4eb1-b676-942055da9dfd");

		private static readonly List<OleMenuCommand> Commands = new List<OleMenuCommand>();
		private const string HistoryItemKey = "HistoryItem";

		private readonly AsyncPackage _package;
		private readonly DTE2 _dte;

		private DocumentsHistoryCommand(AsyncPackage package, OleMenuCommandService commandService, DTE2 dte)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
			_dte = dte ?? throw new ArgumentNullException(nameof(dte));

			var menuCommandId = new CommandID(CommandSet, CommandId);
			var command = new OleMenuCommand(Execute, menuCommandId)
			{
				Visible = false,
				Enabled = false
			};
			command.BeforeQueryStatus += DynamicStartBeforeQueryStatus;
			commandService.AddCommand(command);
		}

		private void DynamicStartBeforeQueryStatus(object sender, EventArgs e)
		{
			var currentCommand = sender as OleMenuCommand ?? throw new InvalidCastException($"Unable to cast {nameof(sender)} to {typeof(OleMenuCommand)}");
			var mcs = ServiceProvider.GetServiceAsync(typeof(IMenuCommandService)).GetAwaiter().GetResult() as OleMenuCommandService
			          ?? throw new InvalidCastException($"Unable to cast {nameof(IMenuCommandService)} to {typeof(OleMenuCommandService)}");

			foreach (var cmd in Commands)
			{
				mcs.RemoveCommand(cmd);
			}

			var history = DocumentHistory.Instance.GetAll()
				.Take(ConfigurationManager.Config.MaxNumberOfHistoryItemsOnMenu);

			currentCommand.Visible = true;
			currentCommand.Text = history.Any() ? "<History>" : "<No History>";

			var j = 1;
			foreach (var item in history)
			{
				var menuCommandId = new CommandID(CommandSet, CommandId + j);
				var command = new OleMenuCommand(DynamicCommandCallback, menuCommandId);

				command.Properties.Add(HistoryItemKey, item);
				command.Text = $"{j++}: {PathFormatter.ShrinkPath(item.FullName, 50)}";
				command.BeforeQueryStatus += (x, y) =>
				{
					(x as OleMenuCommand).Visible = true;
				};

				Commands.Add(command);
				mcs.AddCommand(command);
			}
		}

		private void DynamicCommandCallback(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			var cmd = (OleMenuCommand)sender ?? throw new InvalidCastException($"Unable to cast {nameof(sender)} to {typeof(OleMenuCommand)}");

			var document = (cmd.Properties[HistoryItemKey] as IClosedDocument) ?? NullDocument.Instance;
			if (document.IsValid())
			{
				_dte.ItemOperations.OpenFile(document.FullName, document.Kind);
			}
			
			DocumentHistory.Instance.Remove(document);
		}

		public static DocumentsHistoryCommand Instance
		{
			get;
			private set;
		}

		private IAsyncServiceProvider ServiceProvider => _package;

		public static async Task InitializeAsync(AsyncPackage package, DTE2 dte)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
			Instance = new DocumentsHistoryCommand(package, commandService, dte);
		}

		private void Execute(object sender, EventArgs e)
		{
			//Do nothing, menu should be disabled...
		}
	}
}