using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

//challengesubmission@startglobal.org<challengesubmission@startglobal.org>;

namespace Ringtetris
{
	/// <summary>
	/// Das Hauptfenster der Anwendung
	/// </summary>
	public partial class foMain : Form
	{
		/// <summary>
		/// Das Spielfeld
		/// </summary>
		TArena a;
		/// <summary>
		/// Die visuelle Darstellung des Spielfelds
		/// </summary>
		TVisualArena myArena;
		/// <summary>
		/// Daten für fallende Teile
		/// </summary>
		private TSharedData Data;
		/// <summary>
		/// Anzeige für Punkte usw.
		/// </summary>
		private TDisplayFoMain Display;
		/// <summary>
		/// Segmente zeichnen
		/// </summary>
		private TVisualSegment Segment;

		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		public foMain()
		{
			InitializeComponent();

			this.Data = new TSharedData();
			this.Display = new TDisplayFoMain(this, this.Data);
			this.a = new TArena(this.Display, this.Data);
			this.Segment = new TVisualSegment(this.paTetris);
			this.Segment.registerArena(this.a);
			this.myArena = new TVisualArena(a, this.paTetris, this.Data, this.Segment);
			TSegmentBase.registerForm(this);
		}

		/// <summary>
		/// Wartet Milliseconds Millisekunden
		/// </summary>
		/// <param name="Milliseconds">Anzahl Millisekunden</param>
		public static void wait(int Milliseconds)
		{
			DateTime start = DateTime.Now;
			int zeit;
			do
			{
				Application.DoEvents();
				DateTime end = DateTime.Now;
				zeit = (int)end.Subtract(start).TotalMilliseconds;
			} while (zeit < Milliseconds);
		}

		/// <summary>
		/// Wird aufgerufen, wenn auf eine Taste gedrückt wird
		/// </summary>
		private void foMain_KeyDown(object sender, KeyEventArgs e)
		{
			//Falls das Spiel pausiert ist, keine Änderungen zulassen
			if (this.a.isPause || !this.a.isRunning)
				return;

			if (e.KeyCode == Keys.Right)
			{
				this.a.tryMoveFallendesTeilInDirection(+1, +0);
			}
			else if (e.KeyCode == Keys.Left)
			{
				this.a.tryMoveFallendesTeilInDirection(-1, +0);
			}
			else if (e.KeyCode == Keys.Down)
			{
				this.a.tryMoveFallendesTeilInDirection(+0, -1);
			}
		} //Ende Methode foMain_KeyDown

		/// <summary>
		/// Lädt ein Icon
		/// </summary>
		/// <param name="filename">Der Dateiname des Icons</param>
		private void loadIcon(string filename)
		{
			try
			{
				System.Drawing.Icon ico = new Icon(filename);
				this.Icon = ico;
			}
			catch
			{
			}
		}

		/// <summary>
		/// Wird aufgerufen, wenn auf den Menüpunkt "Anzeige" geklickt wird
		/// </summary>
		private void miAnsicht_Click(object sender, EventArgs e)
		{
			bool einstellung = this.a.isPause;
			if (this.a.isRunning)
				this.a.isPause = true;

			bool result = foAnsicht.showDialog(this);
			if (TSegmentBase.Mode == EDisplayMode.Ring)
			{
				this.Text = "Ringtetris";
				loadIcon("ring.ico");
			}
			else if (TSegmentBase.Mode == EDisplayMode.Trapez)
			{
				this.Text = "Trapeztetris";
				loadIcon("trapez.ico");
			}

			if (result && this.a.isRunning)
			{
				//Anzeige Refresh
				this.Refresh();
			}

			this.a.isPause = einstellung;
		}

		/// <summary>
		/// Wird aufgerufen, wenn auf den Menüpunkt "Hilfe" geklickt wird
		/// </summary>
		private void miHilfe_Click(object sender, EventArgs e)
		{
			bool einstellung = this.a.isPause;
			if (this.a.isRunning)
				this.a.isPause = true;

			foHilfe fo = new foHilfe();
			fo.ShowDialog(this);

			this.a.isPause = einstellung;
		}

		/// <summary>
		/// Wird aufgerufen, wenn auf den Menüpunkt "Pause" geklickt wird
		/// </summary>
		private void miPause_Click(object sender, EventArgs e)
		{
			this.a.isPause = !this.a.isPause;
		}

		/// <summary>
		/// Wird aufgerufen, wenn auf den Menüpunkt "Neues Spiel" geklickt wird
		/// </summary>
		private void miNeuesSpiel_Click(object sender, EventArgs e)
		{
			if (this.a.isRunning)
			{
				bool h = this.a.isPause;
				this.a.isPause = true;
				DialogResult res = MessageBox.Show(
					this, "Willst du das laufende Spiel wirklich beenden?", this.Text, MessageBoxButtons.YesNo);
				this.a.isPause = h;
				if (res == System.Windows.Forms.DialogResult.No)
				{
					return;
				}
			}
			TSegmentBase.stopAnimation();
			this.a.stopGame();

			this.Display.reset();
			this.Data.reset();
			this.a = new TArena(this.Display, this.Data);
			this.Segment.registerArena(this.a);
			this.myArena = new TVisualArena(a, this.paTetris, this.Data, this.Segment);
			this.paTetris.Refresh();
			this.paNext.Refresh();
		}

		/// <summary>
		/// Wird aufgerufen, wenn das Fenster geschlossen wird
		/// </summary>
		private void foMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			TSegmentBase.stopAnimation();
			this.a.stopGame();
		}
	} //Ende Klasse foMain
} //Ende namespace Ringtetris
