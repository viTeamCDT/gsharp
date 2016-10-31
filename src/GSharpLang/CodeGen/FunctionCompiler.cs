using GSharpLang.AST.Nodes;
using GSharpLang.Runtime;
using GSharpLang.Runtime.BuiltinModules.GSharpSystem;
using System.Collections.Generic;

namespace GSharpLang.CodeGen
{
    public class FunctionCompiler
    {
        private SymbolTable symbolTable;
        private GSharpMethod methodBuilder;
        private int currentScope = 0;
        private Stack<GSharpLabel> breakLabels = new Stack<GSharpLabel>();

        public FunctionCompiler(SymbolTable symbolTable, GSharpMethod methodBuilder)
        {
            this.symbolTable = symbolTable;
            this.methodBuilder = methodBuilder;
        }

        public void Visit(Node node)
        {
            if (node is ArgumentsListNode)
                VisitSubnodes(node);
            else if (node is BinaryOperationNode)
            {
                BinaryOperationNode binop = (BinaryOperationNode)node;

                if (binop.BinaryOperation == BinaryOperation.Assignment)
                {
                    Visit(binop.Right);

                    if (binop.Left is IdentifierNode)
                    {
                        IdentifierNode ident = (IdentifierNode)binop.Left;
                        Symbol sym = symbolTable.GetSymbol(ident.Name);
                        if (sym.Type == SymbolType.Local)
                        {
                            methodBuilder.EmitInstruction(OperationCode.StoreLocal, sym.Index);
                            methodBuilder.EmitInstruction(OperationCode.LoadLocal, sym.Index);
                        }
                        else
                        {
                            int globalIndex = methodBuilder.Module.DefineConstant(new GSharpName(ident.Name));
                            methodBuilder.EmitInstruction(OperationCode.StoreGlobal, globalIndex);
                            methodBuilder.EmitInstruction(OperationCode.LoadGlobal, globalIndex);
                        }
                    }
                    else if (binop.Left is GetAttributeNode)
                    {
                        Visit(((GetAttributeNode)binop.Left).Target);
                        int attrIndex = methodBuilder.Module.DefineConstant(new GSharpName(((GetAttributeNode)binop.Left).Field));
                        methodBuilder.EmitInstruction(OperationCode.StoreAttribute, attrIndex);
                        Visit(((GetAttributeNode)binop.Left).Target);
                        methodBuilder.EmitInstruction(OperationCode.LoadAttribute, attrIndex);
                    }
                    else if (binop.Left is IndexerNode)
                    {
                        Visit(((IndexerNode)binop.Left).Target);
                        Visit(((IndexerNode)binop.Left).Index);
                        methodBuilder.EmitInstruction(OperationCode.StoreIndex);
                        Visit(binop.Left);
                    }
                }
                else
                {
                    Visit(binop.Right);
                    Visit(binop.Left);
                    methodBuilder.EmitInstruction(OperationCode.BinaryOperation, (int)binop.BinaryOperation);
                }
            }
            else if (node is BooleanFalseNode)
                methodBuilder.EmitInstruction(OperationCode.LoadFalse);
            else if (node is BooleanTrueNode)
                methodBuilder.EmitInstruction(OperationCode.LoadTrue);
            else if (node is CodeBlock)
                VisitSubnodes(node);
            else if (node is ExpressionNode)
                VisitSubnodes(node);
            else if (node is ForNode)
            {
                GSharpLabel forLabel = methodBuilder.CreateLabel();
                GSharpLabel breakLabel = methodBuilder.CreateLabel();
                breakLabels.Push(breakLabel);
                Visit(((ForNode)node).Initializer);
                methodBuilder.MarkLabelPosition(forLabel);
                Visit(((ForNode)node).Condition);
                methodBuilder.EmitInstruction(OperationCode.JumpIfFalse, breakLabel);
                Visit(((ForNode)node).Body);
                Visit(((ForNode)node).AfterThought);
                methodBuilder.EmitInstruction(OperationCode.Jump, forLabel);
                methodBuilder.MarkLabelPosition(breakLabel);
                breakLabels.Pop();
            }
            else if (node is FunctionCallNode)
            {
                Visit(((FunctionCallNode)node).Arguments);
                Visit(((FunctionCallNode)node).Target);
                methodBuilder.EmitInstruction(OperationCode.Invoke, ((FunctionCallNode)node).Arguments.Children.Count);
            }
            else if (node is GetAttributeNode)
            {
                Visit(((GetAttributeNode)node).Target);
                methodBuilder.EmitInstruction(OperationCode.LoadAttribute, methodBuilder.Module.DefineConstant(new GSharpName(((GetAttributeNode)node).Field)));
            }
            else if (node is IdentifierNode)
            {
                IdentifierNode ident = (IdentifierNode)node;

                if (symbolTable.IsSymbolDefined(ident.Name))
                {
                    Symbol sym = symbolTable.GetSymbol(ident.Name);
                    if (sym.Type == SymbolType.Local)
                        methodBuilder.EmitInstruction(OperationCode.LoadLocal, sym.Index);
                    else
                        methodBuilder.EmitInstruction(OperationCode.LoadGlobal, methodBuilder.Module.DefineConstant(new GSharpName(ident.Name)));
                }
                else
                    methodBuilder.EmitInstruction(OperationCode.LoadGlobal, methodBuilder.Module.DefineConstant(new GSharpName(ident.Name)));
            }
            else if (node is IfNode)
            {
                GSharpLabel elseLabel = methodBuilder.CreateLabel();
                GSharpLabel endLabel = methodBuilder.CreateLabel();
                Visit(((IfNode)node).Condition);
                methodBuilder.EmitInstruction(OperationCode.JumpIfFalse, elseLabel);
                Visit(((IfNode)node).Body);
                methodBuilder.EmitInstruction(OperationCode.Jump, endLabel);
                methodBuilder.MarkLabelPosition(elseLabel);
                Visit(((IfNode)node).ElseBody);
                methodBuilder.MarkLabelPosition(endLabel);
            }
            else if (node is IndexerNode)
            {
                Visit(((IndexerNode)node).Target);
                Visit(((IndexerNode)node).Index);
                methodBuilder.EmitInstruction(OperationCode.LoadIndex);
            }
            else if (node is NumberNode)
                methodBuilder.EmitInstruction(OperationCode.LoadConst, methodBuilder.Module.DefineConstant(new GSharpInteger(((NumberNode)node).Value)));
            else if (node is ReturnNode)
            {
            	VisitSubnodes(node);
            	methodBuilder.EmitInstruction(OperationCode.Return);
            }
            else if (node is ScopeNode)
            {
                symbolTable.CurrentScope = symbolTable.CurrentScope.ChildScopes[currentScope++];
                FunctionCompiler scopeCompiler = new FunctionCompiler(symbolTable, methodBuilder);
                foreach (Node n in node.Children)
                    scopeCompiler.Visit(n);
                symbolTable.CurrentScope = symbolTable.CurrentScope.ParentScope;

            }
            else if (node is StringNode)
                methodBuilder.EmitInstruction(OperationCode.LoadConst, methodBuilder.Module.DefineConstant(new GSharpString(((StringNode)node).Value)));
            else if (node is ThisNode)
                methodBuilder.EmitInstruction(OperationCode.LoadThis);
            else if (node is WhileNode)
            {
                GSharpLabel whileLabel = methodBuilder.CreateLabel();
                GSharpLabel breakLabel = methodBuilder.CreateLabel();
                breakLabels.Push(breakLabel);
                methodBuilder.MarkLabelPosition(whileLabel);
                Visit(((WhileNode)node).Condition);
                methodBuilder.EmitInstruction(OperationCode.JumpIfFalse, breakLabel);
                Visit(((WhileNode)node).Body);
                methodBuilder.EmitInstruction(OperationCode.Jump, whileLabel);
                methodBuilder.MarkLabelPosition(breakLabel);
                breakLabels.Pop();
            }
        }

        private void VisitSubnodes(Node root)
        {
            foreach (Node node in root.Children)
                Visit(node);
        }
    }
}
