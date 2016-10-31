using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class ArgumentsListNode : Node
    {
        public static Node Parse(Parser parser)
        {
            ArgumentsListNode argList = new ArgumentsListNode();
            parser.ExpectToken(TokenType.Parentheses, "(");
            while (!parser.MatchToken(TokenType.Parentheses, ")"))
            {
                argList.Children.Add(ExpressionNode.Parse(parser));
                if (!parser.AcceptToken(TokenType.Comma))
                    break;
            }
            parser.ExpectToken(TokenType.Parentheses, ")");
            return argList;
        }
    }
}
