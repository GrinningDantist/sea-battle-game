using SeaBattleGame.ships;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SeaBattleGame.fields
{
    internal class EnemyField
    {
        private readonly List<INavalShip> ships = new List<INavalShip>(20);

        public int[,] ShotMap { get; private set; } = new int[10, 10];

        public int CellWidth { get; }

        public int CellHeight { get; }

        public EnemyField(int pictureWidth, int pictureHeight)
        {
            CellWidth = pictureWidth / 10;
            CellHeight = pictureHeight / 10;
            GenerateField();
        }

        public void Reset()
        {
            ships.Clear();
            Array.Clear(ShotMap, 0, 100);
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
                {
                    return false;
                }
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
                ShotMap[cell.Item1, cell.Item2] = 2;
                if (ship.DecksIntact > 0) return;
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
                ships.Remove(ship);
            }
        }
    }
}
