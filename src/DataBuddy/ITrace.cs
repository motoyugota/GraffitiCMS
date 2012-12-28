using System;
using System.Data.Common;

namespace DataBuddy
{
	public interface ITrace
	{
		Guid GetMarker();
		void Write(Guid marker, DbCommand cmd);
		void Write(Guid marker, string data);
		void Write(Guid marker, string format, params object[] data);
		void Separator();
	}
}