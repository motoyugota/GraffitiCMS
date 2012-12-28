using System;
using System.Xml.Serialization;

namespace Graffiti.Core
{
	/// <summary>
	///     Represents an Event, manages status, etc.
	/// </summary>
	[Serializable]
	public class EventDetails
	{
		private bool _enabled;
		private GraffitiEvent _event;
		private string _eventType;
		private string _xml;

		public string EventType
		{
			get { return _eventType; }
			set { _eventType = value; }
		}

		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		[XmlIgnore]
		public GraffitiEvent Event
		{
			get { return _event; }
			set { _event = value; }
		}

		public string Xml
		{
			get { return _xml; }
			set { _xml = value; }
		}
	}
}