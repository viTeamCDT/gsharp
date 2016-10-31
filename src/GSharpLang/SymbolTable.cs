using System.Collections.Generic;

namespace GSharpLang
{
    public class Scope
    {
        public Scope ParentScope { get; private set; }
        public IList<Scope> ChildScopes { get { return childScopes; } }
        public int SymbolCount { get { int val = symbols.Count; foreach (Scope scope in childScopes) val += scope.SymbolCount; return val; } }

        private List<Symbol> symbols = new List<Symbol>();
        private List<Scope> childScopes = new List<Scope>();

        public Scope()
        {
            ParentScope = null;
        }

        public Scope(Scope parent)
        {
            ParentScope = parent;
        }

        public int AddSymbol(string name, int index, SymbolType type)
        {
            symbols.Add(new Symbol(name, index, type));
            return index;
        }

        public void AddScope(Scope scope)
        {
            childScopes.Add(scope);
        }

        public Symbol GetSymbol(string name)
        {
            foreach (Symbol sym in symbols)
                if (sym.Name == name)
                    return sym;
            return null;
        }
    }

    public class SymbolTable
    {
        public Scope CurrentScope { get; set; }

        private int nextGlobalIndex = 0;
        private int nextLocalIndex = 0;
        private Scope globalScope = new Scope();

        public SymbolTable()
        {
            CurrentScope = globalScope;
        }

        public void BeginScope()
        {
            Scope newScope = new Scope(CurrentScope);
            CurrentScope.AddScope(newScope);
            CurrentScope = newScope;
        }

        public void EndScope()
        {
            CurrentScope = CurrentScope.ParentScope;
            if (CurrentScope == globalScope)
                nextLocalIndex = 0;
        }

        public int AddSymbol(string name)
        {
            if (CurrentScope.ParentScope != null)
                return CurrentScope.AddSymbol(name, nextLocalIndex++, SymbolType.Local);
            else
                return CurrentScope.AddSymbol(name, nextGlobalIndex++, SymbolType.Global);
        }

        public bool IsSymbolDefined(string name)
        {
            Scope curr = CurrentScope;
            while (curr != null)
            {
                if (curr.GetSymbol(name) != null)
                    return true;
                curr = curr.ParentScope;
            }
            return false;
        }

        public Symbol GetSymbol(string name)
        {
            Scope curr = CurrentScope;
            while (curr != null)
            {
                Symbol sym = curr.GetSymbol(name);
                if (sym != null)
                    return sym;
                curr = curr.ParentScope;
            }
            return null;
        }
    }
}
