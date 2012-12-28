using System;
using RssToolkit.Rss;

namespace Graffiti.Core
{
	[Serializable]
	public class Feed
	{
		private int _RequestInterval;
		private RssDocument _doc;
		private Guid _id;
		private DateTime _lastModified;
		private DateTime _lastSuccessfulRequest;
		private string _name;
		private string _url;

		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}

		public RssDocument Document
		{
			get { return _doc; }
			set { _doc = value; }
		}

		public DateTime LastRequested
		{
			get { return _lastModified; }
			set { _lastModified = value; }
		}

		public DateTime LastSuccessfulRequest
		{
			get { return _lastSuccessfulRequest; }
			set { _lastSuccessfulRequest = value; }
		}


		public int RequestInterval
		{
			get { return _RequestInterval; }
			set { _RequestInterval = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}


		public Guid Id
		{
			get { return _id; }
			set { _id = value; }
		}

		internal void ReLoad()
		{
			ReLoad(false);
		}

		internal void ReLoad(bool throwExceptionOnError)
		{
			try
			{
				LastRequested = DateTime.Now;
				RssDocument doc = RssDocument.Load(new Uri(Url));
				Document = doc;
				LastSuccessfulRequest = DateTime.Now;
				Log.Info("Feed", "The feed {0} was successfully updated", Url);
			}
			catch (Exception ex)
			{
				Log.Error("Feed", "The feed {0} (Name:{2} Id:{3} was not successfully updated. Reason: {1}", Url, ex.Message, Name,
				          Id);

				if (throwExceptionOnError)
					throw;
			}
		}
	}
}