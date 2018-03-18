using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeGen.generators;

namespace CodeGen.parser
{
	/// <inheritdoc />
	/// <summary>
	/// Go language Parser
	/// </summary>
	public class GoParser : Parser
	{
		private static string _classMethod = "";

		///\bug: only names with letters will be parsed, when digits and underscores are allowed for names 
		private const string ClassRegex = @"type\s+(\w+)\s+struct\s*\{\s*((" + FieldRegex + @"\s*)*)\}";

		private const string FieldRegex = @"(\w+)\s+(\w+)\s*(=\s*(\S+))?";

		private static readonly string MethodRegex = @"(func\s*\(\s*(" + _classMethod + @")\s*\)\s*(\w+)\s*\(\s*(" +
		                                             ParameterRegex + @")*\s*\)\s*(\w*)\s*{\s*return\s\w+\s*})";

		private const string ParameterRegex = @"(\w+)\s+(\w+)\s*,?\s*";

		/// <inheritdoc />
		public override Package ParsePackage(string pkg)
		{
			var package = new Package {Classes = ParseClasses(pkg)};
			return package;
		}

		/// <inheritdoc />
		protected override Class[] ParseClasses(string pkg)
		{
			var classesMatch = Regex.Match(pkg, ClassRegex, RegexOptions.Singleline);
			var classes = new List<Class>();

			while (classesMatch.Success)
			{
				var name = classesMatch.Groups[1].Value;
				_classMethod = name;

				classes.Add(new Class
				{
					Name = name,
					Access = char.IsLetter(name[0]) && char.IsUpper(name[0]) ? "public" : "private",
					Fields = ParseFields(classesMatch.Groups[2].Value),
					Methods = ParseMethods(pkg)
				});
				classesMatch = classesMatch.NextMatch();
			}

			return classes.ToArray();
		}

		/// <inheritdoc />
		protected override Field[] ParseFields(string fields)
		{
			var fieldsMatch = Regex.Match(fields, FieldRegex, RegexOptions.Singleline);

			var fieldsResult = new List<Field>();

			while (fieldsMatch.Success)
			{
				var name = fieldsMatch.Groups[1].Value;

				fieldsResult.Add(new Field
				{
					Name = name,
					Access = char.IsLetter(name[0]) && char.IsUpper(name[0]) ? "public" : "private",
					Const = false,
					Static = false,
					Default = fieldsMatch.Groups.Count >= 6 ? fieldsMatch.Groups[5].Value : null,
					Type = fieldsMatch.Groups[2].Value,
					Getter = false, // TODO: parse getters
					Setter = false // TODO: parse setters
				});

				fieldsMatch = fieldsMatch.NextMatch();
			}

			return fieldsResult.ToArray();
		}

		/// <inheritdoc />
		protected override Method[] ParseMethods(string @class)
		{
			var methodsMatch = Regex.Match(@class, MethodRegex, RegexOptions.Singleline);

			var methods = new List<Method>();

			while (methodsMatch.Success)
			{
				var name = methodsMatch.Groups[2].Value;
				methods.Add(new Method
				{
					Name = name,
					Access = char.IsLetter(name[0]) && char.IsUpper(name[0]) ? "public" : "private",
					Const = false,
					Return = methodsMatch.Groups[4].Value,
					Static = false,
					Parameters = ParseParameters(methodsMatch.Groups[3].Value)
				});

				methodsMatch = methodsMatch.NextMatch();
			}

			return methods.ToArray();
		}

		/// <inheritdoc />
		protected override Parameter[] ParseParameters(string parameters)
		{
			var parametersMatch = Regex.Match(parameters, MethodRegex, RegexOptions.Singleline);

			var parametersResult = new List<Parameter>();

			while (parametersMatch.Success)
			{
				parametersResult.Add(new Parameter
				{
					Name = parametersMatch.Groups[1].Value,
					Type = parametersMatch.Groups[2].Value,
					Default = null,
				});

				parametersMatch = parametersMatch.NextMatch();
			}

			return parametersResult.ToArray();
		}
	}
}