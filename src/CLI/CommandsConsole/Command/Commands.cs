using CommandConsole;
using System;
using System.Collections.Generic;
using System.IO;

namespace CommandsConsole.Command
{
    class Commands
    {
        private string commandLine = "";

        private readonly Addons addons;

        private readonly Subcommand subcommand;

        private List<CommandConsole.Command> saveData;

        public Commands()
        {
            this.addons = new Addons();
            this.subcommand = new Subcommand();
            this.saveData = new List<CommandConsole.Command>();

            saveData.Add(new CommandConsole.Command("view-path", "/"));
            saveData.Add(new CommandConsole.Command("drive", GetDrives(0).ToString()));
            saveData.Add(new CommandConsole.Command("path", GetSaveData("drive").DataSubcommand));
        }

        public Addons Addons { get => this.addons; }

        public string GetDrives(int index)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            return allDrives[index].Name;
        }

        public string CommandLine
        {
            get => commandLine;
            set => commandLine = value.Trim();
        }

        public void Load()
        {
            ICommand Addon = this.GetAddon(this.GetKeyCommand());

            if (Addon == null)
                return;

            try
            { 
                subcommand.CommandLine = this.CommandLine.Replace(this.GetKeyCommand(), "").Trim();
                subcommand.СonvertCommand(Addon.CommandMask);

                Addon.Data = subcommand.GetData();
                Addon.SaveData = this.saveData;

                Addon.Load();

                this.saveData = Addon.SaveData;
            }
            catch (CommandException commandException)
            {
                Console.WriteLine(commandException.Message);
            }
            subcommand.ResetData();
        }

        public void OutputBeginningLine()
        {
            Console.Write($"$ {this.GetSaveData("view-path").DataSubcommand} > ");
        }

        public ICommand GetAddon(string keyCommand)
        {
            ICommand Addon;
            foreach (Type t in this.Addons.AddonTypes)
            {
                Addon = (ICommand)Activator.CreateInstance(t);
                if (Addon.KeyCommand == keyCommand)
                {
                    return Addon;
                }
            }

            Console.WriteLine($"Не существует данной '{keyCommand}-' команды!");
            return null;
        }

        public string GetKeyCommand()
        {
            return (this.CommandLine.Contains(' ')) ? this.CommandLine.Substring(0, this.CommandLine.IndexOf(' ')) : this.CommandLine;
        }

        public CommandConsole.Command GetSaveData(string name)
        {
            if (saveData == null)
                return null;

            foreach (CommandConsole.Command SaveData in this.saveData)
            {
                if (SaveData.NameSubcommand == name)
                    return SaveData;
            }
            return null;
        }
    }
}
