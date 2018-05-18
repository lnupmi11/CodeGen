using System;
using System.Collections.Generic;
using CodeGen.generators;
using Xunit;

namespace CodeGen.Test.generators
{
	public class Es6GeneratorTest
	{
		private static Generator Gen { get; } = new Es6Generator();
		private static string Indent { get; } = Gen.GetIndent();

		[Theory]
		[MemberData(nameof(Es6GeneratorTestData.FieldValidData), MemberType = typeof(Es6GeneratorTestData))]
		public void TestGenerateField(Field field, string output)
		{
			Assert.Equal(output, Gen.GenerateField(field));
		}

		[Theory]
		[MemberData(nameof(Es6GeneratorTestData.MethodData), MemberType = typeof(Es6GeneratorTestData))]
		public void TestGenerateMethod(Method method, string result)
		{
			Assert.Equal(Gen.GenerateMethod(method), result);
		}


		private class Es6GeneratorTestData
		{
			public static IEnumerable<object[]> FieldValidData => new List<object[]>
			{
				new object[]
				{
					new Field {Name = "test", Type = "int"},
					$"{Indent}int test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "public"},
					$"{Indent}string test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "public", Default = "\"test\""},
					$"{Indent}string test = \"test\";"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Static = true},
					$"{Indent}string test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Const = true},
					$"{Indent}string test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Static = true, Const = true},
					$"{Indent}string test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "private", Static = true, Const = true},
					$"{Indent}string test = ;"
				},
			};

			public static IEnumerable<object[]> MethodData => new List<object[]>
			{
				new object[]
				{
					new Method {Name = "methodName", Access = "public", Type = "int"},
					$"methodName() {{\n\treturn null;\n}}"
				},
				new object[]
				{
					new Method {Name = "MethodName", Access = "private", Type = "string"},
					$"MethodName() {{\n\treturn null;\n}}"
				},
				new object[]
				{
					new Method
					{
						Name = "MethodName",
						Access = "private",
						Type = "string",
						Parameters = new[]
						{
							new Parameter {Name = "param1", Type = "int", Default = "100"},
							new Parameter {Name = "param2", Type = "string"},
						}
					},
					$"MethodName(param1=100, param2) {{\n\treturn null;\n}}"
				}
			};
		}
	}
}