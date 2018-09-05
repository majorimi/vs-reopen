using System;
using System.Collections.Generic;
using System.Diagnostics;
using VSDocumentReopen.Infrastructure.Logging;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Logging
{
	public class NullLoggerTest
	{
		private class MyListenerThatDoesNotShowDialogOnFail : TraceListener
		{
			public int WriteCount { get; private set; } = 0;
			public int WriteLineCount { get; private set; } = 0;

			public override void Write(string message) => WriteCount++;

			public override void WriteLine(string message) => WriteLineCount++;
		}

		[Fact]
		public void ItShould_Be_Singleton()
		{
			var logger1 = NullLogger.Instance;
			var logger2 = NullLogger.Instance;

			Assert.Same(logger1, logger2);
		}

		[Fact]
		public void ItShould_Call_Debug_WriteLine()
		{
			var methodsWithString = new List<Action<string>>()
			{
				new Action<string>((msg) => NullLogger.Instance.Critical(msg)),
				new Action<string>((msg) => NullLogger.Instance.Error(msg)),
				new Action<string>((msg) => NullLogger.Instance.Info(msg)),
				new Action<string>((msg) => NullLogger.Instance.Trace(msg)),
				new Action<string>((msg) => NullLogger.Instance.Warning(msg))
			};

			var methodsWithException = new List<Action<string, Exception>>()
			{
				new Action<string, Exception>((msg, ex) => NullLogger.Instance.Critical(msg, ex)),
				new Action<string, Exception>((msg, ex) => NullLogger.Instance.Error(msg, ex)),
				new Action<string, Exception>((msg, ex) => NullLogger.Instance.Warning(msg, ex))
			};

			foreach (var action in methodsWithString)
			{
				var listener = new MyListenerThatDoesNotShowDialogOnFail();
				Debug.Listeners.Clear();
				Debug.Listeners.Add(listener);

				action("test");

				Assert.Equal(1, listener.WriteLineCount);
			}

			foreach (var action in methodsWithException)
			{
				var listener = new MyListenerThatDoesNotShowDialogOnFail();
				Debug.Listeners.Clear();
				Debug.Listeners.Add(listener);

				action("test", new Exception("ex msg"));

				Assert.Equal(1, listener.WriteLineCount);
			}
		}
	}
}