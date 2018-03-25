using System.Collections.Generic;
using System.IO;
using CodeGen.generators;
using Xunit;
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
  parent: ParentTestClass");
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
		
		
		/*   XML TEST   */
		[Theory]
		[MemberData(nameof(DeserializeXmlData.ThrowsData), MemberType = typeof(DeserializeXmlData))]
		public void TestDeserializeXmlThrowsError(string data)
		{
			Assert.Throws<InvalidDataException>(() => DeserializeXml(data));
		}

		[Theory]
		[MemberData(nameof(DeserializeXmlData.NullData), MemberType = typeof(DeserializeXmlData))]
		public void TestDeserializeXmlIsNull(string data)
		{
			Assert.Null(DeserializeXml(data).Packages);
		}

		[Theory]
		[MemberData(nameof(DeserializeXmlData.NotNullData), MemberType = typeof(DeserializeXmlData))]
		public void TestDeserializeXmlIsNotNull(string data)
		{
			Assert.NotNull(DeserializeXml(data));
		}
		
		[Fact]
		public void TestDeserializeXml()
		{
			var pak = DeserializeXml(@"
<Package>
	<name>test</name>
</Package>");
			Assert.Equal("test", pak.Name);
			Assert.False(pak.UseSpaces);

			Assert.Null(pak.Classes);

			pak = DeserializeXml(@"
<Package>
	<name>test</name>
	<classes>
		<class>
			<name>TestClass</name>
		</class>
	</classes>
</Package>");
			Assert.NotNull(pak.Classes);
//			Assert.NotEmpty(pak.Classes);

			pak = DeserializeXml(@"
<Package>
	<name>test</name>
	<classes>
		<class>
			<name>TestClass</name>
			<parent>ParentTestClass</parent>
		</class>
	</classes>
</Package>");
//			Assert.NotEmpty(pak.Classes);
//			Assert.Single(pak.Classes);
//			Assert.NotNull(pak.Classes[0]);
//			Assert.Equal("TestClass", pak.Classes[0].Name);
//			Assert.Equal("ParentTestClass", pak.Classes[0].Parent);
		}
		
		private class DeserializeXmlData
		{
			public static IEnumerable<object[]> ThrowsData => new List<object[]>
			{
				new object[] {"error"},
				new object[] {"<>"},
				new object[] {"<classes><class></classes>"},
				new object[] {"<classes></method>"},
				new object[] {@"<classe></classes>"},
				new object[] {@"<classe></classe>"},
			};

			public static IEnumerable<object[]> NullData => new List<object[]>
			{
				new object[] {"<Package></Package>"},
				new object[] {"<Package>null</Package>"},
			};

			public static IEnumerable<object[]> NotNullData => new List<object[]>
			{
				new object[] {"<Package><name>test</name></Package>"},
			};
		}	
		
		
		/* Parse file by format test */

		[Theory]
		[MemberData(nameof(ParseFileByFormatTestData.ThrowsData), MemberType = typeof(ParseFileByFormatTestData))]
		public void ParseFileByFoematTestExceptionThrowing(string body, string fileName)
		{
			Assert.Throws<InvalidDataException>(() => ParseFileByFormat(body, fileName));
		}
		
		[Theory]
		[MemberData(nameof(ParseFileByFormatTestData.NotNullData), MemberType = typeof(ParseFileByFormatTestData))]
		public void ParseFileByFormatTest(string expected, string body, string fileName)
		{
			var pkg = ParseFileByFormat(body, fileName);
			Assert.Equal(expected, pkg.Name);
		}
		
		private class ParseFileByFormatTestData
		{
			public static IEnumerable<object[]> NotNullData => new List<object[]>
			{
				new object[] {"testXml", "<Package><name>testXml</name></Package>", "Test.xml"},
				new object[] {"testJson", @"{""name"": ""testJson""}", "Test.json"},
				new object[] {"testYaml", "name: testYaml", "Test.yml"},
			};
			
			public static IEnumerable<object[]> ThrowsData => new List<object[]>
			{
				new object[] {"<Package><name>testXml</name></Package>", "Test.ml"},
				new object[] {@"{""name"": ""testJson""}", "json."},
				new object[] {"name: testYaml", "Testyml"},
			};
		}
	}
}
