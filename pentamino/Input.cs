using System;
using System.IO;
using System.Collections;

namespace pentamino {
	public class Input {
		
		const int MAX_ROWS = 20;
		const int MAX_COLS = 30;
		
		TextReader reader;
		
		bool isValid = true;
		
		string reason = "";
		
		public int[,] Data {
			get;
			private set;
		}
		
		public Input(TextReader reader) {
			this.reader = reader;
			ConvertToArray();
		}
		
		public bool IsValid() {
			return isValid;
		}
		
		public string GetReason() {
			return reason;
		}
		
		void ConvertToArray() {
			string line;
			ArrayList rows = new ArrayList();
			int maxCharInRow = 0;
			
			while ((line = reader.ReadLine()) != null) {
				char[] chrLine = line.ToCharArray();
				ArrayList currentRow = new ArrayList();
				foreach(char ch in chrLine) {
					if (ch == ' ')
						currentRow.Add(0);
					else if (ch == 'o')
						currentRow.Add(1);
					else {
						isValid = false;
						reason = "Input contains unknown characters";
						return;
					}
				}
				rows.Add(currentRow);
				// maximum value
				if (currentRow.Count > maxCharInRow)
					maxCharInRow = currentRow.Count;
			}
			
			if (rows.Count == 0) {
				isValid = false;
				reason = "Input is empty";
				return;
			}
			
			Data = new int[ rows.Count, maxCharInRow ];
			for(int rowIndex = 0; rowIndex < rows.Count; rowIndex++) {
				ArrayList row = (ArrayList)rows[rowIndex];
				for(int i = 0; i < row.Count; i++) {
					Data[rowIndex, i] = (int)row[i];
				}
			}
		}
		
	} // end of class

} // end of namespace
