using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeGen.generators;

namespace CodeGen.parser
{
	/// <inheritdoc />
	/// <summary>
	/// Go language parser
	/// </summary>
	public class GoParser : Parser
	{
		private static string _classMethod = "";
		private const string ClassRegex = @"(type\s*(\w+)\s*struct\s*{\s*(" + FieldRegex + @")*\s*})";
		private const string FieldRegex = @"" + ParameterRegex + @"(=\s*(\S+))?";
		private static readonly string MethodRegex = @"(func\s*\(\s*(" + _classMethod + @")\s*\)\s*(\w+)\s*\(\s*(" + ParameterRegex + @")*\s*\)\s*(\w*)\s*{\s*return\s\w+\s*})";
		private const string ParameterRegex = @"(\w+)\s+(\w+)\s*,?\s*";

		/// <inheritdoc />
		public override Package ParsePackage(string pkg)
		{
			var package = new Package { Classes = ParseClasses(pkg).ToArray() };
			return package;
		}

		/// <inheritdoc />
		protected override List<Class> ParseClasses(string pkg)
		{
			var classesMatch = Regex.Match(pkg, ClassRegex, RegexOptions.Singleline);
			var classes = new List<Class>();

			while (classesMatch.Success)
			{
				var name = classesMatch.Groups[2].Value;
				_classMethod = name;
				classes.Append(new Class()
				{
					Name = name,
					Access = char.IsLetter(name[0]) && char.IsUpper(name[0]) ? "public" : "private",
					Fields = ParseFields(classesMatch.Groups[2].Value).ToArray(),
					Methods = ParseMethods(pkg).ToArray()
				});
				classesMatch.NextMatch();
			}
			
			return classes;
		}

		/// <inheritdoc />
		protected override List<Field> ParseFields(string fields)
		{
			var fieldsMatch = Regex.Match(fields, FieldRegex);
			
			var fieldsResult = new List<Field>();

			while (fieldsMatch.Success)
			{
				var name = fieldsMatch.Groups[1].Value;
				fieldsResult.Append(new Field()
				{
					Name = name,
					Access = char.IsLetter(name[0]) && char.IsUpper(name[0]) ? "public" : "private",
					Const = false,
					Static = false,
					Default = fieldsMatch.Length >= 6 ? fieldsMatch.Groups[5].Value : null,
					Type = fieldsMatch.Groups[2].Value,
					Getter = false,	// TODO: parse getters
					Setter = false	// TODO: parse setters
				});

				fieldsMatch.NextMatch();
			}
			
			return fieldsResult;
		}

		/// <inheritdoc />
		protected override List<Method> ParseMethods(string @class)
		{
			var methodsMatch = Regex.Match(@class, MethodRegex, RegexOptions.Singleline);
			
			var methods = new List<Method>();
			
			while (methodsMatch.Success)
			{
				var name = methodsMatch.Groups[2].Value;
				methods.Append(new Method()
				{
					Name = name,
					Access = char.IsLetter(name[0]) && char.IsUpper(name[0]) ? "public" : "private",
					Const = false,
					Return = methodsMatch.Groups[4].Value,
					Static = false,
					Parameters = ParseParameters(methodsMatch.Groups[3].Value).ToArray()
				});
				
				methodsMatch.NextMatch();
			}

			return methods;
		}

		/// <inheritdoc />
		protected override List<Parameter> ParseParameters(string parameters)
		{
			var parametersMatch = Regex.Match(parameters, MethodRegex, RegexOptions.Singleline);
			
			var parametersResult = new List<Parameter>();
			
			while (parametersMatch.Success)
			{
				parametersResult.Append(new Parameter()
				{
					Name = parametersMatch.Groups[1].Value,
					Type = parametersMatch.Groups[2].Value,
					Default = null,
				});
				
				parametersMatch.NextMatch();
			}

			return parametersResult;
		}
	}
}
