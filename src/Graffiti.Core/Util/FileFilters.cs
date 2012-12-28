using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Graffiti.Core
{
	public static class FileFilters
	{
		private static Regex excludedFileRegex;
		private static Regex editableFileRegex;
		private static Regex versionableFileRegex;
		private static Regex isDownloadableRegex;
		private static Regex isLinkableRegex;
		private static Regex isDeleteable;

		private static Regex CreateRegex(string key)
		{
			string pattern = "(" +
			                 string.Join("|",
			                             ConfigurationManager.AppSettings[key].Split(new[] {',', ' '},
			                                                                         StringSplitOptions.RemoveEmptyEntries)) +
			                 ")$";
			return new Regex(pattern, RegexOptions.IgnoreCase);
		}

		public static bool IsValidFile(string name)
		{
			if (excludedFileRegex == null)
				excludedFileRegex = CreateRegex("Graffiti:FileBrowser:Excluded");

			return !excludedFileRegex.IsMatch(name);
		}

		public static bool IsEditable(string name)
		{
			if (editableFileRegex == null)
				editableFileRegex = CreateRegex("Graffiti:FileBrowser:Editable");

			return editableFileRegex.IsMatch(name);
		}

		public static bool IsVersionable(string name)
		{
			if (versionableFileRegex == null)
				versionableFileRegex = CreateRegex("Graffiti:FileBrowser:Versionable");

			return versionableFileRegex.IsMatch(name);
		}

		public static bool IsLinkable(string name)
		{
			if (isLinkableRegex == null)
				isLinkableRegex = CreateRegex("Graffiti:FileBrowser:NoLink");

			return !isLinkableRegex.IsMatch(name);
		}

		public static bool IsDownloadable(string name)
		{
			if (isDownloadableRegex == null)
				isDownloadableRegex = CreateRegex("Graffiti:FileBrowser:NoDownload");

			return !isDownloadableRegex.IsMatch(name);
		}

		public static bool IsDeletable(string name)
		{
			if (isDeleteable == null)
				isDeleteable = CreateRegex("Graffiti:FileBrowser:NoDelete");

			return !isDeleteable.IsMatch(name);
		}
	}
}