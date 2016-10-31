namespace GSharpLang
{
    public enum SymbolType
    {
        Local,
        Global
    }

    public class Symbol
    {
        public string Name { get; private set; }
        public int Index { get; private set; }
        public SymbolType Type { get; private set; }

        public Symbol(string name, int index, SymbolType type)
        {
            Name = name;
            Index = index;
            Type = type;
        }
    }
}
