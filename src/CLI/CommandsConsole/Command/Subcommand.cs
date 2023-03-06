using System;
using System.Collections.Generic;
using System.Linq;
using CommandConsole;

namespace CommandsConsole.Command
{
    class Subcommand
    {
        public string CommandLine { set; get; }

        private readonly List<CommandConsole.Command> data;

        public Subcommand()
        {
            this.data = new List<CommandConsole.Command>();
        }

        public Subcommand AddData(string key, string data)
        {
            this.data.Add(new CommandConsole.Command(key, data));
            return this;
        }

        public void ResetData()
        {
            this.data.Clear();
        }

        public List<CommandConsole.Command> GetData()
        {
            return this.data;
        }

        public void СonvertCommand(string mask)
        {
            try
            {
                string[] paramsCommand;
                string[] segments = mask.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                string[] segmentCommandLine = this.ParseT(segments, this.CommandLine);

                if (segments.Length == 0)
                    return;

                if (segmentCommandLine.Length != segments.Length)
                    throw new CommandException("Не правильно ведённая команда!");

                if (segmentCommandLine.Any(str => str == null))
                    throw new CommandException("Не правильно ведённая команда!");

                for (int i = 0; i < segments.Length; i++)
                {
                    if (segments.Length == 1 && segments[i] == "(str)")
                        this.AddData("str", segmentCommandLine[i]);

                    if (this.IsParamCommand(segments[i]) && segments[i] != "(str)")
                    {
                        if (segmentCommandLine[i] == "")
                            throw new CommandException($"Ожидает(юся) параметр(ы)!");

                        paramsCommand = this.GetParams(segments[i]);

                        if (!paramsCommand.Contains(segmentCommandLine[i].Trim().TrimStart(new char[] { '-' })))
                            throw new CommandException($"Не правельный параметр '{segmentCommandLine[i]}'!");

                        this.AddData(
                            segmentCommandLine[i].TrimStart(new char[] { '-' }),
                            this.ResultParamsData(segmentCommandLine, (segments.Length < i + 1) ? "" : segments[i + 1], i)
                            );

                    }
                }
            }
            catch (Exception e)
            {
                throw new CommandException(e.Message, e);
            }
        }

        private string ResultParamsData(string[] segmentCommandLine, string segment, int i)
        {
            try
            {
                if (segment != "(str)" && this.IsParamInLine(segmentCommandLine[i+1]))
                    throw new Exception();

                return segmentCommandLine[i+1];
            }
            catch
            {
                return "y";
            }
        }


        public string[] ParseT(string[] segments, string commandLine)
        {
            string[] arr = new string[segments.Length];
            string commandL = commandLine.Trim();

            if (commandLine == "")
                return new string[1] { this.CommandLine };

            if (this.CountSim(commandL) % 2 != 0)
                throw new CommandException("Синтаксическая ошибка! (').");
            
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i] != "(str)")
                {
                    if (this.ExistSpace(commandL))
                    {
                        arr[i] = commandL.Substring(0, commandL.IndexOf(" ")).Trim();
                        commandL = commandL.Remove(0, commandL.IndexOf(" ")).Trim();
                    }
                    else
                    {
                        arr[i] = commandL;
                        commandL = "$n";
                        break;
                    }
                }
                
                if (segments[i] == "(str)")
                {
                    if (commandL[0] == '\'' && commandL[commandL.Length-1] == '\'')
                    {
                        arr[i] = commandL.TrimStart('\'').TrimEnd('\'').Trim();
                        commandL = "$n";
                        break;
                    }

                    if (commandL[0] == '\'' && commandL.Trim().TrimStart('\'').Contains('\''))
                    {
                        if (this.ExistSpace(commandL))
                        {
                            arr[i] = commandL.TrimStart('\'').Substring(0, commandL.IndexOf("' ")).Trim().TrimEnd('\'').Trim();
                            commandL = commandL.TrimStart('\'').Remove(0, commandL.TrimStart('\'').IndexOf('\'') + 1).Trim();
                        }
                        else
                        {
                            arr[i] = commandL.TrimStart('\'').TrimEnd('\'').Trim();
                            commandL = "$n";
                            break;
                        }
                    }
                    else
                    {
                        if (this.ExistSpace(commandL))
                        {
                            arr[i] = commandL.Substring(0, commandL.IndexOf(" ")).Trim();
                            commandL = commandL.Remove(0, commandL.IndexOf(" ")).Trim();
                        }
                        else
                        {
                            arr[i] = commandL.Trim();
                            commandL = "$n";
                            break;
                        }
                    }
                }
            }

            if (commandL != "$n")
                Array.Resize(ref arr, arr.Length + 1);
            
            return arr;
        }

        private bool ExistSpace(string str)
        {
            string result_str = str.Trim();

            for (int i = 1; i < (this.CountSim(str) / 2); i++)
            {
                string str_1 = result_str.Remove(0, result_str.IndexOf('\'')+1);
                int index_2 = str_1.IndexOf('\'')+2;
                
                result_str = result_str.Remove(result_str.IndexOf('\''), index_2);
            }
            
            return result_str.Contains(' ');
        }

        private int CountSim(string str)
        {
            return str.ToCharArray().Where(c => c == '\'').Count();
        }

        private string[] GetParams(string str)
        {
            if (!this.IsParamCommand(str))
                throw new CommandException("Не правильный шаблон параметров!");

            string str_1 = str.Remove(0, 1);

            return str_1.Remove(str_1.Length - 1).Split(',');
        }

        public bool IsParamInLine(string str)
        {
            return (!str.Contains(' ') && str[0] == '-') ? true : false;
        }

        public bool IsParamCommand(string str)
        {
            return (str[0] == '<' && str.Substring(str.Length - 1) == ">") ? true : false;
        }

        public bool IsParamData(string str)
        {
            return (str[0] == '(' && str.Substring(str.Length - 1) == ")") ? true : false;
        }
    }
}
