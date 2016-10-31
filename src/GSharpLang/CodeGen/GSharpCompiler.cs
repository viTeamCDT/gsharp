using GSharpLang.AST.Nodes;
using GSharpLang.Runtime;

namespace GSharpLang.CodeGen
{
    public class GSharpCompiler
    {
        private SymbolTable symbolTable;

        public GSharpCompiler(SymbolTable symbolTable)
        {
            this.symbolTable = symbolTable;
        }

        public GSharpModule CompileAst(Node ast)
        {
            GSharpModule module = new GSharpModule("");
            ModuleCompiler compiler = new ModuleCompiler(symbolTable, module);
            compiler.Visit(ast);
            return module;
        }
    }
}
