namespace GSharpLang.Runtime
{
    public class GSharpName : GSharpObject
    {
        public string Value { get; private set; }

        public GSharpName(string val)
        {
            Value = val;
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
