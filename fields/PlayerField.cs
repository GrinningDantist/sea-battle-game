using SeaBattleGame.ships;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattleGame.fields
{
    internal class PlayerField
    {
        private readonly List<INavalShip> remainingShips = new List<INavalShip>(20);

        private HashSet<Tuple<Tuple<int, int>, Tuple<int, int>>> sunkShipsCells
            = new HashSet<Tuple<Tuple<int, int>, Tuple<int, int>>>();

        private Tuple<int, int> firstHighlightCell, lastHighlightCell;

        public int[,] ShotMap { get; private set; } = new int[10, 10];

        public int CellWidth { get; }

        public int CellHeight { get; }

        public bool PlacementAllowed { get; private set; }

        public PlayerField(int pictureWidth, int pictureHeight)
        {
            remainingShips = new List<INavalShip>(10);
            PlacementAllowed = false;
            CellWidth = pictureWidth / 10;
            CellHeight = pictureHeight / 10;
        }

        public void RemoveHighlight()
        {
            firstHighlightCell = null;
            lastHighlightCell = null;
            PlacementAllowed = false;
        }

        public void Reset()
        {
            remainingShips.Clear();
            sunkShipsCells.Clear();
            Array.Clear(ShotMap, 0, 100);
        }

        public void Draw(Graphics g)
        {
            DrawShips(g);
            if ((firstHighlightCell != null) && (lastHighlightCell != null))
                DrawHighlight(g);
            DrawGrid(g);
            DrawMarks(g);
        }

        private void DrawShips(Graphics g)
        {
            foreach (INavalShip ship in remainingShips) DrawShip(g, ship.Bow, ship.Stern);
            foreach (Tuple<Tuple<int, int>, Tuple<int, int>> ship in sunkShipsCells)
                DrawShip(g, ship.Item1, ship.Item2);

        }

        private void DrawShip(Graphics g, Tuple<int, int> bow, Tuple<int, int> stern)
        {
            var pen = new Pen(Color.Black, 2);
            int x = bow.Item2 * CellWidth;
            int y = bow.Item1 * CellHeight;
            int width = (stern.Item2 - bow.Item2 + 1) * CellWidth;
            int height = (stern.Item1 - bow.Item1 + 1) * CellHeight;
            g.FillRectangle(Brushes.Gray, x, y, width, height);
            g.DrawRectangle(pen, x, y, width, height);
            pen.Dispose();
        }

        private void DrawHighlight(Graphics g)
        {
            Brush brush;
            if (PlacementAllowed) brush = Brushes.Orange;
            else brush = Brushes.Red;
            int x = firstHighlightCell.Item2 * CellWidth;
            int y = firstHighlightCell.Item1 * CellHeight;
            int width = (lastHighlightCell.Item2 - firstHighlightCell.Item2 + 1) * CellWidth;
            int height = (lastHighlightCell.Item1 - firstHighlightCell.Item1 + 1) * CellHeight;
            g.FillRectangle(brush, x, y, width, height);
            var pen = new Pen(Color.Black, 2);
            g.DrawRectangle(pen, x, y, width, height);
            pen.Dispose();
        }

        private void DrawGrid(Graphics g)
        {
            int gridWidth = CellWidth * 10;
            int gridHeight = CellHeight * 10;
            for (int i = 0; i <= 10; i++)
            {
                int x = i * CellWidth;
                int y = i * CellHeight;
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
                    int x = j * CellWidth;
                    int y = i * CellHeight;
                    if (ShotMap[i, j] == 1) DrawDot(g, x, y);
                    else if (ShotMap[i, j] == 2) DrawCross(g, x, y);
                }
            }
        }

        private void DrawDot(Graphics g, int x, int y)
        {
            g.FillEllipse(Brushes.Black, x / 2 - 2, y / 2 - 2, 5, 5);
        }

        private void DrawCross(Graphics g, int x, int y)
        {
            g.DrawLine(Pens.Black, x, y, x + CellWidth, y + CellHeight);
            g.DrawLine(Pens.Black, x, y + CellWidth, x + CellHeight, y);
        }

        public void HighlightPlacement(int numberOfDocks, int i, int j, bool placeVertically)
        {
            if (!placeVertically)
            {
                firstHighlightCell = new Tuple<int, int>(i, j - (numberOfDocks - 1) / 2);
                lastHighlightCell = new Tuple<int, int>(i, j + numberOfDocks / 2);
            }
            else
            {
                firstHighlightCell = new Tuple<int, int>(i - (numberOfDocks - 1) / 2, j);
                lastHighlightCell = new Tuple<int, int>(i + numberOfDocks / 2, j);
            }
            PlacementAllowed = CheckIfPlacementAllowed();
        }

        private bool CheckIfPlacementAllowed()
        {
            if ((firstHighlightCell.Item1 < 0) || (firstHighlightCell.Item2 < 0)
                   || (lastHighlightCell.Item1 >= 10) || (lastHighlightCell.Item2 >= 10))
            {
                return false;
            }

            foreach (INavalShip ship in remainingShips)
            {
                if ((firstHighlightCell.Item1 <= ship.Stern.Item1 + 1)
                    && (firstHighlightCell.Item2 <= ship.Stern.Item2 + 1)
                    && (lastHighlightCell.Item1 >= ship.Bow.Item1 - 1)
                    && (lastHighlightCell.Item2 >= ship.Bow.Item2 - 1))
                {
                    return false;
                }
            }
            return true;
        }

        public void PlaceShip()
        {
            int numberOfDocks = firstHighlightCell.Item1 == lastHighlightCell.Item1
                ? lastHighlightCell.Item2 - firstHighlightCell.Item2 + 1
                : lastHighlightCell.Item1 - firstHighlightCell.Item1 + 1;
            switch (numberOfDocks)
            {
                case 1:
                    remainingShips.Add(new TorpedoBoat(firstHighlightCell));
                    break;
                case 2:
                    remainingShips.Add(new Destroyer(firstHighlightCell, lastHighlightCell));
                    break;
                case 3:
                    remainingShips.Add(new Cruiser(firstHighlightCell, lastHighlightCell));
                    break;
                case 4:
                    remainingShips.Add(new Battleship(firstHighlightCell, lastHighlightCell));
                    break;
            }
        }

        public bool CheckIfShotLanded(Tuple<int, int> cell, out bool shipDestroyed)
        {
            foreach (INavalShip ship in remainingShips)
            {
                if (!(ship.CheckIfHit(cell.Item1, cell.Item2))) continue;
                ShotMap[cell.Item1, cell.Item2] = 2;
                if (ship.DecksIntact > 0)
                {
                    shipDestroyed = false;
                    return true;
                }
                if (ship.Bow.Item1 == ship.Stern.Item1)
                {
                    int i = cell.Item1;
                    for (int j = ship.Bow.Item2 - 1; j <= ship.Stern.Item2 + 1; j++)
                    {
                        if ((j < 0) || (j >= 10)) continue;
                        if (i - 1 >= 0) ShotMap[i - 1, j] = 1;
                        if (i + 1 < 10) ShotMap[i + 1, j] = 1;
                        if (ShotMap[i, j] == 0) ShotMap[i, j] = 1;
                    }
                }
                else
                {
                    int j = cell.Item2;
                    for (int i = ship.Bow.Item1 - 1; i <= ship.Stern.Item1 + 1; i++)
                    {
                        if ((i < 0) || (i >= 10)) continue;
                        if (j - 1 >= 0) ShotMap[i, j - 1] = 1;
                        if (j + 1 < 10) ShotMap[i, j + 1] = 1;
                        if (ShotMap[i, j] == 0) ShotMap[i, j] = 1;
                    }
                }
                remainingShips.Remove(ship);
                sunkShipsCells.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(
                    ship.Bow, ship.Stern));
                {
                    shipDestroyed = true;
                    return true;
                }
            }
            shipDestroyed = false;
            return false;
        }
    }
}
