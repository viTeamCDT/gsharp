using System;

namespace GSharpLang.Runtime.BuiltinModules.GSharpSystem.GSharpIO
{
    public class GSharpIO : GSharpObject
    {
        public GSharpIO() : base(false)
        {
            SetAttribute("File", new GSharpFile());
            SetAttribute("print", new InternalMethodCallback(Print, null));
            SetAttribute("println", new InternalMethodCallback(PrintLine, null));
        }

        private GSharpObject Print(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            foreach (GSharpObject arg in arguments)
                Console.Write(arg);
            return null;
        }

        private GSharpObject PrintLine(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            foreach (GSharpObject arg in arguments)
                Console.Write(arg);
            Console.WriteLine();
            return null;
        }
    }
}
