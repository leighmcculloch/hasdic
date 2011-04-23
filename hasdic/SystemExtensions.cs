using System;
namespace hasdic
{
	public static class SystemExtensions
	{
        public static bool IsText(this string s)
        {
			foreach(char c in s)
			{
				if(c < ' ' || c > '~')
				{
					return false;
				}
			}
			return true;
        }
		
		public static bool PartialCompare(this byte[] ba, byte[] partial)
		{
			if (ba.Length < partial.Length)
				return false;
			for (int i=0; i < partial.Length; i++)
			{
				if (ba[i] != partial[i])
					return false;
			}
			return true;
		}
	}
}

