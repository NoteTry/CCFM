using System;
using CommandConsole;

namespace CommandsConsole.Command.CommandsList
{
    class Clear : AbstractCommand, ICommand
    {
        public string KeyCommand { get => "clear"; }

        public string CommandMask { get => ""; }

        public void Load()
        {
            Console.Clear();
        }
    }
}
