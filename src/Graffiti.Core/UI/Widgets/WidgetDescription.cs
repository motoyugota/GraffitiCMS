using System;

namespace Graffiti.Core
{
	/// <summary>
	///     Describes a widget
	/// </summary>
	public class WidgetDescription
	{
		private string _Id;
		private string _description;

		private string _name;
		private Type _type;

		public WidgetDescription(string id, string name, string desc, Type type)
		{
			_Id = id;
			_name = name;
			_description = desc;
			_type = type;
		}

		/// <summary>
		///     Although this value is a string, it should be treated like a Guid. :)
		/// </summary>
		public string UniqueId
		{
			get { return _Id; }
		}

		/// <summary>
		///     The name displayed in the available widget drop down.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}


		public string Description
		{
			get { return _description; }
		}


		/// <summary>
		///     The type of widget to invoke based on this attribute. It is externally set to readonly.
		/// </summary>
		public Type WidgetType
		{
			get { return _type; }
		}
	}
}