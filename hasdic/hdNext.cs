using System;
namespace hasdic
{
	public class hdNext
	{
		private byte[] last;
		
		public hdNext ()
			: this(null)
		{
		}
		
		public hdNext (byte[] start)
		{
			this.last = start;
		}
		
		public byte[] Next
		{
			get
			{
				return this.last = GetNext(last);
			}
		}
		
		public virtual byte ByteMin
		{
			get
			{
				return 0x00;
			}
		}
		
		public virtual byte ByteMax
		{
			get
			{
				return 0xFF;
			}
		}
		
		public byte[] GetNext(byte[] last)
		{
			// cover the special cases
			if(last == null)
				return new byte[0];
			if(last.Length == 0)
				return new byte[]{ByteMin};
			
			// increment
			byte[] next = last;
			for(int i=0;i<next.Length;i++)
			{
				// increment the current byte
				next[i]++;
				if(next[i] > ByteMax)
					next[i] = ByteMin;
				
				// if it doens't reset to 0, nothing else to do
				if(next[i]!=ByteMin)
				{
					// exit early
					return next;
				}
			}
			
			// if we get here, we zeroed all the bytes
			// if the highest byte is 0, need to create new zero byte
			if(next[next.Length-1]==ByteMin)
			{
				Array.Resize<byte>(ref next, next.Length+1);
				next[next.Length-1] = ByteMin;
			}
			
			return next;
		}
	}
}

