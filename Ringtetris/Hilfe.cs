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
	/// Zeigt in einem Fenster die Spielregeln an
	/// </summary>
	public partial class foHilfe : Form
	{
		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		public foHilfe()
		{
			InitializeComponent();

			//Titel markieren
			this.rtbHilfe.Find("Spielanleitung", RichTextBoxFinds.MatchCase);

			//Titel fett und grösser
			Font f = this.rtbHilfe.Font;
			this.rtbHilfe.SelectionFont = new Font(f.FontFamily, 14, FontStyle.Bold);

			//Markierung aufheben
			this.rtbHilfe.Select(0, 0);
		}
	} //Ende Klasse foHilfe
} //Ende namespace Ringtetris
