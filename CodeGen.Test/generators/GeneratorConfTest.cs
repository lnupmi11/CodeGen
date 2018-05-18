using System;
using System.Collections.Generic;
using CodeGen.generators;
using Xunit;

namespace CodeGen.Test.generators
{
	public class GeneratorConfTest
	{
		[Theory]
		[MemberData(nameof(GeneratorConfTestData.GetIndentData), MemberType = typeof(GeneratorConfTestData))]
		public void TestGetIndent(bool tabs, int tabStop, string result)
		{
			Assert.Equal(GeneratorConf.GetIndent(tabs, tabStop), result);
		}
		
		[Theory]
		[MemberData(nameof(GeneratorConfTestData.ShiftCodeData), MemberType = typeof(GeneratorConfTestData))]
		public void TestShiftCode(string code, int num, string indent, string result)
		{
			Assert.Equal(GeneratorConf.ShiftCode(code, num, indent), result);
		}
		
		[Theory]
		[MemberData(nameof(GeneratorConfTestData.NormalizeLangData), MemberType = typeof(GeneratorConfTestData))]
		public void TestNormalizeLang(string lang, string result)
		{
			Assert.Equal(GeneratorConf.NormalizeLang(lang), result);
		}
		
		[Theory]
		[MemberData(nameof(GeneratorConfTestData.GetLanguageData), MemberType = typeof(GeneratorConfTestData))]
		public void TestGetLanguage(string lang)
		{
			Assert.Equal(GeneratorConf.GetLanguage(lang), GeneratorConf.Languanges[lang]);
		}

		[Theory]
		[MemberData(nameof(GeneratorConfTestData.GetLanguageDataThrows), MemberType = typeof(GeneratorConfTestData))]
		public void TestGenerateFieldThrows(string lang)
		{
			Assert.Throws<IndexOutOfRangeException>(() => GeneratorConf.GetLanguage(lang));
		}

		private class GeneratorConfTestData
		{
			public static IEnumerable<object[]> GetIndentData => new List<object[]>
			{
				new object[]
				{
					true, 4, "\t"
				},
				new object[]
				{
					false, 4, "    "
				},
				new object[]
				{
					false, 2, "  "
				},
				new object[]
				{
					false, 0, ""
				}
			};
			
			public static IEnumerable<object[]> ShiftCodeData => new List<object[]>
			{
				new object[]
				{
					"code", 1, "\t", "\tcode"
				},
				new object[]
				{
					"code", 0, "\t", "code"	
				},
				new object[]
				{
					"code", 3, "\t", "\t\t\tcode"
				},
				new object[]
				{
					"code", 1, "    ", "    code"	
				},
				new object[]
				{
					"code", 3, "    ", "            code"
				},
				new object[]
				{
					"code", 3, "fun", "funfunfuncode"
				},
				new object[]
				{
					"code", 2, "=D", "=D=Dcode"
				}
			};

			public static IEnumerable<object[]> NormalizeLangData => new List<object[]>
			{
				new object[]
				{
					"js", "js_es6"
				},
				new object[]
				{
					"c#", "csharp"
				},
				new object[]
				{
					"cs", "csharp"
				},
				new object[]
				{
					"yml", "yaml"
				},
				new object[]
				{
					"c++", "cpp"
				},
				new object[]
				{
					"js_es6", "js_es6"
				},
				new object[]
				{
					"csharp", "csharp"
				},
				new object[]
				{
					"yaml", "yaml"
				},
				new object[]
				{
					"cpp", "cpp"
				},
				new object[]
				{
					"Arabic", "Arabic"
				},
				new object[]
				{
					"some_unknown_language", "some_unknown_language"
				},
			};

			public static IEnumerable<object[]> GetLanguageData => new List<object[]>
			{
				new object[]
				{
					"csharp"
				},
				new object[]
				{
					"python"	
				},
				new object[]
				{
					"go"
				},
				new object[]
				{
					"ruby"
				},
				new object[]
				{
					"java"
				},
				new object[]
				{
					"vb"
				}
			};

			public static IEnumerable<object[]> GetLanguageDataThrows => new List<object[]>
			{
				new object[]
				{
					"somelang"
				}
			};
		}
	}
}
