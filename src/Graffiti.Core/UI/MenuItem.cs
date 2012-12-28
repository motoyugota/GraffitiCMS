using System;
using System.Collections.Generic;
using System.Reflection;

namespace Graffiti.Core
{
	public class MenuItem
	{
		public MenuItem(string name, string command, string description, string commandMenu)
		{
			Name = name;
			Command = command;
			Description = description;
			CommandMenu = commandMenu;
		}

		public string Name { get; set; }

		public string Command { get; set; }

		public string Description { get; set; }

		public string CommandMenu { get; set; }

		public static List<MenuItem> GetMenuItems()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();

			var types = assembly.GetTypes();

			var menuItems = new List<MenuItem>();

			foreach (Type t in types)
			{
				if (t.IsClass)
				{
					if (t.GetInterface("IMenuItem") != null)
					{
						object temp = Activator.CreateInstance(t);

						menuItems.AddRange((List<MenuItem>) t.InvokeMember("GetMenuItems",
						                                                   BindingFlags.Default | BindingFlags.InvokeMethod,
						                                                   null, temp, null));
					}
				}
			}

			return menuItems;
		}
	}
}