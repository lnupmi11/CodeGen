using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	public class JavaGenerator : Generator
	{
		private const string ClassFormat = "class {0} {1}{{{2}{3}{4}}}";
		private string Indent { get; set; } = GeneratorConf.GetIndent(true, 4);

		public override Dictionary<string, string> Generate(Package pkg)
		{
			var data = new Dictionary<string, string>();
			Indent = GeneratorConf.GetIndent(!pkg.UseSpaces, 4);
			foreach (var @class in pkg.Classes)
			{
				data[@class.Name] = GenerateClass(@class) + "\n";
			}

			

			return data;
		}

		protected override string GenerateClass(Class @class)
		{
			string fields = "", inherits = "", methods = "", classes = "";
			if (@class.Parent != "")
			{
				inherits = "extends " + @class.Parent + " ";
			}

			fields = @class.Fields?.Aggregate("\n" + fields, (current, field) => current + GenerateField(field) + "\n");

			methods = @class.Methods?.Aggregate("\n" + methods,
				(current, method) => current + GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + "\n");
			return string.Format(ClassFormat, @class.Name, inherits, fields, methods, classes);
		}

		protected override string GenerateField(Field field)
		{
			var result = Indent;
			if (field.Access == "" || field.Access == "default")
			{
				result += "private ";
			}
			else
			{
				result += field.Access + " ";
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
					result += field.Type + " ";
					break;
			}

			result += field.Name;
			if (field.Default != "")
			{
				result += " = " + field.Default;
			}

			result += ";";
			return result;
		}

		protected override string GenerateMethod(Method method)
		{
			var result = "";
			if (method.Access == "" || method.Access == "default")
			{
				result += "private ";
			}
			else
			{
				result += method.Access + " ";
			}

			if (method.Static)
			{
				result += "static ";
			}

			if (method.Return == "string")
				result += "String ";
			else if (method.Return == "")
				result += "void ";
			else
				result += method.Return + " ";

			result += method.Name;
			result += "(";

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

			if (method.Return != "")
			{
				result += "\n" + Indent + 
				          "return new " + (method.Return == "string" ? "String" : method.Return) + "();\n";
			}

			result += "}";
			return result;
		}
	}
}