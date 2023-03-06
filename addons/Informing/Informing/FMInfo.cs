using System;
using System.IO;
using CommandConsole;

namespace Informing
{
    public class FMInfo : AbstractCommand, ICommand
    {
        public string KeyCommand { get => "fm"; }

        public string CommandMask { get => "<f,d>.(str)"; }

        private string MainDir { get => this.GetSaveData("path").DataSubcommand; }

        public void Load()
        {
            Command pathFile = this.GetData("f");
            Command pathDir = this.GetData("d");

            if (pathFile != null)
                this.GetInfoFile((this.IsPath(pathFile.DataSubcommand) ? pathFile.DataSubcommand : this.MainDir + pathFile.DataSubcommand));

            if (pathDir != null)
                this.GetInfoDir((this.IsPath(pathDir.DataSubcommand) ? pathDir.DataSubcommand : this.MainDir + pathDir.DataSubcommand));
        }

        public void GetInfoFile(string path)
        {
            if (!File.Exists(path))
                throw new CommandException("Указаный файл не существует!");

            FileInfo fileInfo = new FileInfo(path);
            Console.WriteLine($"Имя файла: {fileInfo.Name}");
            Console.WriteLine($"Полное имя: {fileInfo.FullName}");
            Console.WriteLine($"Размер: {fileInfo.Length} bytes");
            Console.WriteLine($"Время: {fileInfo.LastWriteTime}");
        }

        public void GetInfoDir(string path)
        {
            if (!Directory.Exists(path))
                throw new CommandException("Указаная директория не существует!");

            DirectoryInfo di = new DirectoryInfo(path);

            Console.WriteLine($"Имя: {di.Name}");
            Console.WriteLine($"Полное имя: {di.FullName}");
            Console.WriteLine($"Время: {di.LastWriteTime}");
        }

        public bool IsPath(string path)
        {
            return path.Contains(":\\");
        }
    }
}
