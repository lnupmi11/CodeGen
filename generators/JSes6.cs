using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using CSharpx;

namespace CodeGen.generators
{
    public class JSes6Generator : Generator
    {
        private const string ClassFormat = "class {0} {1}{{{2}{3}{4}}}";
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
            if (@class.Parent != "")
            {
                inherits = "extends " + @class.Parent + " ";
            }

            if (@class.Fields?.Length > 0)
            {
                fields = "\n" + GeneratorConf.ShiftCode(GenerateInit(@class), 1, Indent) + "\n";
            }
            
            methods = @class.Methods?.Aggregate("\n" + methods,
                (current, method) => current + GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + "\n");
            
            
            return string.Format(ClassFormat, @class.Name, inherits, fields, methods, classes);
        }

        protected override string GenerateField(Field field)
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

        protected override string GenerateMethod(Method method)
        {
            return generateMethodWithBody(method, "");
        }

        protected string generateMethodWithBody(Method method, string body)
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
                if (i + 1 < method.Parameters.Length)
                {
                    result += ", ";
                }
            }

            result += ") {";
            
            if (body != "")
            {
                result += "\n" + GeneratorConf.ShiftCode(body, 1, Indent) + "";
            }

            if (method.Return != "" && method.Name != "constructor")
            {
                result += "\n" + Indent + "return smth;\n";
            }
            
            result += "}";

            return result;
        }

        protected string GenerateInit(Class @class)
        {
            var result = "";
            var body = "";

            var init = new Method
            {
                Name = "constructor",
                Parameters = new Parameter[]
                {
                }
            };

            var previousIsStatic = false;

            for (var i = 0; i < @class.Fields?.Length; i++)
            {
                var field = @class.Fields[i];
                
                if (field.Static)
                {
                    previousIsStatic = true;
                }

                /*init.Parameters = append(init.Parameters, Parameter{
                    Name:    field.Name,
                    Default: field.Default,
                })*/

                body += "this." + field.Name + " = " + field.Name + ";\n";

                /*if (i + 1 < @class.Fields.Length && !previousIsStatic)
                {
                    body += "\n";
                }*/

                previousIsStatic = false;
            }

            if (previousIsStatic && body.Length > Indent.Length)
            {
                body = body[body.Length - Indent.Length ].ToString();
            }

            if (@class.Parent != "")
            {
                body = "super();\n" + body;
                
            }

            result += generateMethodWithBody(init, body);
            
            return result;
        }
    }
}