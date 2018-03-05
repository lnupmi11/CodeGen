using System.Collections.Generic;

namespace CodeGen.generators
{
	/// <inheritdoc />
	/// <summary>
	/// Ruby language generator
	/// </summary>
	public class RubyGenerator : Generator
	{
		/// <inheritdoc />
		public override Dictionary<string, string> Generate(Package pkg)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		protected override string GenerateClass(Class @class)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		protected override string GenerateField(Field field)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		protected override string GenerateMethod(Method method)
		{
			throw new System.NotImplementedException();
		}
	}
}