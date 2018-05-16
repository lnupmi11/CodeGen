using Xunit;
using CodeGen.generators;
using System.Collections.Generic;

namespace CodeGen.Test.generators
{
	public class PythonGeneratorTest
	{
		private static readonly PythonGenerator Gen = new PythonGenerator();
		private static string Indent { get; } = Gen.GetIndent();
		
		[Theory]
		[MemberData(nameof(PythonGeneratorTestData.FieldData), MemberType = typeof(PythonGeneratorTestData))]
		public void TestGenerateField(Field field, string result)
		{
			Assert.Equal(Gen.GenerateField(field), result);
		}
		
		[Theory]
		[MemberData(nameof(PythonGeneratorTestData.MethodData), MemberType = typeof(PythonGeneratorTestData))]
		public void TestGenerateMethod(Method method, string result)
		{
			Assert.Equal(Gen.GenerateMethod(method), result);
		}
		
		[Theory]
		[MemberData(nameof(PythonGeneratorTestData.GenerateInitData), MemberType = typeof(PythonGeneratorTestData))]
		public void TestGenerateInit(Class @class, string result)
		{
			Assert.Equal(Gen.GenerateInit(@class), result);
		}
		
		private class PythonGeneratorTestData
		{
			public static IEnumerable<object[]> FieldData => new List<object[]>
			{
				new object[]
				{
					new Field {Name = "name", Type = "int", Default = "1"},
					$"self.name"
				},
				new object[]
				{
					new Field {Name = "fieldName"},
					$"self.fieldName"
				}
			};
			
			public static IEnumerable<object[]> MethodData => new List<object[]>
			{
				new object[]
				{
					new Method {Name = "methodName", Access = "public", Type = "int", Static = true},
					$"@staticmethod\ndef methodName():\n{Indent}pass\n\t"
				},
				new object[]
				{
					new Method
					{
						Name = "MethodName", Access = "private", Type = "string",
						Parameters = new []
						{
							new Parameter {Name = "param1", Default = "1"}
						}
					},
					$"def __MethodName(self, param1=1):\n{Indent}pass\n\t"
				},
				new object[]
				{
					new Method
					{
						Name = "MethodName", Access = "protected", Type = "string",
						Parameters = new []
						{
							new Parameter {Name = "param1"},
							new Parameter {Name = "param2", Default = "100"}
						}
					},
					$"def _MethodName(self, param1, param2=100):\n{Indent}pass\n\t"
				}
			};
			
			public static IEnumerable<object[]> GenerateInitData => new List<object[]>
			{
				new object[]
				{
					new Class {Fields = new []
					{
						new Field {Name = "field1", Default = "", Access = "private"},
						new Field {Name = "field2", Default = "1.5", Access = "protected"},
						new Field {Name = "field3", Default = "None", Access = "public"},
					}},
					$"def __init__(self, field1, field2=1.5, field3=None):\n{Indent}self.__field1 = field1\n{Indent}self._field2 = field2\n{Indent}self.field3 = field3"
				},
				new object[]
				{
					new Class {},
					$"def __init__(self):\n{Indent}pass\n\t"
				}
			};
		}
	}
}
