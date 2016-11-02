namespace GSharpLang.Runtime
{
    public class GSharpEnum : GSharpObject
    {
        private int nextVal = 0;

        public GSharpEnum() : base("Enumeration")
        { 
            SetAttribute("toString", new InternalMethodCallback(toString, null));
        }

        public void AddItem(string name)
        {
            SetAttribute(name, new GSharpInteger(nextVal++));
        }

        public void AddItem(string name, int val)
        {
            SetAttribute(name, new GSharpInteger(val));
        }

        public GSharpObject toString(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length != 1)
                throw new System.Exception("Invalid number of arguments in enum.ToString().");
            else
                foreach (var a in attributes)
                    if (a.Value == arguments[0])
                        return new GSharpString(a.Key);
            throw new System.Exception("Invalid item in enum.ToString().");
        }
    }
}
