using VSDocumentReopen.Helpers;
using Xunit;

namespace VSDocumentReopen.Test.Helpers
{
	public class PathFormatterTest
	{
		[Theory]
		[InlineData(null, 50)]
		[InlineData("", 50)]
		[InlineData("  ", 50)]
		public void ItShouldHandleInvalidInput(string path, int limit)
		{
			var ret = PathFormatter.ShrinkPath(path, limit);

			Assert.Equal(string.Empty, ret);
		}

		[Theory]
		[InlineData("test", 50)]
		[InlineData("path", 5)]
		[InlineData("123", 3)]
		public void ItShouldHandleShorterPathThenTheLimit(string path, int limit)
		{
			var ret = PathFormatter.ShrinkPath(path, limit);

			Assert.Equal(path, ret);
		}

		[Theory]
		[InlineData("d:/test/subfolder/1/file1.txt", 20, "___")]
		[InlineData("d:/test/subfolder/1/file1.txt", 20, "??")]
		[InlineData("d:/test/subfolder/1/file1.txt", 20, "..")]
		public void ItShouldContainSpacer(string path, int limit, string spacer)
		{
			var ret = PathFormatter.ShrinkPath(path, limit, spacer);

			Assert.False(string.IsNullOrWhiteSpace(ret));
			Assert.Contains(spacer, ret);
		}

		[Theory]
		[InlineData("d:/test/subfolder/1/file1.txt", 50)]
		[InlineData("d:/test/subfolder/2/file1.txt", 20)]
		[InlineData("d:/test/subfolder/3/a.txt", 10)]
		[InlineData("d:/test/subfolder/3/a.txt", 200)]
		[InlineData("d:/test/subfolder/1/subfolder/2/subfolder/3/subfolder/4/subfolder/5/subfolder/6/subfolder/7/subfolder/file1.txt", 20)]
		[InlineData("d:/test/subfolder/1/subfolder/2/subfolder/3/subfolder/4/subfolder/5/subfolder/6/subfolder/7/subfolder/file1.txt", 100)]
		public void ItShouldNotReachLimit(string path, int limit)
		{
			var ret = PathFormatter.ShrinkPath(path, limit);

			Assert.False(string.IsNullOrWhiteSpace(ret));
			Assert.True(ret.Length <= limit, $"Result: \"{ret}\" length of result: {ret.Length} but should not more than: {limit}.");
		}
	}
}