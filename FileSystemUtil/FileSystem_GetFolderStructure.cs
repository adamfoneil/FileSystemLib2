using System;
using System.Collections.Generic;
using System.Linq;

namespace FileSystemUtil
{
	public static partial class FileSystem
	{
		public static Folder GetFolderStructure(IEnumerable<string> files, char separator = '\\', string rootName = @"\\root")
		{
			var result = new Folder() { Name = rootName, Separator = separator };
			GetFolderStructureR(result, files, 0, separator);
			return result;
		}

		private static void GetFolderStructureR(Folder parent, IEnumerable<string> files, int depth, char separator)
		{
			var childFolders = files
				.Where(s => s.StartsWith(parent.GetPath()))
				.Select(s => FolderNameAtDepth(s, depth, separator))
				.Where(s => !string.IsNullOrEmpty(s))
				.GroupBy(s => s).Select(grp => grp.Key).ToArray();

			depth++;

			List<Folder> folders = new List<Folder>();
			foreach (var folderName in childFolders)
			{
				Folder folder = new Folder() { Parent = parent, Name = folderName, Separator = separator };
				string path = folder.GetPath();
				folder.Files = FilesAtDepth(path, files, depth + 1, separator).OrderBy(s => s).ToArray();
				GetFolderStructureR(folder, files, depth, separator);
				folders.Add(folder);
			}
			parent.Folders = folders;

			depth--;
		}

		private static IEnumerable<string> FilesAtDepth(string parentPath, IEnumerable<string> files, int depth, char separator)
		{
			return files
				.Where(s => s.StartsWith(parentPath))
				.Select(s => s.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries))
				.Where(array => array.Length == depth)
				.Select(array => array[depth - 1]);
		}

		private static string FolderNameAtDepth(string path, int depth, char separator)
		{
			var folderNames = path.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToArray();
			if (depth < (folderNames.Length - 1)) return folderNames[depth];
			return null;
		}
	}

	public class Folder
	{
		public Folder Parent { get; set; }
		public string Name { get; set; }
		public IEnumerable<string> Files { get; set; }
		public IEnumerable<Folder> Folders { get; set; }
		public char Separator { get; set; }

		public string GetPath()
		{
			if (Parent != null) return Parent.GetPath() + Separator + Name;
			return string.Empty;
		}

		public string GetPath(string fileName)
		{
			return GetPath() + Separator + fileName;
		}
	}
}