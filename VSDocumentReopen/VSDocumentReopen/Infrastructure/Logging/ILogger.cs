using System;

namespace VSDocumentReopen.Infrastructure.Logging
{
	public interface ILogger
	{
		void Trace(string message);

		void Info(string message);

		void Warning(string message);

		void Warning(string message, Exception exception);

		void Error(string message);

		void Error(string message, Exception exception);

		void Critical(string message);

		void Critical(string message, Exception exception);
	}
}