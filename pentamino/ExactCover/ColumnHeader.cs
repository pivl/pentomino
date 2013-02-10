using System;

namespace ExactCover
{
	// The Column Header that is a special node in a sparse matrix that used in a Dancing Links implementation of Algorythm X 
	public class ColumnHeader : Node
	{
		public int count; // count of nodes in a column
		
		public bool optional = false; // is this constraint required to be in the solution or it is optional

		public void Add(Node node)
		{
	        node.header = this;
			
			// The node being added points down to the header
	        node.down = this;
			
			node.up = this.up;
			
			this.up.down = node;
	        this.up = node;
			
			count++;
	    }
		
		public ColumnHeader()
		{
			header = this;
		}
	}
}
