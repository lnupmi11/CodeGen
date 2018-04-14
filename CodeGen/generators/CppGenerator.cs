using System.Collections.Generic;
using System.Linq;

namespace CodeGen.generators
{
    /// <inheritdoc />
    /// <summary>
    /// Generator for C++
    /// </summary>
    public class CppGenerator : Generator
    {
        private const string ClassFormat = "class {0} {1} \n{{{2}{3}{4}}}";
        private string Indent { get; set; } = GeneratorConf.GetIndent(true, 4);

        /// <inheritdoc />
        protected override string GenerateClass(Class @class)
        {
            string parent = "", @public = "", @protected = "", @private = "";
            if(@class.Parent?.Length > 0)
            {
                parent = " : " + @class.Parent + " " + @class.Parent;
            };
            @public = GenerateSection("public", @class);
        	@protected = GenerateSection("protected", @class);
	        @private = GenerateSection("private", @class);
            return string.Format(ClassFormat, @class.Name, parent, @public, @protected, @private);
        }


        /// <inheritdoc />
        public override string GenerateField(Field field)
        {
            var result = Indent;
            if (field.Static)
            {
                result += "static ";
            }
            if (field.Const)
            {
                result += "const ";
            }
            result += field.Type + " ";
            result += field.Name;
            if (field.Default != "")
            {
                result += " = " + field.Default;
            }
            result += ";";
            return result;
        }

        /// <inheritdoc />
        public override string GenerateMethod(Method method)
        {
            string result = "";

            if (method.Static)
            {
                result += "static ";
            }
            switch (method.Return)
            {
                case "":
                    result += "void ";
                    break;
                default:
                    result += method.Return + " ";
                    break;
            }
            result += method.Name;
            result += "(";
            for (var i = 0; i < method.Parameters?.Length; i++)
            {
                var parameter = method.Parameters[i];
//                if (parameter.Const)
//                {
//                    result += "const ";
//                }
                result += parameter.Type + " " + parameter.Name;
                if (i + 1 < method.Parameters?.Length)
                {
                    result += ", ";
                }
            }

            result += ");";
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access"></param>
        /// <param name="class"></param>
        /// <returns></returns>
        private string GenerateSection(string access, Class @class)
        {
            var result = @class.Fields?.Where(field => access == field.Access).Aggregate("", (current, field) => current + (GenerateField(field) + "\n"));

            result = @class.Methods?.Where(method => access == method.Access).Aggregate(result, (current, method) => current + (GeneratorConf.ShiftCode(GenerateMethod(method), 1, Indent) + "\n"));
            result = @class.Classes?.Where(clas => access == clas.Access).Aggregate(result, (current, clas) => current + (GeneratorConf.ShiftCode(GenerateClass(clas), 1, Indent) + "\n"));
            if (result != "")
            {
                result = "\n" + access + ":\n" + result;
	        }
            return result;
        }
    }
}
