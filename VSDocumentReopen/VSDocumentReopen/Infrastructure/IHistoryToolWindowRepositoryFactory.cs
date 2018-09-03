using VSDocumentReopen.Infrastructure.Repositories;

namespace VSDocumentReopen.Infrastructure
{
	public interface IHistoryToolWindowRepositoryFactory
	{
		IHistoryToolWindowRepository Create();
	}
}