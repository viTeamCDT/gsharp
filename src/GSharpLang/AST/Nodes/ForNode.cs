using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class ForNode : Node
    {
        public Node Initializer { get { return Children[0]; } }
        public Node Condition { get { return Children[1]; } }
        public Node AfterThought { get { return Children[2]; } }
        public Node Body { get { return Children[3]; } }

        public static Node Parse(Parser parser)
        {
            ForNode ret = new ForNode();
            parser.ExpectToken(TokenType.Keyword, "for");
            parser.ExpectToken(TokenType.Parentheses, "(");
            ret.Children.Add(ExpressionNode.Parse(parser));
            parser.ExpectToken(TokenType.Semicolon);
            ret.Children.Add(ExpressionNode.Parse(parser));
            parser.ExpectToken(TokenType.Semicolon);
            ret.Children.Add(ExpressionNode.Parse(parser));
            parser.ExpectToken(TokenType.Parentheses, ")");
            ret.Children.Add(StatementNode.Parse(parser));
            return ret;
        }
    }
}
