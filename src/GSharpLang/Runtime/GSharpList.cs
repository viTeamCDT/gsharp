using System.Collections.Generic;

namespace GSharpLang.Runtime
{
    public class GSharpList : GSharpObject
    {
        public List<GSharpObject> Objects { get; private set; }

        private int iterIndex = 0;

        public GSharpList(GSharpObject[] items) : base(false)
        {
            Objects = new List<GSharpObject>();
            Objects.AddRange(items);
            SetAttribute("size", new InternalMethodCallback(size, this));
            SetAttribute("add", new InternalMethodCallback(add, this));
        }

        public override GSharpObject GetIndex(VirtualMachine vm, GSharpObject key)
        {
            GSharpInteger index = key as GSharpInteger;
            return Objects[index.Value];
        }

        public override void SetIndex(VirtualMachine vm, GSharpObject key, GSharpObject value)
        {
            GSharpInteger index = key as GSharpInteger;
            Objects[index.Value] = value;
        }

        public override GSharpObject IterGetNext(VirtualMachine vm)
        {
            return Objects[iterIndex - 1];
        }

        public override bool IterMoveNext(VirtualMachine vm)
        {
            if (iterIndex >= Objects.Count)
                return false;
            iterIndex++;
            return true;
        }

        public override void IterReset(VirtualMachine vm)
        {
            iterIndex = 0;
        }

        public void Add(GSharpObject obj)
        {
            Objects.Add(obj);
        }

        private GSharpObject size(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            return new GSharpInteger(((GSharpList)self).Objects.Count);
        }

        private GSharpObject add(VirtualMachine vm, GSharpObject self, GSharpObject[] arguments)
        {
            GSharpList list = self as GSharpList;
            foreach (GSharpObject obj in arguments)
                list.Add(obj);
            return null;
        }

        public override int GetHashCode()
        {
            return Objects.GetHashCode();
        }
    }
}
