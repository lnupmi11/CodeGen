namespace CodeGen.generators
{
	/// <summary>
	/// The structure that describes package. Contains classes and subpackages
	/// </summary>
	public struct Package
	{
		/// <summary>
		/// Represents the name of the package. Type: string 
		/// </summary>
		public string Name;
		
		/// <summary>
		/// Represents using of spaces or tabs. Type: boolean
		/// </summary>
		public bool UseSpaces;
		
		/// <summary>
		/// Represents classes. Type: array of type Class
		/// </summary>
		public Class[] Classes;
		
		/// <summary>
		/// Represents subpackages. Type: array of type Package
		/// </summary>
		public Package[] Packages;

		/// <inheritdoc />
		public override string ToString()
		{
			return base.ToString() + string.Format("{{ {0} {1} {2} }}",Name, Classes, Packages);
		}
	}
	
	/// <summary>
	/// The structure that describes class.
	/// Contains name, array of fields, methods and subclasses,
	/// parent class name, access specifier. Overrides ToString() method.
	/// </summary>
	public struct Class
	{
		/// <summary>
		/// Represents the name of the class. Type: string
		/// </summary>
		public string Name;
		
		/// <summary>
		/// Represents fields of the class. Type: array of type Field
		/// </summary>
		public Field[] Fields;
		
		/// <summary>
		/// Represents methods of the class. Type: array of type Method
		/// </summary>
		public Method[] Methods;
		
		/// <summary>
		/// Represents subclasses of the class. Type: array of type Class
		/// </summary>
		public Class[] Classes;
		
		/// <summary>
		/// Represents parent class name. Type: string
		/// </summary>
		public string Parent;
		
		/// <summary>
		/// Represents access level of the class. Type: string
		/// </summary>
		public string Access;

		/// <inheritdoc />
		public override string ToString()
		{
			return base.ToString() + string.Format("{{ {0} {1} {2} {3} {4} }}",Name, Parent, Fields, Methods, Classes);
		}
	}

	/// <summary>
	/// The structure that describes variable. Contains name, type and default value
	/// </summary>
	public class Variable
	{
		/// <summary>
		/// Represents the name of the variable. Type: string
		/// </summary>
		public string Name;
		
		/// <summary>
		/// Represents the type of the variable. Type: string
		/// </summary>
		public string Type;
		
		/// <summary>
		/// Represents default value of the varibale. Type: string
		/// </summary>
		public string Default;
	}

	/// <inheritdoc />
	/// <summary>
	/// The structure that describes field. Contains access, const and static properties. Inherits from Variable
	/// </summary>
	public class Field: Variable
	{
		/// <summary>
		/// Represents access level of the field. Type: string
		/// </summary>
		public string Access;
		
		/// <summary>
		/// Denotes if field is constant or not. Type: boolean
		/// </summary>
		public bool Const;
		
		/// <summary>
		/// Denotes if field is static or not. Type: boolean
		/// </summary>
		public bool Static;
	}

	/// <inheritdoc />
	/// <summary>
	/// The structure that describes parameter. Inherits from Variable
	/// </summary>
	public class Parameter: Variable
	{}
	
	/// <summary>
	/// The structure that describes method. Contains name, return type, access level, const and static properties
	/// and array of parameters 
	/// </summary>
	public struct Method
	{
		/// <summary>
		/// Represents the name of the method. Type: string
		/// </summary>
		public string Name;
		
		/// <summary>
		/// Represents return type of the method. Type: string
		/// </summary>
		public string Return;
		
		/// <summary>
		/// Represents access level of the method. Type: string
		/// </summary>
		public string Access;
		
		/// <summary>
		/// Denotes if the method is const or not. Type: boolean
		/// </summary>
		public bool Const;
		
		/// <summary>
		/// Denotes if the mathod is static or not. Type: boolean
		/// </summary>
		public bool Static;
		
		/// <summary>
		/// Represents parameters of the method. Type: array of type Parameter
		/// </summary>
		public Parameter[] Parameters;
	}
}
