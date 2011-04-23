using System;
using System.Threading;
using System.Text;

namespace hasdic
{
	partial class MainClass
	{
		private static hdFile file = null;
		
		public static void Main(string[] args)
		{
			string filename = "dictionary.sdb";
			hdOptions options = new hdOptions(args);
			
			if(options["--dic"]!=null)
			{
				filename = options["--dic"];
				if(filename.Length == 0)
				{
					Console.WriteLine("Dictionary must be specified.");
					return;
				}
			}
			file = hdFileFactory.CreateFile(filename);
			
			if(options["search"]!=null)
			{
				Console.WriteLine("HASh DICtionary: Searching Dictionary (" + filename + ")");
				Console.WriteLine("Dictionary File Format: " + file.FileFormat);
				Search(options);
			}
			else if(options["create"]!=null)
			{
				Console.WriteLine("HASh DICtionary: Creating Dictionary (" + filename + ")");
				Console.WriteLine("Dictionary File Format: " + file.FileFormat);
				Create(options);
			}
			else
			{
				Console.WriteLine("HASh DICtionary");
				Console.WriteLine("Author: Leigh McCulloch");
				Console.WriteLine();
				Console.WriteLine("Specify a dictionary (optional):");
				Console.WriteLine("    --dic=dictionary.csv          (default)");
				Console.WriteLine();
				Console.WriteLine("Search an existing dictionary:");
				Console.WriteLine("    search --hash=h               where h is the SHA1 hash");
				Console.WriteLine();
				Console.WriteLine("Create a new dictionary. If the dictionary already exists, the existing");
				Console.WriteLine("dictionary will be opened and continued from it's last record:");
				Console.WriteLine("    create                        create dictionary");
				Console.WriteLine("    create --text                 text records only");
				Console.WriteLine("    create --verbose              turn on verbose output");
			}
		}
	}
}

