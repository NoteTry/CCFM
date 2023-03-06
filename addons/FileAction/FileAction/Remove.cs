using System;
using System.IO;
using CommandConsole;

namespace FileAction
{
    public class Remove : AbstractCommand, ICommand
    {
        public string KeyCommand { get => "remove"; }

        public string CommandMask { get => "<f,d>.(str)"; }

        public string Dir { get => this.GetSaveData("path").DataSubcommand; }

        public void Load()
        {
            if (!this.ShouldDelete("Удалить файл(папку)?"))
                throw new CommandException("Команда отменина!");

            Command pathFile = this.GetData("f");
            Command pathDir = this.GetData("d");

            if (pathFile != null)
            {
                if (pathFile.DataSubcommand == "")
                    throw new CommandException("Введите путь до файла или имя файла!");

                this.RemoveFile(
                    (this.IsPath(pathFile.DataSubcommand) ? pathFile.DataSubcommand : this.Dir + pathFile.DataSubcommand)
                    );
            }

            if (pathDir != null)
            {
                if (pathDir.DataSubcommand == "")
                    throw new CommandException("Введите путь до директории или имя директории!");
                
                this.RemoveDir(
                    (this.IsPath(pathDir.DataSubcommand) ? pathDir.DataSubcommand : this.Dir + pathDir.DataSubcommand)
                    );
            }
        }

        public void RemoveFile(string pathFile)
        {
            if (!File.Exists(pathFile))
                throw new CommandException("Не существует данного файла!");

            File.Delete(pathFile);
            Console.WriteLine("Файл успешно удалён!");
        }

        public void RemoveDir(string pathDir)
        {
            if (!Directory.Exists(pathDir))
                throw new CommandException("Не существует данного директории!");

            Directory.CreateDirectory(pathDir);
            string[] files = Directory.GetFiles(pathDir);
            foreach (string file in files)
                File.Delete(file);
            Directory.Delete(pathDir);

            Console.WriteLine("Директория удалена успешно!");
        }

        public bool IsPath(string path)
        {
            return path.Contains(":\\");
        }

        public bool ShouldDelete(string text)
        {
            Console.Write($"{text} (y/n): ");
            string response = Console.ReadLine();
            return (response == "y") ? true : false;
        }
    }
}
