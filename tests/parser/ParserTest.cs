using Xunit;
using System.IO;
using CodeGen.parser;
using System.Collections.Generic;

namespace CodeGen.tests.parser
{
	public class ParserTest
	{
		[Fact]
		public void GetFileFormatTest()
		{
			Assert.Equal("xml", Parser.GetFileFormat("file.xml"));
			Assert.Throws<InvalidDataException>(() => Parser.GetFileFormat("filexml"));
			Assert.Throws<InvalidDataException>(() => Parser.GetFileFormat("filexml..."));
			Assert.Throws<InvalidDataException>(() => Parser.GetFileFormat("."));
		}

		[Fact]
		public void GetExtensionTest()
		{
			Assert.Equal(".java", Parser.GetExtension("java"));
			Assert.Equal(".go", Parser.GetExtension("go"));
			Assert.Equal(".rb", Parser.GetExtension("ruby"));
			Assert.Equal(".py", Parser.GetExtension("python"));
			Assert.Equal(".vb", Parser.GetExtension("vb"));
			Assert.Equal(".cs", Parser.GetExtension("csharp"));
			Assert.Throws<KeyNotFoundException>(() => Parser.GetExtension("somelang"));
		}

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