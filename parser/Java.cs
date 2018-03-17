using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeGen.generators;
using CSharpx;

namespace CodeGen.parser
{
	/// <inheritdoc />
	/// <summary>
	/// Java language Parser
	/// </summary>
	public class JavaParser : Parser
	{
		private const string NameRegex = "[A-Za-z_](?:[A-Za-z_0-9])*";
		private const string SpaceRequired = @"\s+";
		private const string SpaceOptional = @"\s*";

		private const string AccessTypeRegex = "(public|protected|private)";

		private const string ExtendsRegex = SpaceRequired + "extends" + SpaceRequired + "(" + NameRegex + ")";

		private const string FieldRegex =
			"(?:" + AccessTypeRegex + SpaceRequired + ")?" +
			"(?:" + "(static)" + SpaceRequired + ")?" +
			"(?:" + "(const)" + SpaceRequired + ")?" +
			"(" + NameRegex + ")" + SpaceRequired +
			"(" + NameRegex + ")" + SpaceOptional +
			"(?:=" + SpaceOptional + ValueRegex + ")?" +
			";";

		private const string MethodRegex =
			"(?:" + AccessTypeRegex + SpaceRequired + ")?" +
			"(?:" + "(static)" + SpaceRequired + ")?" +
			"(?:" + "(const)" + SpaceRequired + ")?" +
			"(" + NameRegex + ")" + SpaceRequired +
			"(" + NameRegex + ")" + SpaceOptional + @"\(" + ParametersBodyRawRegex + @"\)" + SpaceOptional + "{" +
			"(?:" + BodyRegex + ")*" +
			"}";


		private const string ValueRegex = "(\\S+)";
		private const string BodyRegex = SpaceOptional + @"[^{}]+(?:\{.*\}|;)?" + SpaceOptional;

		private const string ParametersBodyRegex =
			SpaceOptional + "(?:(" + ParameterRegex + ")" + SpaceOptional + ")?" +
			"(?:," + SpaceOptional + "(" + ParameterRegex + ")" + SpaceOptional + ")*";

		private const string ParametersBodyRawRegex =
			SpaceOptional + "(?:(?:" + ParameterRegex + ")" + SpaceOptional + ")?" +
			"(?:," + SpaceOptional + "(?:" + ParameterRegex + ")" + SpaceOptional + ")*";

		private const string ParameterRegex = NameRegex + SpaceRequired + NameRegex + SpaceOptional +
		                                      "(?:=" + SpaceOptional + ValueRegex + ")?";

		private const string ClassRegex =
			SpaceOptional + "(?:" + AccessTypeRegex + SpaceRequired + ")?" +
			"class" + SpaceRequired + "(" + NameRegex + ")(?:" + ExtendsRegex + ")?" + SpaceOptional + "{" +
			"(" + BodyRegex + ")*" +
			"}" + SpaceOptional;

		/// <inheritdoc />
		public override Package ParsePackage(string pkg)
		{
			var package = new Package {Classes = ParseClasses(pkg)};
			return package;
		}

		/// <inheritdoc />
		protected override Class[] ParseClasses(string @class)
		{
//			Console.Out.WriteLine($"ClassRegex = \"{ClassRegex}\"");
			if (string.IsNullOrWhiteSpace(@class))
			{
				return null;
			}

			var classesMatch = Regex.Match(@class, ClassRegex, RegexOptions.Singleline);
			var classes = new List<Class>();

			while (classesMatch.Success)
			{
				var access = classesMatch.Groups[1].Value;
				var className = classesMatch.Groups[2].Value;
				var parent = classesMatch.Groups[3].Value;
				var body = classesMatch.Groups[4].Value;
//				Console.Out.WriteLine("body = {0}", body);
				classes.Add(new Class
				{
					Name = className,
					Parent = parent,
					Fields = ParseFields(body),
					Methods = ParseMethods(body),
					Classes = ParseClasses(body),
					Access = access
				});
				classesMatch = classesMatch.NextMatch();
			}

			return classes.ToArray();
		}

		/// <inheritdoc />
		protected override Field[] ParseFields(string body)
		{
			if (string.IsNullOrWhiteSpace(body))
			{
				return null;
			}

			var fieldMatch = Regex.Match(body, FieldRegex, RegexOptions.Singleline);
			var fields = new List<Field>();
			while (fieldMatch.Success)
			{
//					Console.Out.WriteLine("field = {0}", fieldMatch);
//					var index = 0;
//					fieldMatch.Groups.ForEach(e =>
//					{
//						if (e.Captures.Count > 0)
//							Console.Out.WriteLine($"fieldGroups[{index}] = \"{e}\"");
//						index++;
//					});

				var access = fieldMatch.Groups[1].Value;
				var isStatic = !string.IsNullOrEmpty(fieldMatch.Groups[2].Value);
				var isConst = !string.IsNullOrEmpty(fieldMatch.Groups[3].Value);
				var type = fieldMatch.Groups[4].Value;
				var name = fieldMatch.Groups[5].Value;
				var value = fieldMatch.Groups[6].Value;

				fields.Add(new Field
				{
					Name = name,
					Access = access,
					Const = isConst,
					Default = value,
					Static = isStatic,
					Type = type,
				});
				fieldMatch = fieldMatch.NextMatch();
			}

			return fields.ToArray();
		}

		/// <inheritdoc />
		protected override Method[] ParseMethods(string body)
		{
			if (string.IsNullOrWhiteSpace(body))
			{
				return null;
			}

			var methodMatch = Regex.Match(body, MethodRegex, RegexOptions.Singleline);
			var methods = new List<Method>();

			while (methodMatch.Success)
			{
//				Console.Out.WriteLine("method = {0}", methodMatch);

				var access = methodMatch.Groups[1].Value;
				var isStatic = !string.IsNullOrEmpty(methodMatch.Groups[2].Value);
				var isConst = !string.IsNullOrEmpty(methodMatch.Groups[3].Value);
				var type = methodMatch.Groups[4].Value;
				var name = methodMatch.Groups[5].Value;
				var paramBody = methodMatch.Groups[6].Value;

				methods.Add(new Method
				{
					Name = name,
					Access = access,
					Const = isConst,
					Return = type,
					Static = isStatic,
					Parameters = ParseParameters(paramBody)
				});
				methodMatch = methodMatch.NextMatch();
			}

			return methods.ToArray();
		}

		/// <inheritdoc />
		/// \bug: parse params not working
		protected override Parameter[] ParseParameters(string body)
		{
			if (string.IsNullOrWhiteSpace(body))
			{
				return null;
			}

			var paramMatch = Regex.Match(body, ParameterRegex, RegexOptions.Singleline);
			var parameters = new List<Parameter>();


			while (paramMatch.Success)
			{
				Console.Out.WriteLine("param = {0}", paramMatch);

				paramMatch = paramMatch.NextMatch();
			}

			return parameters.ToArray();
		}
	}
}