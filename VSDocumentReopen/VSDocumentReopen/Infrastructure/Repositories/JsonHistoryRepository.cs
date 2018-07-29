using System;
using System.Collections.Generic;
using System.IO;
using VSDocumentReopen.Domain.Documents;
using VSDocumentReopen.Infrastructure.Helpers;

namespace VSDocumentReopen.Infrastructure.Repositories
{
	public sealed class JsonHistoryRepository : IHistoryRepository
	{
		private readonly IJsonSerializer _serializer;
		private readonly string _historyFile;

		public JsonHistoryRepository(IJsonSerializer serializer, string storageFile)
		{
			_serializer = serializer;
			_historyFile = storageFile;
		}

		public bool SaveHistory(IEnumerable<IClosedDocument> closedDocumentHistories)
		{
			try
			{
				if (File.Exists(_historyFile))
				{
					File.Delete(_historyFile);
				}

				var dir = Path.GetDirectoryName(_historyFile);
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				var json = _serializer.Serialize<IEnumerable<IClosedDocument>>(closedDocumentHistories);
				File.WriteAllText(_historyFile, json);
			}
			catch (Exception e)
			{
				//TODO: log and notify user...
				return false;
			}

			return true;
		}

		public IEnumerable<IClosedDocument> GetHistory()
		{
			if (!File.Exists(_historyFile))
			{
				return new List<IClosedDocument>();
			}

			var history = File.ReadAllText(_historyFile);
			return _serializer.Deserialize<IEnumerable<IClosedDocument>>(history);
		}
	}
}