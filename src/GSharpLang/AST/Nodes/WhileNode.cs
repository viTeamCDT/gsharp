using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class WhileNode : Node
    {
        public Node Condition { get { return Children[0]; } }
        public Node Body { get { return Children[1]; } }

        public static Node Parse(Parser parser)
        {
            WhileNode ret = new WhileNode();
            parser.ExpectToken(TokenType.Keyword, "while");
            parser.ExpectToken(TokenType.Parentheses, "(");
            ret.Children.Add(ExpressionNode.Parse(parser));
            parser.ExpectToken(TokenType.Parentheses, ")");
            ret.Children.Add(StatementNode.Parse(parser));
            return ret;
        }
    }
}
