using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class IfNode : Node
    {
        public Node Condition { get { return Children[0]; } }
        public Node Body { get { return Children[1]; } }
        public Node ElseBody { get { return Children[2]; } }
        public bool HaveElseBody { get; private set; }

        public IfNode(Node predicate, Node body)
        {
            Children.Add(predicate);
            Children.Add(body);
            Children.Add(new CodeBlock());
            HaveElseBody = false;
        }

        public IfNode(Node predicate, Node body, Node elseBody)
        {
            Children.Add(predicate);
            Children.Add(body);
            Children.Add(elseBody);
            HaveElseBody = true;
        }

        public static Node Parse(Parser parser)
        {
            parser.ExpectToken(TokenType.Keyword, "if");
            parser.ExpectToken(TokenType.Parentheses, "(");
            ExpressionNode predicate = new ExpressionNode(ExpressionNode.Parse(parser));
            parser.ExpectToken(TokenType.Parentheses, ")");
            Node ifBody = StatementNode.Parse(parser);
            if (parser.AcceptToken(TokenType.Keyword, "else"))
                return new IfNode(predicate, ifBody, StatementNode.Parse(parser));
            else
                return new IfNode(predicate, ifBody);
        }
    }
}
