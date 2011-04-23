using System;
using System.Security.Cryptography;
using System.Text;
namespace hasdic
{
	public class hdRecord
	{
		private const char StringDelimeter = ',';
		
		private static MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
		private static SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
		
		private byte[] data;
		private byte[] hash_md5;
		private byte[] hash_sha1;
		
		#region Data Accessors
		public byte[] Data
		{
			get
			{
				byte[] data = new byte[this.data.Length];
				Array.Copy(this.data, data, data.Length);
				return data;
			}
		}
		
		public string DataHexString
		{
			get
			{
				return ByteArrayToString(this.data);
			}
		}
		
		public string DataAsText
		{
			get
			{
				char[] chars = new char[this.data.Length];
				Array.Copy(this.data, chars, chars.Length);
				return new string(chars);
			}
		}
		#endregion
		
		#region Hash MD5 Accessors
		public byte[] HashMD5
		{
			get
			{
				byte[] hash_md5 = new byte[this.hash_md5.Length];
				Array.Copy(this.hash_md5, hash_md5, hash_md5.Length);
				return hash_md5;
			}
		}
		
		public string HashMD5HexString
		{
			get
			{
				return ByteArrayToString(this.hash_md5);
			}
		}
		#endregion
		
		#region Hash SHA1 Accessors
		public byte[] HashSHA1
		{
			get
			{
				byte[] hash_sha1 = new byte[this.hash_sha1.Length];
				Array.Copy(this.hash_sha1, hash_sha1, hash_sha1.Length);
				return hash_sha1;
			}
		}
		
		public string HashSHA1HexString
		{
			get
			{
				return ByteArrayToString(this.hash_sha1);
			}
		}
		#endregion
		
		public hdRecord(byte[] data)
		{
			this.data = data;
			GenerateHashes();
		}
		
		public hdRecord(byte[] data, byte[] hash_md5, byte[] hash_sha1)
		{
			this.data = data;
			this.hash_md5 = hash_md5;
			this.hash_sha1 = hash_sha1;
		}
		
		public hdRecord(string recordString)
		{
			string[] pieces = recordString.Split(hdRecord.StringDelimeter);
			this.data = ByteArrayFromString(pieces[0]);
			this.hash_sha1 = ByteArrayFromString(pieces[1]);
		}
		
		private void GenerateHashes()
		{
		    this.hash_md5 = MD5.ComputeHash(this.data);
		    this.hash_sha1 = SHA1.ComputeHash(this.data);
		}
		
		public override string ToString()
		{
			int length = (this.data.Length*2)+1+(this.hash_sha1.Length*2);
			StringBuilder sb = new StringBuilder(length);
			
			// add data
			sb.Append(this.DataHexString);
			
			// add separator
			sb.Append(hdRecord.StringDelimeter);
			
			// add hash md5
			sb.Append(this.HashMD5HexString);
			
			// add separator
			sb.Append(hdRecord.StringDelimeter);
			
			// add hash sha1
			sb.Append(this.HashSHA1HexString);
			
			return sb.ToString();
		}
		
		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder sb = new StringBuilder();
			foreach(byte b in ba)
			{
				sb.AppendFormat("{0:X2}", b);
			}
			return sb.ToString();
		}
		
		public static byte[] ByteArrayFromString(string s)
		{
			int numChars = s.Length;
			byte[] ba = new byte[numChars/2];
			for (int i=0;i<numChars;i+=2)
			{
				ba[i/2] = Convert.ToByte(s.Substring(i, 2), 16);
			}
			return ba;
		}
	}
}

