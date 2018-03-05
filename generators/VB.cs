using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	public class VBGenerator : Generator
	{
		private const string ClassFormat = "Class {0}\n{1}{2}{3}";
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
			string fields = "", methods = "", classes = "";

			fields = @class.Fields?.Aggregate(fields, (current, field) => current + GenerateField(field) + "\n");
			
			methods = @class.Methods?.Aggregate("\n" + methods,
				(current, method) => current + GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + "\n");
			
			classes = @class.Classes?.Aggregate(classes,
				(current, cls) => current + GeneratorConf.ShiftCode(GenerateClass(cls), 1, Indent));
			
			var result = string.Format(ClassFormat, @class.Name, fields, methods, classes);

			var access = "";
			if (@class.Access?.Length > 0)
			{
				access = @class.Access + " ";
			}
			
			return access + result + "\nEnd Class";
		}

		protected override string GenerateField(Field field)
		{
			var result = Indent + field.Access + " " + field.Name + " As " + field.Type;
			if (field.Default?.Length > 0)
			{
				result += " = " + field.Default;
			}
			return result;
		}

		protected override string GenerateMethod(Method method)
		{
			var result = method.Access + " ";
			var type = method.Return?.Length > 0 ? "Function" : "Sub";
			result += type + " " + method.Name + "(";

			for (var i = 0; i < method.Parameters?.Length; i++)
			{
				var parameter = method.Parameters[i].Name + " As " + method.Parameters[i].Type;
				if (method.Parameters[i].Default?.Length > 0)
				{
					result += "Optional " + parameter + " = " + method.Parameters[i].Default;
				}
				else
				{
					result += "ByVal " + parameter;
				}

				if (i + 1 < method.Parameters?.Length)
				{
					result += ", ";
				}
			}

			result += ")";
			if (type == "Function")
			{
				result += " As " + method.Return + "\n" + Indent + "Return 0";
			}
			else
			{
				result += "\n" + Indent + "Return";
			}

			result += "\nEnd " + type + "\n";
			return result;
		}
	}
	
	
}
