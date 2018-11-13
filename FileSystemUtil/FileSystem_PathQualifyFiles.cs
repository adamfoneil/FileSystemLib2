using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemUtil
{
	public static partial class FileSystem
	{
		/// <summary>
		/// Converts a list of files to a dictionary where the key is unique portion of path and the value is the full path
		/// </summary>
		public static Dictionary<string, string> PathQualifyFiles(IEnumerable<string> fileNames)
		{
			// make sure we're starting with a unique list of files
			var uniqueFiles = fileNames.GroupBy(path => path).Select(grp => grp.Key).ToArray();

			var results = uniqueFiles.Select((path, index) => new FileItem()
			{
				Index = index,
				DisplayPath = Path.GetFileName(path),
				FullPath = path
			}).ToDictionary(item => item.Index);

			while (true)
			{
				var dups = results
					.GroupBy(item => item.Value.DisplayPath)
					.Where(grp => grp.Count() > 1);

				foreach (var dupGrp in dups)
				{
					var items = dupGrp.ToArray();
					for (int i = 0; i < items.Length; i++)
					{
						var item = items[i];
						results[item.Key].Depth++;
						results[item.Key].DisplayPath = item.Value.GetFilenameAtDepth();
					}
				}

				if (!dups.Any()) break;
			}

			return results.ToDictionary(item => item.Value.DisplayPath, item => item.Value.FullPath);
		}

		internal class FileItem
		{
			public int Index { get; set; }
			public string DisplayPath { get; set; }
			public string FullPath { get; set; }
			public int Depth { get; set; }

			public string GetFilenameAtDepth()
			{
				string[] folders = FullPath.Split('\\');
				return string.Join("\\", folders.Skip(folders.Length - Depth - 1));
			}
		}
	}
}
