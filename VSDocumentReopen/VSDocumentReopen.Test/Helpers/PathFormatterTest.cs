using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}