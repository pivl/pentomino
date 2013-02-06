using System;

namespace ExactCover
{
	// The node that represents one's in a sparse matrix that used in a Dancing Links implementation of Algorythm X 

	public class Node {
		
		//Link to node to the left in a row
		public Node left;
		//Link to node to the right in a row
		public Node right;
		//Link to up node in a column
		public Node up;
		//Link to down node in a column
		public Node down;
		
		public ColumnHeader header;

		public Object nodedata;

		public Node() { 
			left = right = up = down = this;
		}
	}

} // end of namespace
