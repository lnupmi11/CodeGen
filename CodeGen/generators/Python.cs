using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	/// <inheritdoc />
	/// <summary>
	/// Generator for Python
	/// </summary>
	public class PythonGenerator : Generator
	{
		private const string ClassFormat = "class {0}{1}:\n{2}{3}{4}";
		private string Indent { get; set; } = GeneratorConf.GetIndent(true, 4);
		
		/// <inheritdoc />
		protected override string GenerateClass(Class @class)
		{
			string fields = "", inherits = "", methods = "", classes = "";

			if (!string.IsNullOrWhiteSpace(@class.Parent))
			{
				inherits = $"({@class.Parent})";
			}

			if (@class.Fields?.Length > 0)
			{
				fields = GeneratorConf.ShiftCode(GenerateInit(@class), 1, Indent);
			}
			
			methods = @class.Methods?.Aggregate('\n' + methods,
				(current, method) => $"{current}{GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent)}\n");
			
			classes = @class.Classes?.Aggregate('\n' + classes,
				(current, cls) => current + GeneratorConf.ShiftCode(GenerateClass(cls), 1, Indent));
			
			var result = string.Format(ClassFormat, @class.Name, inherits, fields, methods, classes);
			
			if (result[result.Length - 2] == ':' && result[result.Length - 1] == '\n')
			{
				result += Indent + "pass";
			}

			return result;
		}

		/// <inheritdoc />
		public override string GenerateField(Field field)
		{
			return "self." + field.Name;
		}

		/// <inheritdoc />
		public override string GenerateMethod(Method method)
		{
			return GenerateMethodWithBody(method, "pass");
		}

		private string GenerateMethodWithBody(Method method, string body) 
		{
			var result = "def ";

			switch (method.Access)
			{
				case "private":
					method.Name = "__" + method.Name;
					break;
				case "protected":
					method.Name = '_' + method.Name;
					break;
			}

			result += method.Name + '(';

			if (method.Static)
			{
				result = "@staticmethod\n" + result;
			} 
			else
			{
				result += "self";
				if (method.Parameters?.Length > 0)
				{
					result += ", ";
				}
			}

			for (var i = 0; i < method.Parameters?.Length; i++)
			{
				result += method.Parameters[i].Name;
				if (!string.IsNullOrWhiteSpace(method.Parameters[i].Default))
				{
					result += '=' + method.Parameters[i].Default;
				}
				if (i + 1 < method.Parameters.Length)
				{
					result += ", ";
				}
			}

			if (body == "pass")
			{
				body += '\n';
			}
			
			result += "):\n" + GeneratorConf.ShiftCode(body, 1, Indent);
			return result;
		}

		public string GenerateInit(Class @class)
		{
			string result = "", body = "";
			var init = new Method
			{
				Name = "__init__",
			};
			var previousIsStatic = false;
			var paramList = new List<Parameter>();

			for (var i = 0; i < @class.Fields?.Length; i++)
			{
				var field = @class.Fields[i];

				if (field.Static)
				{
					previousIsStatic = true;
					continue;
				}

				string access;
				switch (field.Access)
				{
					case "private":
						access = "__";
						break;
					case "protected":
						access = "_";
						break;
					default:
						access = "";
						break;
				}

				paramList.Add(new Parameter
				{
					Name = field.Name,
					Default = field.Default
				});
				var fieldName = field.Name;
				field.Name = access + field.Name;
				body += $"{GenerateField(field)} = {fieldName}";

				if (i + 1 < @class.Fields.Length && !previousIsStatic)
				{
					body += "\n";
				}

				previousIsStatic = false;

			}

			if (body == "")
			{
				body = "pass";
			}

			init.Parameters = paramList.ToArray();

			result += GenerateMethodWithBody(init, body);

			return result;
		}
	}
}
