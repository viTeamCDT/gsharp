using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class StatementNode : Node
    {
        public static Node Parse(Parser parser)
        {
            if (parser.MatchToken(TokenType.Keyword, "class"))
                return ClassDeclarationNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Keyword, "function"))
                return FunctionDeclarationNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Keyword, "if"))
                return IfNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Keyword, "while"))
                return WhileNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Keyword, "for"))
                return ForNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Keyword, "return"))
                return ReturnNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Keyword, "incfile"))
                return UseNode.Parse(parser);
            else if (parser.MatchToken(TokenType.Brace, "{"))
                return ScopeNode.Parse(parser);
            else
            {
                ExpressionNode expr = new ExpressionNode(ExpressionNode.Parse(parser));
                parser.ExpectToken(TokenType.Semicolon);
                return expr;
            }
        }
    }
}
