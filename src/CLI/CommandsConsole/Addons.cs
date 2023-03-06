using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CommandConsole;
using CommandsConsole.Command.CommandsList;

namespace CommandsConsole
{
    /// <summary>
    /// Класс Addons
    /// Отвечает за подгрузку классов (dll) команд
    /// Основная директория где находятся dll - Addons
    /// </summary>
    class Addons
    {
        private List<Type> addonTypes = new List<Type>();

        public List<Type> AddonTypes { get => this.addonTypes; }

        public Addons()
        {
            this.Add(typeof(Help))
                .Add(typeof(List))
                .Add(typeof(Clear))
                .Add(typeof(Cd));
        }

        private string GetСatalogApp()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        private string GetCatalogAddons()
        {
            return Path.Combine(this.GetСatalogApp(), "Addons");
        }

        public string[] GetListAssemblies()
        {
            return Directory.GetFiles(this.GetCatalogAddons(), "*.dll");
        }

        /// <summary>
        /// Загрузка dll и классов
        /// </summary>
        public void Load()
        {
            foreach (string file in this.GetListAssemblies())
            {
                Assembly addinAssembly = Assembly.LoadFrom(file);
                foreach (Type t in addinAssembly.GetExportedTypes())
                {
                    // Если это класс, реализующий интерфейс, значит его можно использовать
                    if (t.IsClass && typeof(ICommand).IsAssignableFrom(t))
                        this.addonTypes.Add(t);
                }
            }
        }

        /// <summary>
        /// Добавить класс комнды
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Addons Add(Type obj)
        {
            this.addonTypes.AddRange(new Type[] { obj });
            return this;
        }
    }
}
