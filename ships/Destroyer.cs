namespace SeaBattleGame.ships
{
	internal class Destroyer : INavalShip
	{
		public Cell Bow { get; }

		public Cell Stern { get; }

		public int DecksIntact { get; private set; } = 2;

		public Destroyer(Cell bow, Cell stern)
		{
			Bow = bow;
			Stern = stern;
		}

		public bool CheckIfHit(Cell shot)
		{
			if ((shot.i == Bow.i) && (shot.j == Bow.j)
				|| (shot.i == Stern.i) && (shot.j == Stern.j))
			{
				DecksIntact--;
				return true;
			}
			return false;
		}
	}
}
