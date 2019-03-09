namespace Ringtetris
{
	partial class foMain
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(foMain));
			this.paTetris = new System.Windows.Forms.Panel();
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.miNeuesSpiel = new System.Windows.Forms.MenuItem();
			this.miAnsicht = new System.Windows.Forms.MenuItem();
			this.miHilfe = new System.Windows.Forms.MenuItem();
			this.miPause = new System.Windows.Forms.MenuItem();
			this.laPunkteCaption = new System.Windows.Forms.Label();
			this.laPunkte = new System.Windows.Forms.Label();
			this.laLevelCaption = new System.Windows.Forms.Label();
			this.laLevel = new System.Windows.Forms.Label();
			this.laRingsCaption = new System.Windows.Forms.Label();
			this.laRinge = new System.Windows.Forms.Label();
			this.laNext = new System.Windows.Forms.Label();
			this.paNext = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// paTetris
			// 
			this.paTetris.Dock = System.Windows.Forms.DockStyle.Left;
			this.paTetris.Location = new System.Drawing.Point(0, 0);
			this.paTetris.Name = "paTetris";
			this.paTetris.Size = new System.Drawing.Size(736, 724);
			this.paTetris.TabIndex = 0;
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miNeuesSpiel,
            this.miAnsicht,
            this.miHilfe,
            this.miPause});
			// 
			// miNeuesSpiel
			// 
			this.miNeuesSpiel.Index = 0;
			this.miNeuesSpiel.Text = "Neues Spiel";
			this.miNeuesSpiel.Click += new System.EventHandler(this.miNeuesSpiel_Click);
			// 
			// miAnsicht
			// 
			this.miAnsicht.Index = 1;
			this.miAnsicht.Text = "Ansicht";
			this.miAnsicht.Click += new System.EventHandler(this.miAnsicht_Click);
			// 
			// miHilfe
			// 
			this.miHilfe.Index = 2;
			this.miHilfe.Text = "Hilfe";
			this.miHilfe.Click += new System.EventHandler(this.miHilfe_Click);
			// 
			// miPause
			// 
			this.miPause.Index = 3;
			this.miPause.Text = "Pause";
			this.miPause.Click += new System.EventHandler(this.miPause_Click);
			// 
			// laPunkteCaption
			// 
			this.laPunkteCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.laPunkteCaption.AutoSize = true;
			this.laPunkteCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.laPunkteCaption.Location = new System.Drawing.Point(776, 8);
			this.laPunkteCaption.Name = "laPunkteCaption";
			this.laPunkteCaption.Size = new System.Drawing.Size(68, 24);
			this.laPunkteCaption.TabIndex = 1;
			this.laPunkteCaption.Text = "Punkte";
			// 
			// laPunkte
			// 
			this.laPunkte.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.laPunkte.AutoSize = true;
			this.laPunkte.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.laPunkte.Location = new System.Drawing.Point(776, 40);
			this.laPunkte.Name = "laPunkte";
			this.laPunkte.Size = new System.Drawing.Size(20, 24);
			this.laPunkte.TabIndex = 2;
			this.laPunkte.Text = "0";
			// 
			// laLevelCaption
			// 
			this.laLevelCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.laLevelCaption.AutoSize = true;
			this.laLevelCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.laLevelCaption.Location = new System.Drawing.Point(776, 88);
			this.laLevelCaption.Name = "laLevelCaption";
			this.laLevelCaption.Size = new System.Drawing.Size(55, 24);
			this.laLevelCaption.TabIndex = 3;
			this.laLevelCaption.Text = "Level";
			// 
			// laLevel
			// 
			this.laLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.laLevel.AutoSize = true;
			this.laLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.laLevel.Location = new System.Drawing.Point(776, 128);
			this.laLevel.Name = "laLevel";
			this.laLevel.Size = new System.Drawing.Size(20, 24);
			this.laLevel.TabIndex = 4;
			this.laLevel.Text = "0";
			// 
			// laRingsCaption
			// 
			this.laRingsCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.laRingsCaption.AutoSize = true;
			this.laRingsCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.laRingsCaption.Location = new System.Drawing.Point(776, 168);
			this.laRingsCaption.Name = "laRingsCaption";
			this.laRingsCaption.Size = new System.Drawing.Size(60, 24);
			this.laRingsCaption.TabIndex = 5;
			this.laRingsCaption.Text = "Ringe";
			// 
			// laRinge
			// 
			this.laRinge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.laRinge.AutoSize = true;
			this.laRinge.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.laRinge.Location = new System.Drawing.Point(776, 208);
			this.laRinge.Name = "laRinge";
			this.laRinge.Size = new System.Drawing.Size(20, 24);
			this.laRinge.TabIndex = 6;
			this.laRinge.Text = "0";
			// 
			// laNext
			// 
			this.laNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.laNext.AutoSize = true;
			this.laNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.laNext.Location = new System.Drawing.Point(776, 248);
			this.laNext.Name = "laNext";
			this.laNext.Size = new System.Drawing.Size(63, 24);
			this.laNext.TabIndex = 7;
			this.laNext.Text = "NEXT";
			// 
			// paNext
			// 
			this.paNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.paNext.Location = new System.Drawing.Point(779, 280);
			this.paNext.Name = "paNext";
			this.paNext.Size = new System.Drawing.Size(105, 105);
			this.paNext.TabIndex = 8;
			// 
			// foMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(932, 724);
			this.Controls.Add(this.paNext);
			this.Controls.Add(this.laNext);
			this.Controls.Add(this.laRinge);
			this.Controls.Add(this.laRingsCaption);
			this.Controls.Add(this.laLevel);
			this.Controls.Add(this.laLevelCaption);
			this.Controls.Add(this.laPunkte);
			this.Controls.Add(this.laPunkteCaption);
			this.Controls.Add(this.paTetris);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "foMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ringtetris";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.foMain_FormClosed);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.foMain_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel paTetris;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem miNeuesSpiel;
		private System.Windows.Forms.MenuItem miHilfe;
		private System.Windows.Forms.Label laPunkteCaption;
		private System.Windows.Forms.Label laLevelCaption;
		public System.Windows.Forms.Label laPunkte;
		public System.Windows.Forms.Label laLevel;
		private System.Windows.Forms.Label laRingsCaption;
		public System.Windows.Forms.Label laRinge;
		private System.Windows.Forms.MenuItem miPause;
		private System.Windows.Forms.Label laNext;
		public System.Windows.Forms.Panel paNext;
		public System.Windows.Forms.MenuItem miAnsicht;
	}
}

