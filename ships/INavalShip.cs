namespace SeaBattleGame.ships
{
	internal interface INavalShip
	{
		Cell Bow { get; }

		Cell Stern { get; }

		int DecksIntact { get; }

		bool CheckIfHit(Cell shot);
	}
}
