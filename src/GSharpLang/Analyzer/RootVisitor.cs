using GSharpLang.AST.Nodes;

namespace GSharpLang.Analyzer
{
    public class RootVisitor
    {
        private SymbolTable symbolTable;

        public RootVisitor(SymbolTable symbolTable)
        {
            this.symbolTable = symbolTable;
        }

        public void Visit(Node node)
        {
            if (node is ClassDeclarationNode)
                VisitSubnodes(node);
            else if (node is CodeBlock)
                VisitSubnodes(node);
            else if (node is ExpressionNode)
                throw new System.Exception("Expression is not valid outside function body.");
            else if (node is ForNode)
                throw new System.Exception("For Statement is not valid outside function body.");
            else if (node is FunctionCallNode)
                throw new System.Exception("Function Call is not valid outside function body.");
            else if (node is FunctionDeclarationNode)
            {
                FunctionDeclarationNode funcDecl = (FunctionDeclarationNode)node;
                symbolTable.AddSymbol(funcDecl.Name);
                FunctionVisitor visitor = new FunctionVisitor(symbolTable);
                symbolTable.BeginScope();
                foreach (string param in funcDecl.Parameters)
                    symbolTable.AddSymbol(param);
                visitor.Visit(funcDecl.Children[0]);
                symbolTable.EndScope();
            }
            else if (node is IfNode)
                throw new System.Exception("If Statement is not valid outside function body.");
            else if (node is ReturnNode)
                throw new System.Exception("Return Statement is not valid outside function body.");
            else if (node is ScopeNode)
                VisitSubnodes(node);
            else if (node is WhileNode)
                throw new System.Exception("While Statement is not valid outside function body.");
        }

        private void VisitSubnodes(Node root)
        {
            foreach (Node node in root.Children)
                Visit(node);
        }
    }
}
