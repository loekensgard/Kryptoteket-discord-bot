using System;

namespace Kryptoteket.Bot
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				new KryptoteketBot().StartAsync().GetAwaiter().GetResult();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
