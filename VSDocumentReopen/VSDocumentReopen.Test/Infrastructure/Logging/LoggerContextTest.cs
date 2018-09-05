using VSDocumentReopen.Infrastructure.Logging;
using Xunit;

namespace VSDocumentReopen.Test.Infrastructure.Logging
{
	public class LoggerContextTest
	{
		[Fact]
		public void ItShould_Have_Default_Context()
		{
			Assert.NotNull(LoggerContext.Current);
			Assert.IsType<DefaultLoggerContext>(LoggerContext.Current);
		}

		[Fact]
		public void ItShould_Not_Allow_To_Set_Context_To_Null()
		{
			LoggerContext.Current = null;

			Assert.NotNull(LoggerContext.Current);
			Assert.IsType<DefaultLoggerContext>(LoggerContext.Current);
		}
	}
}