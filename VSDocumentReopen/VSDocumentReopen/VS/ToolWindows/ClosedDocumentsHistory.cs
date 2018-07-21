using System;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using VSDocumentReopen.Infrastructure.Commands;
using Task = System.Threading.Tasks.Task;

namespace VSDocumentReopen.VS.ToolWindows
{
	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	/// </summary>
	/// <remarks>
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
	/// usually implemented by the package implementer.
	/// <para>
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its
	/// implementation of the IVsUIElementPane interface.
	/// </para>
	/// </remarks>
	[Guid("203513ef-1922-433a-ba3e-1801fb4e9894")]
	public class ClosedDocumentsHistory : ToolWindowPane
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ClosedDocumentsHistory"/> class.
		/// </summary>
		public ClosedDocumentsHistory()
			: base(null)
		{
			Caption = "Closed Documents History";

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
			// the object returned by the Content property.

			Content = ContentWindow;
		}


		private static ClosedDocumentsHistoryControl ContentWindow;

		public static async Task InitializeAsync(AsyncPackage package, _DTE dte, IDocumentCommandFactory documentCommandFactory)
		{
			ContentWindow = new ClosedDocumentsHistoryControl(documentCommandFactory);

			await Task.CompletedTask;
		}
	}
}