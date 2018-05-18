using Xunit;
using CodeGen.generators;
using System.Collections.Generic;

namespace CodeGen.Test.generators
{
	public class GoGeneratorTest
	{
		private static Generator Gen { get; } = new GoGenerator();
		private static string Indent { get; } = Gen.GetIndent();
		
		[Theory]
		[MemberData(nameof(GoGeneratorTestData.FieldData), MemberType = typeof(GoGeneratorTestData))]
		public void TestGenerateField(Field field, string result)
		{
			Assert.Equal(Gen.GenerateField(field), result);
		}
		
		[Theory]
		[MemberData(nameof(GoGeneratorTestData.MethodData), MemberType = typeof(GoGeneratorTestData))]
		public void TestGenerateMethod(Method method, string result)
		{
			Assert.Equal(Gen.GenerateMethod(method), result);
		}
		
		private class GoGeneratorTestData
		{
			public static IEnumerable<object[]> FieldData => new List<object[]>
			{
				new object[]
				{
					new Field {Name = "name", Type = "int", Default = "1"},
					$"{Indent}name int"
				},
				new object[]
				{
					new Field {Name = "fieldName", Type = "string"},
					$"{Indent}fieldName string"
				}
			};
			
			public static IEnumerable<object[]> MethodData => new List<object[]>
			{
				new object[]
				{
					new Method {Name = "methodName", Access = "public", Type = "int"},
					$"MethodName() int {{\n{Indent}return nil\n}}"
				},
				new object[]
				{
					new Method {Name = "MethodName", Access = "private", Type = "string"},
					$"methodName() string {{\n{Indent}return nil\n}}"
				},
				new object[]
				{
					new Method
					{
						Name = "MethodName", Access = "private", Type = "string",
						Parameters = new []
						{
							new Parameter {Name = "param1", Type = "int", Default = "100"},
							new Parameter {Name = "param2", Type = "string"},
						}
					},
					$"methodName(param1 int, param2 string) string {{\n{Indent}return nil\n}}"
				}
			};
		}
	}
}
