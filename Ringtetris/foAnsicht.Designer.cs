namespace Ringtetris
{
	partial class foAnsicht
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(foAnsicht));
			this.gbOptions = new System.Windows.Forms.GroupBox();
			this.rbTrapeze = new System.Windows.Forms.RadioButton();
			this.rbRinge = new System.Windows.Forms.RadioButton();
			this.buOK = new System.Windows.Forms.Button();
			this.buAbbrechen = new System.Windows.Forms.Button();
			this.gbOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbOptions
			// 
			this.gbOptions.Controls.Add(this.rbTrapeze);
			this.gbOptions.Controls.Add(this.rbRinge);
			this.gbOptions.Location = new System.Drawing.Point(0, 0);
			this.gbOptions.Name = "gbOptions";
			this.gbOptions.Size = new System.Drawing.Size(152, 80);
			this.gbOptions.TabIndex = 0;
			this.gbOptions.TabStop = false;
			this.gbOptions.Text = "Segmente anzeigen als";
			// 
			// rbTrapeze
			// 
			this.rbTrapeze.AutoSize = true;
			this.rbTrapeze.Location = new System.Drawing.Point(16, 45);
			this.rbTrapeze.Name = "rbTrapeze";
			this.rbTrapeze.Size = new System.Drawing.Size(64, 17);
			this.rbTrapeze.TabIndex = 1;
			this.rbTrapeze.TabStop = true;
			this.rbTrapeze.Text = "Trapeze";
			this.rbTrapeze.UseVisualStyleBackColor = true;
			// 
			// rbRinge
			// 
			this.rbRinge.AutoSize = true;
			this.rbRinge.Location = new System.Drawing.Point(16, 24);
			this.rbRinge.Name = "rbRinge";
			this.rbRinge.Size = new System.Drawing.Size(53, 17);
			this.rbRinge.TabIndex = 0;
			this.rbRinge.TabStop = true;
			this.rbRinge.Text = "Ringe";
			this.rbRinge.UseVisualStyleBackColor = true;
			// 
			// buOK
			// 
			this.buOK.Location = new System.Drawing.Point(40, 104);
			this.buOK.Name = "buOK";
			this.buOK.Size = new System.Drawing.Size(75, 23);
			this.buOK.TabIndex = 1;
			this.buOK.Text = "OK";
			this.buOK.UseVisualStyleBackColor = true;
			this.buOK.Click += new System.EventHandler(this.buOK_Click);
			// 
			// buAbbrechen
			// 
			this.buAbbrechen.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buAbbrechen.Location = new System.Drawing.Point(40, 136);
			this.buAbbrechen.Name = "buAbbrechen";
			this.buAbbrechen.Size = new System.Drawing.Size(75, 23);
			this.buAbbrechen.TabIndex = 2;
			this.buAbbrechen.Text = "Abbrechen";
			this.buAbbrechen.UseVisualStyleBackColor = true;
			this.buAbbrechen.Click += new System.EventHandler(this.buAbbrechen_Click);
			// 
			// foAnsicht
			// 
			this.AcceptButton = this.buOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buAbbrechen;
			this.ClientSize = new System.Drawing.Size(168, 170);
			this.Controls.Add(this.buAbbrechen);
			this.Controls.Add(this.buOK);
			this.Controls.Add(this.gbOptions);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "foAnsicht";
			this.ShowInTaskbar = false;
			this.Text = "Ansicht";
			this.gbOptions.ResumeLayout(false);
			this.gbOptions.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbOptions;
		private System.Windows.Forms.RadioButton rbTrapeze;
		private System.Windows.Forms.RadioButton rbRinge;
		private System.Windows.Forms.Button buOK;
		private System.Windows.Forms.Button buAbbrechen;
	}
}