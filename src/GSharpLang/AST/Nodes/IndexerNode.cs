using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class IndexerNode : Node
    {
        public Node Target { get { return Children[0]; } }
        public Node Index { get { return Children[1]; } }

        public IndexerNode(Node lval, Node index)
        {
            Children.Add(lval);
            Children.Add(index);
        }

        public static Node Parse(Node lval, Parser parser)
        {
            parser.ExpectToken(TokenType.Bracket, "[");
            Node index = ExpressionNode.Parse(parser);
            parser.ExpectToken(TokenType.Bracket, "]");
            return new IndexerNode(lval, index);
        }
    }
}
