using System;
using System.Collections.Generic;
using CodeGen.generators;
using Xunit;

namespace CodeGen.Test.generators
{
	public class JavaGeneratorTest
	{
		private static Generator Gen { get; } = new JavaGenerator();
		private static string Indent { get; } = Gen.GetIndent();

		[Theory]
		[MemberData(nameof(JavaGeneratorTestData.FieldThrowsData), MemberType = typeof(JavaGeneratorTestData))]
		public void TestGenerateFieldThrows(Field field)
		{
			Assert.Throws<ArgumentNullException>(() => Gen.GenerateField(field));
		}

		[Theory]
		[MemberData(nameof(JavaGeneratorTestData.FieldValidData), MemberType = typeof(JavaGeneratorTestData))]
		public void TestGenerateField(Field field, string output)
		{
			Assert.Equal(output, Gen.GenerateField(field));
		}

		[Theory]
		[MemberData(nameof(JavaGeneratorTestData.MethodThrowsData), MemberType = typeof(JavaGeneratorTestData))]
		public void TestGenerateMethodThrows(Method method)
		{
			Assert.Throws<ArgumentNullException>(() => Gen.GenerateMethod(method));
		}

		[Theory]
		[MemberData(nameof(JavaGeneratorTestData.MethodValidData), MemberType = typeof(JavaGeneratorTestData))]
		public void TestGenerateMethod(Method method, string output)
		{
			Assert.Equal(output, Gen.GenerateMethod(method));
		}

		private class JavaGeneratorTestData
		{
			public static IEnumerable<object[]> FieldThrowsData => new List<object[]>
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

			public static IEnumerable<object[]> FieldValidData => new List<object[]>
			{
				new object[]
				{
					new Field {Name = "test", Type = "int"},
					Indent + "private int test;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "String", Access = "public"},
					Indent + "public String test;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "String", Access = "public", Default = "\"test\""},
					Indent + "public String test = \"test\";"
				},
				new object[]
				{
					new Field {Name = "test", Type = "String", Static = true},
					Indent + "private static String test;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "String", Const = true},
					Indent + "private const String test;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "String", Static = true, Const = true},
					Indent + "private const static String test;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "String", Access = "protected", Static = true, Const = true},
					Indent + "protected const static String test;"
				},
			};

			public static IEnumerable<object[]> MethodThrowsData => new List<object[]>
			{
				
			};

			public static IEnumerable<object[]> MethodValidData => new List<object[]>
			{
				
			};
		}
	}
}