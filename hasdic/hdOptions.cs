using System;
namespace hasdic
{
	public class hdOptions
	{
		private string[] args;
		public hdOptions(string[] args)
		{
			this.args = new string[args.Length];
			Array.Copy(args, this.args, args.Length);
		}
		public string this[string option]
		{
			get
			{
				string s = Array.Find(this.args, element => element.StartsWith(option));
				try
				{
					return s.Substring(s.IndexOf('=')+1);
				}
				catch
				{
					if(s == null)
						return null;
					else
						return string.Empty;
				}
			}
		}
	}
}

