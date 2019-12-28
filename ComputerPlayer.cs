using SeaBattleGame.fields;
using System;
using System.Collections.Generic;

namespace SeaBattleGame
{
    internal class ComputerPlayer
    {
        private readonly HashSet<Tuple<int, int>> markedCells = new HashSet<Tuple<int, int>>();

        private readonly PlayerField playerField;

        private Tuple<int, int> startCell;
        private Tuple<int, int> currentCell;

        public ComputerPlayer(PlayerField playerField)
        {
            this.playerField = playerField;
            ChooseRandomCell();
        }

        public void Reset()
        {
            markedCells.Clear();
            ChooseRandomCell();
        }

        public void Shoot()
        {
            bool shotLanded = playerField.CheckIfShotLanded(currentCell, out bool shipDestroyed);
            markedCells.Add(currentCell);
            if (markedCells.Count < 100) ChooseNextCell(shotLanded);
        }

        private void ChooseNextCell(bool shotLanded)
        {
            if (currentCell != startCell) ContinueShelling(shotLanded);
            else if (!shotLanded) ChooseRandomCell();
            else ChooseNearbyCell();
        }

        private void ContinueShelling(bool shotLanded)
        {
            if (currentCell.Item1 == startCell.Item1)
            {

            }
        }

        private void ChooseRandomCell()
        {
            var RNG = new Random();
            Tuple<int, int> cell;
            do
            {
                cell = new Tuple<int, int>(RNG.Next(10), RNG.Next(10));
            }
            while (markedCells.Contains(cell));
            startCell = new Tuple<int, int>(cell.Item1, cell.Item2);
            currentCell = startCell;
        }

        private void ChooseNearbyCell()
        {
            var untouchedNearbyCells = new List<Tuple<int, int>>(4);
            Tuple<int, int> upperCell = (currentCell.Item1 - 1 >= 0) ? new Tuple<int, int>(
                currentCell.Item1 - 1, currentCell.Item2) : null;
            Tuple<int, int> lowerCell = (currentCell.Item1 + 1 < 10) ? new Tuple<int, int>(
                currentCell.Item1 + 1, currentCell.Item2) : null;
            Tuple<int, int> leftCell = (currentCell.Item2 - 1 >= 0) ? new Tuple<int, int>(
                currentCell.Item1, currentCell.Item2 - 1) : null;
            Tuple<int, int> rightCell = (currentCell.Item2 + 1 < 10) ? new Tuple<int, int>(
                currentCell.Item1, currentCell.Item2 + 1) : null;
            if ((upperCell != null) && !(markedCells.Contains(upperCell)))
                untouchedNearbyCells.Add(upperCell);
            if ((lowerCell != null) && !(markedCells.Contains(lowerCell)))
                untouchedNearbyCells.Add(lowerCell);
            if ((leftCell != null) && !(markedCells.Contains(leftCell)))
                untouchedNearbyCells.Add(leftCell);
            if ((rightCell != null) && !(markedCells.Contains(rightCell)))
                untouchedNearbyCells.Add(rightCell);
            currentCell = untouchedNearbyCells[(new Random()).Next(untouchedNearbyCells.Count)];
        }
    }
}
