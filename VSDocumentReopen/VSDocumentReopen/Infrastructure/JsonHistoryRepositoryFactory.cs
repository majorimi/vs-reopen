﻿using System.IO;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure.Helpers;
using VSDocumentReopen.Infrastructure.Repositories;

namespace VSDocumentReopen.Infrastructure
{
	public sealed class JsonHistoryRepositoryFactory : IHistoryRepositoryFactory
	{
		private readonly IJsonSerializer _jsonSerializer;

		public JsonHistoryRepositoryFactory(IJsonSerializer jsonSerializer)
		{
			_jsonSerializer = jsonSerializer;
		}

		public IHistoryRepository CreateHistoryRepository(SolutionInfo solutionInfo)
		{
			var historyFile = Path.Combine(solutionInfo.FullPath,
				ConfigurationManager.Config.VSTempFolderName,
				ConfigurationManager.Config.PackageWorkingDirName,
				ConfigurationManager.Config.HistoryFileName);

			return new JsonHistoryRepository(_jsonSerializer, historyFile);
		}
	}
}