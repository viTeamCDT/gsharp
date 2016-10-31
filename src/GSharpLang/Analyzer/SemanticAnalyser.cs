using GSharpLang.AST.Nodes;

namespace GSharpLang.Analyzer
{
    public class SemanticAnalyser
    {
        public SymbolTable Analyse(Node ast)
        {
            SymbolTable retTable = new SymbolTable();
            RootVisitor visitor = new RootVisitor(retTable);
            visitor.Visit(ast);
            return retTable;
        }
    }
}