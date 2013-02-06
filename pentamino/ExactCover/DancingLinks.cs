using System;
using System.Collections;

namespace ExactCover
{
	// The sparse array organized as double linked circular list of nodes and dancing links algorythm
	public class DancingLinks
	{
		public ArrayList columnHeaders; // of type column header
		public ColumnHeader rootHeader;
		
		public ArrayList Solution {
			get {return solution; }
		}
		
		ArrayList solution = new ArrayList(); // of type Node
		
		int nodes;
		int depth;

		public void InitHeaders(int ncolumns) {
			
			rootHeader = new ColumnHeader();
	
			for (int i = 0; i<ncolumns; i++)
			{
				ColumnHeader header = new ColumnHeader();
				
				columnHeaders.Add(header);
				
				// embedding the header to the list among other headers
				
				header.left = rootHeader.left;
				header.right = rootHeader;
				
				rootHeader.left.right = header;
				rootHeader.left = header;
			}
		
		}
		
		// the algorythm implementation itself
		public bool Solve() {
			// if array empty the solution is found
			if (rootHeader.right == rootHeader)
			{
				return true;
			}
		
			ColumnHeader header = GetShortestColumn();
		
			Remove(header);
		
			Node node = header.down;
			nodes++;
			
			while (node != header)
			{
				//Add node to solution
				solution.Add(node);
				nodes++;
		
				for (Node j = node.right; j != node; j = j.right)
					Remove(j.header);
				
				depth++;
				if (Solve() == true) return true;
				depth--;
				
				//Remove node from solution
				solution.RemoveAt(solution.Count - 1);
				header = node.header;
				for (Node j = node.left; j != node; j = j.left)
					Restore(j.header);
		
				node = node.down;
			}
		
			Restore(header);
			
			return false;
		}
		
		ColumnHeader GetShortestColumn() {
			
			ColumnHeader column = null;
			int minCount = int.MaxValue;
			
			// cycling through all the headers starting from root and ending on root
			for (ColumnHeader header = (ColumnHeader) rootHeader.right; header != rootHeader; header = (ColumnHeader) header.right) {
				if (header.count < minCount) {
					minCount = header.count;
					column = header;
				}
			}
			return column;
		}
		
		// cover
		void Remove(ColumnHeader colHeader) {
			// Remove the header
			colHeader.right.left = colHeader.left;
			colHeader.left.right = colHeader.right;

			// Iterate all the nodes in the column going down
			for (Node row = colHeader.down; row != colHeader; row = row.down) {
				// Remove the rows for every node
				for (Node col = row.right; col != row; col = col.right) {
					// Removing the items
					col.down.up = col.up;
					col.up.down = col.down;
		            col.header.count--;
		        }
			}
		}
		
		// uncover
		void Restore(ColumnHeader colHeader) {
		    //Iterate all the nodes in the column
		    for (Node row = colHeader.up; row != colHeader; row = row.up) {
        		//Reconnect vertically (add the row)
        		for (Node col = row.left; col != row; col = col.left)
        		{
            		col.header.count++;
		            col.down.up = col;
					col.up.down = col;
		        }
		    }
			//Reconnect the heather
		    colHeader.right.left = colHeader;
		    colHeader.left.right = colHeader;
		}		
		
	} // end of class

} // end of namespace
