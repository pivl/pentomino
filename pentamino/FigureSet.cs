using System;
using System.Collections;

namespace pentamino
{
	/*
	The class is part of pentamino solving algorythm.
	It generates and provides all possible figure variations
	It tells the Solver which figures were used on which are not
	*/
	public class FigureSet
	{
		// some constants to help visually set up all source figures
		const byte b0000 = 0x00;
		const byte b0001 = 0x01;
		const byte b0010 = 0x02;
		const byte b0011 = 0x03;
		const byte b0100 = 0x04;
		const byte b0101 = 0x05;
		const byte b0110 = 0x06;
		const byte b0111 = 0x07;
		const byte b1000 = 0x08;
		const byte b1001 = 0x09;
		const byte b1010 = 0x0A;
		const byte b1011 = 0x0B;
		const byte b1100 = 0x0C;
		const byte b1101 = 0x0D;
		const byte b1110 = 0x0E;
		const byte b1111 = 0x0F;
		
		// source data for figures
		static readonly byte[,] sourceFigures = {
			// figure 1
			{
				b0100,
				b0100,
				b0111,
				b0000,
				b0000
			},
		
			// figure 2
			{
				b0010,
				b0011,
				b0001,
				b0001,
				b0000
			},
			
			// figure 3
			{
				b0110,
				b0111,
				b0000,
				b0000,
				b0000
			},
			
			// figure 4
			{
				b0111,
				b0101,
				b0000,
				b0000,
				b0000
			},
			
			// figure 5
			{
				b1111,
				b0100,
				b0000,
				b0000,
				b0000
			},
			
			//figure 6
			{
				b0010,
				b0111,
				b0010,
				b0000,
				b0000
			},
			
			// figure 7
			{
				b0111,
				b0010,
				b0010,
				b0000,
				b0000
			},
			
			// figure 8
			{
				b1111,
				b0001,
				b0000,
				b0000,
				b0000
			},
			
			// figure 9
			{
				b0011,
				b0110,
				b0100,
				b0000,
				b0000
			},
			
			// figure 10
			{
				b0010,
				b0110,
				b0011,
				b0000,
				b0000
			},
			
			// figure 11
			{
				b0110,
				b0010,
				b0011,
				b0000,
				b0000
			},
			
			// figure 12
			{
				b0001,
				b0001,
				b0001,
				b0001,
				b0001
			}
		};
		
		class FigureVariant {
			public int baseFigureId { get;set; }
			public long[] figureData { get;set; }
		}
		
		// All possible representations of figures
		ArrayList figures = new ArrayList(); // of type FigureVariant
		
		// the number of elements that form a figure (maximum number of rows for element)
		const int MAX_DOTS = 5;
		
		public FigureSet()
		{			
			// creating all available variations of figures
			for (int i = 0; i < sourceFigures.GetLength(0); i++) {
				// raw variation
				long[] baseFigure = GetSourceFigure(i);
				figures.Add( new FigureSet.FigureVariant() { baseFigureId = i, figureData = baseFigure} );
				
				// rotated variations
				long[] figure2 = RotateFigure(baseFigure);
				if (!IsFigureAlreadyAdded(figure2))
					figures.Add( new FigureSet.FigureVariant() { baseFigureId = i, figureData = figure2} );
				long[] figure3 = RotateFigure(figure2);
				if (!IsFigureAlreadyAdded(figure3))
					figures.Add( new FigureSet.FigureVariant() { baseFigureId = i, figureData = figure3} );
				long[] figure4 = RotateFigure(figure3);
				if (!IsFigureAlreadyAdded(figure4))
					figures.Add( new FigureSet.FigureVariant() { baseFigureId = i, figureData = figure4} );
				// flipped variations
				long[] flipped1 = FlipFigure(baseFigure);
				if (!IsFigureAlreadyAdded(flipped1))
					figures.Add( new FigureSet.FigureVariant() { baseFigureId = i, figureData = flipped1} );
				long[] flipped2 = FlipFigure(figure2);
				if (!IsFigureAlreadyAdded(flipped2))
					figures.Add( new FigureSet.FigureVariant() { baseFigureId = i, figureData = flipped2} );
				long[] flipped3 = FlipFigure(figure3);
				if (!IsFigureAlreadyAdded(flipped3))
					figures.Add( new FigureSet.FigureVariant() { baseFigureId = i, figureData = flipped3} );
				long[] flipped4 = FlipFigure(figure4);
				if (!IsFigureAlreadyAdded(flipped4))
					figures.Add( new FigureSet.FigureVariant() { baseFigureId = i, figureData = flipped4} );
			}
		}
		
		public long[] GetFigureByIndex(int figureIndex) {
			long[] f = ((FigureVariant)figures[figureIndex]).figureData;
			return (long[])f.Clone();
		}
		
		public int GetFigureIdByIndex(int figureIndex) {
			var id = ((FigureVariant)figures[figureIndex]).baseFigureId;
			return id; 
		}
		
		// count of all posible figure variations
		public int Count {
			get {return figures.Count;}
		}
		
		// count of figures that are used to create set
		public int FigureCount {
			get { return sourceFigures.GetLength(0); }
		}
			
		// returns figure based on figure index in the long structure
		long[] GetSourceFigure(int index) {
			
			long[] retArray = new long[MAX_DOTS];
			int i = 0;
			for (; i < MAX_DOTS; i++) {
				if (sourceFigures[index, i] == 0) break;
				retArray[i] = sourceFigures[index, i];
			}
			if (i < 5) {
				Array.Resize<long>(ref retArray, i);
			}	
			return retArray;
		}
		
		long[] RotateFigure(long[] figure) {
			// rotating using this rule
			// newCol = row
			// newRow = 4 - col
			long[] retVal = new long[MAX_DOTS];
			for (int row = 0; row < figure.Length; row++) {
				for (int col = 0; col < MAX_DOTS; col++) {
					// if the bit at row,col is turned
					if ( (figure[row] & (1 << col)) != 0) {
						int newCol = row;
						int newRow = MAX_DOTS - 1 - col;
						TurnBitOn(ref retVal[newRow], 1 << newCol); 
					}
				}
			}
			// shifting the retVal figure so that there is no spaces in the first row
			while (retVal[0] == 0) {
				for (int r = 0; r < retVal.Length - 1; r++)
					retVal[r] = retVal[r + 1];
				Array.Resize<long>(ref retVal, retVal.Length - 1);
			}
			return retVal;
		}
		
		long[] FlipFigure(long[] figure) {
			// flipping/mirroring using this rule
			// newCol = 4 - col
			// newRow = row
			long[] retVal = new long[figure.Length];
			for (int row = 0; row < figure.Length; row++) {
				for (int col = 0; col < MAX_DOTS; col++) {
					// if the bit at row,col is turned
					if ( (figure[row] & (1 << col)) != 0) {
						int newCol = MAX_DOTS - 1 - col;
						int newRow = row;
						TurnBitOn(ref retVal[newRow], 1 << newCol);
					}
				}
			}
			
			// shifting retVal figure so it is alligned to the right
			while (IsRightColumnEmpty(ref retVal) == true) {
				for (int r = 0; r < retVal.Length; r++) {
					retVal[r] = retVal[r] >> 1;
				}
			}
			
			return retVal;
		}
		
		// returns true if the array of figure variants already contains such figure variant
		bool IsFigureAlreadyAdded(long[] figure) {
			foreach (FigureVariant variant in figures) {
				
				// the figures are different if their row count is different
				if (variant.figureData.Length != figure.Length) continue;
				
				// comparing figures row by row
				bool isTheSame = true;
				for (int row = 0; row < variant.figureData.Length; row++) {
					if (variant.figureData[row] != figure[row])
						isTheSame = false;
				}
				if (isTheSame) return true;
			}
			return false;
		}
		
		bool IsRightColumnEmpty(ref long[] figure) {
			for (int row = 0; row < figure.Length; row++) {
				if ((figure[row] & 1) != 0)
					return false;
			}
			return true;
		}
		
		public static void TurnBitOn(ref long val, long bitToTurnOn)
		{
			val |= bitToTurnOn;
		}
		
		public static void TurnBitOff(ref long val, long bitToTurnOff)
		{
		   val = (val & ~bitToTurnOff);
		}
		
	} // end of class
	
} // end of namespace
