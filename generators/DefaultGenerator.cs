using System.Collections.Generic;

namespace CodeGen.generators
{
	public abstract class Generator
	{
		public abstract Dictionary<string, string> Generate(Package pkg);
		protected abstract string GenerateField(Field field);
		protected abstract string GenerateMethod(Field field);
		protected abstract string GenerateClass(Field field);
	}

	public class DefaultGenerator
	{
		public static Package ExamplePkg = new Package
		{
			Name = "main",
			Classes = new[]
			{
				new Class
				{
					Name = "Fruit",
					Fields = new[]
					{
						new Field
						{
							Name = "colour"
						},
						new Field
						{
							Name = "size"
						}
					}
				},
				new Class
				{
					Name = "Apple"
				}
			}
		};
	}
}