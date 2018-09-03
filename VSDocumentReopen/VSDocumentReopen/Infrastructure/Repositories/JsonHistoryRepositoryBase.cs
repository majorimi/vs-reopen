using System;
using System.IO;
using VSDocumentReopen.Infrastructure.Helpers;
using VSDocumentReopen.Infrastructure.Logging;

namespace VSDocumentReopen.Infrastructure.Repositories
{
	public abstract class JsonHistoryRepositoryBase<T>
	{
		private readonly IJsonSerializer _serializer;
		private readonly string _storageFile;

		public JsonHistoryRepositoryBase(IJsonSerializer serializer, string storageFile)
		{
			_serializer = serializer;
			_storageFile = storageFile;
		}

		public bool Save(T closedDocumentHistories)
		{
			try
			{
				if (File.Exists(_storageFile))
				{
					File.Delete(_storageFile);
				}

				var dir = Path.GetDirectoryName(_storageFile);
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				var json = _serializer.Serialize<T>(closedDocumentHistories);
				File.WriteAllText(_storageFile, json);
			}
			catch (Exception ex)
			{
				LoggerContext.Current.Logger.Error($"{nameof(JsonHistoryRepository)} was not able to save!", ex);
				return false;
			}

			return true;
		}

		public T Load()
		{
			if (!File.Exists(_storageFile))
			{
				return default(T);
			}

			var history = File.ReadAllText(_storageFile);
			return _serializer.Deserialize<T>(history);
		}
	}
}