namespace GSharpLang.Runtime.BuiltinModules.GSharpSystem.GSharpIO
{
    public class GSharpFile : GSharpObject
    {
        public GSharpFile() : base("IO.File")
        {
            SetAttribute("create", new InternalMethodCallback(Create, null));
            SetAttribute("delete", new InternalMethodCallback(Delete, null));
            SetAttribute("exists", new InternalMethodCallback(Exists, null));
            SetAttribute("readAllBytes", new InternalMethodCallback(ReadAllBytes, null));
            SetAttribute("readAllText", new InternalMethodCallback(ReadAllText, null));
            SetAttribute("writeAllBytes", new InternalMethodCallback(WriteAllBytes, null));
            SetAttribute("writeAllText", new InternalMethodCallback(WriteAllText, null));
        }

        private GSharpObject Create(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length != 1)
                throw new System.Exception("Expected file name in system.io.create().");
            else if (!(arguments[0] is GSharpString))
                throw new System.Exception("Expected file name as string in system.io.create().");
            System.IO.File.Create(((GSharpString)arguments[0]).ToString());
            return null;
        }

        private GSharpObject Delete(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length != 1)
                throw new System.Exception("Expected file name in system.io.delete().");
            else if (!(arguments[0] is GSharpString))
                throw new System.Exception("Expected file name as string in system.io.delete().");
            System.IO.File.Delete(((GSharpString)arguments[0]).ToString());
            return null;
        }

        private GSharpObject Exists(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length != 1)
                throw new System.Exception("Expected file name in system.io.exists().");
            else if (!(arguments[0] is GSharpString))
                throw new System.Exception("Expected file name as string in system.io.exists().");
            return new GSharpBool(System.IO.File.Exists(((GSharpString)arguments[0]).ToString()));
        }

        private GSharpObject ReadAllBytes(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length != 1)
                throw new System.Exception("Expected file name in system.io.readAllBytes().");
            else if (!(arguments[0] is GSharpString))
                throw new System.Exception("Expected file name as string in system.io.readlAllBytes().");
            byte[] bytes = System.IO.File.ReadAllBytes(((GSharpString)arguments[0]).ToString());
            GSharpList toRet = new GSharpList(new GSharpObject[] { });
            foreach (byte b in bytes)
                toRet.Add(new GSharpInteger(b));
            return toRet;
        }

        private GSharpObject ReadAllText(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length != 1)
                throw new System.Exception("Expected file name in system.io.readAllText().");
            else if (!(arguments[0] is GSharpString))
                throw new System.Exception("Expected file name as string in system.io.readlAllText().");
            return new GSharpString(System.IO.File.ReadAllText(((GSharpString)arguments[0]).ToString()));
        }

        private GSharpObject WriteAllBytes(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length < 2)
                throw new System.Exception("Expected file name in system.io.writeAllBytes().");
            else if (!(arguments[0] is GSharpString))
                throw new System.Exception("Expected file name as string in system.io.writeAllBytes().");
            string filename = ((GSharpString)arguments[0]).ToString();
            System.Collections.Generic.List<byte> list = new System.Collections.Generic.List<byte>();
            foreach (GSharpObject obj in arguments)
                if (obj is GSharpInteger)
                    list.Add((byte)((GSharpInteger)obj).Value);
                else if (obj is GSharpList)
                    foreach (GSharpObject i in ((GSharpList)obj).Objects)
                    {
                        if (i is GSharpInteger)
                            list.Add((byte)((GSharpInteger)i).Value);
                    }
            System.IO.File.WriteAllBytes(filename, list.ToArray());
            return null;
        }

        private GSharpObject WriteAllText(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length < 2)
                throw new System.Exception("Expected file name and new file contents in system.io.writeAllText().");
            else if (!(arguments[0] is GSharpString))
                throw new System.Exception("Expected file name as string in system.io.writeAllText().");
            string filename = ((GSharpString)arguments[0]).ToString();
            string contents = "";
            for (int i = 1; i < arguments.Length; i++)
                contents += arguments[i].ToString();
            System.IO.File.WriteAllText(filename, contents);
            return null;
        }
    }
}
