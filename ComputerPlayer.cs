using SeaBattleGame.fields;
using System;
using System.Collections.Generic;

namespace SeaBattleGame
{
	internal class ComputerPlayer
	{
		private readonly PlayerField playerField;

		private Cell startCell;
		private Cell currentCell;

		public ComputerPlayer(PlayerField playerField)
		{
			this.playerField = playerField;
			ChooseRandomCell();
		}

		public void Reset()
		{
			ChooseRandomCell();
		}

		public void Shoot()
		{
			bool shotLanded = playerField.CheckShot(currentCell, out bool shipDestroyed);
			if (!(playerField.ShipsDestroyed)) ChooseNextCell(shotLanded, shipDestroyed);
		}

		private void ChooseNextCell(bool shotLanded, bool shipDestroyed)
		{
			if (shipDestroyed) ChooseRandomCell();
			else if (!shotLanded && (currentCell == startCell))
				ChooseRandomCell();
			else if ((currentCell == startCell) || !shotLanded
				&& ((Math.Abs(currentCell.i - startCell.i) == 1)
				|| (Math.Abs(currentCell.j - startCell.j) == 1)))
				ChooseCellNearStart();
			else ContinueShelling(shotLanded);
		}

		private void ContinueShelling(bool shotLanded)
		{
			int next_i, next_j;
			if (currentCell.i == startCell.i)
			{
				next_i = currentCell.i;
				next_j = GetNextCellIndex(currentCell.j, startCell.j, shotLanded);
			}
			else
			{
				next_j = currentCell.j;
				next_i = GetNextCellIndex(currentCell.i, startCell.i, shotLanded);
			}
			currentCell = new Cell(next_i, next_j);
		}

		private int GetNextCellIndex(int currentCellIndex, int startCellIndex, bool shotLanded)
		{
			if (currentCellIndex < startCellIndex)
			{
				if (shotLanded && currentCellIndex - 1 >= 0)
					return currentCellIndex - 1;
				else return startCellIndex + 1;
			}
			else
			{
				if (shotLanded && currentCellIndex + 1 < 10)
					return currentCellIndex + 1;
				else return startCellIndex - 1;
			}
		}

		private void ChooseRandomCell()
		{
			var RNG = new Random();
			Cell cell;
			do cell = new Cell(RNG.Next(10), RNG.Next(10));
			while (playerField.CheckIfMarked(cell));
			startCell = cell;
			currentCell = startCell;
		}

		private void ChooseCellNearStart()
		{
			var freeCells = new List<Cell>(4);
			if (startCell.i - 1 >= 0)
				TryAddCell(new Cell(startCell.i - 1, startCell.j), freeCells);
			if (startCell.i + 1 < 10)
				TryAddCell(new Cell(startCell.i + 1, startCell.j), freeCells);
			if (startCell.j - 1 >= 0)
				TryAddCell(new Cell(startCell.i, startCell.j - 1), freeCells);
			if (startCell.j + 1 < 10)
				TryAddCell(new Cell(startCell.i, startCell.j + 1), freeCells);
			currentCell = freeCells[(new Random()).Next(freeCells.Count)];
		}

		private void TryAddCell(Cell cell, List<Cell> list)
		{
			if (!(playerField.CheckIfMarked(cell))) list.Add(cell);
		}
	}
}
