namespace GSharpLang.AST.Nodes
{
	public enum BinaryOperation
	{
		Assignment,
		Addition,
		Subtraction,
		Division,
		Multiplication,
		Equals,
		LessThan,
		GreaterThan,
		NotEqualTo,
		LesserOrEqual,
		GreaterOrEqual,
		BooleanAnd,
		BooleanOr,
		Modulus,
		InstanceOf
	}
	
	public class BinaryOperationNode : Node
	{
		public BinaryOperation BinaryOperation { get; set; }
		public Node Left { get { return Children[0]; } }
		public Node Right { get { return Children[1]; } }
		
		public BinaryOperationNode(BinaryOperation type, Node left, Node right)
		{
			BinaryOperation = type;
			Children.Add(left);
			Children.Add(right);
		}
	}
}