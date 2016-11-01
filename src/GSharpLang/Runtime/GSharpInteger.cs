using GSharpLang.AST.Nodes;

namespace GSharpLang.Runtime
{
    public class GSharpInteger : GSharpObject
    {
        public int Value { get; private set; }

        public GSharpInteger(int val) : base("Integer")
        {
            Value = val;
            SetAttribute("toChar", new InternalMethodCallback(toChar, null));
            SetAttribute("toString", new InternalMethodCallback(toString, null));
        }

        public override GSharpObject PerformBinaryOperation(VirtualMachine vm, BinaryOperation binop, GSharpObject rval)
        {
            GSharpInteger intVal = rval as GSharpInteger;

            if (intVal == null)
                throw new System.Exception("Right value must be an integer.");

            switch (binop)
            {
                case BinaryOperation.Addition:
                    return new GSharpInteger(Value + intVal.Value);
                case BinaryOperation.Subtraction:
                    return new GSharpInteger(Value - intVal.Value);
                case BinaryOperation.Multiplication:
                    return new GSharpInteger(Value * intVal.Value);
                case BinaryOperation.Division:
                    return new GSharpInteger(Value / intVal.Value);
                case BinaryOperation.Modulus:
                    return new GSharpInteger(Value % intVal.Value);
                case BinaryOperation.Equals:
                    return new GSharpBool(Value == intVal.Value);
                case BinaryOperation.NotEqualTo:
                    return new GSharpBool(Value != intVal.Value);
                case BinaryOperation.GreaterThan:
                    return new GSharpBool(Value > intVal.Value);
                case BinaryOperation.GreaterOrEqual:
                    return new GSharpBool(Value >= intVal.Value);
                case BinaryOperation.LessThan:
                    return new GSharpBool(Value < intVal.Value);
                case BinaryOperation.LesserOrEqual:
                    return new GSharpBool(Value <= intVal.Value);
            }

            return null;
        }

        public override GSharpObject toString(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpString(Value.ToString());
        }

        private GSharpObject toChar(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpString(((char)Value).ToString());
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
