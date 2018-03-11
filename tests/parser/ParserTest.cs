using Xunit;
using System.IO;
using CodeGen.parser;
using System.Collections.Generic;

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
			Assert.Equal("xml", Parser.GetFileFormat("file.xml"));
			Assert.Throws<InvalidDataException>(() => Parser.GetFileFormat("filexml"));
			Assert.Throws<InvalidDataException>(() => Parser.GetFileFormat("filexml..."));
			Assert.Throws<InvalidDataException>(() => Parser.GetFileFormat("."));
		}

		/// <summary>
		/// 
		/// </summary>
		[Fact]
		public void ValidateArgsTest()
		{
			Assert.Throws<InvalidDataException>(() => Parser.ValidateArgs("", "file.xml", "http://url.com"));
			Assert.Throws<InvalidDataException>(() => Parser.ValidateArgs("python", "file.xml", "http://url.com"));
			Assert.Throws<InvalidDataException>(() => Parser.ValidateArgs("go", "", ""));
			Parser.ValidateArgs("python", "file.xml", "");
			Parser.ValidateArgs("python", "", "http://url.com");
		}
	}
}