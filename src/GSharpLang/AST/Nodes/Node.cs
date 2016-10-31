using System.Collections.Generic;

namespace GSharpLang.AST.Nodes
{
	public abstract class Node
	{
		public List<Node> Children { get { return children; } }
		
		private List<Node> children = new List<Node>();
	}
}