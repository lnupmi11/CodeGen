using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using CodeGen.generators;
using Xunit;
using Xunit.Sdk;
using static CodeGen.ExecuteConf;

namespace CodeGen.Test
{
	public class ExecuteTest
	{
		
		/*   JSON TEST   */
		[Theory]
		[MemberData(nameof(DeserializeJsonData.ThrowsData), MemberType = typeof(DeserializeJsonData))]
		public void TestDeserializeJsonThrowsError(string data)
		{
			Assert.Throws<InvalidDataException>(() => DeserializeJson(data));
		}

		[Theory]
		[MemberData(nameof(DeserializeJsonData.NullData), MemberType = typeof(DeserializeJsonData))]
		public void TestDeserializeJsonIsNull(string data)
		{
			Assert.Null(DeserializeJson(data));
		}

		[Theory]
		[MemberData(nameof(DeserializeJsonData.NotNullData), MemberType = typeof(DeserializeJsonData))]
		public void TestDeserializeJsonIsNotNull(string data)
		{
			Assert.NotNull(DeserializeJson(data));
		}
		
		[Fact]
		public void TestDeserializeJson()
		{
			// valid data
			var pak = DeserializeJson(@"
            {
				""name"": ""test""
			}
            ");
			Assert.Equal("test", pak.Name);
			Assert.False(pak.UseSpaces);

			pak = DeserializeJson(@"
            {
				""useSpaces"": true
			}
            ");
			Assert.True(pak.UseSpaces);
			Assert.Null(pak.Classes);

			pak = DeserializeJson(@"
            {
				""useSpaces"": true,
				""useSpaces"": false
			}
            ");
			Assert.False(pak.UseSpaces);

			
			pak = DeserializeJson(@"
            {
				""useSpaces"": 6
			}
            ");
			Assert.True(pak.UseSpaces);
			
			
			pak = DeserializeJson(@"
            {
				""classes"": []
			}
            ");
			Assert.NotNull(pak.Classes);
			Assert.Empty(pak.Classes);

			pak = DeserializeJson(@"
            {
				""classes"": [
					{
						""name"": ""TestClass"",
						""parent"": ""ParentTestClass""
					}
				]
			}
            ");
			Assert.NotEmpty(pak.Classes);
			Assert.Single(pak.Classes);
			Assert.NotNull(pak.Classes[0]);
			Assert.Equal("TestClass", pak.Classes[0].Name);
			Assert.Equal("ParentTestClass", pak.Classes[0].Parent);
			
			
			/*
			// valid file
			const string testFileName = "../../../testAssets/ExecuteTestAssets/Package.json";
			Assert.True(File.Exists(testFileName), $"File {testFileName} doesn't exist in {Directory.GetCurrentDirectory()}");
			var testData = File.ReadAllText(testFileName);
			pak = DeserializeJson(testData);
			Assert.Equal("Fruit", pak.Classes[0].Name);
			*/
		}

		private class DeserializeJsonData
		{
			public static IEnumerable<object[]> ThrowsData => new List<object[]>
			{
				new object[] {"error"},
				new object[] {"{"},
				new object[] {"{{}"},
				new object[] {"{]"},
				new object[] {@"
            
				""classe"": [
				]
			}
            "},
				new object[] {@"
            {
				""classe"": 
				]
			}
            "},
			};

			public static IEnumerable<object[]> NullData => new List<object[]>
			{
				new object[] {""},
				new object[] {"null"},
			};

			public static IEnumerable<object[]> NotNullData => new List<object[]>
			{
				new object[] {"{}"},
				new object[] {"{\"name\":\"test\"}"},
			};
			
//			public static IEnumerable<object[]> IsValidData => new List<object[]>
//			{
//				new object[] {new Package(), "{}"},
//				new object[] {new Package{Name = "test"}, "{\"name\":\"test\"}"},
//			};
		}

		
		
		/*   YAML TEST   */
		[Theory]
		[MemberData(nameof(DeserializeYamlData.ThrowsData), MemberType = typeof(DeserializeYamlData))]
		public void TestDeserializeYamlThrowsError(string data)
		{
			Assert.Throws<InvalidDataException>(() => DeserializeYaml(data));
		}

		[Theory]
		[MemberData(nameof(DeserializeYamlData.NullData), MemberType = typeof(DeserializeYamlData))]
		public void TestDeserializeYamlIsNull(string data)
		{
			Assert.Null(DeserializeYaml(data));
		}

		[Theory]
		[MemberData(nameof(DeserializeYamlData.NotNullData), MemberType = typeof(DeserializeYamlData))]
		public void TestDeserializeYamlIsNotNull(string data)
		{
			Assert.NotNull(DeserializeYaml(data));
		}
		
		[Fact]
		public void TestDeserializeYaml()
		{
			var pak = DeserializeYaml(@"name: test");
			Assert.Equal("test", pak.Name);
			Assert.False(pak.UseSpaces);

			Assert.Null(pak.Classes);

			pak = DeserializeYaml(@"
name: test
classes: 
- name: TestClass");
			Assert.NotNull(pak.Classes);
			Assert.NotEmpty(pak.Classes);

			pak = DeserializeYaml(@"
name: test
classes:
- name: TestClass
  parent: ParentTestClass
            ");
			Assert.NotEmpty(pak.Classes);
			Assert.Single(pak.Classes);
			Assert.NotNull(pak.Classes[0]);
			Assert.Equal("TestClass", pak.Classes[0].Name);
			Assert.Equal("ParentTestClass", pak.Classes[0].Parent);
		}
		
		private class DeserializeYamlData
		{
			public static IEnumerable<object[]> ThrowsData => new List<object[]>
			{
				new object[] {"error"},
				new object[] {"error: test"},
				new object[] {"{"},
				new object[] {"{{}"},
				new object[] {"{]"},
			};

			public static IEnumerable<object[]> NullData => new List<object[]>
			{
				new object[] {""},
				new object[] {"null"},
			};

			public static IEnumerable<object[]> NotNullData => new List<object[]>
			{
				new object[] {"name: test"},
			};
		}
		
	}
}