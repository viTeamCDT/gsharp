using GSharpLang.AST.Nodes;

namespace GSharpLang.Runtime
{
    public class GSharpString : GSharpObject
    {
        public string Value { get; private set; }

        private int iterIndex = 0;

        public GSharpString(string val) : base("String")
        {
            Value = val;
            SetAttribute("contains", new InternalMethodCallback(contains, null));
            SetAttribute("isDigit", new InternalMethodCallback(isDigit, null));
            SetAttribute("isLetters", new InternalMethodCallback(isLetters, null));
            SetAttribute("isWhitespace", new InternalMethodCallback(isWhitespace, null));
            SetAttribute("size", new InternalMethodCallback(size, null));
            SetAttribute("toString", new InternalMethodCallback(toString, null));
        }

        public override GSharpObject PerformBinaryOperation(VirtualMachine vm, BinaryOperation binop, GSharpObject rval)
        {
            GSharpString strVal = rval as GSharpString;

            if (strVal == null)
                strVal = (GSharpString)rval.GetAttribute("toString").Invoke(vm, new GSharpObject[] { });

            switch (binop)
            {
                case BinaryOperation.Equals:
                    return new GSharpBool(strVal.Value == Value);
                case BinaryOperation.NotEqualTo:
                    return new GSharpBool(strVal.Value != Value);
                case BinaryOperation.Addition:
                    return new GSharpString(Value + strVal.Value);

            }

            return null;
        }
        
        public GSharpObject contains(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            if (arguments.Length != 1)
                throw new System.Exception("Invalid number of arguments to String.contains()");
            return new GSharpBool(Value.Contains(arguments[0].ToString()));
        }
        
        public GSharpObject isDigit(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            foreach (char ch in Value)
                if (!char.IsDigit(ch))
                    return GSharpBool.False;
            return GSharpBool.True;
        }
        
        public GSharpObject isLetters(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            foreach (char ch in Value)
                if (!char.IsLetter(ch))
                    return GSharpBool.False;
            return GSharpBool.True;
        }
        
        public GSharpObject isWhitespace(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            foreach (char ch in Value)
                if (!char.IsWhiteSpace(ch))
                    return GSharpBool.False;
            return GSharpBool.True;
        }
        
        public GSharpObject toString(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpString(Value);
        }

        public override GSharpObject GetIndex(VirtualMachine vm, GSharpObject key)
        {
            GSharpInteger index = key as GSharpInteger;
            return new GSharpString(Value[index.Value].ToString());
        }

        public override GSharpObject IterGetNext(VirtualMachine vm)
        {
            return new GSharpString(Value[iterIndex - 1].ToString());
        }

        public override bool IterMoveNext(VirtualMachine vm)
        {
            if (iterIndex >= Value.Length)
                return false;
            iterIndex++;
            return true;
        }

        public override void IterReset(VirtualMachine vm)
        {
            iterIndex = 0;
        }

        public GSharpObject size(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpInteger(Value.Length);
        }

        public override string ToString()
        {
            return Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
