namespace GSharpLang.AST.Nodes
{
	public class FunctionCallNode : Node
	{
		public Node Target { get { return Children[0]; } }
		public Node Arguments { get { return Children[1]; } }
		
		public FunctionCallNode(Node target, Node arguments)
		{
			Children.Add(target);
			Children.Add(arguments);
		}
	}
}