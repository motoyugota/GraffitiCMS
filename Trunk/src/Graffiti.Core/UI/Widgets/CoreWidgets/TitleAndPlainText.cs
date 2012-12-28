using System.Collections.Specialized;
using System.Web;

namespace Graffiti.Core
{
	/// <summary>
	///     A widget which displays a title and the a block of text
	/// </summary>
	[WidgetInfo("e5aacfe7-7f00-4cfe-9233-bfe1a7d226c7", "Title and PlainText",
		"Renders a title and the data from a textbox")]
	public class TitleAndPlainText : Widget
	{
		public string Text;

		public override string Name
		{
			get
			{
				if (string.IsNullOrEmpty(Title))
					return "An empty box";
				else
					return Title + " (Title and PlainText widget)";
			}
		}

		protected override FormElementCollection AddFormElements()
		{
			FormElementCollection fec = new FormElementCollection();
			fec.Add(new TextFormElement("Title", "Title", "The title of the section"));
			fec.Add(new TextAreaFormElement("Text", "Your Content", null, 5));
			return fec;
		}

		public override StatusType SetValues(HttpContext context,
		                                     NameValueCollection nvc)
		{
			base.SetValues(context, nvc);
			Text = nvc["Text"];
			return StatusType.Success;
		}

		protected override NameValueCollection DataAsNameValueCollection()
		{
			NameValueCollection nvc = base.DataAsNameValueCollection();
			nvc["Text"] = Text;
			return nvc;
		}

		public override string RenderData()
		{
			return Text;
		}
	}
}