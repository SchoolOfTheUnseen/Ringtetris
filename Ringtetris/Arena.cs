using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Ringtetris
{
	/// <summary>
	/// Spielfeld
	/// </summary>
	public class TArena
	{
		/// <summary>
		/// Die Anzahl Schichten, durch die ein Teil hinunterfallen kann
		/// </summary>
		//public const int AnzahlSchichten = 10;
		public const int AnzahlSchichten = 18;
		/// <summary>
		/// Die "Breite" eines Teils
		/// </summary>
		public const int AnzahlSektoren = 8;

		/// <summary>
		/// Das Spielfeld, wo bereits gefallene Teile gespeichert werden
		/// </summary>
		private bool[,] Spielfeld;

		/// <summary>
		/// Zeiger auf die graphische Darstellung des Spielfeldes
		/// </summary>
		private TVisualArena myVisual;

		/// <summary>
		/// Schnittstelle zur Anzeige
		/// </summary>
		private TDisplay _MyDisplay;

		/// <summary>
		/// Timer für das Fallen der Teile
		/// </summary>
		private Timer _MyTimer;

		/// <summary>
		/// Ist <c>true</c>, wenn das Spiel pausiert ist
		/// </summary>
		private bool _isPause;
		/// <summary>
		/// Eigenschaft für die Pause
		/// </summary>
		public bool isPause
		{
			get
			{
				return this._isPause;
			}
			set
			{
				if (value != this._isPause)
				{
					this._isPause = value;
					if (this._isPause)
					{
						this._MyTimer.Stop();
						//TSegmentBase.stopAnimation();
					}
					else if (!TSegmentBase.AnimationIsRunning)
						this._MyTimer.Start();
					this.myVisual.update();
					this._MyDisplay.refreshNext();
				}
			}
		}

		/// <summary>
		/// Gibt an, ob das Spiel noch läuft
		/// </summary>
		private bool _isRunning;
		/// <summary>
		/// Lese-Zugriff darauf, ob das Spiel noch läuft
		/// </summary>
		public bool isRunning
		{
			get
			{
				return this._isRunning;
			}
		}

		/// <summary>
		/// Informationen über die fallenden Teile
		/// </summary>
		private TSharedData _SharedData;

		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		/// <param name="display">Schnittstelle zur Anzeige</param>
		public TArena(TDisplay display, TSharedData data)
		{
			this._MyDisplay = display;
			this._SharedData = data;

			this.Spielfeld = new bool[AnzahlSektoren, AnzahlSchichten];

			//Die innerste Schicht ist besetzt
			for (int i = 0; i < AnzahlSektoren; i++)
				this.Spielfeld[i, 0] = true;

			initTimer();

			this._isPause = false;
			this._isRunning = true;
		}

		/// <summary>
		/// Das fallende Teil wird zufällig zu einem neuen 
		/// Teil und an die Startposition gesetzt
		/// </summary>
		/// <returns><c>true</c>, falls das Teil Platz hat; 
		/// <c>false</c> bei Kollision</returns>
		private bool startNewFallingTeil()
		{
			this._SharedData.startNewFallingTeil();

			if (collidesWith(this._SharedData.FallingTeil, 0, 0))
			{
				this._MyTimer.Stop();
				this._isRunning = false;
				return false;
			}

			return true;
		}

		/// <summary>
		/// Initialisiert den Timer
		/// </summary>
		private void initTimer()
		{
			this._MyTimer = new Timer();
			this._MyTimer.Interval = 1500;
			this._MyTimer.Tick += _MyTimer_Tick;
			this._MyTimer.Enabled = true;
			this._MyTimer.Start();
		}

		/// <summary>
		/// Wird aufgerufen, wenn der Timer abläuft und das fallende Teil nach unten bewegt werden soll
		/// </summary>
		void _MyTimer_Tick(object sender, EventArgs e)
		{
			//Versuchen, das fallende Teil nach unten zu bewegen
			bool h = this.tryMoveFallendesTeilInDirection(+0, -1);
			if (!h)
			//Teil ist unten angekommen
			{
				this.saveTeil(this._SharedData.FallingTeil);
				this.myVisual.saveTeil(this._SharedData.FallingTeil);

				Graphics g = this.myVisual.createGraphics();
				this.deleteFullRings(g);

				bool ok = startNewFallingTeil();

				if (!this.isPause && this._isRunning)
				{
					this.myVisual.drawFallendesTeil(ok);
					this._MyDisplay.displayNext(this._SharedData.NextFallingTeil, this._SharedData.NextFallingColor);
				}
				else
					this.myVisual.drawFallendesTeil(ok);
			}
		} //Ende Methode _MyTimer_Tick

		/// <summary>
		/// Prüft, ob an der angegebenen Stelle ein Segment liegt
		/// </summary>
		/// <param name="breite">Sektor</param>
		/// <param name="hoehe">Schicht</param>
		/// <returns><c>true</c>, falls an dieser Stelle ein Segment liegt; 
		/// <c>false</c> sonst.</returns>
		public bool hasSegment(int breite, int hoehe)
		{
			return this.Spielfeld[breite, hoehe];
		}

		/// <summary>
		/// Berechnet einen neuen Sektor
		/// </summary>
		/// <param name="sektor">Der alte Sektor</param>
		/// <param name="deltaSektor">Die Verschiebung des Sektors</param>
		/// <returns>Der neue Sektor</returns>
		public int calcNewSektor(int sektor, int deltaSektor)
		{
			int result = sektor + deltaSektor;
			if (result < 0)
				result += AnzahlSektoren;
			result = result % AnzahlSektoren; //Die Position im Sektor geht rundum
			return result;
		}

		/// <summary>
		/// Prüft, ob das Teil mit anderen bereits gefallenen Segmenten kollidiert, wenn es entsprechend der Vorgabe verschoben wird
		/// </summary>
		/// <param name="t">Das fallende Teil</param>
		/// <param name="deltaSektor">Verschiebung im Sektor</param>
		/// <param name="deltaSchicht">Verschiebung in der Schicht</param>
		/// <returns><c>true</c> bei Kollision; <c>false</c> sonst</returns>
		private bool collidesWith(TTeil t, int deltaSektor, int deltaSchicht)
		{
			int positionSektor = calcNewSektor(t.PositionSektor, deltaSektor);
			int positionSchicht = t.PositionSchicht + deltaSchicht;

			for (int i = 0; i < TTeil.MaxBreite; i++)
				for (int k = 0; k < TTeil.MaxHoehe; k++)
					if (t.hasSegment(i, k))
					//Das Teil hat an dieser Stelle ein Segment
					{
						int pos = calcNewSektor(positionSektor, i);
						int schicht = positionSchicht - k;

						if (schicht >= 0 &&
							this.Spielfeld[pos, schicht]) //Überschneidung gefunden
							return true;
					}

			//Keine Überschneidung gefunden
			return false;
		}

		/// <summary>
		/// Speichert das Teil, das unten angekommen ist
		/// </summary>
		/// <param name="t">Das zu speichernde Teil</param>
		public void saveTeil(TTeil t)
		{
			for (int i = 0; i < TTeil.MaxBreite; i++)
				for (int k = 0; k < TTeil.MaxHoehe; k++)
					if (t.hasSegment(i, k))
					//Das Teil hat an dieser Stelle ein Segment
					{
						int pos = calcNewSektor(t.PositionSektor, i);
						int schicht = t.PositionSchicht - k;
						if (schicht > 0)
							this.Spielfeld[pos, schicht] = true;
					}
		}

		/// <summary>
		/// Gibt das Spielfeld auf der Console aus (Debug-Output)
		/// </summary>
		public void printSpielfeld()
		{
			for (int i = AnzahlSchichten - 1; i >= 0; i--)
			{
				for (int k = 0; k < AnzahlSektoren; k++)
					if (this.Spielfeld[k, i])
						Console.Write("1 ");
					else
						Console.Write("0 ");
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		/// <summary>
		/// Registriert eine Arena für die visuelle Darstellung
		/// </summary>
		/// <param name="a">Die zu registrierende Arena</param>
		public void registerArena(TVisualArena a)
		{
			this.myVisual = a;
			this._MyDisplay.registerArena(a);
		}

		/// <summary>
		/// Verschiebt das fallende Teil falls möglich
		/// </summary>
		/// <param name="deltaSektor">Verschiebung im Sektor</param>
		/// <param name="deltaSchicht">Verschiebung in der Schicht</param>
		/// <returns><c>true</c>, falls das Teil bewegt wurde; 
		/// <c>false</c> sonst.</returns>
		public bool tryMoveFallendesTeilInDirection(int deltaSektor, int deltaSchicht)
		{
			if (!collidesWith(this._SharedData.FallingTeil, deltaSektor, deltaSchicht))
			{
				int newSektor = calcNewSektor(this._SharedData.FallingTeil.PositionSektor, deltaSektor);
				int newSchicht = this._SharedData.FallingTeil.PositionSchicht + deltaSchicht;
				if (this.myVisual != null)
					this.myVisual.updateFallingTeil(newSektor, newSchicht);
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Prüft, ob es volle Ringe gibt und löscht diese ggf.
		/// </summary>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		public void deleteFullRings(Graphics g)
		{
			//Timer anhalten
			this._MyTimer.Stop();

			//Volle Ringe zählen
			int hoehe = TTeil.MaxHoehe;
			int anzahlVolleRinge = 0;
			bool[] linien = new bool[TTeil.MaxHoehe];
			for (int i = 0; i < TTeil.MaxHoehe; i++)
				linien[i] = false;

			int schichtStart = this._SharedData.FallingTeil.PositionSchicht;
			int schichtStop;
			int l = 0;
			if (this._SharedData.FallingTeil.PositionSchicht - hoehe + 1 <= 0)
			{
				//Unterste Reihe ist fix, deshalb Stop bei 1
				schichtStop = 1;
			}
			else
			{
				schichtStop = this._SharedData.FallingTeil.PositionSchicht - hoehe + 1;
			}

			//Die vollen Linien bestimmen
			for (int i = schichtStart; i >= schichtStop; i--)
			{
				bool ok = true; //Wird false, wenn eine Stelle nicht besetzt ist
				for (int k = 0; k < AnzahlSektoren; k++)
					if (!this.Spielfeld[k, i])
						ok = false;

				if (ok)
				{
					anzahlVolleRinge++;
					linien[l] = true;
				}
				l++;
			}

			//Animation
			if (anzahlVolleRinge == TTeil.MaxHoehe)
			//Tetris
			{
				//Animation: Blinken
				if (this.myVisual != null)
				{
					Color c = this.myVisual.getBackColor();
					this.myVisual._MeinSegment.blink(this._SharedData.FallingTeil.PositionSchicht, g, c, this.myVisual);
				}
			}
			else if (anzahlVolleRinge > 0)
				//Einzelne Linien
			{
				//Animation: Ringe löschen
				if (this.myVisual != null)
				{
					Color c = this.myVisual.getBackColor();
					this.myVisual._MeinSegment.animateClearLines(this._SharedData.FallingTeil.PositionSchicht, g, c, linien, this.myVisual);
				}
			}

			if (anzahlVolleRinge > 0)
			{
				//Zeilen löschen
				int move = 0;
				int t;
				if (this._SharedData.FallingTeil.PositionSchicht - hoehe + 1 <= 0)
					t = this._SharedData.FallingTeil.PositionSchicht - 1;
				else
					t = hoehe - 1;

				for (int i = schichtStop; i <= TArena.AnzahlSchichten - 1; i++)
				{
					if (t >= 0 && linien[t]) //An dieser Stelle ist ein voller Ring
						move++;
					else
					{
						for (int k = 0; k < TArena.AnzahlSektoren; k++)
						{
							this.Spielfeld[k, i - move] = this.Spielfeld[k, i];
						}
					}
					t--;
				}

				//Die obersten Zeilen leeren
				for (int i = TArena.AnzahlSchichten - 1; i >= TArena.AnzahlSchichten - anzahlVolleRinge; i--)
					for (int k = 0; k < TArena.AnzahlSektoren; k++)
					{
						this.Spielfeld[k, i] = false;
					}

				//Punkte
				int level = this._MyDisplay.Level;
				int punkte;
				if (anzahlVolleRinge == 1)
					punkte = (level + 1) * 40;
				else if (anzahlVolleRinge == 2)
					punkte = (level + 1) * 100;
				else if (anzahlVolleRinge == 3)
					punkte = (level + 1) * 300;
				else if (anzahlVolleRinge == 4)
					punkte = (level + 1) * 1200;
				else
					punkte = 0;
				this._MyDisplay.addPoints(punkte);

				//Nächster Level?
				this._MyDisplay.addRings(anzahlVolleRinge);
				int nextLevel = this._MyDisplay.Rings / 10;
				if (nextLevel > this._MyDisplay.Level)
				{
					this._MyDisplay.setLevel(nextLevel);

					//Teile fallen jetzt schneller
					if (nextLevel >= 15)
						this._MyTimer.Interval = 100;
					else
						this._MyTimer.Interval = 1500 - nextLevel * 100;
				}

				int posTeil = this._SharedData.FallingTeil.PositionSchicht;

				//Sonst wird das fallende Teil nach dem Löschen weiterhin angezeigt
				this._SharedData.FallingTeil.Clear();

				this.myVisual.deleteFullRings(linien, schichtStop, anzahlVolleRinge, posTeil);
				this.myVisual.update();
			}

			if (!this._isPause && this._isRunning)
				this._MyTimer.Start();
		} //Ende Methode deleteFullRings

		/// <summary>
		/// Beendet das Spiel
		/// </summary>
		public void stopGame()
		{
			this._MyTimer.Stop();
			this._SharedData.FallingTeil.Clear();
			this._isRunning = false;
		}
	} //Ende Klasse TArena

	/// <summary>
	/// Graphische Darstellung des Spielfelds
	/// </summary>
	public class TVisualArena
	{
		/// <summary>
		/// Das Spielfeld, wo bereits gefallene Teile gespeichert werden
		/// </summary>
		private Color[,] SpielfeldFarben;

		/// <summary>
		/// Die Arena, die angezeigt wird
		/// </summary>
		public TArena myArena;

		/// <summary>
		/// Das Panel, auf dem gezeichnet wird
		/// </summary>
		private Panel myPanel;

		/// <summary>
		/// Wird während der Pause angezeigt
		/// </summary>
		private Label laPause;

		/// <summary>
		/// Beinhaltet die Eckpunkte der Mitte für die Dartsellung als Trapez
		/// </summary>
		private List<PointF> Eckpunkte;
		/// <summary>
		/// Pfad mit den Eckpunkten
		/// </summary>
		private GraphicsPath Path;

		/// <summary>
		/// Informationen über die fallenden Teile
		/// </summary>
		private TSharedData _SharedData;

		/// <summary>
		/// Objekt zum Zeichnen von Segmenten
		/// </summary>
		public TVisualSegment _MeinSegment;

		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		/// <param name="a"></param>
		/// <param name="p"></param>
		public TVisualArena(TArena a, Panel p, TSharedData data, TVisualSegment segment)
		{
			this.myArena = a;
			this.myPanel = p;
			this._MeinSegment = segment;
			this.SpielfeldFarben = new Color[TArena.AnzahlSektoren, TArena.AnzahlSchichten];
			this.myPanel.Paint += myPanel_Paint;

			initLabelPause();

			this.myArena.registerArena(this);
			this._SharedData = data;
		}

		/// <summary>
		/// Initialisiert das Label zur Anzeige der Pause
		/// </summary>
		private void initLabelPause()
		{
			this.laPause = new Label();
			this.laPause.Text = "Pause";
			Font h = this.laPause.Font;
			this.laPause.Font = new Font(h.FontFamily, 18, FontStyle.Bold);
			this.laPause.Parent = this.myPanel;
			this.laPause.AutoSize = true;
			int x = (this.myPanel.ClientRectangle.Width - this.laPause.Width) / 2;
			int y = (this.myPanel.ClientRectangle.Height - this.laPause.Height) / 2;
			this.laPause.Left = x;
			this.laPause.Top = y;
			this.laPause.Anchor = AnchorStyles.Top;
			this.laPause.Visible = false;
		}

		/// <summary>
		/// Zeichnet das fallende Teil. 
		/// Zeigt auch das Spielende an, wenn das fallende Teil beim Erzeugen kollidiert
		/// </summary>
		/// <param name="ok">Ist <c>true</c>, wenn das Teil Platz hat, 
		/// ist <c>false</c> bei Kollision</param>
		public void drawFallendesTeil(bool ok)
		{
			//if (this.myArena.isRunning && !this.myArena.isPause)
			//if (!this.myArena.isPause || !ok)
			if ((this.myArena.isRunning || (!this.myArena.isRunning && !ok)) && !this.myArena.isPause)
			{
				Graphics g = this.myPanel.CreateGraphics();
				TVisualTeil.drawTeil(g, this._SharedData.FallingTeil, this._SharedData.FallingColor, this._MeinSegment);
			}

			if (!ok)
			{
				Form fo = this.myPanel.FindForm();
				MessageBox.Show(fo, "Game Over", fo.Text);
			}
		}

		/// <summary>
		/// Wird aufgerufen, wenn das Panel neu gezeichnet wird
		/// </summary>
		void myPanel_Paint(object sender, PaintEventArgs e)
		{
			if (!this.myArena.isRunning)
				return;

			if (this.myArena.isPause)
			{
				this.laPause.Visible = true;
				return;
			}
			else
				this.laPause.Visible = false;

			//Alle Segmente zeichen
			for (int i = TArena.AnzahlSchichten - 1; i >= 1; i--)
				for (int k = 0; k < TArena.AnzahlSektoren; k++)
					if (this.myArena.hasSegment(k, i))
					{
						Color c = this.SpielfeldFarben[k, i];
						this._MeinSegment.drawSegment(k, i, e.Graphics, c);
					}

			TVisualTeil.drawTeil(e.Graphics, this._SharedData.FallingTeil, this._SharedData.FallingColor, this._MeinSegment);
			this._MeinSegment.reclearTrapez(e.Graphics);
		} //Ende Methode myPanel_Paint

		/// <summary>
		/// Speichert das Teil, das unten angekommen ist
		/// </summary>
		/// <param name="t">Das zu speichernde Teil</param>
		public void saveTeil(TTeil t)
		{
			for (int i = 0; i < TTeil.MaxBreite; i++)
				for (int k = 0; k < TTeil.MaxHoehe; k++)
					if (t.hasSegment(i, k))
					//Das Teil hat an dieser Stelle ein Segment
					{
						int pos = this.myArena.calcNewSektor(t.PositionSektor, i);
						this.SpielfeldFarben[pos, t.PositionSchicht - k] = this._SharedData.FallingColor;
					}
		}

		/// <summary>
		/// Zeichnet das ganze Spielfeld neu
		/// </summary>
		public void update()
		{
			this.myPanel.Refresh();
		}

		/// <summary>
		/// Zeichnet das fallende Teil an der neuen Position
		/// </summary>
		/// <param name="newSektor">neuer Sektor</param>
		/// <param name="newSchicht">neue Schicht</param>
		public void updateFallingTeil(int newSektor, int newSchicht)
		{
			Graphics g = this.myPanel.CreateGraphics();
			TVisualTeil.deleteTeil(g, this._SharedData.FallingTeil, this.myPanel.BackColor, this, this._MeinSegment);

			this._SharedData.FallingTeil.move(newSektor, newSchicht);
			TVisualTeil.drawTeil(g, this._SharedData.FallingTeil, this._SharedData.FallingColor, this._MeinSegment);
		}

		/// <summary>
		/// Löscht die vollen Ringe (ohne Animation)
		/// </summary>
		/// <param name="linien">Die Elemente, die auf true sind, bezeichnen volle Ringe.</param>
		/// <param name="posSchicht">Unterste Schicht, ab der verschoben wird</param>
		/// <param name="anzahlVolleRinge">Anzahl gebildete Ringe</param>
		/// <param name="posTeil">Position des gefallenen Teils (für Bestimmung unterste Position in linien)</param>
		public void deleteFullRings(bool[] linien, int posSchicht, int anzahlVolleRinge, int posTeil)
		{
			int move = 0;
			int t;
			if (posTeil - TTeil.MaxHoehe + 1 <= 0)
				t = posTeil - 1;
			else
				t = TTeil.MaxHoehe - 1;

			//Zeilen löschen
			for (int i = posSchicht; i <= TArena.AnzahlSchichten - 1; i++)
			{
				if (t >= 0 && linien[t])
					move++;
				else
				{
					//Zeile verschieben
					for (int k = 0; k < TArena.AnzahlSektoren; k++)
					{
						this.SpielfeldFarben[k, i - move] = this.SpielfeldFarben[k, i];
					}
				}
				t--;
			}

			//Die obersten Zeilen leeren
			for (int i = TArena.AnzahlSchichten - 1; i >= TArena.AnzahlSchichten - anzahlVolleRinge; i--)
				for (int k = 0; k < TArena.AnzahlSektoren; k++)
				{
					this.SpielfeldFarben[k, i] = Color.Empty;
				}
		} //Ende Methode deleteFullRings

		/// <summary>
		/// Erzeugt ein Graphics-Objekt zum Zeichnen
		/// </summary>
		/// <returns>Das Graphics-Objekt</returns>
		public Graphics createGraphics()
		{
			return this.myPanel.CreateGraphics();
		}

		/// <summary>
		/// Gibt die Hintergrundfarbe des Panels zurück
		/// </summary>
		/// <returns>Die Hintergrundfarbe des Panels</returns>
		public Color getBackColor()
		{
			return this.myPanel.BackColor;
		}

		/// <summary>
		/// "Repariert" die benachbarten Segmente
		/// </summary>
		/// <remarks>Durch das Löschen eines Segments werden die schwarzen Begrenzungslinien 
		/// der benachbarten Segmente gelöscht. Diese werden hier wieder nachgezeichnet</remarks>
		/// <param name="positionSektor">Der Sektor des Segments, dessen Nachbarn repariert werden</param>
		/// <param name="positionSchicht">Die Schicht des Segments, dessen Nachbarn repariert werden</param>
		/// <param name="g">Graphics-Objekt zu Zeichnen</param>
		public void repairNachbarn(int positionSektor, int positionSchicht, Graphics g)
		{
			//Linker Nachbar
			int pos = this.myArena.calcNewSektor(positionSektor, -1);
			if (this.myArena.hasSegment(pos, positionSchicht))
				this._MeinSegment.drawRahmen(pos, positionSchicht, g);

			//Rechter Nachbar
			pos = this.myArena.calcNewSektor(positionSektor, +1);
			if (this.myArena.hasSegment(pos, positionSchicht))
				this._MeinSegment.drawRahmen(pos, positionSchicht, g);

			//Unterer Nachbar
			if (positionSchicht > 1 && this.myArena.hasSegment(positionSektor, positionSchicht - 1))
				this._MeinSegment.drawRahmen(positionSektor, positionSchicht - 1, g);

			//Oberer Nachbar
			if (positionSchicht < TArena.AnzahlSchichten - 1 && this.myArena.hasSegment(positionSektor, positionSchicht + 1))
				this._MeinSegment.drawRahmen(positionSektor, positionSchicht + 1, g);
		}

		/// <summary>
		/// Bestimmt alle Segmente in der Schicht, deren Rahmen repariert werden müssen 
		/// und speichert alle diese Pfade in der Liste
		/// </summary>
		/// <param name="positionSchicht">Die zu reparierende Schicht</param>
		/// <param name="liste">In dieser Liste werden alle gefundenen Pfade abgelegt</param>
		public void getRepairPaths(int positionSchicht, List<GraphicsPath> liste)
		{
			int n = TArena.AnzahlSektoren;
			for (int i = 0; i < n; i++)
				if (this.myArena.hasSegment(i, positionSchicht))
				{
					GraphicsPath path = this._MeinSegment.calcRahmen(i, positionSchicht);
					liste.Add(path);
				}
		}
	} //Ende Klasse TVisualArena
} //Ende namespace Ringtetris
