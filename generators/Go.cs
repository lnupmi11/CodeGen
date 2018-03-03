using System;
using System.Collections.Generic;

namespace CodeGen.generators
{
	public class GoGenerator : Generator
	{
		public override Dictionary<string, string> Generate(Package pkg)
		{
			return new Dictionary<string, string>();
		}

		protected override string GenerateField(Field field)
		{
			return "";
		}

		protected override string GenerateMethod(Field field)
		{
			return "";
		}

		protected override string GenerateClass(Field field)
		{
			return "";
		}
	}
}