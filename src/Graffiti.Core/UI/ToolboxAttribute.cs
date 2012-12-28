using System;

namespace Graffiti.Core
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ChalkAttribute : Attribute
	{
		//private bool _RequestOnly;

		//public bool RequestOnly
		//{
		//    get { return _RequestOnly; }
		//    set { _RequestOnly = value; }
		//}

		private string _key;

		public ChalkAttribute(string key)
		{
			_key = key;
		}

		public string Key
		{
			get { return _key; }
		}
	}
}