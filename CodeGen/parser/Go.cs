using System;
using System.Collections.Generic;
using System.Linq;
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
		private const string NameRegex = @"[A-Za-z_0-9]+";
		
		private const string SpaceRequired = @"\s+";
		private const string SpaceOptional = @"\s*";
		
		private const string BodyRegex = SpaceOptional + @"[^{}]+(?:\{.*\})?" + SpaceOptional;
		private const string ParamsRegex = SpaceOptional + @"[^()]+(?:\(.*\))?" + SpaceOptional;
		
		private const string FieldRegex =
			@"(" + NameRegex + @")" + SpaceRequired + 
			@"(" + NameRegex + @")" + SpaceOptional;

		private const string ParameterRegex =
			@"(" + NameRegex + @")" + SpaceRequired + 
			@"(" + NameRegex + @")" + SpaceOptional + 
			@",?" + SpaceOptional;
		
		private const string MethodRegex =
			@"func" + SpaceOptional + 
			@"\((" + NameRegex + @")\)" + SpaceOptional + 
			@"(" + NameRegex + @")" + SpaceOptional + 
			@"\((" + ParamsRegex + @")?\)" + SpaceOptional + 
			@"(" + NameRegex + @")?" + SpaceOptional + 
			@"\{(?:" + BodyRegex + @")?\}";
		
		private const string StructRegex =
			@"type" + SpaceRequired + 
			@"(" + NameRegex + @")" + SpaceRequired +
			@"struct" + SpaceOptional + 
			@"\{(" + BodyRegex + @")?\}";
		
		private string _currentClass = "";

		/// <inheritdoc />
		public override Package ParsePackage(string pkg)
		{
			var package = new Package {Classes = ParseClasses(pkg)};

			for (var i = 0; i < package.Classes?.Length; i++)
			{
				package.Classes[i] = CheckForGetSet(package.Classes[i]);
			}
			
			return package;
		}

		/// <inheritdoc />
		protected override Class[] ParseClasses(string @class)
		{
			var classesMatch = Regex.Match(@class, StructRegex, RegexOptions.Singleline);
			var classes = new List<Class>();

			while (classesMatch.Success)
			{
				var name = classesMatch.Groups[1].Value;
				var access = "private";
				if (char.IsLetter(name[0]) && char.IsUpper(name[0])) access = "public";
				var body = classesMatch.Groups[2].Value;
				_currentClass = name;
				classes.Add(new Class
				{
					Name = name,
					Access = access,
					Fields = ParseFields(body),
					Methods = ParseMethods(@class),
					Classes = null,
					Parent = ""
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
				var type = fieldsMatch.Groups[2].Value;
				var access = "private";
				if (char.IsLetter(name[0]) && char.IsUpper(name[0])) access = "public";
				fieldsResult.Add(new Field
				{
					Name = name,
					Access = access,
					Const = false,
					Static = false,
					Default = null,
					Type = type,
					Getter = false,
					Setter = false
				});
				fieldsMatch = fieldsMatch.NextMatch();
			}
			
			return fieldsResult.ToArray();
		}

		/// <inheritdoc />
		protected override Method[] ParseMethods(string methods)
		{
			var methodsMatch = Regex.Match(methods, MethodRegex, RegexOptions.Singleline);
			var methodsList = new List<Method>();
			while (methodsMatch.Success)
			{
				if (_currentClass == methodsMatch.Groups[1].Value)
				{
					var name = methodsMatch.Groups[2].Value;
					var parameters = methodsMatch.Groups[3].Value;
					var @return = methodsMatch.Groups[4].Value;
					var access = "private";
					if (char.IsLetter(name[0]) && char.IsUpper(name[0])) access = "public";
					methodsList.Add(new Method
					{
						Name = name,
						Access = access,
						Const = false,
						Return = @return,
						Static = false,
						Parameters = ParseParameters(parameters)
					});
				}
				methodsMatch = methodsMatch.NextMatch();
			}

			return methodsList.ToArray();
		}

		/// <inheritdoc />
		protected override Parameter[] ParseParameters(string parameters)
		{
			var parametersMatch = Regex.Match(parameters, ParameterRegex, RegexOptions.Singleline);
			var parametersResult = new List<Parameter>();
			while (parametersMatch.Success)
			{
				var name = parametersMatch.Groups[1].Value;
				var type = parametersMatch.Groups[2].Value;
				parametersResult.Add(new Parameter
				{
					Name = name,
					Type = type,
					Default = null,
				});
				parametersMatch = parametersMatch.NextMatch();
			}

			return parametersResult.ToArray();
		}

		private static Class CheckForGetSet(Class @class)
		{
			for (var i = 0; i < @class.Fields?.Length; i++)
			{
				bool get = false, set = false;
				var methods = @class.Methods;
				AssignGetSet(ref methods, @class.Fields[i].Name, ref get, ref set);
				@class.Fields[i].Getter = get;
				@class.Fields[i].Setter = set;
				@class.Methods = methods;
			}

			for (var i = 0; i < @class.Classes?.Length; i++)
			{
				@class.Classes[i] = CheckForGetSet(@class.Classes[i]);
			}

			return @class;
		}
		
		private static void AssignGetSet(ref Method[] methods, string field, ref bool get, ref bool set)
		{
			var fieldNameRegex = MakeRegexFromString(field);
			
			for (var i = 0; i < methods?.Length; i++)
			{
				if (get && set) break;
				if (Regex.Match(methods[i].Name, @"(?:G|g)(?:E|e)(?:T|t)_*" + fieldNameRegex).Success)
				{
					get = true;
					methods = RemoveMethod(methods, methods[i]);					
				}
				else if (Regex.Match(methods[i].Name, @"(?:S|s)(?:E|e)(?:T|t)_*" + fieldNameRegex).Success)
				{
					set = true;
					methods = RemoveMethod(methods, methods[i]);
				}
			}
		}

		private static string MakeRegexFromString(string @string)
		{
			var result = @"";
			foreach (var @char in @string)
			{
				if (char.IsLetter(@char))
				{
					result += @"(?:" + char.ToUpper(@char) + @"|" + char.ToLower(@char) + @")";
				}
				else
				{
					result += @char;
				}
			}

			return result;
		}

		private static Method[] RemoveMethod(IEnumerable<Method> methods, Method method)
		{
			return methods.Where(val => val != method).ToArray();
		}
	}
}
