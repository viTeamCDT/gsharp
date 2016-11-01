namespace GSharpLang.Runtime
{
    public delegate GSharpObject GSharpMethodCallback(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments);

    public class InternalMethodCallback : GSharpObject
    {
        private GSharpObject self;
        private GSharpMethodCallback callback;

        public InternalMethodCallback(GSharpMethodCallback callback, GSharpObject self) : base("Internal Method Callback")
        {
            this.self = self;
            this.callback = callback;
        }

        public override GSharpObject Invoke(VirtualMachine vm, GSharpObject[] arguments)
        {
            return callback.Invoke(vm, self, arguments);
        }
    }
}
