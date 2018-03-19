using System.IO;
using Xunit;
using static CodeGen.ExecuteConf;

namespace CodeGen.Test
{
	public class ExecuteTest
	{
		[Theory]
		[InlineData("error")]
		[InlineData("will not work")]
		public void TestDeserializeJsonThrowsError(string data)
		{
			Assert.Throws<InvalidDataException>(() => DeserializeJson(data));
		}

		[Theory]
		[InlineData("")]
		[InlineData("null")]
		public void TestDeserializeJsonIsNull(string data)
		{
			Assert.Null(DeserializeJson(data));
		}
		
		[Theory]
		[InlineData("{}")]
		[InlineData("{ \"name\":\"test\"}")]
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
			Assert.Equal(1, pak.Classes.Length);
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

		[Fact]
		public void TestDeserializeYaml()
		{
		}
	}
}