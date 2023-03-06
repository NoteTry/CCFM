using System;
using CommandsConsole.Command;

namespace CommandsConsole
{
    class Program
    {
        private static Commands Commands = new Commands();

        static void Main(string[] args)
        {
            
            try
            {
                Commands.Addons.Load();

                while (true)
                {
                    Commands.OutputBeginningLine();
                    Commands.CommandLine = Console.ReadLine().ToString();

                    if (Commands.CommandLine.Length == 0)
                        continue;

                    Commands.Load();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
