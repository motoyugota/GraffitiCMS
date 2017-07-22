# Chalk Extension Example
View an example of a chalk extension.
{code:c#}
using System;using System.Web;using Graffiti.Core; namespace Graffiti.Demo {
        [Chalk("shareIt")](Chalk(_shareIt_))
        public class ShareIt
        {
            public string HTML(string message, Post post)
            {
                returnstring.Format(shareItBody, message, HttpUtility.HtmlEncode(post.Title), new Macros().FullUrl(post.Url));
            }

            private static readonly string shareItBody = "<div class = \"shareblock\"><strong>{0}</strong>" +
                  " <a title=\"Email {1}\" href = \"mailto:?body=Thought you might like this: {2}&subject={1}\">Email it!</a>" + 
                  " | <a href = \"http://del.icio.us/post?url={2}&title={1}\" title=\"Submit {1} to del.icio.us\" >bookmark it!</a>" +
                  " | <a href = \"http://www.digg.com/submit?url={2}&phase=2\" title=\"Submit {1} to digg.com\">digg it!</a>" +
                  " | <a href = \"http://reddit.com/submit?url={2}&title={1}\" title=\"Submit {1} to reddit.com\">reddit!</a></div>";
        }
 }
{code:c#}