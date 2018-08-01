using VSDocumentReopen.Domain;
using Xunit;

namespace VSDocumentReopen.Test.Domain
{
	public class SolutionInfoTest
	{
		[Theory]
		[InlineData(null, null)]
		[InlineData("", "")]
		[InlineData("path", "name")]
		public void ItShould_SetGivenData(string path, string name)
		{
			var si = new SolutionInfo(path, name);

			Assert.Equal(path, si.FullPath);
			Assert.Equal(name, si.Name);
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData("", "")]
		[InlineData("path", "name")]
		public void ItShould_CorrectlyDeconstruct(string path, string name)
		{
			var si = new SolutionInfo(path, name);

			var (p, n) = si;

			Assert.Equal(p, si.FullPath);
			Assert.Equal(n, si.Name);
		}
	}
}