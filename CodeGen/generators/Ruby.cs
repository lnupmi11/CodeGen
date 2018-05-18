using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	/// <inheritdoc />
	/// <summary>
	/// Generator for Ruby
	/// </summary>
	public class RubyGenerator : Generator
	{
		private const string ClassFormat = "class {0}{1}\n{2}{3}{4}end";
		private string Indent { get; set; } = GeneratorConf.GetIndent(true, 4);

		/// <inheritdoc />
		protected override string GenerateClass(Class @class)
		{
			string fields = "", inherits = "", methods = "", classes = "";

			if (!string.IsNullOrWhiteSpace(@class.Parent))
			{
				inherits = " < " + @class.Parent;
			}

			if (@class.Fields?.Length > 0)
			{
				fields = GeneratorConf.ShiftCode(GenerateInit(@class), 1, Indent);
			}

			methods = @class.Methods?.Aggregate('\n' + methods,
				(current, method) => current + "\n" + GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + "\n");

			classes = @class.Classes?.Aggregate(classes,
				(current, cls) => current + "\n" + GeneratorConf.ShiftCode(GenerateClass(cls), 1, Indent));

			if (!string.IsNullOrWhiteSpace(classes)) classes += "\n";

			return string.Format(ClassFormat, @class.Name, inherits, fields, methods, classes);
		}

		/// <inheritdoc />
		public override string GenerateField(Field field)
		{
			var result = Indent;

//			if (field.Access == "public")
//			{
//				field.Name = field.Name?.First().ToString().ToUpper() + field.Name?.Substring(1);
//			}

//			result += field.Name + ' ' + field.Type;

			return result;
		}

		/// <inheritdoc />
		public override string GenerateMethod(Method method)
		{
			return GenerateMethodWithBody(method, "");
		}

		private string GenerateMethodWithBody(Method method, string body)
		{
			var result = "def ";

			if (method.Access == "private") result = "private " + result;

			if (method.Static) result += "self.";


			result += method.Name;
			if (method.Parameters?.Length > 0)
			{
				result += '(';

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

				result += ")";
			}

			if (!string.IsNullOrWhiteSpace(body)) 
				result += "\n" + GeneratorConf.ShiftCode(body, 1, Indent);


			result += "\nend";
			return result;
		}

		private string GenerateInit(Class @class)
		{
			string result = "", body = "";
			var init = new Method
			{
				Name = "initialize",
			};
			var previousIsStatic = false;
			var paramList = new List<Parameter>();

			for (var i = 0; i < @class.Fields.Length; i++)
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

				body += "@" + field.Name + " = " + field.Name;

				if (i + 1 < @class.Fields.Length && !previousIsStatic)
				{
					body += "\n";
				}

				previousIsStatic = false;
			}

			init.Parameters = paramList.ToArray();

			result += GenerateMethodWithBody(init, body);

			return result;
		}
	}
}