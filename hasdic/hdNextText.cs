using System;
namespace hasdic
{
	public class hdNextText : hdNext
	{
		public hdNextText ()
			: base()
		{
		}
		
		public hdNextText (byte[] start)
			: base(start)
		{
		}
		
		public override byte ByteMin
		{
			get
			{
				return (byte)' ';
			}
		}
		
		public override byte ByteMax
		{
			get
			{
				return (byte)'~';
			}
		}
	}
}

