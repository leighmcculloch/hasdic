using System;
using System.Threading;
using System.Text;

namespace hasdic
{
	partial class MainClass
	{
		private static void Search(hdOptions options)
		{
			string hash = options["--hash"];
			if(hash == null || hash.Length == 0 || hash.Length % 2 != 0)
			{
				Console.Write("Please enter a full or partial SHA1 hash. ");
				Console.WriteLine("A partial SHA1 hash must have even length.");
				return;
			}
			
			int dataLength = 0;
			try
			{
				dataLength = int.Parse(options["--length"]);
			}
			catch { }
			
			file.RecordFound += delegate(hdRecord record)
			{
				Console.Write(record.ToString());
				
				// add text if the data is textual
				if(record.DataAsText.IsText())
				{
					Console.Write(" [" + record.DataAsText + "]");
				}
				
				// new line
				Console.WriteLine();
				
				return false;
			};
			
			Console.WriteLine("====================================");
			Console.WriteLine("Matches: ");
			
			DateTime dtStart = DateTime.Now;
			file.FindRecords(hdRecord.ByteArrayFromString(hash), dataLength);
			DateTime dtFinish = DateTime.Now;
			TimeSpan searchTime = dtFinish.Subtract(dtStart);
			
			// matches will be printed in the Record Found function above which is called back
			
			Console.WriteLine("====================================");
			Console.WriteLine("Search performed in " + searchTime.TotalSeconds + " seconds.");
		}
	}
}


