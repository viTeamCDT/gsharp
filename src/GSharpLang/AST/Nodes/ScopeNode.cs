using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class ScopeNode : Node
    {
        public static Node Parse(Parser parser)
        {
            ScopeNode scope = new ScopeNode();
            parser.ExpectToken(TokenType.Brace, "{");
            while (!parser.MatchToken(TokenType.Brace, "}"))
                scope.Children.Add(StatementNode.Parse(parser));
            parser.ExpectToken(TokenType.Brace, "}");
            return scope;
        }
    }
}
