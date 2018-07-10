using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSDocumentReopen.Commands;
using VSDocumentReopen.Documents;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	[Guid(ReopenPackage.PackageGuidString)]
	[ProvideAutoLoad(UIContextGuids.NoSolution)]
	public sealed class ReopenPackage : AsyncPackage
	{
		/// <summary>
		/// ReopenPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "b30147a1-6fbc-4b94-bf01-123d837c4fe2";

		private readonly DTE2 _dte;
		private readonly SolutionEvents _solutionEvents;
		private readonly DocumentEvents _documentEvents;

		/// <summary>
		/// Initializes a new instance of the <see cref="Reopen"/> class.
		/// </summary>
		public ReopenPackage()
		{
			_dte = GetGlobalService(typeof(DTE)) as DTE2 ?? throw new NullReferenceException($"Unable to get service {nameof(DTE2)}");

			_solutionEvents = _dte.Events.SolutionEvents;
			_documentEvents = _dte.Events.DocumentEvents;
		}

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			// When initialized asynchronously, the current thread may be a background thread at this point.
			// Do any initialization that requires the UI thread after switching to the UI thread.
			await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

			await Reopen.InitializeAsync(this, _dte);
			await ClearHistory.InitializeAsync(this);
			await History.InitializeAsync(this, _dte);

			EnforceKeyBinding();

			_solutionEvents.Opened += () =>
			{
				DocumentTracker.Instance.Clear();
				_documentEvents.DocumentClosing += DocumentEventsOnDocumentClosing;

				Debug.WriteLine("Solution opened");
			};
			_solutionEvents.AfterClosing += () =>
			{
				_documentEvents.DocumentClosing -= DocumentEventsOnDocumentClosing;
				DocumentTracker.Instance.Clear();

				Debug.WriteLine("Solution closed");
			};
		}

		private void DocumentEventsOnDocumentClosing(Document document)
		{
			DocumentTracker.Instance.AddClosed(document);
		}

		/// <summary>
		/// Enforcing the Ctrl+Shift+T because if it was assigned to another command then the default does not work.
		/// </summary>
		private void EnforceKeyBinding()
		{
			var commands = new List<Command>();
			foreach (Command command in _dte.Commands)
			{
				if (!string.IsNullOrEmpty(command.Name))
					commands.Add(command);
			}

			Command comm;
			var guid = Reopen.CommandSet.ToString("B").ToUpper();
			var binding = "Global::Ctrl+Shift+T";

			foreach (var command in commands)
			{
				if (command.Bindings is object[] bindings && bindings.Length > 0)
				{
					var bind = string.Join(" ", bindings.Select(x => (string)x));
					if (bind.Contains(binding) && command.Guid != guid)
					{
						command.Bindings = new object[0];
					}
				}
			}

			foreach (var command in commands)
			{
				if (command.Guid == guid)
				{
					comm = command;
					comm.Bindings = binding;
					break;
				}
			}
		}
	}
}