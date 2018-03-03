namespace CodeGen.generators
{
	public struct Package
	{
		public string Name;
		public Class[] Classes;
	}

	public struct Class
	{
		public string Name;
		public Field[] Fields;
		public Method[] Methods;
	}

	public struct Field
	{
		public string Name;
	}

	public struct Method
	{
		public string Name;
	}
}