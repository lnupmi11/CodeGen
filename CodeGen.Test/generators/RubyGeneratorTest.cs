using Xunit;
using CodeGen.generators;
using System.Collections.Generic;

namespace CodeGen.Test.generators
{
	public class RubyGeneratorTest
	{
		private static Generator Gen { get; } = new RubyGenerator();
		private static string Indent { get; } = Gen.GetIndent();

		[Theory]
		[MemberData(nameof(RubyGeneratorTestData.FieldData), MemberType = typeof(RubyGeneratorTestData))]
		public void TestGenerateField(Field field, string result)
		{
			Assert.Equal(Gen.GenerateField(field), result);
		}

		[Theory]
		[MemberData(nameof(RubyGeneratorTestData.MethodData), MemberType = typeof(RubyGeneratorTestData))]
		public void TestGenerateMethod(Method method, string result)
		{
			Assert.Equal(Gen.GenerateMethod(method), result);
		}

		private class RubyGeneratorTestData
		{
			public static IEnumerable<object[]> FieldData => new List<object[]>
			{
				new object[]
				{
					new Field {Name = "name", Type = "int", Default = "1"},
					$"{Indent}"
				},
				new object[]
				{
					new Field {Name = "fieldName", Type = "string"},
					$"{Indent}"
				}
			};

			public static IEnumerable<object[]> MethodData => new List<object[]>
			{
				new object[]
				{
					new Method {Name = "methodName", Access = "public", Type = "int"},
					$"def methodName\nend"
				},
				new object[]
				{
					new Method {Name = "MethodName", Access = "private", Type = "string"},
					$"private def MethodName\nend"
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
					$"private def MethodName(param1=100, param2)\nend"
				}
			};
		}
	}
}
