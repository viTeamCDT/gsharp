using GSharpLang.Lexer;

namespace GSharpLang.AST.Nodes
{
    public class ExpressionNode : Node
    {
        public ExpressionNode(Node child)
        {
            Children.Add(child);
        }

        public static Node Parse(Parser parser)
        {
            return ParseAssign(parser);
        }

        private static Node ParseAssign(Parser parser)
        {
            Node left = ParseInstanceOf(parser);
            if (parser.AcceptToken(TokenType.Operator, "="))
                return new BinaryOperationNode(BinaryOperation.Assignment, left, ParseAssign(parser));
            else if (parser.AcceptToken(TokenType.Operator, "+="))
                return new BinaryOperationNode(BinaryOperation.Assignment, left, new BinaryOperationNode(BinaryOperation.Addition, left, ParseAssign(parser)));
            else if (parser.AcceptToken(TokenType.Operator, "-="))
                return new BinaryOperationNode(BinaryOperation.Assignment, left, new BinaryOperationNode(BinaryOperation.Subtraction, left, ParseAssign(parser)));
            else if (parser.AcceptToken(TokenType.Operator, "*="))
                return new BinaryOperationNode(BinaryOperation.Assignment, left, new BinaryOperationNode(BinaryOperation.Multiplication, left, ParseAssign(parser)));
            else if (parser.AcceptToken(TokenType.Operator, "/="))
                return new BinaryOperationNode(BinaryOperation.Assignment, left, new BinaryOperationNode(BinaryOperation.Division, left, ParseAssign(parser)));
            else if (parser.AcceptToken(TokenType.Operator, "%="))
                return new BinaryOperationNode(BinaryOperation.Assignment, left, new BinaryOperationNode(BinaryOperation.Modulus, left, ParseAssign(parser)));
            else
                return left;
        }
        
        private static Node ParseInstanceOf(Parser parser)
        {
            Node left = ParseBooleanOr(parser);
            if (parser.AcceptToken(TokenType.Operator, "is"))
                return new BinaryOperationNode(BinaryOperation.InstanceOf, left, ParseInstanceOf(parser));
            else
                return left;
        }
        
        private static Node ParseBooleanOr(Parser parser)
        {
            Node left = ParseBooleanAnd(parser);
            if (parser.AcceptToken(TokenType.Operator, "||"))
                return new BinaryOperationNode(BinaryOperation.BooleanOr, left, ParseBooleanOr(parser));
            else
                return left;
        }

        private static Node ParseBooleanAnd(Parser parser)
        {
            Node left = ParseEquals(parser);
            if (parser.AcceptToken(TokenType.Operator, "&&"))
                return new BinaryOperationNode(BinaryOperation.BooleanAnd, left, ParseBooleanAnd(parser));
            else
                return left;
        }

        private static Node ParseEquals(Parser parser)
        {
            Node left = ParseRelationalOp(parser);
            if (parser.AcceptToken(TokenType.Operator, "=="))
                return new BinaryOperationNode(BinaryOperation.Equals, left, ParseEquals(parser));
            else if (parser.AcceptToken(TokenType.Operator, "!="))
                return new BinaryOperationNode(BinaryOperation.NotEqualTo, left, ParseEquals(parser));
            else
                return left;
        }

        private static Node ParseRelationalOp(Parser parser)
        {
            Node left = ParseAddSub(parser);
            if (parser.AcceptToken(TokenType.Operator, ">"))
                return new BinaryOperationNode(BinaryOperation.GreaterThan, left, ParseRelationalOp(parser));
            else if (parser.AcceptToken(TokenType.Operator, "<"))
                return new BinaryOperationNode(BinaryOperation.LessThan, left, ParseRelationalOp(parser));
            else if (parser.AcceptToken(TokenType.Operator, ">="))
                return new BinaryOperationNode(BinaryOperation.GreaterOrEqual, left, ParseRelationalOp(parser));
            else if (parser.AcceptToken(TokenType.Operator, "<="))
                return new BinaryOperationNode(BinaryOperation.LesserOrEqual, left, ParseRelationalOp(parser));
            else
                return left;
        }

        private static Node ParseAddSub(Parser parser)
        {
            Node left = ParseMulDivMod(parser);
            if (parser.AcceptToken(TokenType.Operator, "+"))
                return new BinaryOperationNode(BinaryOperation.Addition, left, ParseAddSub(parser));
            else if (parser.AcceptToken(TokenType.Operator, "-"))
                return new BinaryOperationNode(BinaryOperation.Subtraction, left, ParseAddSub(parser));
            else
                return left;
        }

        private static Node ParseMulDivMod(Parser parser)
        {
            Node left = ParseFunctionCall(parser);
            if (parser.AcceptToken(TokenType.Operator, "*"))
                return new BinaryOperationNode(BinaryOperation.Multiplication, left, ParseMulDivMod(parser));
            else if (parser.AcceptToken(TokenType.Operator, "/"))
                return new BinaryOperationNode(BinaryOperation.Division, left, ParseMulDivMod(parser));
            else if (parser.AcceptToken(TokenType.Operator, "%"))
                return new BinaryOperationNode(BinaryOperation.Modulus, left, ParseMulDivMod(parser));
            else
                return left;
        }
        
        private static Node ParseFunctionCall(Parser parser)
        {
            return ParseFunctionCall(parser, ParseTerm(parser));
        }
        
        private static Node ParseFunctionCall(Parser parser, Node left)
        {
            if (parser.MatchToken(TokenType.Parentheses, "("))
                return ParseFunctionCall(parser, new FunctionCallNode(left, ArgumentsListNode.Parse(parser)));
            else if (parser.MatchToken(TokenType.Dot))
                return ParseFunctionCall(parser, GetAttributeNode.Parse(left, parser));
            else if (parser.MatchToken(TokenType.Bracket, "["))
                return ParseFunctionCall(parser, IndexerNode.Parse(left, parser));
            else
                return left;
        }

        private static Node ParseTerm(Parser parser)
        {
            Token token = null;
            if (parser.AcceptToken(TokenType.Identifier, ref token))
                return new IdentifierNode(token.Value);
            else if (parser.AcceptToken(TokenType.Number, ref token))
                return new NumberNode(int.Parse(token.Value));
            else if (parser.AcceptToken(TokenType.String, ref token))
                return new StringNode(token.Value);
            else if (parser.AcceptToken(TokenType.Parentheses, "("))
            {
                Node expr = Parse(parser);
                parser.ExpectToken(TokenType.Parentheses, ")");
                return expr;
            }
            else if (parser.AcceptToken(TokenType.Keyword, "true"))
                return new BooleanTrueNode();
            else if (parser.AcceptToken(TokenType.Keyword, "false"))
                return new BooleanFalseNode();
            else if (parser.AcceptToken(TokenType.Keyword, "this"))
                return new ThisNode();
            return null;
        }
    }
}
