using System.Collections.Generic;

namespace CommandConsole
{
    public abstract class AbstractCommand
    {
        public List<Command> Data { get; set; }

        public List<Command> SaveData { get; set; }

        public AbstractCommand()
        {
            SaveData = new List<Command>();
        }

        public Command GetData(string name)
        {
            return this.Data.Find(x => x.NameSubcommand == name);
        }

        public void AddSaveData(string name, string data)
        {
            if (this.SaveData.Exists(x => x.NameSubcommand == name))
            {
                this.SaveData.FindAll(s => s.NameSubcommand == name).ForEach(x => x.DataSubcommand = data);
                return;
            }

            SaveData.Add(new Command(name, data));
        }

        public Command GetSaveData(string name)
        {
            foreach (Command SaveData in this.SaveData)
            {
                if (SaveData.NameSubcommand == name)
                    return SaveData;
            }
            return null;
        }
    }
}
