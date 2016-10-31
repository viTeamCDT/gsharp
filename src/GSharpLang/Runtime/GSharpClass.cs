namespace GSharpLang.Runtime
{
    public class GSharpClass : GSharpObject
    {
        private GSharpMethod constructor;
        private System.Collections.Generic.IList<GSharpMethod> instanceMethods = new System.Collections.Generic.List<GSharpMethod>();

        public GSharpClass(string name, GSharpMethod constructor)
        {
            this.constructor = constructor;
        }

        public void AddInstanceMethod(GSharpMethod method)
        {
            instanceMethods.Add(method);
        }

        public override GSharpObject Invoke(VirtualMachine vm, GSharpObject[] arguments)
        {
            GSharpObject obj = new GSharpObject();
            foreach (GSharpMethod method in instanceMethods)
                obj.SetAttribute(method.Name, method);
            vm.InvokeMethod(constructor, obj, arguments);
            return obj;
        }
    }
}
