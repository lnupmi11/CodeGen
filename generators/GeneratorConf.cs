using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	public abstract class Generator
	{
		public abstract Dictionary<string, string> Generate(Package pkg);
		protected abstract string GenerateClass(Class @class);
		protected abstract string GenerateField(Field field);
		protected abstract string GenerateMethod(Method method);
	}

	public abstract class Normalizer
	{
		public abstract Package NormalizePackage(Package pkg);
		protected abstract Class NormalizeClass(Class @class);
		protected abstract Field NormalizeField(Field field);
		protected abstract Method NormalizeMethod(Method method);
		protected abstract Parameter NormalizeParameter(Parameter parameter);
	}

	public struct Languange
	{
		public Generator Generator;
		public string Extension;
		public string Comment;

		public Languange(Generator generator, string extension = "", string comment = "")
		{
			Generator = generator;
			Extension = extension;
			Comment = comment;
		}
	}

	public static class GeneratorConf
	{
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

		public static readonly Dictionary<string, Languange> Languanges = new Dictionary<string, Languange>
		{
			{"java", new Languange(new JavaGenerator(), "java", "/* {0} */")},
			{"go", new Languange(new GoGenerator(), "go", "/* {0} */")},
			{"ruby", new Languange(new RubyGenerator(), "rb", "# {0}")},
			{"python", new Languange(new PythonGenerator(), "py", "# {0}\n")},
			{"vb", new Languange(new VBGenerator(), "vb", "' {0}\n")},
			{"csharp", new Languange(new CSharpGenerator(), "cs","/* {0} */")},
		};

		public static string GetIndent(bool tabs, int tabStop)
		{
			return tabs ? "\t" : new string(' ', tabStop);
		}

		public static string ShiftCode(string code, int num, string indent)
		{
			indent = string.Concat(Enumerable.Repeat(indent, num));
			return indent + code.Replace("\n", "\n" + indent);
		}

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

		public static Generator GetGenerator(string name)
		{
			name = NormalizeLang(name);
			Languange gen;
			try
			{
				gen = Languanges[name];
			}
			catch (Exception)
			{
				throw new IndexOutOfRangeException("this generator doesn't exist");
			}

			return gen.Generator;
		}
	}
}
