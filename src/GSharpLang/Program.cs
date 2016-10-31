using GSharpLang.Runtime;
using System;
using System.Collections.Generic;

namespace GSharpLang
{
    public class Program
    {
        

        public static void Main(string[] args)
        {
            if (args.Length < 1)
                Console.WriteLine("GSharp Compiler\r\nUsage: gsharp <file>");
            else
            {
                ProgramArguments arguments = new ProgramArguments(); arguments.Parse(args);

                try
                {
                    GSharpModule module = GSharpModule.CompileModule(arguments.FilesToCompile[0]);
                    VirtualMachine vm = new VirtualMachine();
                    if (!module.HasAttribute("main"))
                        throw new Exception("Entry point 'main' not found.");
                    else
                        module.GetAttribute("main").Invoke(vm, ((GSharpMethod)module.GetAttribute("main")).ParameterCount == 0 ? null : new GSharpObject[] { new GSharpString(arguments.ArgumentsToProgram) });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unhandled Exception:\r\n" + ex.Message);
                    Environment.Exit(-1);
                }
            }
        }
    }

    public class ProgramArguments
    {
        public string ArgumentsToProgram { get; private set; }
        public List<string> FilesToCompile { get; private set; }
        
        public ProgramArguments()
        {
            ArgumentsToProgram = "";
            FilesToCompile = new List<string>();
        }

        public void Parse(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--args")
                    ArgumentsToProgram = args[++i];
                else
                    FilesToCompile.Add(args[i]);
            }
        }
    }
}