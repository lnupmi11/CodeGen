using System.Xml.Serialization;

namespace CodeGen.generators
{
	/// <summary>
	/// The structure that describes package. Contains classes and subpackages
	/// </summary>
	[XmlRoot("Package")]
	public class Package
	{
		/// <summary>
		/// Represents the name of the package. Type: string 
		/// </summary>
		[XmlElement("name")]
		public string Name { get; set; }

		/// <summary>
		/// Represents using of spaces or tabs. Type: boolean
		/// </summary>
		[XmlElement("use_spaces")]
		public bool UseSpaces { get; set; }
		
		/// <summary>
		/// Represents classes. Type: array of type Class
		/// </summary>
		[XmlElement("classes")]
		public Class[] Classes { get; set; }
		
		/// <summary>
		/// Represents subpackages. Type: array of type Package
		/// </summary>
		[XmlElement("Packages")]
		public Package[] Packages { get; set; }

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
	public class Class
	{
		/// <summary>
		/// Represents the name of the class. Type: string
		/// </summary>
		[XmlElement("name")]
		public string Name { get; set; }
		
		/// <summary>
		/// Represents fields of the class. Type: array of type Field
		/// </summary>
		[XmlElement("fields")]
		public Field[] Fields { get; set; }
		
		/// <summary>
		/// Represents methods of the class. Type: array of type Method
		/// </summary>
		[XmlElement("methods")]
		public Method[] Methods { get; set; }
		
		/// <summary>
		/// Represents subclasses of the class. Type: array of type Class
		/// </summary>
		[XmlElement("classes")]
		public Class[] Classes { get; set; }
		
		/// <summary>
		/// Represents parent class name. Type: string
		/// </summary>
		[XmlElement("parent")]
		public string Parent { get; set; }
		
		/// <summary>
		/// Represents access level of the class. Type: string
		/// </summary>
		[XmlElement("access")]
		public string Access { get; set; }

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
		[XmlElement("name")]
		public string Name { get; set; }
		
		/// <summary>
		/// Represents the type of the variable. Type: string
		/// </summary>
		[XmlElement("type")]
		public string Type { get; set; }
		
		/// <summary>
		/// Represents default value of the varibale. Type: string
		/// </summary>
		[XmlElement("default")]
		public string Default { get; set; }
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
		[XmlElement("access")]
		public string Access { get; set; }
		
		/// <summary>
		/// Denotes if field is constant or not. Type: boolean
		/// </summary>
		[XmlElement("const")]
		public bool Const { get; set; }
		
		/// <summary>
		/// Denotes if field is static or not. Type: boolean
		/// </summary>
		[XmlElement("static")]
		public bool Static { get; set; }
		
		/// <summary>
		/// Denotes if generate getter or not. Type: boolean
		/// </summary>
		[XmlElement("getter")]
		public bool Getter { get; set; }
		
		/// <summary>
		/// Denotes if generate setter or not. Type: boolean
		/// </summary>
		[XmlElement("setter")]
		public bool Setter { get; set; }
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
	public class Method
	{
		/// <summary>
		/// Represents the name of the method. Type: string
		/// </summary>
		[XmlElement("name")]
		public string Name { get; set; }
		
		/// <summary>
		/// Represents return type of the method. Type: string
		/// </summary>
		[XmlElement("return")]
		public string Return { get; set; }
		
		/// <summary>
		/// Represents access level of the method. Type: string
		/// </summary>
		[XmlElement("access")]
		public string Access { get; set; }
		
		/// <summary>
		/// Denotes if the method is const or not. Type: boolean
		/// </summary>
		[XmlElement("const")]
		public bool Const { get; set; }
		
		/// <summary>
		/// Denotes if the mathod is static or not. Type: boolean
		/// </summary>
		[XmlElement("static")]
		public bool Static { get; set; }
		
		/// <summary>
		/// Represents parameters of the method. Type: array of type Parameter
		/// </summary>
		[XmlElement("parameters")]
		public Parameter[] Parameters { get; set; }
	}
}
