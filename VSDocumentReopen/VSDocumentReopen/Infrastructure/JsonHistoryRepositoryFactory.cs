using System.IO;
using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure.Helpers;
using VSDocumentReopen.Infrastructure.Repositories;

namespace VSDocumentReopen.Infrastructure
{
	public class JsonHistoryRepositoryFactory : IHistoryRepositoryFactory
	{
		private readonly IJsonSerializer _jsonSerializer;

		public JsonHistoryRepositoryFactory(IJsonSerializer jsonSerializer)
		{
			_jsonSerializer = jsonSerializer;
		}

		public IHistoryRepository CreateHistoryRepository(SolutionInfo solutionInfo)
		{
			var historyFile = Path.Combine(solutionInfo.FullPath, ".vs", "VSDocumentReopen", "history.json");

			return new JsonHistoryRepository(_jsonSerializer, historyFile);
		}
	}
}