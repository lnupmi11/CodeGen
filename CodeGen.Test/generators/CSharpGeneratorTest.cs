using System;
using System.Collections.Generic;
using CodeGen.generators;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;

namespace CodeGen.Test.generators
{
	public class CSharpGeneratorTest
	{
		private FriendlyNameAttribute CSharpGenerator;

		private static CSharpGenerator Gen { get; } = new CSharpGenerator();
		private static string Indent { get; } = Gen.GetIndent();

		[Theory]
		[MemberData(nameof(CSharpGeneratorTestData.ValidData), MemberType = typeof(CSharpGeneratorTestData))]
		public void TestGenerateField(Field field, string output)
		{
			Assert.Equal(output, Gen.GenerateField(field));
		}

		[Theory]
		[MemberData(nameof(CSharpGeneratorTestData.GetterData), MemberType = typeof(CSharpGeneratorTestData))]
		public void TestGenerateGetter(Field fieldVariable, string output)
		{
			Assert.Equal(output, Gen.GenerateGetter(fieldVariable));
		}

	//	[Theory]
		[MemberData(nameof(CSharpGeneratorTestData.ValidData), MemberType = typeof(CSharpGeneratorTestData))]
		public void TestGenerateSetter(Variable fieldVariable)
		{
		}

		private class CSharpGeneratorTestData
		{
			public static IEnumerable<object[]> ThrowsData => new List<object[]>
			{
				new object[] {null},
				new object[] {new Field {Name = ""}},
				new object[] {new Field {Name = "      \t\t\t\n\n\n"}},
				new object[] {new Field {Name = "test"}},
				new object[] {new Field {Type = "test"}},
				new object[] {new Field {Name = "test value", Type = "test"}},
				new object[] {new Field {Name = "test", Type = "test value"}},
				new object[] {new Field {Name = "test", Type = "value", Access = "unknown"}},
			};

			public static IEnumerable<object[]> ValidData => new List<object[]>
			{
				new object[]
				{
					new Field {Name = "test", Type = "int"},
					Indent + "private int test;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "public"},
					Indent + "public string test;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "public", Default = "\"test\""},
					Indent + "public string test = \"test\";"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Static = true},
					Indent + "private static string test;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Const = true},
					Indent + "private const string test;"
				}
			};

			public static IEnumerable<object[]> GetterData => new List<object[]>
			{
				new object[]
				{
					new Field {Name = "test", Type = "int"},
					"public int getTest()\n{\n" + Indent + "return test;\n}"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "public"},
					"public string getTest()\n{\n" + Indent + "return test;\n}"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "protected", Default = "\"test\""},
					"public string getTest()\n{\n" + Indent + "return test;\n}"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Static = true},
					"public static string getTest()\n{\n" + Indent + "return test;\n}"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Const = true},
					"public string getTest()\n{\n" + Indent + "return test;\n}"
				}
			};
		}
	}
}