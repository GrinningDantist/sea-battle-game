using SeaBattleGame.ships;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattleGame.fields
{
    internal class EnemyField
    {
        private readonly List<INavalShip> ships = new List<INavalShip>(20);

        private readonly Dictionary<Tuple<int, int>, CellState> CellAndStatePairs;

        public int CellWidth { get; }

        public int CellHeight { get; }

        public EnemyField(int pictureWidth, int pictureHeight)
        {
            CellAndStatePairs = new Dictionary<Tuple<int, int>, CellState>(100);
            CellWidth = pictureWidth / 10;
            CellHeight = pictureHeight / 10;
            GenerateField();
        }

        public void Reset()
        {
            ships.Clear();
            CellAndStatePairs.Clear();
            GenerateField();
        }

        public void Draw(Graphics g)
        {
            DrawGrid(g);
            DrawMarks(g);
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
            var pen = new Pen(Color.Black, 2);
            foreach (KeyValuePair<Tuple<int, int>, CellState> cell in CellAndStatePairs)
            {
                int x = cell.Key.Item2 * CellWidth;
                int y = cell.Key.Item1 * CellHeight;
                if (cell.Value == CellState.Miss)
                {
                    g.FillEllipse(Brushes.Black, x / 2 - 2, y / 2 - 2, 5, 5);
                }
                else if (cell.Value == CellState.Hit)
                {
                    g.DrawLine(pen, x, y, x + CellWidth, y + CellHeight);
                    g.DrawLine(pen, x, y + CellWidth, x + CellHeight, y);
                }
            }
            pen.Dispose();
        }

        private void GenerateField()
        {
            var RNG = new Random();
            for (int numberOfDocks = 4; numberOfDocks > 0; numberOfDocks--)
            {
                for (int shipsToPlace = 5 - numberOfDocks; shipsToPlace > 0; shipsToPlace--)
                {
                    int i, j;
                    Tuple<int, int> bow, stern;
                    do
                    {
                        bool placeVertically = Convert.ToBoolean(RNG.Next(2));
                        i = RNG.Next(10 - (placeVertically ? numberOfDocks - 1 : 0));
                        j = RNG.Next(10 - (!placeVertically ? numberOfDocks - 1 : 0));
                        bow = new Tuple<int, int>(i, j);
                        stern = new Tuple<int, int>(i + (placeVertically ? numberOfDocks - 1 : 0),
                            j + (!placeVertically ? numberOfDocks - 1 : 0));
                    }
                    while (!CheckIfPlacementAllowed(bow, stern));
                    PlaceShip(bow, stern);
                }
            }
        }

        private bool CheckIfPlacementAllowed(Tuple<int, int> bow, Tuple<int, int> stern)
        {
            foreach (INavalShip ship in ships)
            {
                if ((bow.Item1 <= ship.Stern.Item1 + 1)
                    && (bow.Item2 <= ship.Stern.Item2 + 1)
                    && (stern.Item1 >= ship.Bow.Item1 - 1)
                    && (stern.Item2 >= ship.Bow.Item2 - 1))
                    return false;
            }
            return true;
        }

        public void PlaceShip(Tuple<int, int> bow, Tuple<int, int> stern)
        {
            int numberOfDocks = bow.Item1 == stern.Item1
                ? stern.Item2 - bow.Item2 + 1
                : stern.Item1 - bow.Item1 + 1;
            switch (numberOfDocks)
            {
                case 1:
                    ships.Add(new TorpedoBoat(bow));
                    break;
                case 2:
                    ships.Add(new Destroyer(bow, stern));
                    break;
                case 3:
                    ships.Add(new Cruiser(bow, stern));
                    break;
                case 4:
                    ships.Add(new Battleship(bow, stern));
                    break;
            }
        }

        public void CheckShot(Tuple<int, int> cell)
        {
            foreach (INavalShip ship in ships)
            {
                if (!(ship.CheckIfHit(cell.Item1, cell.Item2))) continue;
                CellAndStatePairs.Add(new Tuple<int, int>(cell.Item1, cell.Item2), CellState.Hit);
                if (ship.DecksIntact > 0) return;
                if (ship.Bow.Item1 == ship.Stern.Item1)
                {
                    int i = cell.Item1;
                    for (int j = ship.Bow.Item2 - 1; j <= ship.Stern.Item2 + 1; j++)
                    {
                        if ((j < 0) || (j >= 10)) continue;
                        Tuple<int, int> upperCell = i - 1 >= 0 ? new Tuple<int, int>(i - 1, j) : null;
                        Tuple<int, int> lowerCell = i + 1 < 10 ? new Tuple<int, int>(i + 1, j) : null;
                        var centralCell = new Tuple<int, int>(i, j);
                        if ((upperCell != null) && !(CellAndStatePairs.ContainsKey(upperCell)))
                            CellAndStatePairs.Add(upperCell, CellState.Miss);
                        if ((lowerCell != null) && !(CellAndStatePairs.ContainsKey(lowerCell)))
                            CellAndStatePairs.Add(lowerCell, CellState.Miss);
                        if (!CellAndStatePairs.ContainsKey(centralCell))
                            CellAndStatePairs.Add(centralCell, CellState.Miss);
                    }
                }
                else
                {
                    int j = cell.Item2;
                    for (int i = ship.Bow.Item1 - 1; i <= ship.Stern.Item1 + 1; i++)
                    {
                        if ((i < 0) || (i >= 10)) continue;
                        Tuple<int, int> leftCell = j - 1 >= 0 ? new Tuple<int, int>(i, j - 1) : null;
                        Tuple<int, int> rightCell = j + 1 < 10 ? new Tuple<int, int>(i, j + 1) : null;
                        var centralCell = new Tuple<int, int>(i, j);
                        if ((leftCell != null) && !(CellAndStatePairs.ContainsKey(leftCell)))
                            CellAndStatePairs.Add(leftCell, CellState.Miss);
                        if ((rightCell != null) && !(CellAndStatePairs.ContainsKey(rightCell)))
                            CellAndStatePairs.Add(rightCell, CellState.Miss);
                        if (!CellAndStatePairs.ContainsKey(centralCell))
                            CellAndStatePairs.Add(centralCell, CellState.Miss);
                    }
                }
                ships.Remove(ship);
                return;
            }
        }
    }
}
