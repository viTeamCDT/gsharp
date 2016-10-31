using GSharpLang.Lexer;
using System.Collections.Generic;

namespace GSharpLang.AST.Nodes
{
    public class ClassDeclarationNode : Node
    {
        public string Name { get; private set; }
        public FunctionDeclarationNode Constructor { get { return (FunctionDeclarationNode)Children[0]; } set { Children[0] = value; } }

        public ClassDeclarationNode(string name)
        {
            Name = name;
            FunctionDeclarationNode defaultCtor = new FunctionDeclarationNode(name, new List<string>());
            defaultCtor.Children.Add(new ScopeNode());
            Children.Add(defaultCtor);
        }

        public static Node Parse(Parser parser)
        {
            parser.ExpectToken(TokenType.Keyword, "class");
            string name = parser.ExpectToken(TokenType.Identifier).Value;
            ClassDeclarationNode clazz = new ClassDeclarationNode(name);
            parser.ExpectToken(TokenType.Brace, "{");
            while (!parser.MatchToken(TokenType.Brace, "}"))
                if (parser.MatchToken(TokenType.Keyword, "function"))
                {
                    FunctionDeclarationNode func = (FunctionDeclarationNode)FunctionDeclarationNode.Parse(parser);
                    if (func.Name == name)
                        clazz.Constructor = func;
                    clazz.Children.Add(func);
                }
                else
                    parser.ExpectToken(TokenType.Keyword, "function");
            parser.ExpectToken(TokenType.Brace, "}");
            return clazz;
        }
    }
}
