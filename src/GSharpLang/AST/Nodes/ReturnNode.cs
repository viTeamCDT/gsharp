using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class ReturnNode : Node
    {
        public Node Value { get { return Children[0]; } }
        
        public ReturnNode(Node val)
        {
            Children.Add(val);
        }
        
        public static Node Parse(Parser parser)
        {
            parser.ExpectToken(TokenType.Keyword, "return");
            if (parser.AcceptToken(TokenType.Semicolon))
                return new ReturnNode(null);
            else
                return new ReturnNode(ExpressionNode.Parse(parser));
		}
	}
}