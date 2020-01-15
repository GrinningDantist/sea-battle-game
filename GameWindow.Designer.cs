namespace SeaBattleGame
{
    partial class GameWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			this.btnBattleship = new System.Windows.Forms.Button();
			this.btnCruiser = new System.Windows.Forms.Button();
			this.btnDestroyer = new System.Windows.Forms.Button();
			this.btnTorpedoBoat = new System.Windows.Forms.Button();
			this.playerFieldArea = new System.Windows.Forms.PictureBox();
			this.computerFieldArea = new System.Windows.Forms.PictureBox();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnGenerateField = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.playerFieldArea)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.computerFieldArea)).BeginInit();
			this.SuspendLayout();
			// 
			// btnBattleship
			// 
			this.btnBattleship.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnBattleship.Location = new System.Drawing.Point(232, 12);
			this.btnBattleship.Name = "btnBattleship";
			this.btnBattleship.Size = new System.Drawing.Size(168, 38);
			this.btnBattleship.TabIndex = 2;
			this.btnBattleship.Text = "ЛИНКОР";
			this.btnBattleship.UseVisualStyleBackColor = true;
			this.btnBattleship.Click += new System.EventHandler(this.btnBattleship_Click);
			// 
			// btnCruiser
			// 
			this.btnCruiser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCruiser.Location = new System.Drawing.Point(232, 56);
			this.btnCruiser.Name = "btnCruiser";
			this.btnCruiser.Size = new System.Drawing.Size(168, 38);
			this.btnCruiser.TabIndex = 3;
			this.btnCruiser.Text = "КРЕЙСЕР";
			this.btnCruiser.UseVisualStyleBackColor = true;
			this.btnCruiser.Click += new System.EventHandler(this.btnCruiser_Click);
			// 
			// btnDestroyer
			// 
			this.btnDestroyer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDestroyer.Location = new System.Drawing.Point(232, 100);
			this.btnDestroyer.Name = "btnDestroyer";
			this.btnDestroyer.Size = new System.Drawing.Size(168, 38);
			this.btnDestroyer.TabIndex = 4;
			this.btnDestroyer.Text = "ЭСМИНЕЦ";
			this.btnDestroyer.UseVisualStyleBackColor = true;
			this.btnDestroyer.Click += new System.EventHandler(this.btnDestroyer_Click);
			// 
			// btnTorpedoBoat
			// 
			this.btnTorpedoBoat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnTorpedoBoat.Location = new System.Drawing.Point(232, 144);
			this.btnTorpedoBoat.Name = "btnTorpedoBoat";
			this.btnTorpedoBoat.Size = new System.Drawing.Size(168, 38);
			this.btnTorpedoBoat.TabIndex = 5;
			this.btnTorpedoBoat.Text = "КАТЕР";
			this.btnTorpedoBoat.UseVisualStyleBackColor = true;
			this.btnTorpedoBoat.Click += new System.EventHandler(this.btnTorpedoBoat_Click);
			// 
			// playerFieldArea
			// 
			this.playerFieldArea.BackColor = System.Drawing.Color.Transparent;
			this.playerFieldArea.Location = new System.Drawing.Point(232, 232);
			this.playerFieldArea.Name = "playerFieldArea";
			this.playerFieldArea.Size = new System.Drawing.Size(345, 345);
			this.playerFieldArea.TabIndex = 1;
			this.playerFieldArea.TabStop = false;
			this.playerFieldArea.MouseClick += new System.Windows.Forms.MouseEventHandler(this.playerFieldArea_MouseClick);
			this.playerFieldArea.MouseLeave += new System.EventHandler(this.playerFieldArea_MouseLeave);
			this.playerFieldArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.playerFieldArea_MouseMove);
			// 
			// computerFieldArea
			// 
			this.computerFieldArea.BackColor = System.Drawing.Color.Transparent;
			this.computerFieldArea.Enabled = false;
			this.computerFieldArea.Location = new System.Drawing.Point(12, 12);
			this.computerFieldArea.Name = "computerFieldArea";
			this.computerFieldArea.Size = new System.Drawing.Size(214, 214);
			this.computerFieldArea.TabIndex = 0;
			this.computerFieldArea.TabStop = false;
			this.computerFieldArea.MouseClick += new System.Windows.Forms.MouseEventHandler(this.computerFieldArea_MouseClick);
			// 
			// btnStart
			// 
			this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnStart.Enabled = false;
			this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStart.Font = new System.Drawing.Font("Impact", 12F);
			this.btnStart.Location = new System.Drawing.Point(466, 34);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(111, 60);
			this.btnStart.TabIndex = 6;
			this.btnStart.Text = "СТАРТ";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnReset
			// 
			this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReset.Font = new System.Drawing.Font("Impact", 12F);
			this.btnReset.Location = new System.Drawing.Point(466, 122);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(111, 60);
			this.btnReset.TabIndex = 7;
			this.btnReset.Text = "ЗАНОВО";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnGenerateField
			// 
			this.btnGenerateField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnGenerateField.Location = new System.Drawing.Point(232, 188);
			this.btnGenerateField.Name = "btnGenerateField";
			this.btnGenerateField.Size = new System.Drawing.Size(168, 38);
			this.btnGenerateField.TabIndex = 8;
			this.btnGenerateField.Text = "СГЕНЕРИРОВАТЬ";
			this.btnGenerateField.UseVisualStyleBackColor = true;
			this.btnGenerateField.Click += new System.EventHandler(this.btnGenerateField_Click);
			// 
			// GameWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(591, 599);
			this.Controls.Add(this.btnGenerateField);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnTorpedoBoat);
			this.Controls.Add(this.btnDestroyer);
			this.Controls.Add(this.btnCruiser);
			this.Controls.Add(this.btnBattleship);
			this.Controls.Add(this.playerFieldArea);
			this.Controls.Add(this.computerFieldArea);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "GameWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Морской бой";
			((System.ComponentModel.ISupportInitialize)(this.playerFieldArea)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.computerFieldArea)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox computerFieldArea;
        private System.Windows.Forms.PictureBox playerFieldArea;
        private System.Windows.Forms.Button btnBattleship;
        private System.Windows.Forms.Button btnCruiser;
        private System.Windows.Forms.Button btnDestroyer;
        private System.Windows.Forms.Button btnTorpedoBoat;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnGenerateField;
	}
}

