using System;

namespace SeaBattleGame.ships
{
    internal class Cruiser : INavalShip
    {
        public Tuple<int, int> Bow { get; }

        public Tuple<int, int> Stern { get; }

        public int DecksIntact { get; private set; } = 3;

        public Cruiser(Tuple<int, int> bow, Tuple<int, int> stern)
        {
            Bow = bow;
            Stern = stern;
        }

        public bool CheckIfHit(int i, int j)
        {
            if ((i >= Bow.Item1) && (i <= Stern.Item1)
                && (j >= Bow.Item2) && (j <= Stern.Item2))
            {
                DecksIntact--;
                return true;
            }
            return false;
        }
    }
}
