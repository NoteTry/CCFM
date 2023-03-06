using CommandConsole;
using System.IO;

namespace CommandsConsole.Command.CommandsList
{
    /// <summary>
    /// Класс Cd
    /// Отвечат за переход по деректориям
    /// </summary>
    public class Cd : AbstractCommand, ICommand
    {
        public string KeyCommand { get => "cd"; }

        public string CommandMask { get => "(str)"; }

        public void Load()
        {
            string param = this.GetData("str").DataSubcommand;

            this.AddSaveData("path", this.GetPath(param));
            this.AddSaveData("view-path", this.GetViewPath(param));
        }

        public string GetPath(string param)
        {
            string drive = this.GetDrives(0).ToString();

            if (param == "")
                return this.GetDrives(0).ToString();

            if (!Directory.Exists(drive + param))
                throw new CommandException($"Не существует данной '{drive + param}' директории!");

            return drive + param;
        }

        public string GetViewPath(string param)
        {
            string drive = this.GetDrives(0).ToString();

            if (param == drive || param == "")
                return "/";

            return param;
        }

        public string GetDrives(int index)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            return allDrives[index].Name;
        }
    }
}
