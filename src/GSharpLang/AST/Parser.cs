using GSharpLang.AST.Nodes;
using GSharpLang.Lexer;
using System.Collections.Generic;

namespace GSharpLang.AST
{
    public class Parser
    {
        public bool EndOfStream { get { return tokens.Count <= position; } }

        private List<Token> tokens = new List<Token>();
        private int position = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public Node Parse()
        {
            CodeBlock block = new CodeBlock();
            while (!EndOfStream)
                block.Children.Add(StatementNode.Parse(this));
            return block;
        }

        public bool MatchToken(TokenType type)
        {
            return PeekToken() != null && PeekToken().Type == type;
        }

        public bool MatchToken(TokenType type, string value)
        {
            return PeekToken() != null && PeekToken().Type == type && PeekToken().Value == value;
        }

        public bool AcceptToken(TokenType type)
        {
            if (MatchToken(type))
            {
                ReadToken();
                return true;
            }

            return false;
        }

        public bool AcceptToken(TokenType type, ref Token token)
        {
            if (MatchToken(type))
            {
                token = ReadToken();
                return true;
            }

            return false;
        }

        public bool AcceptToken(TokenType type, string value)
        {
            if (MatchToken(type, value))
            {
                ReadToken();
                return true;
            }

            return false;
        }

        public bool AcceptToken(TokenType type, string value, ref Token token)
        {
            if (MatchToken(type, value))
            {
                token = ReadToken();
                return true;
            }

            return false;
        }

        public Token ExpectToken(TokenType type)
        {
            Token ret = null;
            if (AcceptToken(type, ref ret))
                return ret;
            else
            {
                ret = ReadToken();
                if (ret != null)
                    throw new System.Exception("Unexpected '" + ret.Value + "' at line " + ret.Location.Line + ", column " + ret.Location.Column + " (Expected " + type + ").");
                else
                    throw new System.Exception("Unexpected end of file (Expected " + type + ").");
            }
        }

        public Token ExpectToken(TokenType type, string value)
        {
            Token ret = null;
            if (AcceptToken(type, value, ref ret))
                return ret;
            else
            {
                ret = ReadToken();
                if (ret != null)
                    throw new System.Exception("Unexpected '" + ret.Value + "' at line " + ret.Location.Line + ", column " + ret.Location.Column + " (Expected " + type + " with value '" + value + "').");
                else
                    throw new System.Exception("Unexpected end of file (Expected " + type + " with value '" + value + "').");
            }
        }

        private Token PeekToken(int n = 0)
        {
            if (position + n < tokens.Count)
                return tokens[position + n];
            else
                return null;
        }

        private Token ReadToken()
        {
            if (position < tokens.Count)
                return tokens[position++];
            else
                return null;
        }
    }
}
