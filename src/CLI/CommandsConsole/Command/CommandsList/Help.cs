using System;
using System.Collections;
using CommandConsole;

namespace CommandsConsole.Command.CommandsList
{
    class Help : AbstractCommand, ICommand
    {
        public string KeyCommand { get => "help"; }

        public string CommandMask { get => ""; }

        private PrintConsol PrintConsol;

        private ArrayList Arlist;

        public void Load()
        {
            this.PrintConsol = new PrintConsol(50);
            this.Arlist = new ArrayList();

            this.info();
        }

        private void info()
        {
            Addons Addons = new Addons();

            Addons.Load();

            ICommand Addon;

            foreach (Type t in Addons.AddonTypes)
            {
                Addon = (ICommand)Activator.CreateInstance(t);
                this.Arlist.Add(new string[] { Addon.KeyCommand, Addon.CommandMask });
            }
            string[] title = new string[] { "Commands", "Parameters" };
            PrintConsol.PrintLine();
            PrintConsol.PrintRow(title[0], title[1]);
            PrintConsol.PrintLine();
            foreach (string[] item in Arlist)
            {
                PrintConsol.PrintRow(item[0], item[1]);
            }
            PrintConsol.PrintLine();
        }
    }
}
