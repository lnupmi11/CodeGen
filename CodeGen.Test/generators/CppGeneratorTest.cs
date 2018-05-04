using System;
using System.Collections.Generic;
using CodeGen.generators;
using Xunit;

namespace CodeGen.Test.generators
{
	public class CppGeneratorTest
	{
		private static Generator Gen { get; } = new CppGenerator();
		private static string Indent { get; } = Gen.GetIndent();

		[Theory]
		[MemberData(nameof(CppGeneratorTestData.FieldThrowsData), MemberType = typeof(CppGeneratorTestData))]
		public void TestGenerateFieldThrows(Field field)
		{
			Assert.Throws<ArgumentNullException>(() => Gen.GenerateField(field));
		}

		[Theory]
		[MemberData(nameof(CppGeneratorTestData.FieldValidData), MemberType = typeof(CppGeneratorTestData))]
		public void TestGenerateField(Field field, string output)
		{
			Assert.Equal(output, Gen.GenerateField(field));
		}

//		[Theory]
//		[MemberData(nameof(CppGeneratorTestData.MethodThrowsData), MemberType = typeof(CppGeneratorTestData))]
//		public void TestGenerateMethodThrows(Method method)
//		{
//			Assert.Throws<ArgumentNullException>(() => Gen.GenerateMethod(method));
//		}

//		[Theory]
//		[MemberData(nameof(CppGeneratorTestData.MethodValidData), MemberType = typeof(CppGeneratorTestData))]
//		public void TestGenerateMethod(Method method, string output)
//		{
//			Assert.Equal(output, Gen.GenerateMethod(method));
//		}

		private class CppGeneratorTestData
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
					Indent + "int test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "public"},
					Indent + "string test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "public", Default = "\"test\""},
					Indent + "string test = \"test\";"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Static = true},
					Indent + "static string test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Const = true},
					Indent + "const string test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Static = true, Const = true},
					Indent + "static const string test = ;"
				},
				new object[]
				{
					new Field {Name = "test", Type = "string", Access = "protected", Static = true, Const = true},
					Indent + "static const string test = ;"
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