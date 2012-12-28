using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Graffiti.Core
{
	/// <summary>
	///     Enables adding a dropdown list to a dynamic form.
	/// </summary>
	public class ListFormElement : FormElement
	{
		private Queue<ListItemFormElement> _items = new Queue<ListItemFormElement>();

		public ListFormElement(string name, string desc, string tip)
			: base(name, desc, tip)
		{
		}


		public void Add(ListItemFormElement item)
		{
			_items.Enqueue(item);
		}

		public override void Write(StringBuilder sb, NameValueCollection nvc)
		{
			string checkedValue = nvc[Name];
			if (checkedValue == null)
			{
				foreach (ListItemFormElement li in _items)
				{
					if (li.IsDefault)
						checkedValue = li.Value;
				}
			}


			sb.Append("\n<h2>");
			sb.Append(Description);

			sb.AppendFormat(": <select id=\"{0}\" name=\"{0}\" style=\"width:150px;margin-left:5px;\">",
			                Name);


			foreach (ListItemFormElement li in _items)
			{
				sb.AppendFormat("<option value=\"{0}\" {2}>{1}</option>", li.Value,
				                li.Text,
				                checkedValue == li.Value ? "selected=\"selected\"" : null
					);
			}

			sb.Append("</select>");

			sb.Append(SafeToolTip(true));
			sb.Append("</h2>");
			sb.Append("\n");
		}
	}

	[Serializable]
	public class ListItemFormElement
	{
		private bool _isDefault;
		private string _text;
		private string _value;

		public ListItemFormElement()
		{
		}

		public ListItemFormElement(string text, string val)
			: this(text, val, false)
		{
		}

		public ListItemFormElement(string text, string val, bool isDefault)
		{
			_text = text;
			_value = val;
			_isDefault = isDefault;
		}

		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}

		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		public bool IsDefault
		{
			get { return _isDefault; }
			set { _isDefault = value; }
		}
	}
}