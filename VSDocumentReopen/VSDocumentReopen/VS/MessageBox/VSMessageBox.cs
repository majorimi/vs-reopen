using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics.CodeAnalysis;

namespace VSDocumentReopen.VS.MessageBox
{
	[ExcludeFromCodeCoverage]
	public sealed class VSMessageBox : IMessageBox
	{
		private readonly IServiceProvider _serviceProvider;

		public VSMessageBox(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public void ShowError(string title, string message)
		{
			Show(title, message, OLEMSGICON.OLEMSGICON_CRITICAL);
		}

		public void ShowInfo(string title, string message)
		{
			Show(title, message, OLEMSGICON.OLEMSGICON_INFO);
		}

		private void Show(string title, string message, OLEMSGICON icon)
		{
			VsShellUtilities.ShowMessageBox(
					_serviceProvider,
					message,
					title,
					icon,
					OLEMSGBUTTON.OLEMSGBUTTON_OK,
					OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
		}
	}
}