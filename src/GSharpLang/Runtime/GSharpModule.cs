using GSharpLang.Analyzer;
using GSharpLang.AST;
using GSharpLang.AST.Nodes;
using GSharpLang.CodeGen;
using GSharpLang.Lexer;
using System.Collections.Generic;
using System.IO;

namespace GSharpLang.Runtime
{
    public class GSharpModule : GSharpObject
    {
        public string Name { get; private set; }
        public IList<GSharpObject> ConstantPool { get { return constantPool; } }

        private List<GSharpObject> constantPool = new List<GSharpObject>();
        
        public GSharpModule(string name) : base("Module")
        {
            Name = name;
        }

        public void AddMethod(GSharpMethod method)
        {
            SetAttribute(method.Name, method);
        }

        public int DefineConstant(GSharpObject obj)
        {
            constantPool.Add(obj);
            return constantPool.Count - 1;
        }

        public static GSharpModule CompileModule(string file)
        {
            if (FindModule(file) != null)
            {
                Node abstractSyntaxTree = (new Parser(new Tokenizer(File.ReadAllText(FindModule(file))).Lex())).Parse();
                SymbolTable symbolTable = (new SemanticAnalyser()).Analyse(abstractSyntaxTree);
                GSharpModule module = (new GSharpCompiler(symbolTable)).CompileAst(abstractSyntaxTree);
                return module;
            }
            else
                throw new System.Exception("Invalid module file " + file + ".");
        }

        public static string FindModule(string name)
        {
            if (File.Exists(name))
                return name;
            else if (File.Exists(name + ".gs"))
                return name + ".gs";
            else
                return null;
        }
    }
}
