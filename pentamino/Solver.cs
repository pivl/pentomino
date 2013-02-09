using System;
using System.Collections;

namespace pentamino
{
	public class Solver
	{
		// class that keeps all the information 1's in the ma
		class NodeFigure {
			public int figureId;
		}
		
		class NodeItem {
			public int x;
			public int y;
		}
		
		int[,] fieldSource = { // default field
			{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 0, 0, 1, 1, 1 },
			{ 1, 1, 1, 0, 0, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 1, 1, 1, 1 }
		};
		
		NodeItem[,] field;
		NodeFigure[] figureNodes;
		
		ExactCover.DancingLinks dancingLinks = new ExactCover.DancingLinks();
		
		FigureSet figureSet = new FigureSet();
		
		// total number of rows in a dancing links matrix
		int nLines;
		
		public char[,] Solution {
			get;
			protected set;
		}
		
		public Solver()
		{
			InitMatrix();
			FillMatrix();
		}
		
		public Solver(int[,] sourceData) {
			fieldSource = sourceData;
			
			InitMatrix();
			FillMatrix();
		}
		
		public bool Solve() {
			// matrix doesn't have any rows
			if (nLines == 0)
				return false;
			
			bool result = dancingLinks.Solve();
			if (result) {
				// transforming the result to the form of array of chars that can be easily output to screen
				Solution = new char[field.GetLength(0), field.GetLength(1)];
				
				// filling all with spaces
				for (int x = 0; x < Solution.GetLength (0); x++)
					for (int y = 0; y < Solution.GetLength (1); y++)
						Solution[x,y] = ' ';
				
				foreach (ExactCover.Node figure in dancingLinks.Solution) {
					// finding figure id (type)
					int figureId = 0;
					ExactCover.Node node = figure;
					do {
						if (node.nodedata.GetType() == typeof(NodeFigure)) {
							NodeFigure figureType = (NodeFigure) node.nodedata;
							if (figureType != null) {
								figureId = figureType.figureId;
								break;
							}
						}
						node = node.right;
					} while (node != figure);
					// placing all nodes in the char[,] array
					
					node = figure;
					do {
						if (node.nodedata.GetType() == typeof(NodeItem)) {
							NodeItem item = (NodeItem) node.nodedata;
							if (item != null) {
								Solution[item.x, item.y] = (char)((int)'A' + figureId);
							}
						}
						node = node.right;
					} while (node != figure);
				}
			}
			return result;
		}
		
		void InitMatrix() {
			// counting the number of source nodes and creating field of them
			field = new NodeItem[ fieldSource.GetLength(0), fieldSource.GetLength(1) ];
			
			// counting nodes
			int nodeItemCount = 0;
			for (int x = 0; x < fieldSource.GetLength(0); x++) {
				for (int y = 0; y < fieldSource.GetLength(1); y++) {
					if (fieldSource[x, y] == 1) {
						NodeItem item = new NodeItem() { x = x, y = y };
						field[x,y] = item;
						nodeItemCount++;
					}
				}
			}
			
			dancingLinks.InitHeaders(nodeItemCount + figureSet.FigureCount);
			
			// assigning a node to every column header just to make it distinct
			ExactCover.Node header = dancingLinks.rootHeader.right;
			for (int x = 0; x < field.GetLength(0); x++) {
				for (int y = 0; y < field.GetLength(1); y++) {
					if (field[x, y] != null) {
						header.nodedata = field[x, y];
						header = header.right;
					}
				}
			}
			
			// assigning a node containing figure id to header columns
			figureNodes = new NodeFigure[ figureSet.FigureCount ];
			for (int i = 0; i < figureSet.FigureCount; i++) {
				var newNode = new NodeFigure() { figureId = i };
				header.nodedata = newNode;
				header = header.right;
				figureNodes[i] = newNode;
			}
		}
		
		// filling in matrix with rows of every possible position of figure
		void FillMatrix() {			
			for (int figVariantId = 0; figVariantId < figureSet.Count; figVariantId++) {
				long[] figure = figureSet.GetFigureByIndex(figVariantId);
				int figureId = figureSet.GetFigureIdByIndex(figVariantId);
				// for every possible cell (position) on board
				for (int row = 0; row < field.GetLength(0); row++) {
					for (int col = 0; col < field.GetLength(1); col++) {
						if (IsFigurePlacedOnBoard(figure, col, row) ) {
							AddFigureToMatrix(figure, figureId, col, row);
							nLines++;
						}
					}
				}
			}
			Console.WriteLine("Every figure can be placed " + nLines.ToString() + " times");
		}
		
		bool IsFigurePlacedOnBoard(long[] figure, int fCol, int fRow) {
			// compare node by node
			int nFigureRows = figure.Length;
			if (fRow + nFigureRows > field.GetLength(0)) return false;
			for (int row = 0; row < nFigureRows; row++) {
				for (int col = 0; col < 5; col++) {
					if (( figure[row] & (1 << col)) != 0) {
						if (fCol + col >= field.GetLength(1)) return false;
					}
					if ( ((figure[row] & (1 << col)) != 0 ) && (field[fRow + row, fCol + col] == null) )
						return false;
				}
			}
			return true;
		}
		
		void AddFigureToMatrix(long[] figure, int figureId, int fCol, int fRow) {
			int nFigureRows = figure.Length;
			ArrayList newRow = new ArrayList();
			
			// adding all five field nodes 
			for (int row = 0; row < nFigureRows; row++) {
				for (int col = 0; col < 5; col++) {
					if ( (figure[row] & (1 << col)) != 0) {
						// add new node to header
						NodeItem item = field[row + fRow, col + fCol];
						foreach (ExactCover.ColumnHeader colHeader in dancingLinks.columnHeaders) {
							if (colHeader.nodedata == item) {
								ExactCover.Node newNode = new ExactCover.Node() { nodedata = item };
								colHeader.Add(newNode);
								newRow.Add(newNode);
								break;
							}
						}
					}
				}
			}
			// adding figure node
			NodeFigure nodeFigure = Array.Find<NodeFigure>(figureNodes, (fNode) => ( fNode.figureId == figureId ) );
			foreach(ExactCover.ColumnHeader colHeader in dancingLinks.columnHeaders) {
				if (colHeader.nodedata == nodeFigure) {
					ExactCover.Node newNode = new ExactCover.Node() { nodedata = nodeFigure };
					colHeader.Add(newNode);
					newRow.Add(newNode);
					break;
				}
			}
			
			// establishing links between nodes in a row through left and right pointers
			ExactCover.Node startNode = null;
			ExactCover.Node previousNode = null;
			for (ExactCover.Node node = dancingLinks.rootHeader.right; node != dancingLinks.rootHeader; node = node.right) {
				
				foreach (ExactCover.Node rowNode in newRow) {
					if ((startNode == null) && (node.nodedata == rowNode.nodedata)) {
						startNode = rowNode;
						previousNode = rowNode;
						continue;
					}
					if ((startNode != null) && (node.nodedata == rowNode.nodedata)) {
						ExactCover.Node currentNode = rowNode;
						previousNode.right = currentNode;
						currentNode.left = previousNode;
						previousNode = currentNode;
					}
				}
			}
			previousNode.right = startNode;
			startNode.left = previousNode;
		}
		
	} // end of class

} // end of namespace

