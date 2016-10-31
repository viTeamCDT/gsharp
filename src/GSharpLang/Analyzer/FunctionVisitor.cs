using GSharpLang.AST.Nodes;

namespace GSharpLang.Analyzer
{
    public class FunctionVisitor
    {
        private SymbolTable symbolTable;

        public FunctionVisitor(SymbolTable symbolTable)
        {
            this.symbolTable = symbolTable;
        }

        public void Visit(Node node)
        {
            if (node is ArgumentsListNode)
                VisitSubnodes(node);
            else if (node is BinaryOperationNode)
            {
                BinaryOperationNode binop = (BinaryOperationNode)node;
                if (binop.BinaryOperation == BinaryOperation.Assignment)
                    if (binop.Left is IdentifierNode)
                    {
                        IdentifierNode ident = (IdentifierNode)binop.Left;
                        if (!symbolTable.IsSymbolDefined(ident.Name))
                            symbolTable.AddSymbol(ident.Name);
                    }
                VisitSubnodes(node);
            }
            else if (node is ClassDeclarationNode)
                throw new System.Exception("Can't define an class inside of an function.");
            else if (node is CodeBlock)
                VisitSubnodes(node);
            else if (node is EnumDeclarationNode)
                symbolTable.AddSymbol(((EnumDeclarationNode)node).Name);
            else if (node is ExpressionNode)
                VisitSubnodes(node);
            else if (node is ForNode)
                VisitSubnodes(node);
            else if (node is FunctionCallNode)
                VisitSubnodes(node);
            else if (node is FunctionDeclarationNode)
                throw new System.Exception("Can't define an function inside of an function.");
            else if (node is GetAttributeNode)
                VisitSubnodes(node);
            else if (node is IfNode)
                VisitSubnodes(node);
            else if (node is IndexerNode)
                VisitSubnodes(node);
            else if (node is ReturnNode)
                VisitSubnodes(node);
            else if (node is ScopeNode)
            {
                symbolTable.BeginScope();
                VisitSubnodes(node);
                symbolTable.EndScope();
            }
            else if (node is FunctionDeclarationNode)
                throw new System.Exception("Using statement is not valid inside function body.");
            else if (node is WhileNode)
                VisitSubnodes(node);
        }

        private void VisitSubnodes(Node root)
        {
            foreach (Node node in root.Children)
                Visit(node);
        }
    }
}
