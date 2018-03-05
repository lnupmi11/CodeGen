using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
	/// <inheritdoc />
	/// <summary>
	/// Go language generator
	/// </summary>
	public class GoGenerator : Generator
	{
		private const string ClassFormat = "type {0} struct {{{1}}}{2}{3}";
		private string Indent { get; set; } = GeneratorConf.GetIndent(true, 4);

		/// <inheritdoc />
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

		/// <inheritdoc />
		protected override string GenerateClass(Class @class)
		{
			string fields = "", methods = "", classes = "";
			fields = @class.Fields?.Aggregate("\n" + fields, (current, field) => current + GenerateField(field) + "\n");
			methods = @class.Methods?.Aggregate("\n\n" + methods,
				(current, method) => current + "func (" + @class.Name + ") " + GenerateMethod(method) + "\n\n");
			classes = @class.Classes?.Aggregate(classes, (current, cls) => current + GenerateClass(cls));
			return string.Format(ClassFormat, @class.Name, fields, methods, classes);
		}

		/// <inheritdoc />
		protected override string GenerateField(Field field)
		{
			var result = Indent;

			if (field.Access == "public")
			{
				field.Name = field.Name?.First().ToString().ToUpper() + field.Name?.Substring(1);
			}

			result += field.Name + " " + field.Type;

			return result;
		}

		/// <inheritdoc />
		protected override string GenerateMethod(Method method)
		{
			var result = "";
			if (method.Access == "public")
			{
				method.Name = method.Name?.First().ToString().ToUpper() + method.Name?.Substring(1);
			}
			result += method.Name + "(";

			for (var i = 0; i < method.Parameters?.Length; i++)
			{
				result += method.Parameters[i].Name + " " + method.Parameters[i].Type;
				if (i + 1 < method.Parameters.Length)
				{
					result += ", ";
				}
			}

			result += ")";

			if (method.Return != "")
			{
				result += " " + method.Return;
			}

			result += " {";

			if (method.Return != "")
			{
				result += "\n" + Indent + "return nil\n";
			}

			result += "}";

			return result;
		}
	}
}