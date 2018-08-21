using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VSDocumentReopen.Infrastructure.Logging
{
	public sealed class NullLogger : ILogger
	{
		private static readonly NullLogger _instance;

		public static ILogger Instance => _instance;

		static NullLogger()
		{
			_instance = new NullLogger();
		}

		private NullLogger()
		{ }

		public void Trace(string message)
		{
			Log(message);
		}

		public void Info(string message)
		{
			Log(message);
		}

		public void Warning(string message)
		{
			Log(message);
		}

		public void Warning(string message, Exception exception)
		{
			Log(message, exception);
		}

		public void Error(string message)
		{
			Log(message);
		}

		public void Error(string message, Exception exception)
		{
			Log(message, exception);
		}
		public void Critical(string message)
		{
			Log(message);
		}
		public void Critical(string message, Exception exception)
		{
			Log(message, exception);
		}

		private void Log(string message, Exception exception = null, [CallerMemberName] string callerName = "")
		{
			Debug.WriteLine($"{callerName}: {message} {exception}");
		}
	}
}