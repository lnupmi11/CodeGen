using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	public class VBGenerator : Generator
	{
		private const string ClassFormat = "Class {0}\n{1}{2}{3}{4}";
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

			if (@class.Parent?.Length > 0)
			{
				inherits = Indent + "Inherits " + @class.Parent + "\n\n";
			}
			
			fields = @class.Fields?.Aggregate(fields, (current, field) => current + GenerateField(field) + "\n");
			
			methods = @class.Methods?.Aggregate("\n" + methods,
				(current, method) => current + GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + "\n");
			
			classes = @class.Classes?.Aggregate(classes,
				(current, cls) => current + GeneratorConf.ShiftCode(GenerateClass(cls), 1, Indent));
			
			var result = string.Format(ClassFormat, @class.Name, inherits, fields, methods, classes);

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

	class VBNormalizer : Normalizer
	{
		public override Package NormalizePackage(Package pkg)
		{
			for(var i = 0; i < pkg.Classes?.Length; i++)
			{
				pkg.Classes[i] = NormalizeClass(pkg.Classes[i]);
			}

			for(var i = 0; i < pkg.Packages?.Length; i++)
			{
				pkg.Packages[i] = NormalizePackage(pkg.Packages[i]);
			}

			return pkg;
		}

		protected override Class NormalizeClass(Class @class)
		{
			for (var i = 0; i < @class.Fields?.Length; i++)
			{
				@class.Fields[i] = NormalizeField(@class.Fields[i]);
			}
			
			for (var i = 0; i < @class.Methods?.Length; i++)
			{
				@class.Methods[i] = NormalizeMethod(@class.Methods[i]);
			}
			
			for (var i = 0; i < @class.Classes?.Length; i++)
			{
				@class.Classes[i] = NormalizeClass(@class.Classes[i]);
			}

			@class.Access = Title(@class.Access);

			return @class;
		}

		protected override Field NormalizeField(Field field)
		{
			switch (field.Type)
			{
				case "int":
					field.Type = "Integer";
					break;
				default:
					field.Type = Title(field.Type);
					break;
			}

			field.Access = Title(field.Access);
			return field;
		}

		protected override Method NormalizeMethod(Method method)
		{
			switch (method.Return)
			{
				case "int":
					method.Return = "Integer";
					break;
				case "void":
					method.Return = "";
					break;
				default:
					method.Return = Title(method.Return);
					break;
			}

			method.Access = Title(method.Access);

			for (var i = 0; i < method.Parameters?.Length; i++)
			{
				method.Parameters[i] = NormalizeParameter(method.Parameters[i]);
			}

			return method;
		}

		protected override Parameter NormalizeParameter(Parameter parameter)
		{
			switch (parameter.Type)
			{
				case "int":
					parameter.Type = "Integer";
					break;
				default:
					parameter.Type = Title(parameter.Type);
					break;
			}

			return parameter;
		}
		
		private static string Title(string @string)
		{
			return @string?.First().ToString().ToUpper() + @string?.Substring(1).ToLower();
		}
		
	}
}
