using SeaBattleGame.fields;
using System.Drawing;
using System.Windows.Forms;

namespace SeaBattleGame
{
	public partial class GameWindow : Form
	{
		public bool placeVertically = false;

		private readonly PlayerField playerField;
		private readonly ComputerField computerField;

		private readonly ComputerPlayer computer;

		private int selectedShip_dockNumber = 0;

		private int unplacedBattleships = 1;
		private int unplacedCruisers = 2;
		private int unplacedDestroyers = 3;
		private int unplacedTorpedoBoats = 4;

		public GameWindow()
		{
			InitializeComponent();
			playerField = new PlayerField(playerFieldArea.Width, playerFieldArea.Height);
			computerField = new ComputerField(computerFieldArea.Width, computerFieldArea.Height);
			computer = new ComputerPlayer(playerField);
			DrawPlayerField();
			DrawComputerField();
		}

		private void DrawPlayerField()
		{
			var bmp = new Bitmap(playerFieldArea.Width, playerFieldArea.Height);
			var g = Graphics.FromImage(bmp);
			playerField.Draw(g);
			playerFieldArea.Image = bmp;
		}

		private void DrawComputerField()
		{
			var bmp = new Bitmap(computerFieldArea.Width, computerFieldArea.Height);
			var g = Graphics.FromImage(bmp);
			computerField.Draw(g);
			computerFieldArea.Image = bmp;
		}

		private void btnBattleship_Click(object sender, System.EventArgs e)
		{
			DeselectShipButton();
			ChangeSelectedShip(4);
		}

		private void btnCruiser_Click(object sender, System.EventArgs e)
		{
			DeselectShipButton();
			ChangeSelectedShip(3);
		}

		private void btnDestroyer_Click(object sender, System.EventArgs e)
		{
			DeselectShipButton();
			ChangeSelectedShip(2);
		}

		private void btnTorpedoBoat_Click(object sender, System.EventArgs e)
		{
			DeselectShipButton();
			ChangeSelectedShip(1);
		}

		private void DeselectShipButton()
		{
			ChangeShipButtonColor(Color.White, selectedShip_dockNumber);
		}

		private void ChangeSelectedShip(int newShip_dockNumber)
		{
			if (selectedShip_dockNumber == newShip_dockNumber)
			{
				selectedShip_dockNumber = 0;
				return;
			}
			selectedShip_dockNumber = newShip_dockNumber;
			ChangeShipButtonColor(Color.LightGray, newShip_dockNumber);
		}

		private void ChangeShipButtonColor(Color color, int ship_dockNumber)
		{
			switch (ship_dockNumber)
			{
				case 1:
					btnTorpedoBoat.BackColor = color;
					break;
				case 2:
					btnDestroyer.BackColor = color;
					break;
				case 3:
					btnCruiser.BackColor = color;
					break;
				case 4:
					btnBattleship.BackColor = color;
					break;
			}
		}

		private void btnGenerateField_Click(object sender, System.EventArgs e)
		{
			DisableShipButtons();
			playerField.Reset();
			playerField.GenerateField();
			DrawPlayerField();
			playerFieldArea.Enabled = false;
			btnStart.Enabled = true;
		}

		private void DisableShipButtons()
		{
			btnBattleship.Enabled = false;
			btnCruiser.Enabled = false;
			btnDestroyer.Enabled = false;
			btnTorpedoBoat.Enabled = false;
		}

		private void btnStart_Click(object sender, System.EventArgs e)
		{
			btnStart.Enabled = false;
			btnGenerateField.Enabled = false;
			computerFieldArea.Enabled = true;
		}

		private void AutoPlay()
		{
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					Cell targetCell = new Cell(i, j);
					if (computerField.CheckIfMarked(targetCell)) continue;
					PlayerTurn(targetCell);
					if (computerField.CheckIfComputerLost())
					{
						Congratulate();
						computerFieldArea.Enabled = false;
						return;
					}
					ComputerTurn();
					if (playerField.CheckIfPlayerLost())
					{
						ShowGameOverMessage();
						computerFieldArea.Enabled = false;
						return;
					}
				}
			}
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			btnStart.Enabled = false;
			ResetDrawingAreas();
			ResetFields();
			RedrawFields();
			computer.Reset();
			ResetShipCounters();
			ReenableShipButtons();
			btnGenerateField.Enabled = true;
		}
		
		private void ResetDrawingAreas()
		{
			computerFieldArea.Enabled = false;
			playerFieldArea.Enabled = true;
		}

		private void ResetFields()
		{
			playerField.Reset();
			computerField.Reset();
		}

		private void RedrawFields()
		{
			DrawPlayerField();
			DrawComputerField();
		}

		private void ResetShipCounters()
		{
			unplacedBattleships = 1;
			unplacedCruisers = 2;
			unplacedDestroyers = 3;
			unplacedTorpedoBoats = 4;
		}

		private void ReenableShipButtons()
		{
			btnBattleship.Enabled = true;
			btnCruiser.Enabled = true;
			btnDestroyer.Enabled = true;
			btnTorpedoBoat.Enabled = true;
		}

		private void playerFieldArea_MouseMove(object sender, MouseEventArgs e)
		{
			if (selectedShip_dockNumber == 0) return;
			Cell targetCell = GetTargetCell(playerFieldArea, e.X, e.Y);
			if (targetCell == null) return;
			playerField.HighlightPlacement(selectedShip_dockNumber, targetCell, placeVertically);
			DrawPlayerField();
		}

		private Cell GetTargetCell(PictureBox fieldArea, int x, int y)
		{
			int i = y / (fieldArea.Height / 10);
			int j = x / (fieldArea.Width / 10);
			if (i < 0 || j < 0 || i >= 10 || j >= 10) return null;
			else return new Cell(i, j);
		}

		private void playerFieldArea_MouseLeave(object sender, System.EventArgs e)
		{
			playerField.RemoveHighlight();
			DrawPlayerField();
		}

		private void playerFieldArea_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle || selectedShip_dockNumber == 0
				|| !(playerField.PlacementAllowed)) return;
			else if (e.Button == MouseButtons.Right)
			{
				placeVertically = !placeVertically;
				Cell targetCell = GetTargetCell(playerFieldArea, e.X, e.Y);
				if (targetCell == null) return;
				playerField.HighlightPlacement(selectedShip_dockNumber, targetCell, placeVertically);
				DrawPlayerField();
				return;
			}
			DeselectShipButton();
			switch (selectedShip_dockNumber)
			{
				case 1:
					unplacedTorpedoBoats--;
					if (unplacedTorpedoBoats == 0)
						btnTorpedoBoat.Enabled = false;
					break;
				case 2:
					unplacedDestroyers--;
					if (unplacedDestroyers == 0)
						btnDestroyer.Enabled = false;
					break;
				case 3:
					unplacedCruisers--;
					if (unplacedCruisers == 0)
						btnCruiser.Enabled = false;
					break;
				case 4:
					unplacedBattleships--;
					if (unplacedBattleships == 0)
						btnBattleship.Enabled = false;
					break;
			}
			selectedShip_dockNumber = 0;
			playerField.PlaceShip();
			playerField.RemoveHighlight();
			DrawPlayerField();
			if ((unplacedBattleships == 0) && (unplacedCruisers == 0)
				&& (unplacedDestroyers == 0) && (unplacedTorpedoBoats == 0))
			{
				playerFieldArea.Enabled = false;
				btnStart.Enabled = true;
			}
		}

		private void computerFieldArea_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;
			Cell targetCell = GetTargetCell(computerFieldArea, e.X, e.Y);
			if (targetCell == null || computerField.CheckIfMarked(targetCell)) return;
			PlayerTurn(targetCell);
			if (computerField.CheckIfComputerLost())
			{
				Congratulate();
				computerFieldArea.Enabled = false;
				return;
			}
			ComputerTurn();
			if (playerField.CheckIfPlayerLost())
			{
				ShowGameOverMessage();
				computerFieldArea.Enabled = false;
				return;
			}
		}

		private void PlayerTurn(Cell targetCell)
		{
			computerField.CheckShot(targetCell);
			DrawComputerField();
		}

		private void ComputerTurn()
		{
			computer.Shoot();
			DrawPlayerField();
		}

		private void ShowGameOverMessage()
		{
			MessageBox.Show("ИГРА ОКОНЧЕНА");
		}

		private void Congratulate()
		{
			MessageBox.Show("ВЫ ВЫИГРАЛИ!");
		}
	}
}
