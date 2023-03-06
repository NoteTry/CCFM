using System;
using CommandConsole;

namespace Branches
{
    public class Tree : AbstractCommand, ICommand
    {
        public string KeyCommand { get => "tree"; }

        public string CommandMask { get => "(str)"; }

        public void Load()
        {
            string PathToFolder = this.GetSaveData("path").DataSubcommand;
            int numericValue;
            int maxDepth = !(this.GetData("str") != null && this.GetData("str").DataSubcommand != "" && int.TryParse(this.GetData("str").DataSubcommand, out numericValue)) ? 3 : Convert.ToInt32(this.GetData("str").DataSubcommand);
            try
            {
                new Ban(PathToFolder)
                {
                    ShowAll = false,
                    MaxDepth = maxDepth
                }.Print();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
