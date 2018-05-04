using System;
using CodeGen.generators;
using Xunit;

namespace CodeGen.Test.generators
{
	public class GeneratorConfTest
	{
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
			Assert.Equal("funfunfuncode", GeneratorConf.ShiftCode("code", 3, "fun"));
			Assert.Equal("=D=Dcode", GeneratorConf.ShiftCode("code", 2, "=D"));
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
			Assert.Equal("Arabic", GeneratorConf.NormalizeLang("Arabic"));
			Assert.Equal("some_unknown_language", GeneratorConf.NormalizeLang("some_unknown_language"));
		}

		[Fact]
		public void GetLanguageTest()
		{
			Assert.Equal(GeneratorConf.Languanges["csharp"], GeneratorConf.GetLanguage("csharp"));
			Assert.Equal(GeneratorConf.Languanges["python"], GeneratorConf.GetLanguage("python"));
			Assert.Equal(GeneratorConf.Languanges["go"], GeneratorConf.GetLanguage("go"));
			Assert.Equal(GeneratorConf.Languanges["ruby"], GeneratorConf.GetLanguage("ruby"));
			Assert.Equal(GeneratorConf.Languanges["java"], GeneratorConf.GetLanguage("java"));
			Assert.Equal(GeneratorConf.Languanges["vb"], GeneratorConf.GetLanguage("vb"));
			Assert.Throws<IndexOutOfRangeException>(() => GeneratorConf.GetLanguage("somelang"));
		}

		
		
	}
}