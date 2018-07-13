using VSDocumentReopen.Domain;
using VSDocumentReopen.Infrastructure.Repositories;

namespace VSDocumentReopen.Infrastructure
{
	public interface IHistoryRepositoryFactory
	{
		IHistoryRepository CreateHistoryRepository(SolutionInfo solutionInfo);
	}
}