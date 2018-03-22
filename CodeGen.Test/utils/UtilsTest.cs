using System.Collections.Generic;
using Xunit;
using System.IO;
using CodeGen.utils;

namespace CodeGen.Test.utils
{
	public class UtilsTest
	{
		[Fact]
		public void ReadTest()
		{
			var testvar = TestConf.BaseDir + "/CodeGen/examples/Package.json";
			Assert.Equal(File.ReadAllText(testvar), Utils.Read(testvar));
			Assert.Throws<FileNotFoundException>(() => Utils.Read("test"));
		}

		[Fact]
		public void DownloadTest()
		{
			Assert.Throws<System.Net.WebException>(() => Utils.Download("testFailed"));
		}

		[Fact]
		public void GetFileFormatTest()
		{
			Assert.Equal("xml", Utils.GetFileFormat("file.xml"));
			Assert.Throws<InvalidDataException>(() => Utils.GetFileFormat("filexml"));
			Assert.Throws<InvalidDataException>(() => Utils.GetFileFormat("filexml..."));
			Assert.Throws<InvalidDataException>(() => Utils.GetFileFormat("."));
		}

		[Fact]
		public void ValidateArgsTest()
		{
			Assert.Throws<InvalidDataException>(() => Utils.ValidateArgs("", "file.xml"));
			Assert.Throws<InvalidDataException>(() => Utils.ValidateArgs("python", ""));
			Utils.ValidateArgs("python", "file.xml");
		}

		[Theory]
		[MemberData(nameof(TitleTestData.TitleData), MemberType = typeof(TitleTestData))]
		public void TitleTest(string source, string expected)
		{
			Assert.Equal(expected, Utils.Title(source));
		}

		private class TitleTestData
		{
			public static IEnumerable<object[]> TitleData => new List<object[]>
			{
				new object[] {"hello", "Hello"},
				new object[] {"t", "T"},
			};
		}
	}
}