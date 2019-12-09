using System;

namespace SeaBattleGame.ships
{
    internal class TorpedoBoat : INavalShip
    {
        public Tuple<int, int> Bow { get; }

        public Tuple<int, int> Stern { get; }

        public int DecksIntact { get; private set; } = 1;

        public TorpedoBoat(Tuple<int, int> cell)
        {
            Bow = cell;
            Stern = cell;
        }

        public bool CheckIfHit(int i, int j)
        {
            if ((i == Bow.Item1) && (j == Bow.Item2))
            {
                DecksIntact = 0;
                return true;
            }
            return false;
        }
    }
}
