using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ringtetris
{
	/// <summary>
	/// Auflistung der Darstellungsoptionen
	/// </summary>
	public enum EDisplayMode
	{
		Ring, Trapez
	}

	/// <summary>
	/// Enthält alle Daten zum erneuten Löschen der Trapez-Segmente
	/// </summary>
	/// <remarks>Dies wird nötig, wenn das Fenster während der Lösch-Animation minimiert und dann wieder angezeigt wird</remarks>
	class TrapezData
	{
		/// <summary>
		/// Die Schicht, wo das fallende Teil angehalten hat
		/// </summary>
		public int positionSchicht;
		/// <summary>
		/// Die Hintergrundfarbe zum Zeichnen
		/// </summary>
		public Color backColor;
		/// <summary>
		/// Bestimmt die vollen Linien
		/// </summary>
		public bool[] lines;
		/// <summary>
		/// Der aktuelle Sektor, der gerade gelöscht wird
		/// </summary>
		public int CurrentSektor;
		/// <summary>
		/// Bestimmt, ob gerade ein Lösch-Vorgang stattfindet
		/// </summary>
		public bool isActive;

		/// <summary>
		/// Standard-Kostruktor
		/// </summary>
		public TrapezData()
		{
			this.positionSchicht = 0;
			this.backColor = Color.Empty;
			this.lines = null;
			this.CurrentSektor = 0;
			this.isActive = false;
		}
	} //Ende Klasse TrapezData

	/// <summary>
	/// Basisklasse für die graphische Darstellung eines Segments
	/// </summary>
	public abstract class TSegmentBase
	{
		/// <summary>
		/// Die ausgewählte Darstellungsoption
		/// </summary>
		public static EDisplayMode Mode;

		/// <summary>
		/// X-Koordinate des Kreismittelpunktes
		/// </summary>
		protected float CenterX;
		/// <summary>
		/// Y-Koordinate des Kreismittelpunktes
		/// </summary>
		protected float CenterY;

		/// <summary>
		/// Die Breite eines Segments in Pixel
		/// </summary>
		protected double SegmentBreite;
		/// <summary>
		/// Lese-Zugriff auf die Breite der Segmente
		/// </summary>
		public double Breite
		{
			get
			{
				return this.SegmentBreite;
			}
		}

		/// <summary>
		/// Der Sektor, in dem das zu zeichnende Segment liegt
		/// </summary>
		private int PositionSektor;
		/// <summary>
		/// Die Schicht, in der das zu zeichnende Segment liegt
		/// </summary>
		private int PositionSchicht;

		//Shift, damit die Teile "von oben" fallen
		//Sonst fallen die Teile von rechts
		/// <summary>
		/// Verschiebung in rad
		/// </summary>
		private const double Shift = -Math.PI / 2;
		/// <summary>
		/// Verschiebung in Grad
		/// </summary>
		private const int ShiftDegree = -90;

		/// <summary>
		/// Bestimmt, ob die Animation noch läuft, oder abgebrochen wird
		/// </summary>
		/// <remarks>Wird das Fenster während einer Animation geschlossen, so führt das sonst zu einer Exception</remarks>
		private static bool _AnimationIsRunning;
		/// <summary>
		/// Eigenschaft dafür, ob eine Animation läuft
		/// </summary>
		public static bool AnimationIsRunning
		{
			get
			{
				return _AnimationIsRunning;
			}
			set
			{
				if (value != _AnimationIsRunning)
				{
					_AnimationIsRunning = value;
					//Die Ansicht soll während einer Animation nicht geändert werden können
					if (_MyForm != null)
						_MyForm.miAnsicht.Enabled = !_AnimationIsRunning;
				}
			}
		}

		/// <summary>
		/// Verweis auf das Hauptfenster
		/// </summary>
		private static foMain _MyForm;

		/// <summary>
		/// Das Panel, auf dem die Segmente dargestellt werden
		/// </summary>
		protected Panel _MyPanel;

		/// <summary>
		/// Beinhaltet die Eckpunkte der Mitte für die Dartsellung als Trapez
		/// </summary>
		private List<PointF> Eckpunkte;

		/// <summary>
		/// Pfad mit den Eckpunkten
		/// </summary>
		private GraphicsPath Path;

		/// <summary>
		/// Verweis auf die Arena
		/// </summary>
		private TArena _MyArena;

		/// <summary>
		/// Enthält die nötigen Daten zum erneuten Löschen der Trapez-Segmente
		/// </summary>
		/// <remarks>Dies wird nötig, wenn das Fenster während der Lösch-Animation minimiert und dann wieder angezeigt wird</remarks>
		private TrapezData _TrapezData;

		/// <summary>
		/// Pinsel zum Zeichnen
		/// </summary>
		private static SolidBrush _MyBrush;
		/// <summary>
		/// Stift zum Zeichnen
		/// </summary>
		private static Pen _MyPen;

		/// <summary>
		/// Statischer Konstruktor
		/// </summary>
		static TSegmentBase()
		{
			Mode = EDisplayMode.Ring;
			_MyBrush = new SolidBrush(Color.Black);
			_MyPen = new Pen(Color.Black);
		}

		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		/// <param name="pa">Das Panel, auf dem die Segmente dargestellt werden</param>
		public TSegmentBase(Panel pa)
		{
			this._MyPanel = pa;
			this.CenterX = calcCenterX(pa);
			this.CenterY = calcCenterY(pa);
			this.SegmentBreite = calcBreite();
			this._MyArena = null;
			this._MyPanel.Paint += _MyPanel_Paint;
			this._TrapezData = new TrapezData();
		}

		/// <summary>
		/// Registriert eine Arena
		/// </summary>
		/// <param name="a">Die zu registrierende Arena</param>
		public void registerArena(TArena a)
		{
			this._MyArena = a;
		}

		/// <summary>
		/// Wird aufgerufen, wenn das Panel neu gezeichnet wird
		/// </summary>
		private void _MyPanel_Paint(object sender, PaintEventArgs e)
		{
			if (this._MyArena == null || !this._MyArena.isPause)
				drawCenter(e.Graphics);
		}

		#region Center
		/// <summary>
		/// Zeichnet die Mitte in der Ring-Darstellung
		/// </summary>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		private void drawCenterRing(Graphics g)
		{
			_MyBrush.Color = Color.Black;
			int radius = (int)this.SegmentBreite;
			g.FillEllipse(_MyBrush, CenterX - radius, CenterY - radius, 2 * radius, 2 * radius);
		}

		/// <summary>
		/// Initialisiert die Eckpunkte für die Mitte in der Trapez-Darstellung
		/// </summary>
		private void initEckpunkte()
		{
			int n = TArena.AnzahlSektoren;
			Eckpunkte = new List<PointF>(n);
			for (int i = 0; i < n; i++)
			{
				double r = this.SegmentBreite;
				double alpha = 2 * Math.PI / TArena.AnzahlSektoren;
				double phi = i * alpha;
				double x = this.CenterX + Math.Cos(phi) * r;
				double y = this.CenterY + Math.Sin(phi) * r;
				PointF p = new PointF((float)x, (float)y);
				Eckpunkte.Add(p);
			}
		}

		/// <summary>
		/// Initialisert den Pfad für die Mitte in der Trapez-Darstellung
		/// </summary>
		private void initPath()
		{
			if (Eckpunkte == null)
				initEckpunkte();

			Path = new GraphicsPath();
			int n = Eckpunkte.Count;
			for (int i = 0; i < n - 1; i++)
			{
				PointF a = Eckpunkte[i];
				PointF b = Eckpunkte[i + 1];
				Path.AddLine(a, b);
			}
			Path.CloseFigure();
		}

		/// <summary>
		/// Zeichnet die Mitte in der Trapez-Darstellung
		/// </summary>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		private void drawCenterTrapez(Graphics g)
		{
			if (Eckpunkte == null)
				initEckpunkte();

			if (Path == null)
				initPath();

			_MyBrush.Color = Color.Black;
			g.FillPath(_MyBrush, Path);
		}

		/// <summary>
		/// Zeichnet die Mitte entsprechend der Einstellung
		/// </summary>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		protected void drawCenter(Graphics g)
		{
			if (Mode == EDisplayMode.Ring)
				drawCenterRing(g);
			else if (Mode == EDisplayMode.Trapez)
				drawCenterTrapez(g);
		}
		#endregion //Center

		/// <summary>
		/// Berechnet die X-Koordinate des Kreiszentrums
		/// </summary>
		/// <param name="pa">Das Panel, auf dem die Segmente dargestellt werden</param>
		/// <returns>Die X-Koordinate</returns>
		protected abstract float calcCenterX(Panel pa);
		/// <summary>
		/// Berechnet die Y-Koordinate des Kreiszentrums
		/// </summary>
		/// <param name="pa">Das Panel, auf dem die Segmente dargestellt werden</param>
		/// <returns>Die Y-Koordinate</returns>
		protected abstract float calcCenterY(Panel pa);
		/// <summary>
		/// Bestimmt die Breite eines Segments in Pixel
		/// </summary>
		/// <returns>Die Breite eines Segments</returns>
		protected abstract float calcBreite();

		/// <summary>
		/// Berechnet den Innenwinkel eines Segments
		/// </summary>
		/// <returns>Der Winkel Alpha in rad</returns>
		private double berechneAlpha()
		{
			//return (2 * Math.PI / 360) * (360 / TArena.AnzahlSektoren);
			return 2 * Math.PI / TArena.AnzahlSektoren;
		}

		#region Trapez-Eckpunkte
		/// <summary>
		/// Berechnet den ersten Eckpunkt des Trapez'
		/// </summary>
		/// <returns>A</returns>
		private PointF berechnePunktA()
		{
			double r = (PositionSchicht + 1) * SegmentBreite;
			double alpha = berechneAlpha();
			double phi = PositionSektor * alpha;
			double x = this.CenterX + Math.Cos(phi + Shift) * r;
			double y = this.CenterY + Math.Sin(phi + Shift) * r;
			return new PointF((float)x, (float)y);
		}

		/// <summary>
		/// Berechnet den zweiten Eckpunkt des Trapez'
		/// </summary>
		/// <returns>B</returns>
		private PointF berechnePunktB()
		{
			double r = (PositionSchicht + 1) * SegmentBreite;
			double alpha = berechneAlpha();
			double phi = (PositionSektor + 1) * alpha;
			double x = this.CenterX + Math.Cos(phi + Shift) * r;
			double y = this.CenterY + Math.Sin(phi + Shift) * r;
			return new PointF((float)x, (float)y);
		}

		/// <summary>
		/// Berechnet den dritten Eckpunkt des Trapez'
		/// </summary>
		/// <returns>C</returns>
		private PointF berechnePunktC()
		{
			double r = PositionSchicht * SegmentBreite;
			double alpha = berechneAlpha();
			double phi = (PositionSektor + 1) * alpha;
			double x = this.CenterX + Math.Cos(phi + Shift) * r;
			double y = this.CenterY + Math.Sin(phi + Shift) * r;
			return new PointF((float)x, (float)y);
		}

		/// <summary>
		/// Berechnet den vierten Eckpunkt des Trapez'
		/// </summary>
		/// <returns>D</returns>
		private PointF berechnePunktD()
		{
			double r = PositionSchicht * SegmentBreite;
			double alpha = berechneAlpha();
			double phi = PositionSektor * alpha;
			double x = this.CenterX + Math.Cos(phi + Shift) * r;
			double y = this.CenterY + Math.Sin(phi + Shift) * r;
			return new PointF((float)x, (float)y);
		}
		#endregion //Trapez-Eckpunkte

		/// <summary>
		/// Erzeugt einen Pfad zur Darstellung als Trapez
		/// </summary>
		/// <returns>Der gewünscte Pfad</returns>
		private GraphicsPath createPathTrapez()
		{
			GraphicsPath result = new GraphicsPath();

			PointF A = berechnePunktA();
			PointF B = berechnePunktB();
			PointF C = berechnePunktC();
			PointF D = berechnePunktD();

			result.AddLine(A, B);
			result.AddLine(B, C);
			result.AddLine(C, D);
			result.AddLine(D, A);

			return result;
		}

		/// <summary>
		/// Erzeugt einen Pfad zur Darstellung als Ring
		/// </summary>
		/// <returns>Der gewünscte Pfad</returns>
		private GraphicsPath createPathRing()
		{
			//Verschiedene Winkel berechnen
			float alpha = (float)berechneAlpha();
			float alphaDegree = (float)((alpha / (2 * Math.PI)) * 360);
			float phi = PositionSektor * alpha;
			float phi2 = (PositionSektor + 1) * alpha;
			float phiDegree = (float)((phi / (2 * Math.PI)) * 360);
			float phi2Degree = (float)((phi2 / (2 * Math.PI)) * 360);

			//Radien berechnen
			float rAussen = (float)((PositionSchicht + 1) * SegmentBreite);
			float rInnen = (float)(PositionSchicht * SegmentBreite);

			//Rechtecke berechnen
			RectangleF recAussen = new RectangleF(
				(float)(this.CenterX - rAussen),
				(float)(this.CenterY - rAussen),
				(float)(2 * rAussen),
				(float)(2 * rAussen));

			RectangleF recInnen = new RectangleF(
				(float)(this.CenterX - rInnen),
				(float)(this.CenterY - rInnen),
				(float)(2 * rInnen),
				(float)(2 * rInnen));

			//Pfad berechnen
			GraphicsPath result = new GraphicsPath();
			result.AddArc(recAussen, phiDegree + ShiftDegree, alphaDegree); //Bogen A-B

			//Punkt C berechnen
			double Cx = this.CenterX + Math.Cos(phi2 + Shift) * rInnen;
			double Cy = this.CenterY + Math.Sin(phi2 + Shift) * rInnen;
			PointF C = new PointF((float)Cx, (float)Cy);

			PointF B = result.GetLastPoint();
			result.AddLine(B, C); //B-C

			result.AddArc(recInnen, phi2Degree + ShiftDegree, -alphaDegree); //Bogen C-D

			result.CloseFigure(); //D-A

			return result;
		}

		/// <summary>
		/// Erzeugt einen Pfad entsprechend der Einstellungen
		/// </summary>
		/// <returns>Der entsprechende Pfad</returns>
		protected GraphicsPath createPath()
		{
			switch (Mode)
			{
				case EDisplayMode.Ring:
					return createPathRing();
				case EDisplayMode.Trapez:
					return createPathTrapez();
				default:
					return null;
			}
		}

		/// <summary>
		/// Zeichnet das Segment
		/// </summary>
		/// <param name="positionSektor">Der Sektor, in dem das zu zeichnende Segment liegt</param>
		/// <param name="positionSchicht">Die Schicht, in der das zu zeichnende Segment liegt</param>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="c">Die Farbe für das Segment</param>
		public void drawSegment(int positionSektor, int positionSchicht, Graphics g, Color c)
		{
			PositionSektor = positionSektor;
			PositionSchicht = positionSchicht;

			GraphicsPath path = createPath();
			_MyPen.Color = Color.Black;
			_MyBrush.Color = c;
			g.FillPath(_MyBrush, path);
			g.DrawPath(_MyPen, path);
		}

		/// <summary>
		/// Löscht das Segment
		/// </summary>
		/// <param name="positionSektor">Der Sektor, in dem das zu zeichnende Segment liegt</param>
		/// <param name="positionSchicht">Die Schicht, in der das zu zeichnende Segment liegt</param>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="c">Die Hintergrundfarbe des beinhaltenden Panels</param>
		public void deleteSegment(int positionSektor, int positionSchicht, Graphics g, Color c)
		{
			PositionSektor = positionSektor;
			PositionSchicht = positionSchicht;

			GraphicsPath path = createPath();
			_MyPen.Color = c;
			_MyBrush.Color = c;
			g.FillPath(_MyBrush, path);
			g.DrawPath(_MyPen, path);
		}

		/// <summary>
		/// Zeichnet den Rahmen des Segments
		/// </summary>
		/// <param name="positionSektor">Der Sektor, in dem das zu zeichnende Segment liegt</param>
		/// <param name="positionSchicht">Die Schicht, in der das zu zeichnende Segment liegt</param>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		public void drawRahmen(int positionSektor, int positionSchicht, Graphics g)
		{
			PositionSektor = positionSektor;
			PositionSchicht = positionSchicht;

			GraphicsPath path = createPath();
			_MyPen.Color = Color.Black;
			g.DrawPath(_MyPen, path);
		}

		/// <summary>
		/// Berechnet den Rahmen des Segments
		/// </summary>
		/// <param name="positionSektor">Der Sektor, in dem das zu berechnende Segment liegt</param>
		/// <param name="positionSchicht">Die Schicht, in der das zu berechnende Segment liegt</param>
		public GraphicsPath calcRahmen(int positionSektor, int positionSchicht)
		{
			PositionSektor = positionSektor;
			PositionSchicht = positionSchicht;

			GraphicsPath path = createPath();
			return path;
		}

		//Siehe https://de.wikipedia.org/wiki/Schnittpunkt
		/// <summary>
		/// Berechnet den Schnittpunkt zweier Geraden
		/// </summary>
		/// <param name="P1">Der erste Punkt der ersten Gerade</param>
		/// <param name="P2">Der zweite Punkt der ersten Gerade</param>
		/// <param name="Q1">Der erste Punkt der zweiten Gerade</param>
		/// <param name="Q2">Der zweite Punkt der zweiten Gerade</param>
		/// <returns>Der Schnittpunkt der beiden Geraden</returns>
		private PointF calcSchnittpunkt(PointF P1, PointF P2, PointF Q1, PointF Q2)
		{
			float a = (Q2.X - Q1.X) * (P2.X * P1.Y - P1.X * P2.Y) - (P2.X - P1.X) * (Q2.X * Q1.Y - Q1.X * Q2.Y);
			float b = (Q2.Y - Q1.Y) * (P2.X - P1.X) - (P2.Y - P1.Y) * (Q2.X - Q1.X);
			float c = (P1.Y - P2.Y) * (Q2.X * Q1.Y - Q1.X * Q2.Y) - (Q1.Y - Q2.Y) * (P2.X * P1.Y - P1.X * P2.Y);
			float d = (Q2.Y - Q1.Y) * (P2.X - P1.X) - (P2.Y - P1.Y) * (Q2.X - Q1.X);

			return new PointF(a / b, c / d);
		}

		#region Ringe löschen
		/// <summary>
		/// Bestimmt die Linien, die repariert werden müssen
		/// </summary>
		/// <param name="lines">Gibt die vollen Linien an, die gelöscht werden</param>
		/// <param name="positionSchicht">Die Position des gefallenen Teils</param>
		/// <returns>Liste mit den zu reparierenden Zeilen</returns>
		private List<int> findLinesToRepair(bool[] lines, int positionSchicht)
		{
			List<int> result = new List<int>(0);

			//Hilfs-Array erzeugen
			int n = lines.Length;
			bool[] h = new bool[n + 2];
			h[0] = false;
			h[n + 1] = false;
			for (int i = 0; i < n; i++)
				h[i + 1] = lines[i];

			for (int i = 1; i <= n; i++)
				if (h[i])
				{
					//Linie darunter prüfen
					if (!h[i + 1] && positionSchicht - i > 0)
						result.Add(positionSchicht - i);

					//Linie darüber prüfen
					if (!h[i - 1] && positionSchicht - i + 2 > 0)
						result.Add(positionSchicht - i + 2);
				}

			return result;
		} //Ende Methode findLinesToRepair

		/// <summary>
		/// Animation: Die vollen Ring-Zeilen werden gelöscht
		/// </summary>
		/// <param name="positionSchicht">Die oberste Schicht, wo das Teil liegt</param>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="backColor">Hintergrundfarbe zum Löschen</param>
		/// <param name="lines">Gibt an, welche Zeilen voll sind</param>
		/// <param name="arena">Verweis auf die Arena (nötig für Reparatur)</param>
		private void animateClearRings(int positionSchicht, Graphics g, Color backColor, bool[] lines, TVisualArena arena)
		{
			AnimationIsRunning = true;

			//Pfade zum Reparieren bestimmen
			List<int> repairLines = findLinesToRepair(lines, positionSchicht);
			List<GraphicsPath> repairPaths = new List<GraphicsPath>(1);
			int m = repairLines.Count;
			for (int l = 0; l < m; l++)
			{
				arena.getRepairPaths(repairLines[l], repairPaths);
			}
			Pen myPenRepair = new Pen(Color.Black);

			int n = lines.Length;

			//Radien berechnen
			float[] rInnen = new float[n];
			for (int i = 0; i < n; i++)
			{
				int schicht = positionSchicht - i;
				rInnen[i] = (float)(schicht * this.SegmentBreite);
			}

			float[] rAussen = new float[n];
			for (int i = 0; i < n; i++)
			{
				int schicht = positionSchicht - i;
				rAussen[i] = (float)((schicht + 1) * this.SegmentBreite);
			}

			//Rechtecke berechnen
			RectangleF[] recInnen = new RectangleF[n];
			for (int i = 0; i < n; i++)
			{
				float r = rInnen[i];
				recInnen[i] = new RectangleF(
					this.CenterX - r,
					this.CenterY - r,
					2 * r,
					2 * r);
			}
			RectangleF[] recAussen = new RectangleF[n];
			for (int i = 0; i < n; i++)
			{
				float r = rAussen[i];
				recAussen[i] = new RectangleF(
					this.CenterX - r,
					this.CenterY - r,
					2 * r,
					2 * r);
			}

			for (int winkel = 1; winkel <= 360; winkel++)
			{
				for (int k = 0; k < n; k++)
					if (lines[k])
					{
						//Pfad berechnen
						GraphicsPath path = new GraphicsPath();
						path.AddArc(recAussen[k], 0 + ShiftDegree, winkel); //Bogen A-B

						//Punkt C berechnen
						double winkel2 = winkel * (Math.PI / 180);
						double Cx = this.CenterX + Math.Cos(winkel2 + Shift) * rInnen[k];
						double Cy = this.CenterY + Math.Sin(winkel2 + Shift) * rInnen[k];
						PointF C = new PointF((float)Cx, (float)Cy);

						PointF B = path.GetLastPoint();
						path.AddLine(B, C); //B-C

						path.AddArc(recInnen[k], winkel + ShiftDegree, -winkel); //Bogen C-D

						path.CloseFigure(); //D-A

						//Farbe jedes Mal neu setzen (nötig, wenn das Fenster zwischendurch minimiert wird)
						_MyPen.Color = backColor;
						_MyBrush.Color = backColor;
						g.FillPath(_MyBrush, path);
						g.DrawPath(_MyPen, path);
					}

				//Reparieren
				int anz = repairPaths.Count;
				for (int loop = 0; loop < anz; loop++)
				{
					GraphicsPath path = repairPaths[loop];
					g.DrawPath(myPenRepair, path);
				}

				//kurz warten
				foMain.wait(10);

				while (arena.myArena.isPause)
				{
					Application.DoEvents();

					if (!AnimationIsRunning)
						return;
				}

				if (!AnimationIsRunning)
					return;
			}

			AnimationIsRunning = false;
		} //Ende Methode clearRings

		/// <summary>
		/// Animation: Die vollen Trapez-Zeilen werden gelöscht
		/// </summary>
		/// <param name="positionSchicht">Die oberste Schicht, wo das Teil liegt</param>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="backColor">Hintergrundfarbe zum Löschen</param>
		/// <param name="lines">Gibt an, welche Zeilen voll sind</param>
		/// <param name="arena">Verweis auf die Arena (nötig für Reparatur)</param>
		private void animateClearRingsTrapez(int positionSchicht, Graphics g, Color backColor, bool[] lines, TVisualArena arena)
		{
			AnimationIsRunning = true;

			this._TrapezData.positionSchicht = positionSchicht;
			this._TrapezData.backColor = backColor;
			this._TrapezData.lines = lines;
			this._TrapezData.isActive = true;

			//Pfade zum Reparieren bestimmen
			List<int> repairLines = findLinesToRepair(lines, positionSchicht);
			List<GraphicsPath> repairPaths = new List<GraphicsPath>(1);
			int m = repairLines.Count;
			for (int l = 0; l < m; l++)
			{
				arena.getRepairPaths(repairLines[l], repairPaths);
			}
			Pen myPenRepair = new Pen(Color.Black);

			PointF Center = new PointF(this.CenterX, this.CenterY);
			double r = SegmentBreite;
			float alpha = (float)berechneAlpha();
			float degree = 1 * (float)Math.PI / 180;
			int n = lines.Length;

			//Für alle Sektoren
			for (int i = 0; i < TArena.AnzahlSektoren; i++)
			{
				this._TrapezData.CurrentSektor = i;

				float angle = i * alpha;
				float end = angle + alpha;

				//Für Winkelstücke kleiner Alpha
				while (angle < end)
				{
					for (int k = 0; k < n; k++)
					{
						if (lines[k])
						{
							PositionSektor = i;
							PositionSchicht = positionSchicht - k;

							//Punkte berechnen
							PointF A = berechnePunktA();
							PointF B = berechnePunktB();
							PointF C = berechnePunktC();
							PointF D = berechnePunktD();

							//Punkt auf Kreis berechnen
							double x = this.CenterX + Math.Cos(angle + Shift) * r;
							double y = this.CenterY + Math.Sin(angle + Shift) * r;
							PointF pCircle = new PointF((float)x, (float)y);

							//Schnittpunkte berechnen
							PointF S1 = calcSchnittpunkt(Center, pCircle, A, B);
							PointF S2 = calcSchnittpunkt(Center, pCircle, C, D);

							//Pfad erstellen
							GraphicsPath path = new GraphicsPath();
							path.AddLine(A, S1);
							path.AddLine(S1, S2);
							path.AddLine(S2, D);
							path.AddLine(D, A);

							//UNDONE: Man müsste auch die Trapeze neu löschen
							//Farbe jedes Mal neu setzen (nötig, wenn das Fenster zwischendurch minimiert wird)
							_MyPen.Color = backColor;
							_MyBrush.Color = backColor;

							//Pfad löschen
							g.FillPath(_MyBrush, path);
							g.DrawPath(_MyPen, path);

							//Reparieren
							int anz = repairPaths.Count;
							for (int loop = 0; loop < anz; loop++)
							{
								GraphicsPath pathRepair = repairPaths[loop];
								g.DrawPath(myPenRepair, pathRepair);
							}

							//kurz warten
							foMain.wait(10);

							while (arena.myArena.isPause)
							{
								Application.DoEvents();

								if (!AnimationIsRunning)
									return;
							}

							if (!AnimationIsRunning)
								return;

							//Nächster Winkel
							angle += degree;
						}
					}
				}

				//Den letzten Rest löschen (es entstehen sonst unschöne Ränder)
				for (int k = 0; k < n; k++)
					if (lines[k])
					{
						PositionSchicht = positionSchicht - k;
						deleteSegment(PositionSektor, PositionSchicht, g, backColor);
					}
			}

			AnimationIsRunning = false;
			this._TrapezData.isActive = false;
		} //Ende Methode animateClearRingsTrapez

		/// <summary>
		/// Animation: Die vollen Zeilen werden gelöscht
		/// </summary>
		/// <param name="positionSchicht">Die oberste Schicht, wo das Teil liegt</param>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="backColor">Hintergrundfarbe zum Löschen</param>
		/// <param name="lines">Gibt an, welche Zeilen voll sind</param>
		/// <param name="arena">Verweis auf die Arena (nötig für Reparatur)</param>
		public void animateClearLines(int positionSchicht, Graphics g, Color backColor, bool[] lines, TVisualArena arena)
		{
			if (Mode == EDisplayMode.Ring)
				animateClearRings(positionSchicht, g, backColor, lines, arena);
			else if (Mode == EDisplayMode.Trapez)
				animateClearRingsTrapez(positionSchicht, g, backColor, lines, arena);
		}
		#endregion //Ringe löschen

		#region Ringe blinken
		/// <summary>
		/// So viele Male blinken die vollen Ringe, bevor sie gelöscht werden
		/// </summary>
		private const int anzBlinken = 3;

		/// <summary>
		/// Der Spieler hat einen Tetris gemacht. Die vollen Ringe blinken, bevor sie gelöscht werden
		/// </summary>
		/// <param name="positionSchicht">Die oberste Schicht, wo das Teil liegt</param>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="backColor">Hintergrundfarbe zum Löschen</param>
		/// <param name="arena">Verweis auf die Arena</param>
		private void blinkRings(int positionSchicht, Graphics g, Color backColor, TVisualArena arena)
		{
			AnimationIsRunning = true;

			int n = TTeil.MaxHoehe;

			//Radien berechnen
			float rInnen = (float)((positionSchicht - n + 1) * this.SegmentBreite);
			float rAussen = (float)((positionSchicht + 1) * this.SegmentBreite);

			//Rechtecke berechnen
			float r = rInnen;
			RectangleF recInnen = new RectangleF(
				this.CenterX - r,
				this.CenterY - r,
				2 * r,
				2 * r);

			r = rAussen;
			RectangleF recAussen = new RectangleF(
				this.CenterX - r,
				this.CenterY - r,
				2 * r,
				2 * r);

			//Pfad berechnen
			int winkel = 360;
			GraphicsPath path = new GraphicsPath();
			path.AddArc(recAussen, 0, winkel); //Bogen A-B

			//Punkt C berechnen
			double winkel2 = winkel * (Math.PI / 180);
			double Cx = this.CenterX + Math.Cos(winkel2) * rInnen;
			double Cy = this.CenterY + Math.Sin(winkel2) * rInnen;
			PointF C = new PointF((float)Cx, (float)Cy);

			PointF B = path.GetLastPoint();
			path.AddLine(B, C); //B-C

			path.AddArc(recInnen, winkel, -winkel); //Bogen C-D

			path.CloseFigure(); //D-A

			for (int i = 1; i <= anzBlinken; i++)
			{
				//Die Farbe muss jedes Mal neu gesetzt werden, weil sie sonst beim Update verfälscht wird
				_MyPen.Color = backColor;
				_MyBrush.Color = backColor;
				g.FillPath(_MyBrush, path);
				g.DrawPath(_MyPen, path);

				foMain.wait(500);

				while (arena.myArena.isPause)
				{
					Application.DoEvents();

					if (!AnimationIsRunning)
						return;
				}

				if (!AnimationIsRunning)
					return;

				arena.update();
				foMain.wait(500);

				while (arena.myArena.isPause)
				{
					Application.DoEvents();

					if (!AnimationIsRunning)
						return;
				}

				if (!AnimationIsRunning)
					return;
			}

			AnimationIsRunning = false;
		} //Ende Methode blinkRings

		/// <summary>
		/// Der Spieler hat einen Tetris gemacht. Die vollen Trapeze blinken, bevor sie gelöscht werden
		/// </summary>
		/// <param name="positionSchicht">Die oberste Schicht, wo das Teil liegt</param>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="backColor">Hintergrundfarbe zum Löschen</param>
		/// <param name="arena">Verweis auf die Arena</param>
		private void blinkTrapez(int positionSchicht, Graphics g, Color backColor, TVisualArena arena)
		{
			AnimationIsRunning = true;

			GraphicsPath path = new GraphicsPath();

			//Pfad innen
			int schichtInnen = positionSchicht - TTeil.MaxHoehe + 1;
			int n = TArena.AnzahlSektoren;
			double alpha = 2 * Math.PI / TArena.AnzahlSektoren;
			List<PointF> EckpunkteInnen = new List<PointF>(n);
			double rInnen = schichtInnen * this.SegmentBreite;

			//Eckpunkte innen
			for (int i = 0; i < n; i++)
			{
				double phi = i * alpha;
				double x = this.CenterX + Math.Cos(phi) * rInnen;
				double y = this.CenterY + Math.Sin(phi) * rInnen;
				PointF p = new PointF((float)x, (float)y);
				EckpunkteInnen.Add(p);
			}

			//Pfad innen
			for (int i = 0; i < n - 1; i++)
			{
				PointF a = EckpunkteInnen[i];
				PointF b = EckpunkteInnen[i + 1];
				path.AddLine(a, b);
			}
			path.CloseFigure();

			//Pfad aussen
			List<PointF> EckpunkteAussen = new List<PointF>(n);
			double rAussen = (positionSchicht + 1) * this.SegmentBreite;
			for (int i = n; i >= 0; i--)
			{
				double phi = i * alpha;
				double x = this.CenterX + Math.Cos(phi) * rAussen;
				double y = this.CenterY + Math.Sin(phi) * rAussen;
				PointF p = new PointF((float)x, (float)y);
				EckpunkteAussen.Add(p);
			}

			//Pfad aussen
			PointF last = path.GetLastPoint();
			path.AddLine(last, EckpunkteAussen[0]);
			for (int i = 0; i < EckpunkteAussen.Count - 1; i++)
			{
				PointF a = EckpunkteAussen[i];
				PointF b = EckpunkteAussen[i + 1];
				path.AddLine(a, b);
			}

			path.CloseFigure();

			//Blinken
			for (int i = 1; i <= anzBlinken; i++)
			{
				//Die Farbe muss jedes Mal neu gesetzt werden, weil sie sonst beim Update verfälscht wird
				_MyBrush.Color = backColor;
				_MyPen.Color = backColor;
				g.FillPath(_MyBrush, path);
				g.DrawPath(_MyPen, path);
				foMain.wait(500);

				while (arena.myArena.isPause)
				{
					Application.DoEvents();

					if (!AnimationIsRunning)
						return;
				}

				if (!AnimationIsRunning)
					return;

				arena.update();
				foMain.wait(500);

				while (arena.myArena.isPause)
				{
					Application.DoEvents();

					if (!AnimationIsRunning)
						return;
				}

				if (!AnimationIsRunning)
					return;
			}

			AnimationIsRunning = false;
		} //Ende Methode blinkTrapez

		/// <summary>
		/// Der Spieler hat einen Tetris gemacht. Die vollen Ringe/Trapeze blinken, bevor sie gelöscht werden
		/// </summary>
		/// <param name="positionSchicht">Die oberste Schicht, wo das Teil liegt</param>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="backColor">Hintergrundfarbe zum Löschen</param>
		/// <param name="arena">Verweis auf die Arena</param>
		public void blink(int positionSchicht, Graphics g, Color backColor, TVisualArena arena)
		{
			if (Mode == EDisplayMode.Ring)
				blinkRings(positionSchicht, g, backColor, arena);
			else if (Mode == EDisplayMode.Trapez)
				blinkTrapez(positionSchicht, g, backColor, arena);
		}
		#endregion //Ringe blinken

		/// <summary>
		/// Stopt eine laufende Animation
		/// </summary>
		public static void stopAnimation()
		{
			AnimationIsRunning = false;
		}

		/// <summary>
		/// Registriert einen Verweis auf das Hauptfenster
		/// </summary>
		/// <param name="fo">Das Fenster</param>
		public static void registerForm(foMain fo)
		{
			_MyForm = fo;
		}

		/// <summary>
		/// Die Trapez-Ringe werden erneut gelöscht
		/// </summary>
		/// <remarks>Dies wird nötig, wenn das Fenster während der Lösch-Animation minimiert und dann wieder angezeigt wird</remarks>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		public void reclearTrapez(Graphics g)
		{
			if (this._TrapezData.isActive)
			{
				for (int i = 0; i < this._TrapezData.CurrentSektor; i++)
					for (int k = 0; k < this._TrapezData.lines.Length; k++)
						if (this._TrapezData.lines[k])
						{
							int posSchicht = this._TrapezData.positionSchicht - k;
							int sektor = i;
							this.deleteSegment(sektor, posSchicht, g, this._TrapezData.backColor);
						}
			}
		}
	} //Ende Klasse TSegmentBase
} //Ende namespace Ringtetris
