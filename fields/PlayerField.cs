using SeaBattleGame.ships;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattleGame.fields
{
	internal class PlayerField
	{
		private readonly int cellWidth;
		private readonly int cellHeight;

		private List<INavalShip> remainingShips = new List<INavalShip>(10);

		private HashSet<Tuple<Cell, Cell>> sunkShipsEnds = new HashSet<Tuple<Cell, Cell>>();

		private Cell highlightBow = null;
		private Cell highlightStern = null;

		private int[,] shotMap = new int[10, 10];

		public bool PlacementAllowed { get; private set; } = false;

		public bool ShipsDestroyed { get { return remainingShips.Count == 0; } }

		public PlayerField(int pictureWidth, int pictureHeight)
		{
			cellWidth = pictureWidth / 10;
			cellHeight = pictureHeight / 10;
		}

		public void RemoveHighlight()
		{
			highlightBow = null;
			highlightStern = null;
			PlacementAllowed = false;
		}

		public void Reset()
		{
			remainingShips.Clear();
			sunkShipsEnds.Clear();
			Array.Clear(shotMap, 0, 100);
		}

		public void Draw(Graphics g)
		{
			DrawShips(g);
			if ((highlightBow != null) && (highlightStern != null))
				DrawHighlight(g);
			DrawGrid(g);
			DrawMarks(g);
		}

		private void DrawShips(Graphics g)
		{
			foreach (INavalShip ship in remainingShips) DrawShip(g, ship.Bow, ship.Stern);
			foreach (Tuple<Cell, Cell> ship in sunkShipsEnds) DrawShip(g, ship.Item1, ship.Item2);

		}

		private void DrawShip(Graphics g, Cell bow, Cell stern)
		{
			var pen = new Pen(Color.Black, 2);
			int x = bow.j * cellWidth;
			int y = bow.i * cellHeight;
			int width = (stern.j - bow.j + 1) * cellWidth;
			int height = (stern.i - bow.i + 1) * cellHeight;
			g.FillRectangle(Brushes.Gray, x, y, width, height);
			g.DrawRectangle(pen, x, y, width, height);
			pen.Dispose();
		}

		private void DrawHighlight(Graphics g)
		{
			Brush brush;
			if (PlacementAllowed) brush = Brushes.Orange;
			else brush = Brushes.Red;
			int x = highlightBow.j * cellWidth;
			int y = highlightBow.i * cellHeight;
			int width = (highlightStern.j - highlightBow.j + 1) * cellWidth;
			int height = (highlightStern.i - highlightBow.i + 1) * cellHeight;
			g.FillRectangle(brush, x, y, width, height);
			var pen = new Pen(Color.Black, 2);
			g.DrawRectangle(pen, x, y, width, height);
			pen.Dispose();
		}

		private void DrawGrid(Graphics g)
		{
			int gridWidth = cellWidth * 10;
			int gridHeight = cellHeight * 10;
			for (int i = 0; i <= 10; i++)
			{
				int x = i * cellWidth;
				int y = i * cellHeight;
				g.DrawLine(Pens.Black, 0, y, gridWidth, y);
				g.DrawLine(Pens.Black, x, 0, x, gridHeight);
			}
		}

		private void DrawMarks(Graphics g)
		{
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					int x = j * cellWidth;
					int y = i * cellHeight;
					if (shotMap[i, j] == 1) DrawDot(g, x, y);
					else if (shotMap[i, j] == 2) DrawCross(g, x, y);
				}
			}
		}

		private void DrawDot(Graphics g, int x, int y)
		{
			g.FillEllipse(Brushes.Black, x + cellWidth / 2 - 3, y + cellHeight / 2 - 3, 6, 6);
		}

		private void DrawCross(Graphics g, int x, int y)
		{
			using (Pen pen = new Pen(Color.Black, 2))
			{
				g.DrawLine(pen, x, y, x + cellWidth, y + cellHeight);
				g.DrawLine(pen, x, y + cellWidth, x + cellHeight, y);
			}
		}

		public void GenerateField()
		{
			var RNG = new Random();
			for (int dockNumber = 4; dockNumber > 0; dockNumber--)
			{
				for (int shipsToPlace = 5 - dockNumber; shipsToPlace > 0; shipsToPlace--)
				{
					int i, j;
					Cell bow, stern;
					do
					{
						bool placeVertically = Convert.ToBoolean(RNG.Next(2));
						i = RNG.Next(10 - (placeVertically ? dockNumber - 1 : 0));
						j = RNG.Next(10 - (!placeVertically ? dockNumber - 1 : 0));
						bow = new Cell(i, j);
						stern = new Cell(i + (placeVertically ? dockNumber - 1 : 0),
							j + (!placeVertically ? dockNumber - 1 : 0));
					}
					while (!(CheckIfPlacementAllowed(bow, stern)));
					PlaceShip(bow, stern);
				}
			}
		}

		public void HighlightPlacement(int DockNumber, Cell cursorCell, bool placeVertically)
		{
			if (!placeVertically)
			{
				highlightBow = new Cell(cursorCell.i, cursorCell.j - (DockNumber - 1) / 2);
				highlightStern = new Cell(cursorCell.i, cursorCell.j + DockNumber / 2);
			}
			else
			{
				highlightBow = new Cell(cursorCell.i - (DockNumber - 1) / 2, cursorCell.j);
				highlightStern = new Cell(cursorCell.i + DockNumber / 2, cursorCell.j);
			}
			PlacementAllowed = CheckIfPlacementAllowed();
		}

		private bool CheckIfPlacementAllowed()
		{
			return CheckIfPlacementAllowed(highlightBow, highlightStern);
		}

		private bool CheckIfPlacementAllowed(Cell bow, Cell stern)
		{
			if ((bow.i < 0) || (bow.j < 0) || (stern.i >= 10) || (stern.j >= 10))
			{
				return false;
			}
			foreach (INavalShip ship in remainingShips)
			{
				if ((bow.i <= ship.Stern.i + 1)
					&& (bow.j <= ship.Stern.j + 1)
					&& (stern.i >= ship.Bow.i - 1)
					&& (stern.j >= ship.Bow.j - 1))
				{
					return false;
				}
			}
			return true;
		}

		public void PlaceShip()
		{
			PlaceShip(highlightBow, highlightStern);
		}

		public void PlaceShip(Cell bow, Cell stern)
		{
			int dockNumber = bow.i == stern.i
				? stern.j - bow.j + 1
				: stern.i - bow.i + 1;
			switch (dockNumber)
			{
				case 1:
					remainingShips.Add(new TorpedoBoat(bow));
					break;
				case 2:
					remainingShips.Add(new Destroyer(bow, stern));
					break;
				case 3:
					remainingShips.Add(new Cruiser(bow, stern));
					break;
				case 4:
					remainingShips.Add(new Battleship(bow, stern));
					break;
			}
		}

		public bool CheckShot(Cell shot, out bool shipDestroyed)
		{
			foreach (INavalShip ship in remainingShips)
			{
				if (!(ship.CheckIfHit(shot))) continue;
				shotMap[shot.i, shot.j] = 2;
				if (ship.DecksIntact > 0)
				{
					shipDestroyed = false;
					return true;
				}
				if (ship.Bow.i == ship.Stern.i)
				{
					int i = shot.i;
					for (int j = ship.Bow.j - 1; j <= ship.Stern.j + 1; j++)
					{
						if ((j < 0) || (j >= 10)) continue;
						if (i - 1 >= 0) shotMap[i - 1, j] = 1;
						if (i + 1 < 10) shotMap[i + 1, j] = 1;
						if (shotMap[i, j] == 0) shotMap[i, j] = 1;
					}
				}
				else
				{
					int j = shot.j;
					for (int i = ship.Bow.i - 1; i <= ship.Stern.i + 1; i++)
					{
						if ((i < 0) || (i >= 10)) continue;
						if (j - 1 >= 0) shotMap[i, j - 1] = 1;
						if (j + 1 < 10) shotMap[i, j + 1] = 1;
						if (shotMap[i, j] == 0) shotMap[i, j] = 1;
					}
				}
				remainingShips.Remove(ship);
				sunkShipsEnds.Add(new Tuple<Cell, Cell>(ship.Bow, ship.Stern));
				{
					shipDestroyed = true;
					return true;
				}
			}
			shotMap[shot.i, shot.j] = 1;
			shipDestroyed = false;
			return false;
		}

		public bool CheckIfMarked(Cell cell)
		{
			if (shotMap[cell.i, cell.j] > 0) return true;
			else return false;
		}

		public bool CheckIfPlayerLost()
		{
			if (ShipsDestroyed) return true;
			else return false;
		}
	}
}
