using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telligent.Glow;

namespace Graffiti.Core
{
	public class Menu : WebControl
	{
		#region Javascript fired when a menu item is clicked

		private string InsertElementJS = @"
            function SetText(target, text)
            {
                targetel = document.getElementById(target);
                
                // ie
                if (document.selection)
                {
                    targetel.focus();
                    var sel = document.selection.createRange();
                    sel.moveStart('character',0);
                    sel.select();
                    sel.moveEnd('character',1);
                    sel.text = '';
                    sel.select();
                    sel.text = text;
                }
                // moz/ns
                else if (targetel.selectionStart || targetel.selectionStart == '0') 
                {
                    var startPos = targetel.selectionStart;
                    var endPos = targetel.selectionEnd;
                    targetel.value = targetel.value.substring(0, startPos) + text + targetel.value.substring(endPos, targetel.value.length);
                    targetel.selectionStart = startPos + text.length;
                    targetel.selectionEnd = startPos + text.length;
                    targetel.focus();
                } 
                else 
                {
                    targetel.value += text;
                }

                return false;
            }
        ";

		#endregion

		private string _editor;
		private string editorControl; // set in onload
		private string mainMenuUL;

		public string Editor
		{
			get { return _editor; }
			set { _editor = value; }
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (String.IsNullOrEmpty(_editor))
				throw new Exception(
					"Editor is a required property. Be sure you set Editor=\"TextBoxName\" in the declaration of the control.");

			if (FindControl(_editor) != null)
				editorControl = (FindControl(_editor)).ClientID;
			else
				throw new Exception(String.Format("The editor with the value \"{0}\" could not be found.", _editor));

			// add the javascript
			Page.ClientScript.RegisterClientScriptBlock(GetType(), "insertElementJS", InsertElementJS, true);

			// get all of the menu items
			var menuItems = MenuItem.GetMenuItems();

			StringBuilder mainMenu = new StringBuilder();

			// get all the main menu parents
			var mainMenuParents = new List<string>();
			foreach (MenuItem item in menuItems)
			{
				if (!mainMenuParents.Contains(item.CommandMenu.Trim()))
					mainMenuParents.Add(item.CommandMenu.Trim());
			}

			// build the menu
			mainMenu.Append("<ul class=\"MenuHeader\">");

			// create a new popupmenu for each menu parent and populate it's children
			foreach (string mainMenuParent in mainMenuParents)
			{
				PopupMenu menu = new PopupMenu();
				menu.ID = "mnu" + mainMenuParent;

				menu.CloseOnMouseOut = true;
				menu.MenuGroupCssClass = "MenuGroup";
				menu.MenuItemCssClass = "MenuItem";
				menu.MenuItemSelectedCssClass = "MenuItemSelected";
				menu.MenuItemExpandedCssClass = "MenuItemSelected";

				var thisMenuItems = menuItems.FindAll(
					delegate(MenuItem mi) { return mi.CommandMenu.Trim() == mainMenuParent; });

				foreach (MenuItem mi in thisMenuItems)
				{
					PopupMenuItem popupItem = new PopupMenuItem();
					popupItem.Text = "<span title=\"" + mi.Description + "\">" + mi.Name + "</span>";
					popupItem.OnClickClientFunction = "new Function ('SetText(\\'" + editorControl + "\\', \\'" + mi.Command + "\\')')";

					menu.MenuItems.Add(popupItem);
				}

				Controls.Add(menu);

				mainMenu.Append(String.Format("<li><a onmouseover=\"{0}.OpenAtElement(this);\">{1}</a></li>",
				                              menu.ClientID, mainMenuParent));
			}

			mainMenu.Append("</ul>");

			mainMenuUL = mainMenu.ToString();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

			writer.Write(mainMenuUL);
		}
	}
}