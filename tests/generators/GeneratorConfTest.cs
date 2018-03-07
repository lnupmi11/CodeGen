using System;
using Xunit;
using CodeGen.generators;

namespace CodeGen {
	public class GeneratorConfTest{

		[Fact]
		public void GetIndentTest()
		{
			Assert.Equal("\t", GeneratorConf.GetIndent(true, 4));
			Assert.Equal("    ", GeneratorConf.GetIndent(false, 4));
			Assert.Equal("  ", GeneratorConf.GetIndent(false, 2));
			Assert.Equal("", GeneratorConf.GetIndent(false, 0));
		}

		[Fact]
		public void ShiftCodeTest()
		{
			Assert.Equal("\tcode", GeneratorConf.ShiftCode("code", 1, "\t"));
			Assert.Equal("code", GeneratorConf.ShiftCode("code", 0, "\t"));
			Assert.Equal("\t\t\tcode", GeneratorConf.ShiftCode("code", 3, "\t"));
			Assert.Equal("    code", GeneratorConf.ShiftCode("code", 1, "    "));
			Assert.Equal("            code", GeneratorConf.ShiftCode("code", 3, "    "));
		}

		[Fact]
		public void NormalizeLangTest()
		{
			Assert.Equal("js_es6", GeneratorConf.NormalizeLang("js"));
			Assert.Equal("csharp", GeneratorConf.NormalizeLang("c#"));
			Assert.Equal("csharp", GeneratorConf.NormalizeLang("cs"));
			Assert.Equal("yaml", GeneratorConf.NormalizeLang("yml"));
			Assert.Equal("cpp", GeneratorConf.NormalizeLang("c++"));
			Assert.Equal("js_es6", GeneratorConf.NormalizeLang("js_es6"));
			Assert.Equal("csharp", GeneratorConf.NormalizeLang("csharp"));
			Assert.Equal("yaml", GeneratorConf.NormalizeLang("yaml"));
			Assert.Equal("cpp", GeneratorConf.NormalizeLang("cpp"));
		}

		[Fact]
		public void GetLanguageTest()
		{
			Assert.NotNull(GeneratorConf.GetLanguage("csharp"));
			Assert.NotNull(GeneratorConf.GetLanguage("python"));
			Assert.NotNull(GeneratorConf.GetLanguage("go"));
			Assert.NotNull(GeneratorConf.GetLanguage("ruby"));
			Assert.NotNull(GeneratorConf.GetLanguage("java"));
			Assert.NotNull(GeneratorConf.GetLanguage("vb"));
			Assert.Throws<IndexOutOfRangeException>(() => GeneratorConf.GetLanguage("somelang"));
		}
	}
}
