using GSharpLang.AST.Nodes;
using System.Collections.Generic;

namespace GSharpLang.Runtime
{
    public class VirtualMachine
    {
        public GSharpStack Stack { get; private set; }

        private Dictionary<string, GSharpObject> globalDictionary = new Dictionary<string, GSharpObject>();

        public VirtualMachine()
        {
            Stack = new GSharpStack();
            BuiltinModules.GSharpSystem.GSharpSystem.SetupGSharpSystemModule(globalDictionary);
        }

        public GSharpObject InvokeMethod(GSharpMethod method, GSharpObject self, GSharpObject[] arguments)
        {
            Stack.NewFrame(method.LocalCount, method, self);
            int insCount = method.Body.Count;
            int i = 0;

            foreach (string param in method.Parameters.Keys)
                Stack.StoreLocal(method.Parameters[param], arguments[i++]);

            StackFrame top = Stack.Top;

            while (top.InstructionPointer < insCount && !top.AbortExecution)
            {
                Instruction currInstr = method.Body[Stack.InstructionPointer++];
                ExecuteInstruction(currInstr);
            }

            if (top.AbortExecution)
                return null;

            GSharpObject retVal = Stack.Pop();
            Stack.EndFrame();

            return retVal;
        }

        public GSharpObject InvokeMethod(GSharpMethod method, StackFrame frame, GSharpObject self, GSharpObject[] arguments)
        {
            Stack.NewFrame(frame);
            int insCount = method.Body.Count;
            int i = 0;

            foreach (string param in method.Parameters.Keys)
                Stack.StoreLocal(method.Parameters[param], arguments[i++]);

            StackFrame top = Stack.Top;

            while (top.InstructionPointer < insCount && !top.AbortExecution)
            {
                Instruction currInstr = method.Body[Stack.InstructionPointer++];
                ExecuteInstruction(currInstr);
            }

            if (top.AbortExecution)
                return null;

            GSharpObject retVal = Stack.Pop();
            Stack.EndFrame();

            return retVal;
        }

        private void ExecuteInstruction(Instruction ins)
        {
            switch (ins.OperationCode)
            {
                case OperationCode.Pop:
                    Stack.Pop();
                    break;
                case OperationCode.LoadTrue:
                    Stack.Push(GSharpBool.True);
                    break;
                case OperationCode.LoadFalse:
                    Stack.Push(GSharpBool.False);
                    break;
                case OperationCode.StoreLocal:
                    Stack.StoreLocal(ins.Argument, Stack.Pop());
                    break;
                case OperationCode.LoadLocal:
                    Stack.Push(Stack.LoadLocal(ins.Argument));
                    break;
                case OperationCode.StoreGlobal:
                    {
                        string name = ((GSharpName)Stack.CurrentModule.ConstantPool[ins.Argument]).Value;
                        if (Stack.CurrentModule.HasAttribute(name))
                            Stack.CurrentModule.SetAttribute(name, Stack.Pop());
                        else
                            globalDictionary[name] = Stack.Pop();
                        break;
                    }
                case OperationCode.LoadGlobal:
                    {
                        string name = ((GSharpName)Stack.CurrentModule.ConstantPool[ins.Argument]).Value;
                        if (globalDictionary.ContainsKey(name))
                            Stack.Push(globalDictionary[name]);
                        else
                            Stack.Push(Stack.CurrentModule.GetAttribute(name));
                        break;
                    }
                case OperationCode.StoreAttribute:
                    {
                        GSharpObject target = Stack.Pop();
                        GSharpObject value = Stack.Pop();
                        string attribute = ((GSharpName)Stack.CurrentModule.ConstantPool[ins.Argument]).Value;
                        target.SetAttribute(attribute, value);
                        break;
                    }
                case OperationCode.LoadAttribute:
                    {
                        GSharpObject target = Stack.Pop();
                        string attribute = ((GSharpName)Stack.CurrentModule.ConstantPool[ins.Argument]).Value;
                        if (target.HasAttribute(attribute))
                            Stack.Push(target.GetAttribute(attribute));
                        else
                            throw new System.Exception("Could not find attribute " + attribute + ".");
                        break;
                    }
                case OperationCode.LoadConst:
                    Stack.Push(Stack.CurrentModule.ConstantPool[ins.Argument]);
                    break;
                case OperationCode.LoadNull:
                    Stack.Push(null);
                    break;
                case OperationCode.LoadIndex:
                    {
                        GSharpObject index = Stack.Pop();
                        GSharpObject target = Stack.Pop();
                        Stack.Push(target.GetIndex(this, index));
                        break;
                    }
                case OperationCode.StoreIndex:
                    {
                        GSharpObject index = Stack.Pop();
                        GSharpObject target = Stack.Pop();
                        GSharpObject value = Stack.Pop();
                        target.SetIndex(this, index, value);
                        break;
                    }
                case OperationCode.LoadThis:
                    Stack.Push(Stack.Self);
                    break;
                case OperationCode.BinaryOperation:
                    Stack.Push(Stack.Pop().PerformBinaryOperation(this, (BinaryOperation)ins.Argument, Stack.Pop()));
                    break;
                case OperationCode.InstanceOf:
                    {
                        GSharpObject o1 = Stack.Pop();
                        GSharpObject o2 = Stack.Pop();
                        Stack.Push(new GSharpBool(o1.Type == o2.Type));
                        break;
                    }
                case OperationCode.Invoke:
                    {
                        GSharpObject target = Stack.Pop();
                        GSharpObject[] arguments = new GSharpObject[ins.Argument];
                        for (int i = 1; i <= ins.Argument; i++)
                            arguments[ins.Argument - i] = Stack.Pop();
                        Stack.Push(target.Invoke(this, arguments));
                        break;
                    }
                case OperationCode.Return:
                    Stack.InstructionPointer = int.MaxValue;
                    break;
                case OperationCode.JumpIfTrue:
                    if (Stack.Pop().IsTrue())
                        Stack.InstructionPointer = ins.Argument;
                    break;
                case OperationCode.JumpIfFalse:
                    if (!Stack.Pop().IsTrue())
                        Stack.InstructionPointer = ins.Argument;
                    break;
                case OperationCode.Jump:
                    Stack.InstructionPointer = ins.Argument;
                    break;
            }
        }
    }
}
