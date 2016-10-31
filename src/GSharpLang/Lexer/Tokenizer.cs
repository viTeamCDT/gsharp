using System;
using System.Collections.Generic;

namespace GSharpLang.Lexer
{
    public class Tokenizer
    {
        private string code;
        private int position;
        private Location location;
        private List<Token> result;

        public Tokenizer(string code)
        {
            this.code = code;
            position = 0;
            location = new Location(1, 1);
        }

        public List<Token> Lex()
        {
            result = new List<Token>();

            EatWhiteSpaces();

            while (PeekChar() != -1)
            {
                Location loc = new Location(location.Line, location.Column);
                if ((char)PeekChar() == '/')
                {
                    if ((char)PeekChar(1) == '/' || (char)PeekChar(1) == '*')
                        ReadComment();
                    else
                        result.Add(ReadOperator());
                }
                else if (char.IsLetter((char)PeekChar()) || (char)PeekChar() == '_')
                    result.Add(ReadIdentifier());
                else if (char.IsDigit((char)PeekChar()))
                    result.Add(ReadNumber());
                else if ((char)PeekChar() == '"')
                    result.Add(ReadString());
                else if ("+-*=<>!&^|%".Contains(((char)PeekChar()).ToString()))
                    result.Add(ReadOperator());
                else if ("()".Contains(((char)PeekChar()).ToString()))
                    result.Add(new Token(TokenType.Parentheses, ((char)ReadChar()).ToString(), loc));
                else if ("{}".Contains(((char)PeekChar()).ToString()))
                    result.Add(new Token(TokenType.Brace, ((char)ReadChar()).ToString(), loc));
                else if ("[]".Contains(((char)PeekChar()).ToString()))
                    result.Add(new Token(TokenType.Bracket, ((char)ReadChar()).ToString(), loc));
                else if ((char)PeekChar() == ';')
                    result.Add(new Token(TokenType.Semicolon, ((char)ReadChar()).ToString(), loc));
                else if ((char)PeekChar() == ',')
                    result.Add(new Token(TokenType.Comma, ((char)ReadChar()).ToString(), loc));
                else if ((char)PeekChar() == '.')
                    result.Add(new Token(TokenType.Dot, ((char)ReadChar()).ToString(), loc));
                else
                    throw new Exception("Unrecognized character at line " + location.Line + ", column " + location.Column + ".");

                EatWhiteSpaces();
            }

            return result;
        }

        private void ReadComment()
        {
            if ((char)PeekChar() == '/')
            {
                if ((char)PeekChar(1) == '/')
                {
                    while (PeekChar() != -1 && (char)PeekChar() != '\n')
                        ReadChar();
                }
                else if ((char)PeekChar(1) == '*')
                {
                    ReadChar();
                    ReadChar();
                    while (PeekChar() != -1)
                    {
                        if (PeekChar(1) != -1 && PeekChar() == '*' && PeekChar(1) == '/')
                            break;
                        ReadChar();
                    }
                    ReadChar();
                    ReadChar();
                }
            }
        }

        private Token ReadIdentifier()
        {
            string str = "";
            Location loc = new Location(location.Line, location.Column);
            while (char.IsLetterOrDigit((char)PeekChar()) || (char)PeekChar() == '_')
                str += ((char)ReadChar()).ToString();

            switch (str)
            {
                case "function":
                case "if":
                case "else":
                case "for":
                case "while":
                case "true":
                case "false":
                case "return":
                case "incfile":
                case "class":
                case "this":
                    return new Token(TokenType.Keyword, str, loc);
                default:
                    return new Token(TokenType.Identifier, str, loc);
            }
        }

        private Token ReadNumber()
        {
            string str = "";
            Location loc = new Location(location.Line, location.Column);
            while (char.IsDigit((char)PeekChar()))
                str += ((char)ReadChar()).ToString();
            return new Token(TokenType.Number, str, loc);
        }

        private Token ReadString()
        {
            string str = "";
            ReadChar();
            Location loc = new Location(location.Line, location.Column);
            while (PeekChar() != -1 && (char)PeekChar() != '"')
                if ((char)PeekChar() == '\\')
                {
                    ReadChar();
                    str += ParseEscapeCode().ToString();
                }
                else
                    str += ((char)ReadChar()).ToString();
            ReadChar();
            return new Token(TokenType.String, str, loc);
        }

        private char ParseEscapeCode()
        {
            switch ((char)ReadChar())
            {
                case '"':
                    return '"';
                case 'n':
                    return '\n';
                case 'b':
                    return '\b';
                case 'r':
                    return '\r';
                case 't':
                    return '\t';
                case 'f':
                    return '\f';
                case '\\':
                    return '\\';
                default:
                    throw new Exception("Unreccognized escape sequence \\" + (char)PeekChar());
            }
        }

        private Token ReadOperator()
        {
            Location loc = new Location(location.Line, location.Column);
            char op = (char)ReadChar();
            string nextTwoChars = op + ((char)PeekChar()).ToString();

            switch (nextTwoChars)
            {
                case ">>":
                case "<<":
                case "&&":
                case "||":
                case "==":
                case "!=":
                case "=>":
                case "<=":
                case ">=":
                case "+=":
                case "-=":
                case "*=":
                case "/=":
                case "%=":
                case "++":
                case "--":
                    ReadChar();
                    return new Token(TokenType.Operator, nextTwoChars, loc);
                default:
                    return new Token(TokenType.Operator, op.ToString(), loc);
            }
        }

        private void EatWhiteSpaces()
        {
            while (char.IsWhiteSpace((char)PeekChar()))
                ReadChar();
        }

        private int PeekChar(int n = 0)
        {
            if (position + n < code.Length)
                return code[position + n];
            else
                return -1;
        }
        
        private int ReadChar()
        {
            if (position < code.Length)
            {
                if (code[position] == '\n')
                    location.IncrementLine();
                else if (code[position] == '\t')
                    for (int i = 0; i < 4; i++)
                        location.IncrementColumn();
                else
                    location.IncrementColumn();
                return code[position++];
            }
            else
                return -1;
        }
    }
}
