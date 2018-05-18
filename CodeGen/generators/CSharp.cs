using System.Collections.Generic;
using System.Linq;
using CodeGen.utils;

namespace CodeGen.generators
{
	/// <inheritdoc />
	/// <summary>
	/// Generator for C#
	/// </summary>
	public class CSharpGenerator : Generator
	{
		private const string ClassFormat = "{0}class {1}{2}{{{3}{4}{5}}}";
		private string Indent { get; set; } = GeneratorConf.GetIndent(UseTabs, 4);

		/// <inheritdoc />
		protected override string GenerateClass(Class @class)
		{
			string fields = "", inherits = " ", methods = "", classes = "";
			if (@class.Parent?.Length > 0)
			{
				inherits = " : " + @class.Parent + ' ';
			}

			fields = @class.Fields?.Aggregate('\n' + fields,
				(current, field) => current + GenerateField(field) + '\n');

			methods = @class.Methods?.Aggregate(methods,
				(current, method) => current + '\n' + GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + '\n');

			if (@class.Fields?.Length > 0)
			{
				methods += GeneratorConf.ShiftCode(GenerateGettersSetters(@class.Fields), 1, Indent);
			}

			classes = @class.Classes?.Aggregate(classes,
				          (current, cls) => current + '\n' + GeneratorConf.ShiftCode(GenerateClass(cls), 1, Indent) + '\n') + '\n';

			var access = "";
			if (@class.Access?.Length > 0)
			{
				access = @class.Access + ' ';
			}

			return string.Format(ClassFormat, access, @class.Name, inherits, fields, methods, classes);
		}

		/// <inheritdoc />
		public override string GenerateField(Field field)
		{
			var result = Indent;
			if (string.IsNullOrWhiteSpace(field.Access)  || field.Access == "default")
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


			result += field.Type + ' ';

			result += field.Name;
			if (field.Default?.Length > 0)
			{
				result += " = " + field.Default;
			}

			result += ';';
			return result;
		}

		/// <inheritdoc />
		public override string GenerateMethod(Method method)
		{
			var result = "";
			if (method.Access == "" || method.Access == "default"|| method.Access == null)
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

			if (method.Type == null || method.Type?.Length == 0)
				result += "void ";
			else
				result += method.Type + ' ';

			result += method.Name + '(';

			for (var i = 0; i < method.Parameters?.Length; i++)
			{
				var parameter = method.Parameters[i];

				result += parameter.Type + ' ' + parameter.Name;
				if (i + 1 < method.Parameters.Length)
				{
					result += ", ";
				}
			}

			result += ")\n{";
			if (!string.IsNullOrWhiteSpace(method.Type) && method.Type != "void")
			{
				string defaultVal;
				if (CSharpNormalizer.BuiltInDefaults.ContainsKey(method.Type))
				{
					defaultVal = CSharpNormalizer.BuiltInDefaults[method.Type];
				}
				else
				{
					defaultVal = "new " + method.Type + "()";
				}

				result += '\n' + Indent + "return " + defaultVal + ";\n";
			}

			result += '}';
			return result;
		}

		/// <summary>
		/// Generates getters and setters for fields
		/// </summary>
		/// <param name="fields"></param>
		/// <returns></returns>
		public string GenerateGettersSetters(IEnumerable<Field> fields)
		{
			if (fields == null) return "";
			var result = "";
			foreach (var field in fields)
			{
				if (field.Getter)
				{
					result += '\n' + GenerateGetter(field) + '\n';
				}

				if (field.Setter)
				{
					result += '\n' + GenerateSetter(field) + '\n';
				}
			}

			return result;
		}

		/// <summary>
		/// Generates getter for field
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public string GenerateGetter(Field field)
		{
			return $"public {field.Type} get{Utils.Title(field.Name)}()\n{{\n{Indent}return {field.Name};\n}}";
		}

		/// <summary>
		/// Generates setter for field 
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public string GenerateSetter(Variable field)
		{
			return $"public void set{Utils.Title(field.Name)}({field.Type} newValue)\n{{\n{Indent}{field.Name} = newValue;\n}}";
		}
	}
	
	/// <inheritdoc />
	/// <summary>Normalizer for C#</summary>
	public class CSharpNormalizer : Normalizer
	{
		private static Normalizer _singletonInstance;
		
		private CSharpNormalizer()
		{
		}

		/// <summary>
		/// The dictionary of built in values
		/// </summary>
		public static readonly Dictionary<string, string> BuiltInDefaults = new Dictionary<string, string>
		{
			{"int", "0"},
			{"double", "0.0"},
			{"float", "0.0f"},
			{"char", "''"},
			{"bool", "false"},
			{"string", "\"\""},
		};

		/// <summary>
		/// Method for getting a singleton
		/// </summary>
		/// <returns>Normalizer instance</returns>
		public static Normalizer GetNormalizer()
		{
			return _singletonInstance ?? (_singletonInstance = new CSharpNormalizer());
		}

		/// <inheritdoc />
		protected override string NormalizeType(string type)
		{
			if (string.IsNullOrWhiteSpace(type))
			{
				type = "void";
			}

			return type;
		}
	}
}
