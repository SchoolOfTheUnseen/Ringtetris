using System;
using System.Drawing;

namespace Ringtetris
{
	/// <summary>
	/// Diese Klasse ist eine Schnittstelle zur Anzeige
	/// </summary>
	public abstract class TDisplay
	{
		/// <summary>
		/// Die Punkteanzahl
		/// </summary>
		private int _Points;
		/// <summary>
		/// Lese-Zugriff auf die Punkteanzahl
		/// </summary>
		public int Points
		{
			get
			{
				return this._Points;
			}
		}

		/// <summary>
		/// Der aktuelle Level
		/// </summary>
		private int _Level;
		/// <summary>
		/// Lese-Zugriff auf den aktuellen Level
		/// </summary>
		public int Level
		{
			get
			{
				return this._Level;
			}
		}

		/// <summary>
		/// Anzahl gebildete Ringe
		/// </summary>
		private int _Rings;
		/// <summary>
		/// Lese-Zugriff auf die Anzahl gebildeter Ringe
		/// </summary>
		public int Rings
		{
			get
			{
				return this._Rings;
			}
		}

		/// <summary>
		/// Verweis auf die Arena
		/// </summary>
		protected TVisualArena _Arena;

		/// <summary>
		/// Maximalwert für die Werte
		/// Sonst wrappen die Werte ins Negative um, wenn sie nahe int.MaxValue sind!
		/// </summary>
		/// <remarks>int.MaxValue=2147483647</remarks>
		private const int Maximum =  999999999;

		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		public TDisplay()
		{
		}

		/// <summary>
		/// Erhöht den Punktestand
		/// </summary>
		/// <param name="points">Die erzielten Punkte</param>
		public virtual void addPoints(int points)
		{
			if (points < 0) //Overflow
				this._Points = Maximum;
			//else if (this._Points + points > Maximum)
			else if (this._Points > Maximum - points)
				this._Points = Maximum;
			else
				this._Points += points;
		}

		/// <summary>
		/// Setzt den Level
		/// </summary>
		/// <param name="level">Der zu setzende Level</param>
		public virtual void setLevel(int level)
		{
			this._Level = level;
		}

		/// <summary>
		/// Erhöht die Anzahl gebildeter Ringe
		/// </summary>
		/// <param name="rings">Die Anzahl gebildeter Ringe</param>
		public virtual void addRings(int rings)
		{
			//if (this._Rings + rings > Maximum)
			if (this._Rings > Maximum - rings)
				this._Rings = Maximum;
			else
				this._Rings += rings;
		}

		/// <summary>
		/// Setzt die Anzeige auf Anfang zurück
		/// </summary>
		public virtual void reset()
		{
			this._Points = 0;
			this._Level = 0;
			this._Rings = 0;
		}

		/// <summary>
		/// Die Anzeige für das nächste Teil wird neu gezeichnet
		/// </summary>
		/// <remarks>Wird benötigt für Pause</remarks>
		public abstract void refreshNext();

		/// <summary>
		/// Zeigt das nächste Teil an
		/// </summary>
		/// <param name="t">Das nächste Teil</param>
		/// <param name="c">Die Farbe des nächsten Teils</param>
		public abstract void displayNext(TTeil t, Color c);

		/// <summary>
		/// Registriert eine Arena
		/// </summary>
		/// <param name="arena">Die zu registrierende Arena</param>
		public virtual void registerArena(TVisualArena arena)
		{
			this._Arena = arena;
		}
	} //Ende Klasse TDisplay

	/// <summary>
	/// Klasse zur Anzeige auf dem Hauptfenster
	/// </summary>
	public class TDisplayFoMain : TDisplay
	{
		/// <summary>
		/// Zeiger auf das Hauptfenster
		/// </summary>
		private foMain _Form;

		/// <summary>
		/// Zum Zeichnen der Segmente
		/// </summary>
		private TVisualMiniSegment _Segment;

		/// <summary>
		/// Daten für fallende Teile
		/// </summary>
		private TSharedData _Data;

		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		/// <param name="fo">Zeiger auf das Hauptfenster</param>
		public TDisplayFoMain(foMain fo, TSharedData data)
		{
			this._Form = fo;
			this._Segment = new TVisualMiniSegment(fo.paNext);
			fo.paNext.Paint += paNext_Paint;
			this._Data = data;
			this.reset();
		}

		/// <summary>
		/// Registriert eine Arena
		/// </summary>
		/// <param name="arena">Die zu registrierende Arena</param>
		public override void registerArena(TVisualArena arena)
		{
			this._Arena = arena;
			this._Segment.registerArena(arena.myArena);
		}

		/// <summary>
		/// Wird aufgerufen, wenn das Panel mit der Anzeige des nächsten Teils neu gezeichnet wird
		/// </summary>
		void paNext_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (this._Arena != null && !this._Arena.myArena.isPause)
			{
				Graphics g = e.Graphics;
				TTeil t = this._Data.NextFallingTeil;
				Color c = this._Data.NextFallingColor;
				TVisualTeil.drawTeil(g, t, c, this._Segment);
			}
		}

		/// <summary>
		/// Erhöht den Punktestand
		/// </summary>
		/// <param name="points">Die erzielten Punkte</param>
		public override void addPoints(int points)
		{
			base.addPoints(points);
			this._Form.laPunkte.Text = this.Points.ToString();
		}

		/// <summary>
		/// Setzt den Level
		/// </summary>
		/// <param name="level">Der zu setzende Level</param>
		public override void setLevel(int level)
		{
			base.setLevel(level);
			this._Form.laLevel.Text = this.Level.ToString();
		}

		/// <summary>
		/// Erhöht die Anzahl gebildeter Ringe
		/// </summary>
		/// <param name="rings">Die Anzahl gebildeter Ringe</param>
		public override void addRings(int rings)
		{
			base.addRings(rings);
			this._Form.laRinge.Text = this.Rings.ToString();
		}

		/// <summary>
		/// Setzt die Anzeige auf Anfang zurück
		/// </summary>
		public override void reset()
		{
			base.reset();
			this._Form.laPunkte.Text = this.Points.ToString();
			this._Form.laLevel.Text = this.Level.ToString();
			this._Form.laRinge.Text = this.Rings.ToString();
		}

		/// <summary>
		/// Zeigt das nächste Teil an
		/// </summary>
		/// <param name="t">Das nächste Teil</param>
		/// <param name="c">Die Farbe des nächsten Teils</param>
		public override void displayNext(TTeil t, Color c)
		{
			this._Form.paNext.Refresh();
			Graphics g = this._Form.paNext.CreateGraphics();
			TVisualTeil.drawTeil(g, t, c, this._Segment);
		}

		/// <summary>
		/// Die Anzeige für das nächste Teil wird neu gezeichnet
		/// </summary>
		/// <remarks>Wird benötigt für Pause</remarks>
		public override void refreshNext()
		{
			this._Form.paNext.Refresh();
		}
	} //Ende Klasse TDisplayFoMain
} //Endenamespace Ringtetris
