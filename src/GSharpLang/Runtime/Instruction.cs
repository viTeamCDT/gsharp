namespace GSharpLang.Runtime
{
    public enum OperationCode
    {
        BinaryOperation,
        InstanceOf,
        Pop,
        LoadTrue,
        LoadFalse,
        LoadLocal,
        StoreLocal,
        LoadGlobal,
        StoreGlobal,
        LoadAttribute,
        StoreAttribute,
        LoadConst,
        LoadNull,
        LoadIndex,
        StoreIndex,
        LoadThis,
        Invoke,
        Return,
        JumpIfTrue,
        JumpIfFalse,
        Jump
    }

    public class Instruction
    {
        public OperationCode OperationCode { get; set; }
        public int Argument { get; set; }

        public Instruction(OperationCode opcode)
        {
            OperationCode = opcode;
            Argument = 0;
        }

        public Instruction(OperationCode opcode, int arg)
        {
            OperationCode = opcode;
            Argument = arg;
        }
    }
}
