using SeaBattleGame.fields;
using System.Drawing;
using System.Windows.Forms;

namespace SeaBattleGame
{
    public partial class GameWindow : Form
    {
        public enum ShipTypes { None, Battleship, Cruiser, Destroyer, TorpedoBoat }

        private readonly PlayerField playerField;
        private readonly EnemyField enemyField;

        private ShipTypes shipType = ShipTypes.None;

        private int unplacedBattleships = 1;
        private int unplacedCruisers = 2;
        private int unplacedDestroyers = 3;
        private int unplacedTorpedoBoats = 4;

        public GameWindow()
        {
            InitializeComponent();
            playerField = new PlayerField(playerFieldArea.Width, playerFieldArea.Height);
            enemyField = new EnemyField(enemyFieldArea.Width, enemyFieldArea.Height);
            DrawPlayerField();
            DrawEnemyField();
        }

        private void DrawPlayerField()
        {
            var bmp = new Bitmap(playerFieldArea.Width, playerFieldArea.Height);
            var g = Graphics.FromImage(bmp);
            playerField.Draw(g);
            playerFieldArea.Image = bmp;
        }

        private void DrawEnemyField()
        {
            var bmp = new Bitmap(enemyFieldArea.Width, enemyFieldArea.Height);
            var g = Graphics.FromImage(bmp);
            enemyField.Draw(g);
            enemyFieldArea.Image = bmp;
        }

        private void btnBattleship_Click(object sender, System.EventArgs e)
        {
            switch (shipType)
            {
                case ShipTypes.Battleship:
                    shipType = ShipTypes.None;
                    btnBattleship.BackColor = Color.White;
                    return;
                case ShipTypes.Cruiser:
                    btnCruiser.BackColor = Color.White;
                    break;
                case ShipTypes.Destroyer:
                    btnDestroyer.BackColor = Color.White;
                    break;
                case ShipTypes.TorpedoBoat:
                    btnTorpedoBoat.BackColor = Color.White;
                    break;
            }
            shipType = ShipTypes.Battleship;
            btnBattleship.BackColor = Color.LightGray;
        }

        private void btnCruiser_Click(object sender, System.EventArgs e)
        {
            switch (shipType)
            {
                case ShipTypes.Battleship:
                    btnBattleship.BackColor = Color.White;
                    break;
                case ShipTypes.Cruiser:
                    shipType = ShipTypes.None;
                    btnCruiser.BackColor = Color.White;
                    return;
                case ShipTypes.Destroyer:
                    btnDestroyer.BackColor = Color.White;
                    break;
                case ShipTypes.TorpedoBoat:
                    btnTorpedoBoat.BackColor = Color.White;
                    break;
            }
            shipType = ShipTypes.Cruiser;
            btnCruiser.BackColor = Color.LightGray;
        }

        private void btnDestroyer_Click(object sender, System.EventArgs e)
        {
            switch (shipType)
            {
                case ShipTypes.Battleship:
                    btnBattleship.BackColor = Color.White;
                    break;
                case ShipTypes.Cruiser:
                    btnCruiser.BackColor = Color.White;
                    break;
                case ShipTypes.Destroyer:
                    shipType = ShipTypes.None;
                    btnDestroyer.BackColor = Color.White;
                    return;
                case ShipTypes.TorpedoBoat:
                    btnTorpedoBoat.BackColor = Color.White;
                    break;
            }
            shipType = ShipTypes.Destroyer;
            btnDestroyer.BackColor = Color.LightGray;
        }

        private void btnTorpedoBoat_Click(object sender, System.EventArgs e)
        {
            switch (shipType)
            {
                case ShipTypes.Battleship:
                    btnBattleship.BackColor = Color.White;
                    break;
                case ShipTypes.Cruiser:
                    btnCruiser.BackColor = Color.White;
                    break;
                case ShipTypes.Destroyer:
                    btnDestroyer.BackColor = Color.White;
                    break;
                case ShipTypes.TorpedoBoat:
                    shipType = ShipTypes.None;
                    btnTorpedoBoat.BackColor = Color.White;
                    return;
            }
            shipType = ShipTypes.TorpedoBoat;
            btnTorpedoBoat.BackColor = Color.LightGray;
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {

        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            playerField.Reset();
            enemyField.Reset();
            unplacedBattleships = 1;
            unplacedCruisers = 2;
            unplacedDestroyers = 3;
            unplacedTorpedoBoats = 4;
            btnBattleship.Enabled = true;
            btnCruiser.Enabled = true;
            btnDestroyer.Enabled = true;
            btnTorpedoBoat.Enabled = true;
            btnStart.Enabled = false;
            DrawPlayerField();
            DrawEnemyField();
        }

        private void playerFieldArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (shipType == ShipTypes.None) return;
            int i = e.Y / (playerFieldArea.Height / 10);
            int j = e.X / (playerFieldArea.Width / 10);
            if (i < 0 || j < 0 || i >= 10 || j >= 10) return;
            int numberOfDocks = 0;
            switch (shipType)
            {
                case ShipTypes.Battleship:
                    numberOfDocks = 4;
                    break;
                case ShipTypes.Cruiser:
                    numberOfDocks = 3;
                    break;
                case ShipTypes.Destroyer:
                    numberOfDocks = 2;
                    break;
                case ShipTypes.TorpedoBoat:
                    numberOfDocks = 1;
                    break;
            }
            playerField.HighlightPlacement(numberOfDocks, i, j, verticallyCheckBox.Checked);
            DrawPlayerField();
        }

        private void playerFieldArea_MouseLeave(object sender, System.EventArgs e)
        {
            playerField.RemoveHighlight();
            DrawPlayerField();
        }

        private void playerFieldArea_Click(object sender, System.EventArgs e)
        {
            if (!(playerField.PlacementAllowed)) return;
            switch (shipType)
            {
                case ShipTypes.Battleship:
                    unplacedBattleships--;
                    if (unplacedBattleships == 0)
                    {
                        shipType = ShipTypes.None;
                        btnBattleship.BackColor = Color.White;
                        btnBattleship.Enabled = false;
                    }
                    break;
                case ShipTypes.Cruiser:
                    unplacedCruisers--;
                    if (unplacedCruisers == 0)
                    {
                        shipType = ShipTypes.None;
                        btnCruiser.BackColor = Color.White;
                        btnCruiser.Enabled = false;
                    }
                    break;
                case ShipTypes.Destroyer:
                    unplacedDestroyers--;
                    if (unplacedDestroyers == 0)
                    {
                        shipType = ShipTypes.None;
                        btnDestroyer.BackColor = Color.White;
                        btnDestroyer.Enabled = false;
                    }
                    break;
                case ShipTypes.TorpedoBoat:
                    unplacedTorpedoBoats--;
                    if (unplacedTorpedoBoats == 0)
                    {
                        shipType = ShipTypes.None;
                        btnTorpedoBoat.BackColor = Color.White;
                        btnTorpedoBoat.Enabled = false;
                    }
                    break;
            }
            playerField.PlaceShip();
            playerField.RemoveHighlight();
            DrawPlayerField();
            if ((unplacedBattleships == 0) && (unplacedCruisers == 0)
                && (unplacedDestroyers == 0) && (unplacedTorpedoBoats == 0))
            {
                btnStart.Enabled = true;
                playerFieldArea.Enabled = false;
            }
        }
    }
}
