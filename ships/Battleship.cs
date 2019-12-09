using System;

namespace SeaBattleGame.ships
{
    internal class Battleship : INavalShip
    {
        public Tuple<int, int> Bow { get; }

        public Tuple<int, int> Stern { get; }

        public int DecksIntact { get; private set; } = 4;

        public Battleship(Tuple<int, int> bow, Tuple<int, int> stern)
        {
            Bow = bow;
            Stern = stern;
        }

        public bool CheckIfHit(int i, int j)
        {
            if ((i >= Bow.Item1) && (j >= Bow.Item2)
                && (i <= Stern.Item1) && (j <= Stern.Item2))
            {
                DecksIntact--;
                return true;
            }
            return false;
        }
    }
}
