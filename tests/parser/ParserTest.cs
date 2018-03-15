using Xunit;
using System.IO;
using CodeGen.utils;

namespace CodeGen.tests.parser
{
	/// <summary>
	/// 
	/// </summary>
	public class ParserTest
	{
		/// <summary>
		/// 
		/// </summary>
		[Fact]
		public void GetFileFormatTest()
		{
			Assert.Equal("xml", Utils.GetFileFormat("file.xml"));
			Assert.Throws<InvalidDataException>(() => Utils.GetFileFormat("filexml"));
			Assert.Throws<InvalidDataException>(() => Utils.GetFileFormat("filexml..."));
			Assert.Throws<InvalidDataException>(() => Utils.GetFileFormat("."));
		}

		/// <summary>
		/// 
		/// </summary>
		[Fact]
		public void ValidateArgsTest()
		{
			Assert.Throws<InvalidDataException>(() => Utils.ValidateArgs("", "file.xml"));
			Assert.Throws<InvalidDataException>(() => Utils.ValidateArgs("python", ""));
			Utils.ValidateArgs("python", "file.xml");
		}
	}
}