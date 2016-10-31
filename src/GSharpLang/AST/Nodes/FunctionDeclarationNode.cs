using GSharpLang.Lexer;
using System.Collections.Generic;

namespace GSharpLang.AST.Nodes
{
    public class FunctionDeclarationNode : Node
    {
        public string Name { get; private set; }
        public IList<string> Parameters { get; private set; }

        public FunctionDeclarationNode(string name, IList<string> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public static Node Parse(Parser parser)
        {
            parser.ExpectToken(TokenType.Keyword, "function");
            Token ident = parser.ExpectToken(TokenType.Identifier);
            List<string> parameters = ParseFunctionParameters(parser);
            FunctionDeclarationNode decl = new FunctionDeclarationNode(ident.Value, parameters);
            decl.Children.Add(StatementNode.Parse(parser));
            return decl;
        }

        private static List<string> ParseFunctionParameters(Parser parser)
        {
            List<string> ret = new List<string>();
            parser.ExpectToken(TokenType.Parentheses, "(");
            while (!parser.MatchToken(TokenType.Parentheses, ")"))
            {
                Token param = parser.ExpectToken(TokenType.Identifier);
                ret.Add(param.Value);
                if (!parser.AcceptToken(TokenType.Comma))
                    break;
            }
            parser.ExpectToken(TokenType.Parentheses, ")");
            return ret;
        }
    }
}
