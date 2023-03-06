using System;

namespace CommandConsole
{
    public class Command
    {
        public string NameSubcommand { get; set; }

        public string DataSubcommand { get; set; }

        public Command(string NameSubcommand, string DataSubcommand)
        {
            this.NameSubcommand = NameSubcommand;
            this.DataSubcommand = DataSubcommand;
        }
    }
}
