using GSharpLang.AST.Nodes;
using System.Collections.Generic;
using System;

namespace GSharpLang.Runtime
{
    public class GSharpObject
    {
        public string Type { get; private set; }
        protected Dictionary<string, GSharpObject> attributes = new Dictionary<string, GSharpObject>();
        
        public GSharpObject(string type = "Object")
        {
            Type = type;
        }
        
        public void SetAttribute(string name, GSharpObject value)
        {
            if (value is GSharpMethod)
                if (((GSharpMethod)value).IsInstanceMethod)
                {
                    attributes[name] = new InstanceMethodCallback((GSharpMethod)value, this);
                    return;
                }
            else if (value is InstanceMethodCallback)
            {
                attributes[name] = new InstanceMethodCallback(((InstanceMethodCallback)value).Method, this);
                return;
            }
            
            attributes[name] = value;
        }

        public GSharpObject GetAttribute(string name)
        {
            return attributes[name];
        }

        public bool HasAttribute(string name)
        {
            return attributes.ContainsKey(name);
        }

        public virtual void SetIndex(VirtualMachine vm, GSharpObject key, GSharpObject value)
        { }

        public virtual GSharpObject GetIndex(VirtualMachine vm, GSharpObject key)
        {
            return null;
        }

        public virtual GSharpObject PerformBinaryOperation(VirtualMachine vm, BinaryOperation binop, GSharpObject rval)
        {
            GSharpObject[] arguments = new GSharpObject[] { rval };

            switch (binop)
            {
                case BinaryOperation.Addition:
                    return GetAttribute("_add").Invoke(vm, arguments);
                case BinaryOperation.Subtraction:
                    return GetAttribute("_sub").Invoke(vm, arguments);
                case BinaryOperation.Multiplication:
                    return GetAttribute("_mul").Invoke(vm, arguments);
                case BinaryOperation.Division:
                    return GetAttribute("_div").Invoke(vm, arguments);
                case BinaryOperation.BooleanAnd:
                    return GetAttribute("_boolAnd").Invoke(vm, arguments);
                case BinaryOperation.BooleanOr:
                    return GetAttribute("_boolOr").Invoke(vm, arguments);
                case BinaryOperation.Modulus:
                    return GetAttribute("_mod").Invoke(vm, arguments);
                case BinaryOperation.Equals:
                    return GetAttribute("_equals").Invoke(vm, arguments);
                case BinaryOperation.NotEqualTo:
                    return GetAttribute("_notEquals").Invoke(vm, arguments);
                case BinaryOperation.LessThan:
                    return GetAttribute("_lessThan").Invoke(vm, arguments);
                case BinaryOperation.GreaterThan:
                    return GetAttribute("_greaterThan").Invoke(vm, arguments);
                case BinaryOperation.LesserOrEqual:
                    return GetAttribute("_lessOrEqual").Invoke(vm, arguments);
                case BinaryOperation.GreaterOrEqual:
                    return GetAttribute("_greaterOrEqual").Invoke(vm, arguments);
                default:
                    return null;
            }
        }

        public virtual GSharpObject Invoke(VirtualMachine vm, GSharpObject[] arguments)
        {
            throw new System.Exception("Object does not support invocation.");
        }

        public virtual bool IsTrue()
        {
            return false;
        }

        public virtual GSharpObject IterGetNext(VirtualMachine vm)
        {
            return GetAttribute("_iterGetNext").Invoke(vm, new GSharpObject[] { });
        }

        public virtual bool IterMoveNext(VirtualMachine vm)
        {
            return GetAttribute("_iterMoveNext").Invoke(vm, new GSharpObject[] { }).IsTrue();
        }

        public virtual void IterReset(VirtualMachine vm)
        {
            GetAttribute("_iterReset").Invoke(vm, new GSharpObject[] { });
        }
        
        public override string ToString()
        {
            return Type;
        }
        
        public override int GetHashCode()
        {
            int accum = 17;
            unchecked
            {
                foreach (GSharpObject obj in attributes.Values)
                    accum += 23 * obj.GetHashCode();
            }
            return accum;
        }
    }
}
