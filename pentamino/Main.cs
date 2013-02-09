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
			if (args.Length == 0) {
				Console.WriteLine("Enter an input file name as an argument");
				return;
			}
			
			string filename = args[0];
			Input input;
			
			try {
				// opening file to read
				using (var file = new System.IO.StreamReader(filename)) {
					// reading in from the file
					input = new Input(file);
				}
			}
			catch (System.IO.IOException exception) { // file not found exception
				Console.WriteLine(exception.Message);
				return;
			}
			
			if (input.IsValid() == false) {
				Console.WriteLine("Input is invalid");
				Console.WriteLine(input.GetReason());
				return;
			}
			
			// getting the source array from the input and feeding it to our solver
			Solver solver = new Solver(input.Data);
			bool result = solver.Solve();
			if (result) {
				Console.WriteLine("The solution has been found");
				WriteSolution(solver);
			}
			else {
				Console.WriteLine("No solutions found");
			}
		}
		
	} // end of class

} // end of namespace
