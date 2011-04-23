using System;
namespace hasdic
{
	public static class hdFileFactory
	{
		public static hdFile CreateFile(string filename)
		{
			string[] fileParts = filename.Split('.');
			
			// validate
			if(fileParts.Length == 0||fileParts[fileParts.Length-1].Length==0)
				throw new ArgumentException("no filename provided");
			
			// switch on the file extension
			switch(fileParts[fileParts.Length-1])
			{
			case "sdb":
			default:
				return new hdFileSqlite(filename);
			}
		}
	}
}

