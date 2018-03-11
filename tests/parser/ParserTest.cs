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
			Assert.Throws<InvalidDataException>(() => Parser.ValidateArgs("", "file.xml"));
			Assert.Throws<InvalidDataException>(() => Parser.ValidateArgs("python", ""));
			Parser.ValidateArgs("python", "file.xml");
		}
	}
}