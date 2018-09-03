using System;
using System.IO;
using VSDocumentReopen.Infrastructure.Helpers;
using VSDocumentReopen.Infrastructure.Repositories;

namespace VSDocumentReopen.Infrastructure
{
	public class JsonIHistoryToolWindowRepositoryFactory : IHistoryToolWindowRepositoryFactory
	{
		private readonly IJsonSerializer _jsonSerializer;

		public JsonIHistoryToolWindowRepositoryFactory(IJsonSerializer jsonSerializer)
		{
			_jsonSerializer = jsonSerializer;
		}

		public IHistoryToolWindowRepository Create()
		{
			var historyFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				ConfigurationManager.Current.Config.PackageWorkingDirName,
				ConfigurationManager.Current.Config.ToolWindowSettingsFileName);

			return new JsonHistoryToolWindowRepository(_jsonSerializer, historyFile);
		}
	}
}