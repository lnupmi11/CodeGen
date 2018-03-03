namespace CodeGen.generators
{
	public struct Package
	{
		public string Name;
		public bool UseSpaces;
		public Class[] Classes;
		public Package[] Packages;
		public override string ToString()
		{
			return base.ToString() + string.Format("{{ {0} {1} {2} }}",Name, Classes, Packages);
		}
	}

	public struct Parent
	{
		public string Name;
	}
	
	public struct Class
	{
		public string Name;
		public Field[] Fields;
		public Method[] Methods;
		public Class[] Classes;
		public string Parent;
		public string Access;
		public override string ToString()
		{
			return base.ToString() + string.Format("{{ {0} {1} {2} {3} {4} }}",Name, Parent, Fields, Methods, Classes);
		}
	}

	public class Variable
	{
		public string Name;
		public string Type;
		public string Default;
	}

	public class Field: Variable
	{
		public string Access;
		public bool Const;
		public bool Static;
	}

	public class Parameter: Variable
	{}
	
	public struct Method
	{
		public string Name;
		public string Return;
		public string Access;
		public bool Const;
		public bool Static;
		public Parameter[] Parameters;
	}
}