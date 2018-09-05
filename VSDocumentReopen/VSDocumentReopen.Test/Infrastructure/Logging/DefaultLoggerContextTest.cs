using VSDocumentReopen.Infrastructure.Logging;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Logging
{
	public class DefaultLoggerContextTest
	{
		[Fact]
		public void ItShould_Return_NullLogger()
		{
			Assert.NotNull(DefaultLoggerContext.Current.Logger);
			Assert.Equal(NullLogger.Instance, DefaultLoggerContext.Current.Logger);
		}
	}
}
