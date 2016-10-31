using System.Collections.Generic;

namespace GSharpLang.Runtime
{
    public class GSharpLabel
    {
        public int Position { get; set; }
        public int LabelID { get; set; }

        public GSharpLabel(int labelID)
        {
            LabelID = labelID;
            Position = 0;
        }
    }
    
    public class InstanceMethodCallback : GSharpObject
    {
        public GSharpMethod Method { get; private set; }
        private GSharpObject self;
        
        public InstanceMethodCallback(GSharpMethod method, GSharpObject self)
        {
            Method = method;
            this.self = self;
        }
        
        public override GSharpObject Invoke(VirtualMachine vm, GSharpObject[] arguments)
        {
            return vm.InvokeMethod(Method, self, arguments);
        }
    }

    public class GSharpMethod : GSharpObject
    {
        public IList<Instruction> Body { get { return instructions; } }
        public string Name { get; private set; }
        public Dictionary<string, int> Parameters { get; private set; }
        public int ParameterCount { get; private set; }
        public int LocalCount { get; private set; }
        public GSharpModule Module { get; private set; }
        public bool IsInstanceMethod { get; set; }

        protected List<Instruction> instructions = new List<Instruction>();
        private static int nextLabelID = 0;
        private Dictionary<int, GSharpLabel> labelReferences = new Dictionary<int, GSharpLabel>();

        public GSharpMethod(string name, int parameterCount, int localCount, GSharpModule module)
        {
            Name = name;
            Parameters = new Dictionary<string, int>();
            ParameterCount = parameterCount;
            LocalCount = localCount;
            Module = module;
            IsInstanceMethod = false;
        }

        public void EmitInstruction(OperationCode opcode)
        {
            instructions.Add(new Instruction(opcode));
        }

        public void EmitInstruction(OperationCode opcode, int arg)
        {
            instructions.Add(new Instruction(opcode, arg));
        }

        public void EmitInstruction(OperationCode opcode, GSharpLabel label)
        {
            labelReferences[instructions.Count] = label;
            instructions.Add(new Instruction(opcode, 0));
        }

        public int CreateTemporaly()
        {
            return LocalCount++;
        }

        public GSharpLabel CreateLabel()
        {
            return new GSharpLabel(nextLabelID++);
        }

        public void MarkLabelPosition(GSharpLabel label)
        {
            label.Position = instructions.Count;
        }

        public void FinalizeLabels()
        {
            foreach (int position in labelReferences.Keys)
                instructions[position] = new Instruction(instructions[position].OperationCode, labelReferences[position].Position);
        }

        public override GSharpObject Invoke(VirtualMachine vm, GSharpObject[] arguments)
        {
            return vm.InvokeMethod(this, null, arguments);
        }
    }
}
