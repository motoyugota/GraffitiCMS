using System;
using System.Collections.Generic;

namespace Graffiti.Core
{
	[Serializable]
	public class CustomField
	{
		private bool _checked;
		private string _description;
		private bool _enabled;
		private FieldType _fieldType = FieldType.TextBox;
		private List<ListItemFormElement> _fields = new List<ListItemFormElement>();
		private Guid _id;
		private string _name;
		private int _rows = 5;

		public Guid Id
		{
			get { return _id; }
			set { _id = value; }
		}


		public FieldType FieldType
		{
			get { return _fieldType; }
			set { _fieldType = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}


		/// <summary>
		///     The number of rows to render for a text area type field
		/// </summary>
		public int Rows
		{
			get { return _rows; }
			set { _rows = value; }
		}

		public List<ListItemFormElement> ListOptions
		{
			get { return _fields; }
			set { _fields = value; }
		}


		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		/// <summary>
		///     Determines whether a CheckBox type field is checked by default
		/// </summary>
		public bool Checked
		{
			get { return _checked; }
			set { _checked = value; }
		}
	}

	public enum FieldType
	{
		CheckBox,
		List,
		TextBox,
		TextArea,
		File,
		WYSIWYG,
		DateTime
	}
}