namespace GSharpLang.AST.Nodes
{
    public class IdentifierNode : Node
    {
        public string Name { get; private set; }

        public IdentifierNode(string name)
        {
            Name = name;
        }
    }
}
