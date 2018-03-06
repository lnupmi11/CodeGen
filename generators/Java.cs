using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	/// <inheritdoc />
	/// <summary>
	/// Java language generator
	/// </summary>
	public class JavaGenerator : Generator
	{
		private const string ClassFormat = "{0}class {1}{2}{{{3}{4}{5}}}";
		private string Indent { get; set; } = GeneratorConf.GetIndent(true, 4);

		/// <inheritdoc />
		public override Dictionary<string, string> Generate(Package pkg)
		{
			var data = new Dictionary<string, string>();
			Indent = GeneratorConf.GetIndent(!pkg.UseSpaces, 4);
			foreach (var @class in pkg.Classes)
			{
				data[@class.Name] = GenerateClass(@class) + '\n';
			}
			
			return data;
		}

		/// <inheritdoc />
		protected override string GenerateClass(Class @class)
		{
			string fields = "", inherits = " ", methods = "", classes = "";
			if (@class.Parent?.Length > 0)
			{
				inherits = " extends " + @class.Parent + " ";
			}

			fields = @class.Fields?.Aggregate('\n' + fields,
				(current, field) => current + GenerateField(field) + '\n');

			methods = @class.Methods?.Aggregate(methods,
				(current, method) => current + '\n' + GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + '\n');
			
			classes = @class.Classes?.Aggregate(classes,
				(current, cls) => current + '\n' + GeneratorConf.ShiftCode(GenerateClass(cls), 1, Indent) + '\n');
			
			var access = "";
			if (@class.Access?.Length > 0)
			{
				access = @class.Access + ' ';
			}
			
			return string.Format(ClassFormat, access, @class.Name, inherits, fields, methods, classes);;
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

			switch (field.Type)
			{
				case "string":
					result += "String ";
					break;
				default:
					result += field.Type + ' ';
					break;
			}

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

			switch (method.Return)
			{
				case "string":
					result += "String ";
					break;
				case "":
					result += "void ";
					break;
				default:
					result += method.Return + ' ';
					break;
			}

			result += method.Name;
			result += '(';

			for (var i = 0; i < method.Parameters?.Length; i++)
			{
				var parameter = method.Parameters[i];
				if (parameter.Type == "string")
				{
					parameter.Type = "String";
				}

				result += parameter.Type + " " + parameter.Name;
				if (i + 1 < method.Parameters.Length)
				{
					result += ", ";
				}
			}

			result += ") {";

			if (method.Return?.Length > 0)
			{
				result += '\n' + Indent + 
				          "return new " + (method.Return == "string" ? "String" : method.Return) + "();\n";
			}

			result += '}';
			return result;
		}
	}
}
