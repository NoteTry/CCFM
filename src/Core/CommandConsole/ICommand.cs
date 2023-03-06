using System.Collections.Generic;

namespace CommandConsole
{
    public interface ICommand
    {
        string KeyCommand { get; }
        List<Command> Data { get; set; }
        List<Command> SaveData { get; set; }
        string CommandMask { get; }
        void Load();
    }
}
