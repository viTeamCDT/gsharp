namespace GSharpLang.Lexer
{
    public enum TokenType
    {
        Identifier,
        Keyword,
        Number,
        String,
        Operator,
        Parentheses,
        Brace,
        Bracket,
        Semicolon,
        Comma,
        Dot
    }

    public class Token
    {
        public TokenType Type { get; private set; }
        public string Value { get; private set; }
        public Location Location { get; private set; }

        public Token(TokenType type, string value, Location location)
        {
            Type = type;
            Value = value;
            Location = location;
        }
    }
}
