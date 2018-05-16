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

		[Theory]
		[MemberData(nameof(CSharpGeneratorTestData.SetterData), MemberType = typeof(CSharpGeneratorTestData))]
		public void TestGenerateSetter(Field fieldVariable, string output)
		{
			Assert.Equal(output, Gen.GenerateSetter(fieldVariable));
		}
		
		[Theory]
		[MemberData(nameof(CSharpGeneratorTestData.MethodData), MemberType = typeof(CSharpGeneratorTestData))]
		public void TestGenerateMethod(Method fieldVariable, string output)
		{
			Assert.Equal(output, Gen.GenerateMethod(fieldVariable));
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
					"public string getTest()\n{\n" + Indent + "return test;\n}"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Const = true},
					"public string getTest()\n{\n" + Indent + "return test;\n}"
				}
			};
			
			public static IEnumerable<object[]> SetterData => new List<object[]>
			{
				new object[]
				{
					new Field {Name = "test", Type = "int"},
					"public void setTest(int newValue)\n{\n" + Indent + "test = newValue;\n}"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "public"},
					"public void setTest(string newValue)\n{\n" + Indent + "test = newValue;\n}"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "protected", Default = "\"test\""},
					"public void setTest(string newValue)\n{\n" + Indent + "test = newValue;\n}"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Static = true},
					"public void setTest(string newValue)\n{\n" + Indent + "test = newValue;\n}"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Const = true},
					"public void setTest(string newValue)\n{\n" + Indent + "test = newValue;\n}"
				}
			};
			
			public static IEnumerable<object[]> MethodData => new List<object[]>
			{
				new object[]
				{
					new Method() {Name = "test"},
					"private void test()\n{}"
				},
				new object[]
				{
					new Method() {Name = "test", Access = "default"},
					"private void test()\n{}"
				},
				new object[]
				{
					new Method() {Name = "test", Access = "protected"},
					"protected void test()\n{}"
				},
				new object[]
				{
					new Method() {Name = "test", Static = true},
					"private static void test()\n{}"
				},
				new object[]
				{
					new Method() {Name = "test", Access = "default", Static = true},
					"private static void test()\n{}"
				},
				new object[]
				{
					new Method() {Name = "test", Access = "protected", Static = true},
					"protected static void test()\n{}"
				},
				new object[]
				{
					new Method() {Name = "test",
						Parameters = new []
						{
							new Parameter {Type = "int", Name = "a"}
						}},
					"private void test(int a)\n{}"
				},
				new object[]
				{
					new Method() {Name = "test", Type = "int"},
					"private int test()\n{\n"+Indent+"return 0;\n}"
				},
				new object[]
				{
					new Method() {Name = "test", Access = "default", Type = "double"},
					"private double test()\n{\n"+Indent+"return 0.0;\n}"
				},
				new object[]
				{
					new Method() {Name = "test", Access = "protected", Type = "float"},
					"protected float test()\n{\n"+Indent+"return 0.0f;\n}"
				},
				new object[]
				{
					new Method() {Name = "test", Static = true, Type = "char"},
					"private static char test()\n{\n"+Indent+"return '';\n}"
				},
				new object[]
				{
					new Method() {Name = "test", Access = "default", Static = true, Type = "bool"},
					"private static bool test()\n{\n"+Indent+"return false;\n}"
				},
				new object[]
				{
					new Method() {Name = "test", Access = "protected", Static = true, Type = "string"},
					"protected static string test()\n{\n"+Indent+"return \"\";\n}"
				},
				new object[]
				{
					new Method() {Name = "test", Type = "int",
						Parameters = new []
						{
							new Parameter {Type = "int", Name = "a"}
						}},
					"private int test(int a)\n{\n"+Indent+"return 0;\n}"
				}
			};
		}
	}
}