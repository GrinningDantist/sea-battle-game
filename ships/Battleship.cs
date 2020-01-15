namespace SeaBattleGame.ships
{
	internal class Battleship : INavalShip
	{
		public Cell Bow { get; }

		public Cell Stern { get; }

		public int DecksIntact { get; private set; } = 4;

		public Battleship(Cell bow, Cell stern)
		{
			Bow = bow;
			Stern = stern;
		}

		public bool CheckIfHit(Cell shot)
		{
			if ((shot.i >= Bow.i) && (shot.i <= Stern.i)
				&& (shot.j >= Bow.j) && (shot.j <= Stern.j))
			{
				DecksIntact--;
				return true;
			}
			return false;
		}
	}
}
