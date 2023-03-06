using System;
using System.IO;
using CommandConsole;

namespace EditFM
{
    public class Copy : AbstractCommand, ICommand
    {
        public string KeyCommand { get => "cp"; }

        public string CommandMask { get => "<f,d>.(str).<>>.(str)"; }

        public void Load()
        {
            Command fileCopy = this.GetData("f");
            Command dirCopy = this.GetData("d");
            Command dir = this.GetData(">");
            string mainDir = this.GetSaveData("path").DataSubcommand;

            if (dir == null)
                throw new CommandException("Укажите директорию куда нужно скопировать!");

            if (!Directory.Exists(dir.DataSubcommand))
                throw new CommandException("Не правельно указана директория!");

            if (fileCopy != null)
                this.FileCopy(
                    this.IsPath(fileCopy.DataSubcommand) ? fileCopy.DataSubcommand : mainDir + fileCopy.DataSubcommand,
                    this.IsPath(dir.DataSubcommand) ? dir.DataSubcommand : mainDir + dir.DataSubcommand
                    );

            if (dirCopy != null)
                this.DirCopy(
                    this.IsPath(dirCopy.DataSubcommand) ? dirCopy.DataSubcommand : mainDir + dirCopy.DataSubcommand,
                    this.IsPath(dir.DataSubcommand) ? dir.DataSubcommand : mainDir + dir.DataSubcommand
                    );
        }

        public void FileCopy(string file, string dir)
        {
            if (!File.Exists(file))
                throw new CommandException("Файл не существует!");

            try
            {
                FileInfo infoFile = new FileInfo(file);

                File.Copy(file, (dir.Substring(dir.Length) == "/" || dir.Substring(dir.Length) == "\\") ? dir + infoFile.Name : dir + "/" + infoFile.Name);
            }
            catch
            {
                throw new CommandException("Не удалось копировать файл!");
            }
        }

        public void DirCopy(string dirCopy, string dir)
        {
            if (!Directory.Exists(dirCopy))
                throw new CommandException("Директории не существует!");

            try
            {
                this.DirectoryCopy(dirCopy, dir);
            }
            catch
            {
                throw new CommandException("Ошибка при копирование! Возможно было скопировано частично!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin_dir"></param>
        /// <param name="end_dir"></param>
        private void DirectoryCopy(string begin_dir, string end_dir)
        {
            //Берём нашу исходную папку
            DirectoryInfo dir_inf = new DirectoryInfo(begin_dir);
            //Перебираем все внутренние папки
            foreach (DirectoryInfo dir in dir_inf.GetDirectories())
            {
                //Проверяем - если директории не существует, то создаём;
                if (Directory.Exists(end_dir + "\\" + dir.Name) != true)
                {
                    Directory.CreateDirectory(end_dir + "\\" + dir.Name);
                }

                //Рекурсия (перебираем вложенные папки и делаем для них то-же самое).
                DirectoryCopy(dir.FullName, end_dir + "\\" + dir.Name);
            }

            //Перебираем файлики в папке источнике.
            foreach (string file in Directory.GetFiles(begin_dir))
            {
                //Определяем (отделяем) имя файла с расширением - без пути (но с слешем "\").
                string filik = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                //Копируем файлик с перезаписью из источника в приёмник.
                File.Copy(file, end_dir + "\\" + filik, true);
            }
        }

        public bool IsPath(string path)
        {
            return path.Contains(":\\");
        }
    }
}
