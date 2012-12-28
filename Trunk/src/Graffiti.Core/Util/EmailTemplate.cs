namespace Graffiti.Core
{
	public class EmailTemplate
	{
		private bool _isHtml = true;
		public string TemplateName { get; set; }

		public string Subject { get; set; }

		public string To { get; set; }

		public string From { get; set; }

		public string ReplyTo { get; set; }

		public EmailTemplateToolboxContext Context { get; set; }

		public bool IsHTML
		{
			get { return _isHtml; }
			set { _isHtml = value; }
		}
	}
}