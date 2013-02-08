using System;

namespace pentamino
{
	class MainClass
	{
		public static void WriteSolution(Solver s) {
			char [,] solution = s.Solution;
			int rowCount = solution.GetLength(0);
			int colCount = solution.GetLength(1);
			
			for (int row = 0; row < rowCount; row++) {
				for (int col = 0; col < colCount; col++) {
					Console.Write(solution[row, col]);
				}
				Console.WriteLine("");
			}
		}		
		
		public static void Main (string[] args)
		{
			Console.WriteLine ("Pentamino Solver using Dancing Links");
			Solver solver = new Solver();
			bool result = solver.Solve();
			if (result) {
				Console.WriteLine("The solution has been found");
				WriteSolution(solver);
			}
			else {
				Console.WriteLine("No solutions found");
			}
		}
	}
}
