using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	/// <inheritdoc />
	/// <summary>
	/// Generator for JavaScript ES6 
	/// </summary>
	public class Es6Generator : Generator
	{
		private const string ClassFormat = "class {0} {1}{{{2}{3}}}{4}";
		private string Indent { get; set; } = GeneratorConf.GetIndent(true, 4);

		/// <inheritdoc />
		protected override string GenerateClass(Class @class)
		{
			string fields = "", inherits = "", methods = "", classes = "";
			if (!string.IsNullOrWhiteSpace(@class.Parent))
			{
				inherits = "extends " + @class.Parent + " ";
			}

			if (@class.Fields?.Length > 0)
			{
				fields = "\n" + GeneratorConf.ShiftCode(GenerateInit(@class), 1, Indent) + "\n";
			}

			methods = @class.Methods?.Aggregate("\n" + methods,
				(current, method) => current + GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + "\n");

			var staticFields = @class.Fields?.Aggregate("",
				(current, field) =>
				{
					if (!field.Static) return current;
					current += "\n" + @class.Name + "." + field.Name + " = ";
					current += (string.IsNullOrEmpty(field.Default) ? "null" : field.Default) + ";";
					return current;
				});
			
			classes = @class.Classes?.Aggregate(classes,
				(current, cls) => current + "\n" + @class.Name + "." + cls.Name + " = " + GenerateClass(cls));

			return string.Format(ClassFormat, @class.Name, inherits, fields, methods, staticFields + classes);
		}

		/// <inheritdoc />
		public override string GenerateField(Field field)
		{
			var result = Indent;

			result += field.Type + " ";
			result += field.Name;
			if (field.Default != "")
			{
				result += " = " + field.Default;
			}

			result += ";";

			return result;
		}

		/// <inheritdoc />
		public override string GenerateMethod(Method method)
		{
			return GenerateMethodWithBody(method, "");
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="method"></param>
		/// <param name="body"></param>
		/// <returns></returns>
		private string GenerateMethodWithBody(Method method, string body)
		{
			var result = "";
			if (method.Static)
			{
				result += "static ";
			}

			result += method.Name;
			result += "(";

			for (var i = 0; i < method.Parameters?.Length; i++)
			{
				var parameter = method.Parameters[i];

				result += parameter.Name;
				if (!string.IsNullOrEmpty(parameter.Default))
				{
					result += "=" + parameter.Default;
				}

				if (i + 1 < method.Parameters.Length)
				{
					result += ", ";
				}
			}

			result += ") {";

			if (body != "")
			{
				result += "\n" + GeneratorConf.ShiftCode(body, 1, Indent) + "\n";
			}

			if (method.Type != "" && method.Name != "constructor")
			{
				result += "\n" + Indent + "return null;\n";
			}

			result += "}";

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="class"></param>
		/// <returns></returns>
		private string GenerateInit(Class @class)
		{
			var result = "";
			var body = "";

			var init = new Method
			{
				Name = "constructor"
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

				paramList.Add(new Parameter
				{
					Name = field.Name,
					Default = field.Default
				});

				body += "this." + field.Name + " = " + field.Name + ";";

				if (i + 1 < @class.Fields.Length && !previousIsStatic)
				{
					body += "\n";
				}

				previousIsStatic = false;
			}

			init.Parameters = paramList.ToArray();

			if (previousIsStatic && body.Length > Indent.Length)
			{
				body = body.Substring(0, body.Length - Indent.Length + 1);
			}

			if (!string.IsNullOrWhiteSpace(@class.Parent))
			{
				body = "super();\n" + body;
			}

			result += GenerateMethodWithBody(init, body);

			return result;
		}
	}
}