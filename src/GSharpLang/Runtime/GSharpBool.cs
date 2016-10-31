using GSharpLang.AST.Nodes;

namespace GSharpLang.Runtime
{
    public class GSharpBool : GSharpObject
    {
        public static readonly GSharpBool True = new GSharpBool(true);
        public static readonly GSharpBool False = new GSharpBool(false);
        public bool Value { get; private set; }

        public GSharpBool(bool val)
        {
            Value = val;
            SetAttribute("toString", new InternalMethodCallback(toString, null));
        }

        public override GSharpObject PerformBinaryOperation(VirtualMachine vm, BinaryOperation binop, GSharpObject rval)
        {
            GSharpBool boolVal = rval as GSharpBool;

            switch (binop)
            {
                case BinaryOperation.Equals:
                    return new GSharpBool(boolVal.Value == Value);
                case BinaryOperation.NotEqualTo:
                    return new GSharpBool(boolVal.Value != Value);
                case BinaryOperation.BooleanAnd:
                    return new GSharpBool(boolVal.Value && Value);
                case BinaryOperation.BooleanOr:
                    return new GSharpBool(boolVal.Value || Value);
            }

            return null;
        }

        private GSharpObject toString(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpString(Value.ToString());
        }

        public override bool IsTrue()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
