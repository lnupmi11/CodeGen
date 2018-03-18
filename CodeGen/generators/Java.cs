using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	/// <inheritdoc />
	/// <summary>
	/// Generator for Java
	/// </summary>
	public class JavaGenerator : Generator
	{
		private const string ClassFormat = "{0}class {1} {2}{{{3}{4}{5}}}";
		private string Indent { get; set; } = GeneratorConf.GetIndent(true, 4);

		/// <inheritdoc />
		protected override string GenerateClass(Class @class)
		{
			string access = "", fields = "", inherits = "", methods = "", classes = "";

			if (@class.Parent?.Length > 0)
			{
				inherits = "extends " + @class.Parent + " ";
			}

			fields = @class.Fields?.Aggregate('\n' + fields,
				(current, field) => current + GenerateField(field) + '\n');

			methods = @class.Methods?.Aggregate(methods,
				(current, method) => current + '\n' + GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + '\n');

			classes = @class.Classes?.Aggregate(classes,
				(current, cls) => current + '\n' + GeneratorConf.ShiftCode(GenerateClass(cls), 1, Indent) + '\n');

			if (@class.Access?.Length > 0)
			{
				access = @class.Access + ' ';
			}

			return string.Format(ClassFormat, access, @class.Name, inherits, fields, methods, classes);
		}

		/// <inheritdoc />
		protected override string GenerateField(Field field)
		{
			var result = Indent;
			if (field.Access == "" || field.Access == "default")
			{
				result += "private ";
			}
			else
			{
				result += field.Access + ' ';
			}

			if (field.Const)
			{
				result += "const ";
			}

			if (field.Static)
			{
				result += "static ";
			}

			result += field.Type + " ";

			result += field.Name;
			if (field.Default?.Length > 0)
			{
				result += " = " + field.Default;
			}

			result += ';';
			return result;
		}

		/// <inheritdoc />
		protected override string GenerateMethod(Method method)
		{
			var result = "";
			if (method.Access == "" || method.Access == "default")
			{
				result += "private ";
			}
			else
			{
				result += method.Access + ' ';
			}

			if (method.Static)
			{
				result += "static ";
			}

			result += method.Return + " ";

			result += method.Name;
			result += '(';

			for (var i = 0; i < method.Parameters?.Length; i++)
			{
				var parameter = method.Parameters[i];
				result += parameter.Type + " " + parameter.Name;
				if (i + 1 < method.Parameters.Length)
				{
					result += ", ";
				}
			}

			result += ") {";

			if (!string.IsNullOrWhiteSpace(method.Return) && method.Return != "void")
			{
				string defaultVal;
				if (JavaNormalizer.BuiltInDefaults.ContainsKey(method.Return))
				{
					defaultVal = JavaNormalizer.BuiltInDefaults[method.Return];
				}
				else
				{
					defaultVal = "new " + method.Return + "()";
				}

				result += '\n' + Indent + "return " + defaultVal + ";\n";
			}

			result += '}';
			return result;
		}
	}

	/// <inheritdoc />
	/// <summary>Normalizer for Java</summary>
	public class JavaNormalizer : Normalizer
	{
		private static Normalizer _singletonInstance = null;

		private JavaNormalizer()
		{
			
		}

		/// <summary>
		/// The dictionary of built in values
		/// </summary>
		public static readonly Dictionary<string, string> BuiltInDefaults = new Dictionary<string, string>
		{
			{"int", "0"},
			{"double", "0.0"},
			{"char", "'a'"},
			{"boolean", "false"},
			{"String", "\"\""},
		};

		/// <summary>
		/// Method for getting a singleton
		/// </summary>
		/// <returns>Normalizer instance</returns>
		public static Normalizer GetNormalizer()
		{
			return _singletonInstance ?? (_singletonInstance = new JavaNormalizer());
		}

		/// <inheritdoc />
		protected override string NormalizeType(string type)
		{
			if (type == "bool")
				type = "boolean";
			else if (type == "string")
				type = "String";
			else if (type == "float")
				type = "double";
			else if (string.IsNullOrWhiteSpace(type))
				type = "void";

			return type;
		}
	}
}