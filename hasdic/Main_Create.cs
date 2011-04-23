using System;
using System.Threading;
using System.Text;

namespace hasdic
{
	partial class MainClass
	{
		private static void Create(hdOptions options)
		{
			// determine the starting point in the creation of the dictinoary (or continuing)
			hdNext n;
			hdRecord last = file.GetLastRecord();
			if(options["--text"]!=null)
			{
				if(last==null)
					n = new hdNextText();
				else
					n = new hdNextText(last.Data);
			}
			else
			{
				if(last==null)
					n = new hdNext();
				else
					n = new hdNext(last.Data);
			}
			
			if(last!=null)
			{
				Console.Write("Dictionary already exists, last being: "+last.DataHexString);
				if(last.DataAsText.IsText())
				{
					Console.Write(" [" + last.DataAsText + "]");
				}
				Console.WriteLine();
			}
			
			Int64 count=0;
			DateTime start = DateTime.Now;
			file.AppendOpen();
			while(true)
			{
				// create record for next
				hdRecord r = new hdRecord(n.Next);
				
				// only record records that are textual
				if(options["--verbose"]!=null)
				{
					Console.Write(r);
					if(r.DataAsText.IsText())
					{
						Console.Write(" [" + r.DataAsText + "]");
					}
					Console.WriteLine();
				}
				else if(count % 1000 == 0)
				{
					Console.Write('.');
				}
				
				// update file with next record
				file.AppendRecord(r);
				
				// flush
				if((count+1) % 100000 == 0)
				{
					file.AppendClose();
					file.AppendOpen();
					Console.WriteLine();
					Console.Write("100000 records in "+(DateTime.Now.Subtract(start).TotalSeconds)+" seconds");
					Console.Write(": Last being "+r.DataHexString);
					if(r.DataAsText.IsText())
					{
						Console.Write(" [" + r.DataAsText + "]");
					}
					Console.WriteLine();
					start = DateTime.Now;
				}
				
				count++;
			}
		}
	}
}

