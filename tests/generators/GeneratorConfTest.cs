using Xunit;
using CodeGen.generators;

namespace CodeGen {
	public class GeneratorConfTest{

		[Fact]
		public void PassingTest()
		{
			Assert.Equal("\t", GeneratorConf.GetIndent(true, 4));
			Assert.Equal("    ", GeneratorConf.GetIndent(false, 4));
			Assert.Equal("  ", GeneratorConf.GetIndent(false, 2));
			Assert.Equal("", GeneratorConf.GetIndent(false, 0));
		}
	}
}
