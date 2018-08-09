using System;
using System.Reflection;
using Microsoft.VisualStudio.Shell;
using Moq;

namespace VSDocumentReopen.Test.VS.Commands
{
	public abstract class VisualStudioCommandTestBase<TCommand>
	{
		protected readonly Mock<AsyncPackage> _asyncPackageMock;

		protected VisualStudioCommandTestBase()
		{
			_asyncPackageMock = new Mock<AsyncPackage>();
		}

		protected void InvoceCommand(TCommand command, string method = "Execute", object sender = null)
		{
			var commandType = typeof(TCommand);
			var methodInfo = commandType.GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance);

			methodInfo?.Invoke(command, new object[] { sender, new EventArgs() });
		}
	}
}