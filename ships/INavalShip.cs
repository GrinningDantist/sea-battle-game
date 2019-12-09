using System;

namespace SeaBattleGame.ships
{
    internal interface INavalShip
    {
        Tuple<int, int> Bow { get; }

        Tuple<int, int> Stern { get; }

        int DecksIntact { get; }

        bool CheckIfHit(int i, int j);
    }
}
