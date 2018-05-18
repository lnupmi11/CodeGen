using Xunit;
using CodeGen.generators;
using System.Collections.Generic;

namespace CodeGen.Test.generators
{
	public class VbGeneratorTest
	{
		private static Languange Lang { get; } = GeneratorConf.GetLanguage("vb");
		private static string Indent { get; } = Lang.Generator.GetIndent();

		[Theory]
		[MemberData(nameof(GoGeneratorTestData.FieldData), MemberType = typeof(GoGeneratorTestData))]
		public void TestGenerateField(Package pkg, string result)
		{
			Lang.Normalizer.NormalizePackage(ref pkg);
			Assert.Equal(Lang.Generator.GenerateField(pkg.Classes[0].Fields[0]), result);
		}

		[Theory]
		[MemberData(nameof(GoGeneratorTestData.MethodData), MemberType = typeof(GoGeneratorTestData))]
		public void TestGenerateMethod(Package pkg, string result)
		{
			Lang.Normalizer.NormalizePackage(ref pkg);
			Assert.Equal(Lang.Generator.GenerateMethod(pkg.Classes[0].Methods[0]), result);
		}

		private class GoGeneratorTestData
		{
			public static IEnumerable<object[]> FieldData => new List<object[]>
			{
				new object[]
				{
					new Package
					{
						Classes = new []
						{
							new Class
							{
								Fields = new []
								{
									new Field {Name = "name", Type = "int", Default = "1", Access = "private"}
								}
							}
						}
					},
					"\tPrivate name As Integer = 1"
				},
				new object[]
				{
					new Package
					{
						Classes = new []
						{
							new Class
							{
								Fields = new []
								{
									new Field {Name = "fieldName", Type = "string"},
								}
							}
						}
					},
					"\tPublic fieldName As String"
				}
			};

			public static IEnumerable<object[]> MethodData => new List<object[]>
			{
				new object[]
				{
					new Package
					{
						Classes = new []
						{
							new Class
							{
								Methods = new []
								{
									new Method {Name = "methodName", Access = "public", Type = "int"}	
								}
							}
						}
					},
					$"Public Function methodName() As Integer\n{Indent}Return 0\nEnd Function"
				},
				new object[]
				{
					new Package
					{
						Classes = new []
						{
							new Class
							{
								Methods = new []
								{
									new Method {Name = "MethodName", Access = "private", Type = "string"}
								}
							}
						}
					},
					$"Private Function MethodName() As String\n{Indent}Return 0\nEnd Function"
				},
				new object[]
				{
					new Package
					{
						Classes = new []
						{
							new Class
							{
								Methods = new []
								{
									new Method
									{
										Name = "MethodName", Access = "private",
										Parameters = new []
										{
											new Parameter {Name = "param1", Type = "int", Default = "100"},
											new Parameter {Name = "param2", Type = "string"},
										}
									}
								}
							}
						}
					},
					$"Private Sub MethodName(Optional param1 As Integer = 100, ByVal param2 As String)\n{Indent}Return\nEnd Sub"
				}
			};
		}
	}
}
