using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class GetAttributeNode : Node
    {
        public Node Target { get { return Children[0]; } }
        public string Field { get; private set; }

        public GetAttributeNode(Node target, string field)
        {
            Children.Add(target);
            Field = field;
        }

        public static Node Parse(Node lval, Parser parser)
        {
            parser.ExpectToken(TokenType.Dot);
            Token ident = parser.ExpectToken(TokenType.Identifier);
            return new GetAttributeNode(lval, ident.Value);
        }
    }
}
