using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class UseNode : Node
    {
        public string Module { get; private set; }
        
        public UseNode(string module)
        {
            Module = module;
        }
        
        public static Node Parse(Parser parser)
        {
		    parser.ExpectToken(TokenType.Keyword, "incfile");
		    string module;
		    if (parser.MatchToken(TokenType.String))
		        module = (parser.ExpectToken(TokenType.String)).Value;
		    else
		        module = (parser.ExpectToken(TokenType.Identifier)).Value;
		    parser.ExpectToken(TokenType.Semicolon);
		    return new UseNode(module);
        }
    }
}