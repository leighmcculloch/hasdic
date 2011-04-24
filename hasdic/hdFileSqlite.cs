using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
namespace hasdic
{
	public class hdFileSqlite : hdFile
	{
		private SqliteConnection connection = null;
		private SqliteTransaction transaction = null;
		private SqliteCommand appendCommand = null;
		private SqliteCommand findHashCommand = null;
		private SqliteCommand findHashWithLengthCommand = null;
		private SqliteCommand getLastCommand = null;
		public event hdFileRecordFoundEvent RecordFound;
		
		public string FileFormat { get { return "Sqlite Database"; } }
		
		public hdFileSqlite (string filename)
		{
			// create and open connection to database file
			this.connection = new SqliteConnection("Data Source="+filename+";Version=3;New=True;Journal Mode=Off;Synchronous=Off;Compression=True;");
			this.connection.Open();
			
			// create the database tables if they exist
			if(this.connection.GetSchema("Tables").Select("Table_Name = 'dictionary'").Length == 0)
			{
				SqliteCommand command = this.connection.CreateCommand();
				command.CommandText = "CREATE TABLE dictionary(data blob, hash_md5 blob, hash_sha1 blob);";
				command.ExecuteNonQuery();
			}
			
			// setup the append command and it's parameters
			this.appendCommand = this.connection.CreateCommand();
			this.appendCommand.CommandText = "INSERT INTO dictionary(data, hash_md5, hash_sha1) VALUES(?, ?, ?)";
			this.appendCommand.Parameters.Add(new SqliteParameter());
			this.appendCommand.Parameters.Add(new SqliteParameter());
			this.appendCommand.Parameters.Add(new SqliteParameter());
			
			// setup the find hash command and it's parameters
			this.findHashCommand = this.connection.CreateCommand();
			this.findHashCommand.CommandText = "SELECT * FROM dictionary WHERE hash_md5 = @hash"
																		+ " OR hash_sha1 = @hash";
			this.findHashCommand.Parameters.Add(new SqliteParameter("@hash"));
			
			// setup the find hash command and it's parameters
			this.findHashWithLengthCommand = this.connection.CreateCommand();
			this.findHashWithLengthCommand.CommandText = "SELECT * FROM dictionary WHERE length(data) == @dataLength"
																				+ " AND (hash_md5 = @hash"
																				+ " OR hash_sha1 = @hash)";
			this.findHashWithLengthCommand.Parameters.Add(new SqliteParameter("@dataLength"));
			this.findHashWithLengthCommand.Parameters.Add(new SqliteParameter("@hash"));
			
			// setup the get last record command
			this.getLastCommand = this.connection.CreateCommand();
			this.getLastCommand.CommandText = "SELECT * FROM dictionary ORDER BY rowid DESC LIMIT 1";
		}
		
		public void AppendOpen()
		{
			// begin a transaction if one has not begun already
			if(this.transaction==null)
				this.transaction = this.connection.BeginTransaction();
		}
		
		public void AppendRecord(hdRecord record)
		{
			// set the values for the parameters
			this.appendCommand.Parameters[0].Value = record.Data;
			this.appendCommand.Parameters[1].Value = record.HashMD5;
			this.appendCommand.Parameters[2].Value = record.HashSHA1;
			
			// execute the append command
			this.appendCommand.ExecuteNonQuery();
		}
		
		public void AppendClose()
		{
			// close off the transaction
			if(this.transaction!=null)
			{
				this.transaction.Commit();
				this.transaction = null;
			}
		}
		
		public hdRecord[] FindRecords(byte[] hash)
		{
			return FindRecords(hash, 0);
		}
		
		public hdRecord[] FindRecords(byte[] hash, int dataLength)
		{
			AppendClose();
			
			SqliteDataReader reader;
			
			if(dataLength > 0)
			{
				// set the values for the parameters
				this.findHashWithLengthCommand.Parameters[0].Value = dataLength;
				this.findHashWithLengthCommand.Parameters[1].Value = hash;
				
				// execute the find hash command
				reader = this.findHashWithLengthCommand.ExecuteReader();
			}
			else
			{
				// set the values for the parameters
				this.findHashCommand.Parameters[0].Value = hash;
				
				// execute the find hash command
				reader = this.findHashCommand.ExecuteReader();
			}
			
			// read each record that matches and store and trigger the event for it
			List<hdRecord> matches = new List<hdRecord>();
			while(reader.Read())
			{
				hdRecord r = new hdRecord((byte[])reader["data"], (byte[])reader["hash_md5"], (byte[])reader["hash_sha1"]);
				matches.Add(r);
				if (RecordFound != null)
				{
					if(!RecordFound(r))
					{
						return matches.ToArray();
					}
				}
			}
			
			return matches.ToArray();
		}
		
		public hdRecord GetLastRecord()
		{
			AppendClose();
			
			SqliteDataReader reader = this.getLastCommand.ExecuteReader();
			
			if(reader.HasRows)
			{
				if(reader.Read())
				{
					hdRecord r = new hdRecord((byte[])reader["data"], (byte[])reader["hash_md5"], (byte[])reader["hash_sha1"]);
					return r;
				}
			}
			
			return null;
		}
	}
}

