namespace GSharpLang.AST.Nodes
{
	public class NumberNode : Node
	{
		public int Value { get; private set; }
		
		public NumberNode(int value)
		{
			Value = value;
		}
	}
}