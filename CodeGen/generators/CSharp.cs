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

			if (method.Return?.Length == 0)
				result += "void ";
			else
				result += method.Return + ' ';

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

			result += ") {";

			if (method.Return?.Length > 0)
			{
				result += '\n' + Indent + "return new " + method.Return + "();\n";
			}

			result += '}';
			return result;
		}

		/// <summary>
		/// 
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
		/// 
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public string GenerateGetter(Field field)
		{
			var isStatic = "";
			if (field.Static)
			{
				isStatic = "static ";
			}
			return "public " + isStatic + field.Type + " get" + Utils.Title(field.Name) + "()\n{\n" + Indent +
			       "return " + field.Name + ";\n}";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public string GenerateSetter(Variable field)
		{
			return "public void set" + Utils.Title(field.Name) + "(" + field.Type + " newValue) {\n" + Indent +
			       field.Name + " = newValue;\n}";
		}
	}
}