using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ringtetris
{
	/// <summary>
	/// Fenster zum Wählen der Anzeige (Ringe oder Trapeze)
	/// </summary>
	public partial class foAnsicht : Form
	{
		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		public foAnsicht()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Wird aufgerufen, wenn auf den OK-Button geklickt wird
		/// </summary>
		private void buOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Wird aufgerufen, wenn auf den Abbrechen-Button geklickt wird
		/// </summary>
		private void buAbbrechen_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		/// <summary>
		/// Zeigt ein Fenster an und speichert ggf die gewählte Einstellung
		/// </summary>
		/// <param name="owner">Das übergeordnete Fenster</param>
		/// <returns><c>true</c>, falls der Dialog mit OK beendet wurde; 
		/// <c>false</c> sonst.</returns>
		public static bool showDialog(Form owner)
		{
			//Fenster erzeugen
			foAnsicht fo = new foAnsicht();

			//Voreinstellung
			EDisplayMode mode = TSegmentBase.Mode;
			if (mode == EDisplayMode.Ring)
				fo.rbRinge.Checked = true;
			else if (mode == EDisplayMode.Trapez)
				fo.rbTrapeze.Checked = true;

			//Fenster anzeigen
			fo.ShowDialog(owner);

			if (fo.DialogResult == DialogResult.OK)
				//Das Fenster wurde mit OK beendet
			{
				//Wahl speichern
				if (fo.rbRinge.Checked)
					TSegmentBase.Mode = EDisplayMode.Ring;
				else if (fo.rbTrapeze.Checked)
					TSegmentBase.Mode = EDisplayMode.Trapez;
				return true;
			}

			return false;
		}
	} //Ende Klasse foAnsicht
} //Ende namespace Ringtetris
