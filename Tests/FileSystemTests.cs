using FileSystemUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class FileSystemTests
	{
		private static string[] GetFileList()
		{
			return new string[]
			{
				@"C:\Users\Adam\Repos\Hello\Whatever\bin\Debug\Whatever.exe",
				@"C:\Users\Adam\Repos\Hello\Whatever\bin\Release\Whatever.exe",
				@"C:\Users\Adam\Repos\Hello\Whatever\Shaskra.doc",
			};
		}

		[TestMethod]
		public void PathQualifyFilesExample()
		{
			string[] files = GetFileList();

			var result = FileSystem.PathQualifyFiles(files);

			Assert.IsTrue(result.ContainsKey(@"Debug\Whatever.exe"));
			Assert.IsTrue(result.ContainsKey(@"Release\Whatever.exe"));
			Assert.IsTrue(result.ContainsKey(@"Shaskra.doc"));
		}

		[TestMethod]
		public void GetFolderStructureExample()
		{
			var result = FileSystem.GetFolderStructure(GetFileList());
		}
	}
}