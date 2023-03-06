using CommandConsole;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace CommandsConsole.Command.CommandsList
{
    class List : AbstractCommand, ICommand
    {
        public string KeyCommand { get => "ls"; }

        public string CommandMask { get => "(str)"; }

        public void Load()
        {
            string dir = this.GetData("str").DataSubcommand;

            this.ViewContentsDirectory(this.GetSaveData("path").DataSubcommand + dir);
        }

        public void ViewContentsDirectory(string path)
        {
            if (!Directory.Exists(path))
                throw new CommandException($"Не существует такой директории! '{path}'");

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var file in directoryInfo.GetFiles())
            {
                Console.WriteLine(file.Name);
            }
            Console.ResetColor();

            foreach (string folder in Directory.GetDirectories(path))
            {
                var folderData = Directory.CreateDirectory(folder);
                Console.WriteLine(folderData.Name + "\t");
            }
        }
    }
}
