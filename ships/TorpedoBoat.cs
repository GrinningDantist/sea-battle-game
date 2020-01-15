namespace SeaBattleGame.ships
{
	internal class TorpedoBoat : INavalShip
	{
		public Cell Bow { get; }

		public Cell Stern { get; }

		public int DecksIntact { get; private set; } = 1;

		public TorpedoBoat(Cell cell)
		{
			Bow = cell;
			Stern = cell;
		}

		public bool CheckIfHit(Cell shot)
		{
			if ((shot.i == Bow.i) && (shot.j == Bow.j))
			{
				DecksIntact = 0;
				return true;
			}
			return false;
		}
	}
}
