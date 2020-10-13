using System;

namespace Kryptoteket.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                new Startup().StartAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name}: {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
