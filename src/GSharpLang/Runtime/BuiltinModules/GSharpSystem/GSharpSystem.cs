﻿namespace GSharpLang.Runtime.BuiltinModules.GSharpSystem
{
    public class GSharpSystem : GSharpObject
    {
        public GSharpSystem() : base("Builtin Module")
        { }
        
        public static void SetupGSharpSystemModule(System.Collections.Generic.Dictionary<string, GSharpObject> gd)
        {
            gd["io"] = new GSharpIO.GSharpIO();
            gd["Boolean"] = new InternalMethodCallback(Boolean, null);
            gd["getType"] = new InternalMethodCallback(getType, null);
            gd["Integer"] = new InternalMethodCallback(Integer, null);
            gd["List"] = new InternalMethodCallback(List, null);
            gd["Object"] = new InternalMethodCallback(Object, null);
            gd["String"] = new InternalMethodCallback(String, null);
        }

        private static GSharpObject Boolean(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpBool(false);
        }
        
        private static GSharpObject getType(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length != 1)
                throw new System.Exception("Expected one argument to getType().");
            return new GSharpString(arguments[0].Type);
        }

        private static GSharpObject Integer(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpInteger(0);
        }

        private static GSharpObject List(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpList(arguments);
        }

        private static GSharpObject Object(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpObject();
        }

        private static GSharpObject String(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpString("");
        }
    }
}
