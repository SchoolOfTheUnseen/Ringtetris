namespace Ringtetris
{
	partial class foHilfe
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(foHilfe));
			this.rtbHilfe = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// rtbHilfe
			// 
			this.rtbHilfe.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbHilfe.Location = new System.Drawing.Point(0, 0);
			this.rtbHilfe.Name = "rtbHilfe";
			this.rtbHilfe.ReadOnly = true;
			this.rtbHilfe.Size = new System.Drawing.Size(284, 262);
			this.rtbHilfe.TabIndex = 0;
			this.rtbHilfe.Text = resources.GetString("rtbHilfe.Text");
			// 
			// foHilfe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.rtbHilfe);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "foHilfe";
			this.Text = "Hilfe";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox rtbHilfe;
	}
}