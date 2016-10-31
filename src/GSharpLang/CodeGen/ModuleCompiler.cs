using GSharpLang.AST.Nodes;
using GSharpLang.Runtime;

namespace GSharpLang.CodeGen
{
    public class ModuleCompiler
    {
        private SymbolTable symbolTable;
        private int currentScope = 0;
        private GSharpModule module;

        public ModuleCompiler(SymbolTable symbolTable, GSharpModule module)
        {
            this.symbolTable = symbolTable;
            this.module = module;
        }

        public void Visit(Node node)
        {
            if (node is ClassDeclarationNode)
            {
                GSharpClass clazz = new GSharpClass(((ClassDeclarationNode)node).Name, CompileMethod(((ClassDeclarationNode)node).Constructor, true));
                for (int i = 1; i < node.Children.Count; i++)
                    clazz.AddInstanceMethod(CompileMethod((FunctionDeclarationNode)node.Children[i], true));
                module.SetAttribute(((ClassDeclarationNode)node).Name, clazz);
            }
            else if (node is CodeBlock)
                VisitSubnodes(node);
            else if (node is FunctionDeclarationNode)
                module.AddMethod(CompileMethod((FunctionDeclarationNode)node, false));
            else if (node is ScopeNode)
                VisitSubnodes(node);
            else if (node is UseNode)
            {
            	GSharpModule import = GSharpModule.CompileModule(((UseNode)node).Module);
            	if (import != null)
            			module.SetAttribute(((UseNode)node).Module, import);
            }
        }

        private GSharpMethod CompileMethod(FunctionDeclarationNode funcDecl, bool isInstanceMethod)
        {
            symbolTable.CurrentScope = symbolTable.CurrentScope.ChildScopes[currentScope++];
            GSharpMethod methodBuilder = new GSharpMethod(funcDecl.Name, funcDecl.Parameters.Count, symbolTable.CurrentScope.SymbolCount, module);
            FunctionCompiler compiler = new FunctionCompiler(symbolTable, methodBuilder);
            for (int i = 0; i < funcDecl.Parameters.Count; i++)
                methodBuilder.Parameters[funcDecl.Parameters[i]] = symbolTable.GetSymbol(funcDecl.Parameters[i]).Index;
            methodBuilder.IsInstanceMethod = isInstanceMethod;
            compiler.Visit(funcDecl.Children[0]);
            symbolTable.CurrentScope = symbolTable.CurrentScope.ParentScope;
            methodBuilder.EmitInstruction(OperationCode.LoadNull);
            methodBuilder.FinalizeLabels();
            return methodBuilder;
        }

        private void VisitSubnodes(Node root)
        {
            foreach (Node node in root.Children)
                Visit(node);
        }
    }
}
