using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class EnumDeclarationNode : Node
    {
        public string Name { get; private set; }
        public System.Collections.Generic.Dictionary<string, int> Items { get; private set; }
        public FunctionDeclarationNode Constructor { get { return (FunctionDeclarationNode)Children[0]; } set { Children[0] = value; } }

        public EnumDeclarationNode(string name)
        {
            Name = name;
            Items = new System.Collections.Generic.Dictionary<string, int>();
        }

        public static Node Parse(Parser parser)
        {
            parser.ExpectToken(TokenType.Keyword, "enum");
            string name = parser.ExpectToken(TokenType.Identifier).Value;
            EnumDeclarationNode decl = new EnumDeclarationNode(name);
            parser.ExpectToken(TokenType.Brace, "{");
            int defaultVal = -1;

            while (!parser.MatchToken(TokenType.Brace, "}"))
            {
                string ident = parser.ExpectToken(TokenType.Identifier).Value;
                if (parser.AcceptToken(TokenType.Operator, "="))
                {
                    string val = parser.ExpectToken(TokenType.Number).Value;
                    int numVal = 0;
                    if (val != "")
                        numVal = int.Parse(val);
                    decl.Items[ident] = numVal;
                }
                else
                    decl.Items[ident] = defaultVal--;

                if (!parser.AcceptToken(TokenType.Comma))
                    break;
            }

            parser.ExpectToken(TokenType.Brace, "}");
            return decl;
        }
    }
}
