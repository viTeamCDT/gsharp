namespace GSharpLang.AST.Nodes
{
    public class StringNode : Node
    {
        public string Value { get; private set; }

        public StringNode(string value)
        {
            Value = value;
        }
    }
}
