using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Document.Tracking;
using VSDocumentReopen.Infrastructure.Helpers;
using VSDocumentReopen.Infrastructure.HistoryCommands;
using VSDocumentReopen.Infrastructure.Logging;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.VS.Commands
{
	public sealed class DocumentsHistoryCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0200;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("d968b4de-3a69-4eb1-b676-942055da9dfd");

		private static readonly List<OleMenuCommand> Commands = new List<OleMenuCommand>();
		private const string HistoryItemKey = "HistoryItem";

		private readonly AsyncPackage _package;
		private readonly IDocumentHistoryQueries _documentHistoryQueries;
		private readonly IHistoryCommandFactory _reopenSomeDocumentsCommandFactory;

		private DocumentsHistoryCommand(AsyncPackage package,
			OleMenuCommandService commandService,
			IDocumentHistoryQueries documentHistoryQueries,
			IHistoryCommandFactory reopenSomeDocumentsCommandFactory)
		{
			_package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
			_documentHistoryQueries = documentHistoryQueries ?? throw new ArgumentNullException(nameof(documentHistoryQueries));
			_reopenSomeDocumentsCommandFactory = reopenSomeDocumentsCommandFactory ?? throw new ArgumentNullException(nameof(reopenSomeDocumentsCommandFactory));

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
			var currentCommand = (sender as OleMenuCommand) ?? throw new InvalidCastException($"Unable to cast {nameof(sender)} to {typeof(OleMenuCommand)}");
			var mcs = _package.GetServiceAsync(typeof(IMenuCommandService)).GetAwaiter().GetResult() as OleMenuCommandService
			          ?? throw new InvalidCastException($"Unable to cast {nameof(IMenuCommandService)} to {typeof(OleMenuCommandService)}");

			foreach (var cmd in Commands)
			{
				mcs.RemoveCommand(cmd);
			}

			var history = _documentHistoryQueries.Get(Infrastructure.ConfigurationManager.Current.Config.MaxNumberOfHistoryItemsOnMenu);

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
			var cmd = (OleMenuCommand)sender ?? throw new InvalidCastException($"Unable to cast {nameof(sender)} to {typeof(OleMenuCommand)}");

			var document = (cmd.Properties[HistoryItemKey] as IClosedDocument) ?? NullDocument.Instance;
			var command = _reopenSomeDocumentsCommandFactory.CreateCommand(document);
			command.Execute();

			LoggerContext.Current.Logger.Info($"VS Command: {nameof(DocumentsHistoryCommand)} was executed with {command.GetType()}");
		}

		public static DocumentsHistoryCommand Instance
		{
			get;
			private set;
		}

		public static async Task InitializeAsync(AsyncPackage package, IDocumentHistoryQueries documentHistoryQueries, IHistoryCommandFactory reopenSomeDocumentsCommandFactory)
		{
			var commandService = package == null
				? null
				: await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			Instance = new DocumentsHistoryCommand(package, commandService, documentHistoryQueries, reopenSomeDocumentsCommandFactory);
		}

		private void Execute(object sender, EventArgs e)
		{
			//Do nothing, menu should be disabled...
		}
	}
}