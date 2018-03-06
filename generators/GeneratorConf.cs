using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	/// <summary>
	/// Interface of language generator
	/// </summary>
	public abstract class Generator
	{
		/// <summary>
		/// Package generator: generates package with classes and subpackages from given package object
		/// </summary>
		/// <param name="pkg">Package object</param>
		/// <returns>Dictionary of file names (keys) and generated code (values)</returns>
		public abstract Dictionary<string, string> Generate(Package pkg);
		
		/// <summary>
		///	Class generator: generates class with fields, methods and subclasses from given class object
		/// </summary>
		/// <param name="class">Class object</param>
		/// <returns>String of generated code of class</returns>
		protected abstract string GenerateClass(Class @class);
		
		/// <summary>
		/// Field generator: generates field from given field object
		/// </summary>
		/// <param name="field">Field object</param>
		/// <returns>String of generated code of field</returns>
		protected abstract string GenerateField(Field field);
		
		/// <summary>
		/// Method generator: generates method from given method object
		/// </summary>
		/// <param name="method">Method object</param>
		/// <returns>String of generated code of method</returns>
		protected abstract string GenerateMethod(Method method);
	}

	/// <summary>
	/// Interface of language normalizer: normalizes package data according to specified language
	/// </summary>
	public abstract class Normalizer
	{
		/// <summary>
		/// Package normalizer: normalizes package with classes and subpackages
		/// </summary>
		/// <param name="pkg">Package object</param>
		/// <returns>Normalized package object</returns>
		public abstract Package NormalizePackage(Package pkg);
		
		/// <summary>
		/// Class normalizer: normalizes class with fields, methods and subclasses
		/// </summary>
		/// <param name="class">Class object</param>
		/// <returns>Normalized class object</returns>
		protected abstract Class NormalizeClass(Class @class);
		
		/// <summary>
		/// Field normalizer: normalizes field
		/// </summary>
		/// <param name="field">Field object</param>
		/// <returns>Normalized field object</returns>
		protected abstract Field NormalizeField(Field field);
		
		/// <summary>
		/// Method normalizer: normalizes method
		/// </summary>
		/// <param name="method">Method object</param>
		/// <returns>Normalized method object</returns>
		protected abstract Method NormalizeMethod(Method method);
		
		/// <summary>
		/// Parameter normalizer: normalizes parameter
		/// </summary>
		/// <param name="parameter">Parameter object</param>
		/// <returns>Normalized parameter object</returns>
		protected abstract Parameter NormalizeParameter(Parameter parameter);
	}

	/// <summary>
	/// The class that describes programming language and has a generator for it
	/// </summary>
	public struct Languange
	{
		/// <summary>
		/// Holds the generator of the language. Field is read only
		/// </summary>
		public readonly Generator Generator;
		
		/// <summary>
		/// Holds the extension of the file. Field is read only
		/// </summary>
		public readonly string Extension;
		
		/// <summary>
		/// Holds comment format. Field is read only
		/// </summary>
		public readonly string Comment;
		
		/// <summary>
		/// Holds language normalizer. Field is read only
		/// </summary>
		private readonly Normalizer _normalizer;

		/// <summary>
		/// Constructor for language, used to avoid struct initializers
		/// </summary>
		/// <param name="generator">Language generator</param>
		/// <param name="extension">File extension</param>
		/// <param name="comment">Comment format</param>
		/// <param name="normalizer">Language normalizer</param>
		public Languange(Generator generator, string extension = "", string comment = "", Normalizer normalizer = null)
		{
			Generator = generator;
			Extension = extension;
			Comment = comment;
			_normalizer = normalizer;
		}
	}

	/// <summary>
	/// Holds the configuration of generator
	/// </summary>
	public static class GeneratorConf
	{
		/// <summary>
		/// Contains example package
		/// </summary>
		public static Package ExamplePkg = new Package
		{
			Name = "main",
			UseSpaces = true,
			Classes = new[]
			{
				new Class {Name = "Fruit"},
				new Class
				{
					Name = "Apple",
					Parent = "Fruit",
					Fields = new[]
					{
						new Field
						{
							Access = "public",
							Type = "string",
							Name = "colour",
							Default = "\"red\""
						},
						new Field
						{
							Access = "public",
							Type = "string",
							Static = true,
							Name = "sort",
							Default = "\"Golden\""
						},
						new Field
						{
							Access = "private",
							Type = "int",
							Name = "size",
							Default = "1"
						}
					},
					Methods = new[]
					{
						new Method
						{
							Access = "private",
							Return = "",
							Name = "print",
							Parameters = new[]
							{
								new Parameter
								{
									Name = "colour",
									Type = "string"
								}
							}
						},
						new Method
						{
							Access = "protected",
							Return = "int",
							Static = true,
							Name = "getSize"
						},
						new Method
						{
							Access = "public",
							Return = "string",
							Name = "getColor",
							Const = true
						}
					},
					Classes = new[]
					{
						new Class
						{
							Access = "private",
							Name = "Seed",
							Fields = new[]
							{
								new Field
								{
									Access = "public",
									Type = "int",
									Name = "size"
								}
							},
							Methods = new[]
							{
								new Method
								{
									Static = true,
									Access = "public",
									Return = "int",
									Name = "transform",
									Const = true
								}
							}
						}
					}
				},
			}
		};

		/// <summary>
		/// Dictionary of language names (keys) and its Language objects (values) 
		/// </summary>
		public static readonly Dictionary<string, Languange> Languanges = new Dictionary<string, Languange>
		{
			{"java", new Languange(new JavaGenerator(), "java", "/* {0} */")},
			{"go", new Languange(new GoGenerator(), "go", "/* {0} */")},
			{"ruby", new Languange(new RubyGenerator(), "rb", "# {0}")},
			{"python", new Languange(new PythonGenerator(), "py", "# {0}\n")},
			{"vb", new Languange(new VbGenerator(), "vb", "' {0}\n")},
			{"csharp", new Languange(new CSharpGenerator(), "cs","/* {0} */")},
			{"js_es6", new Languange(new JSes6Generator(), "js","/* {0} */")},
		};
		
		/// <summary>
		/// Creates indent using given parameters
		/// </summary>
		/// <param name="tabs">Use tabs or spaces</param>
		/// <param name="tabStop">Number of spaces</param>
		/// <returns>Indent string</returns>
		public static string GetIndent(bool tabs, int tabStop)
		{
			return tabs ? "\t" : new string(' ', tabStop);
		}

		/// <summary>
		/// Shifts code using given parameters
		/// </summary>
		/// <param name="code">Code to be shifted</param>
		/// <param name="num">Number of indents</param>
		/// <param name="indent">Indent string</param>
		/// <returns>Shifted code</returns>
		public static string ShiftCode(string code, int num, string indent)
		{
			indent = string.Concat(Enumerable.Repeat(indent, num));
			return indent + code.Replace("\n", "\n" + indent);
		}

		/// <summary>
		/// Converts given language into language which is used for identification of generator
		/// </summary>
		/// <param name="lang">Inputed language</param>
		/// <returns>Normalized language</returns>
		public static string NormalizeLang(string lang)
		{
			if (lang == "js")
			{
				lang = "js_es6";
			}
			else if (lang == "c#" || lang == "cs")
			{
				lang = "csharp";
			}
			else if (lang == "yml")
			{
				lang = "yaml";
			}
			else if (lang == "c++")
			{
				lang = "cpp";
			}

			return lang;
		}

		/// <summary>
		/// Creates generator if it exists, else throws an error
		/// </summary>
		/// <param name="name">name of a language</param>
		/// <returns>language</returns>
		/// <exception cref="IndexOutOfRangeException">If the language is not found</exception>
		public static Languange GetLanguage(string name)
		{
			name = NormalizeLang(name);
			Languange lang;
			try
			{
				lang = Languanges[name];
			}
			catch (Exception)
			{
				throw new IndexOutOfRangeException("this language doesn't exist");
			}

			return lang;
		}
	}
}
