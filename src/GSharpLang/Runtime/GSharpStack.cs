using System.Collections.Generic;

namespace GSharpLang.Runtime
{
    public class GSharpStack
    {
        public GSharpMethod CurrentMethod { get { return top.Method; } }
        public GSharpModule CurrentModule { get { return top.Module; } }
        public GSharpObject Self { get { return top.Self; } }
        public StackFrame Top { get { return top; } }
        public int Frames { get; private set; }
        public int InstructionPointer { get { return top.InstructionPointer; } set { top.InstructionPointer = value; } }

        private Stack<StackFrame> frames = new Stack<StackFrame>();
        private StackFrame top;

        public void NewFrame(StackFrame frame)
        {
            Frames++;
            top = frame;
            frames.Push(frame);
        }

        public void NewFrame(int localCount, GSharpMethod method, GSharpObject self)
        {
            Frames++;
            top = new StackFrame(localCount, method, self);
            frames.Push(top);
        }

        public void EndFrame()
        {
            Frames--;
            frames.Pop();
            if (frames.Count != 0)
                top = frames.Peek();
            else
                top = null;
        }

        public void StoreLocal(int index, GSharpObject obj)
        {
            top.StoreLocal(index, obj);
        }

        public GSharpObject LoadLocal(int index)
        {
            return top.LoadLocal(index);
        }

        public void Push(GSharpObject obj)
        {
            top.Push(obj);
        }

        public GSharpObject Pop()
        {
            return top.Pop();
        }
         
        public void Unwind(int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                StackFrame frame = this.frames.Pop();
                frame.AbortExecution = true;
            }
            Frames -= frames;
            top = this.frames.Peek();
        }
    }

    public class StackFrame
    {
        public int LocalCount { get; private set; }
        public bool AbortExecution { get; set; }
        public GSharpMethod Method { get; private set; }
        public GSharpModule Module { get; private set; }
        public GSharpObject Self { get; set; }
        public int InstructionPointer { get; set; }

        private Stack<GSharpObject> stack = new Stack<GSharpObject>();
        private GSharpObject[] locals;

        public StackFrame(int localCount, GSharpMethod method, GSharpObject self)
        {
            LocalCount = localCount;
            Method = method;
            Module = method.Module;
            Self = self;
            locals = new GSharpObject[localCount];
        }

        public void StoreLocal(int index, GSharpObject obj)
        {
            locals[index] = obj;
        }

        public GSharpObject LoadLocal(int index)
        {
            return locals[index];
        }

        public void Push(GSharpObject obj)
        {
            stack.Push(obj);
        }

        public GSharpObject Pop()
        {
            return stack.Pop();
        }
    }
}
