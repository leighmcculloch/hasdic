using System;
namespace hasdic
{
	public delegate bool hdFileRecordFoundEvent(hdRecord record);
	
	public interface hdFile
	{
		string FileFormat { get; }
		
		#region appending records
		void AppendOpen();
		void AppendRecord(hdRecord record);
		void AppendClose();
		#endregion
		
		#region finding records
		event hdFileRecordFoundEvent RecordFound;
		hdRecord[] FindRecords(byte[] hash);
		#endregion
		
		#region get last record
		hdRecord GetLastRecord();
		#endregion
	}
}

