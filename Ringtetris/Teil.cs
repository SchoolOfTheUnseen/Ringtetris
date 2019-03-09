using System;
using System.Drawing;

namespace Ringtetris
{
	/// <summary>
	/// Ein Tetristeil, das herunterfällt
	/// </summary>
	public class TTeil
	{
		/// <summary>
		/// Die maximale Breite eines Teils in Segmenten
		/// </summary>
		public const int MaxBreite = 4;
		/// <summary>
		/// Die maximale Höhe eines Teils in Segmenten
		/// </summary>
		public const int MaxHoehe = 4;

		/// <summary>
		/// Der Sektor, in dem sich das Teil befindet (bzw. der linke Rand des Teils)
		/// </summary>
		public int PositionSektor;
		/// <summary>
		/// Die Schicht, in der sich das Teil befindet (bzw. der obere Rand des Teils)
		/// </summary>
		public int PositionSchicht;

		/// <summary>
		/// Speichert die Segmente des Teils
		/// </summary>
		/// <remarks><c>true</c> bedeutet, dass da ein Segment ist; 
		/// <c>false</c> bedeutet, dass der Platz leer ist</remarks>
		public bool[,] SegmentArray;

		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		public TTeil()
		{
			this.SegmentArray = new bool[MaxBreite, MaxHoehe];
			Clear();
		}

		/// <summary>
		/// Löscht alle Segmente in diesem Teil
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < TTeil.MaxBreite; i++)
				for (int k = 0; k < TTeil.MaxHoehe; k++)
					this.SegmentArray[i, k] = false;
		}

		/// <summary>
		/// Kopiert dieses Objekt in das andere
		/// </summary>
		/// <param name="other">Das andere Objekt, in das kopiert wird</param>
		public void copyTo(TTeil other)
		{
			for (int i = 0; i < TTeil.MaxBreite; i++)
				for (int k = 0; k < TTeil.MaxHoehe; k++)
					other.SegmentArray[i, k] = this.SegmentArray[i, k];
		}

		/// <summary>
		/// Prüft, ob an der angegebenen Stelle ein Segment ist
		/// </summary>
		/// <param name="breite">Position der Breite</param>
		/// <param name="hoehe">Position der Höhe</param>
		/// <returns><c>true</c>, falls dieses Teil an dieser Stelle ein Segment hat; 
		/// <c>false</c>, wenn die Stelle leer ist.</returns>
		public bool hasSegment(int breite, int hoehe)
		{
			return this.SegmentArray[breite, hoehe];
		}

		/// <summary>
		/// Verschiebt das Teil an eine neue Position
		/// </summary>
		/// <param name="positionSektor">Der neue Sektor</param>
		/// <param name="positionSchicht">Die neue Schicht</param>
		public void move(int positionSektor, int positionSchicht)
		{
			this.PositionSektor = positionSektor % TArena.AnzahlSektoren;
			if (this.PositionSektor < 0)
				this.PositionSektor += TArena.AnzahlSektoren;
			this.PositionSchicht = positionSchicht;
		}

		/// <summary>
		/// Füllt dieses Teil mit einem zufällig gewählten Teil
		/// </summary>
		/// <param name="r">Random-Objekt</param>
		public void randomize(Random r)
		{
			int nr = r.Next(6);
			//int nr = r.Next(4);
			//int nr = 3;
			switch (nr)
			{
				case 0:
					TTeilSingle.fillTeil(this);
					break;
				case 1:
					TTeil2Segmente.fillTeil(this);
					break;
				case 2:
					TTeil3Segmente.fillTeil(this);
					break;
				case 3:
					TTeil4erTurm.fillTeil(this);
					break;
				case 4:
					TTeilEcke1.fillTeil(this);
					break;
				case 5:
					TTeilEcke2.fillTeil(this);
					break;
			}
		}

		/// <summary>
		/// Erzeugt eine zufällige Farbe
		/// </summary>
		/// <param name="r">Random-Objekt für Zufallszahl</param>
		/// <returns>Eine zufällige Farbe</returns>
		public static Color createRandomColor(Random r)
		{
			int red = r.Next(256);
			int green = r.Next(256);
			int blue = r.Next(256);
			Color result = Color.FromArgb(red, green, blue);
			return result;
		}
	} //Ende Klasse TTeil

	/// <summary>
	/// Graphische Darstellung eines Teils
	/// </summary>
	public static class TVisualTeil
	{
		/// <summary>
		/// Zeichnet das Teil mit allen seinen Segmenten
		/// </summary>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="t">Das zu zeichnende Teil</param>
		/// <param name="c">Die Farbe des Teils</param>
		/// <param name="segment">Das Segment-Objekt, mit dem gezeichnet wird</param>
		public static void drawTeil(Graphics g, TTeil t, Color c, TSegmentBase segment)
		{
			for (int i = 0; i < TTeil.MaxBreite; i++)
				for (int k = 0; k < TTeil.MaxHoehe; k++)
					if (t.hasSegment(i, k))
					{
						int positionSektor = (t.PositionSektor + i) % TArena.AnzahlSektoren;
						int positionSchicht = t.PositionSchicht - k;
						segment.drawSegment(positionSektor, positionSchicht, g, c);
					}
		}

		/// <summary>
		/// Löscht das Teil und alle seine Segmente
		/// </summary>
		/// <param name="g">Graphics-Objekt zum Zeichnen</param>
		/// <param name="t">Das zu zeichnende Teil</param>
		/// <param name="backColor">Hintergrundfarbe des Panels</param>
		/// <param name="arena">Verweis auf die Arena</param>
		/// <param name="segment">Verweis auf das Segment zum Zeichnen</param>
		public static void deleteTeil(Graphics g, TTeil t, Color backColor, TVisualArena arena, TSegmentBase segment)
		{
			for (int i = 0; i < TTeil.MaxBreite; i++)
				for (int k = 0; k < TTeil.MaxHoehe; k++)
					if (t.hasSegment(i, k))
					{
						int positionSektor = (t.PositionSektor + i) % TArena.AnzahlSektoren;
						int positionSchicht = t.PositionSchicht - k;
						segment.deleteSegment(positionSektor, positionSchicht, g, backColor);
						arena.repairNachbarn(positionSektor, positionSchicht, g);
					}
		}
	} //Ende Klasse TVisualTeil

	/// <summary>
	/// Dieses Teil besteht aus einem einzelnen Segment
	/// </summary>
	public static class TTeilSingle
	{
		/// <summary>
		/// Füllt das Teil t mit den entsprechenden Segmenten
		/// </summary>
		/// <param name="t">Das zu füllende Teil</param>
		public static void fillTeil(TTeil t)
		{
			t.SegmentArray[0, 0] = true;
		}
	} //Ende Klasse TTeilSingle

	/// <summary>
	/// Dieses Teil besteht aus zwei Segmenten
	/// </summary>
	public static class TTeil2Segmente
	{
		/// <summary>
		/// Füllt das Teil t mit den entsprechenden Segmenten
		/// </summary>
		/// <param name="t">Das zu füllende Teil</param>
		public static void fillTeil(TTeil t)
		{
			t.SegmentArray[0, 0] = true;
			t.SegmentArray[1, 0] = true;
		}
	} //Ende Klasse TTeil2Segmente

	/// <summary>
	/// Dieses Teil besteht aus drei Segmenten
	/// </summary>
	public static class TTeil3Segmente
	{
		/// <summary>
		/// Füllt das Teil t mit den entsprechenden Segmenten
		/// </summary>
		/// <param name="t">Das zu füllende Teil</param>
		public static void fillTeil(TTeil t)
		{
			t.SegmentArray[0, 0] = true;
			t.SegmentArray[1, 0] = true;
			t.SegmentArray[2, 0] = true;
		}
	} //Ende Klasse TTeil3Segmente

	/// <summary>
	/// Dieses Teil besteht aus vier Segmenten, die übereinander liegen
	/// </summary>
	public static class TTeil4erTurm
	{
		/// <summary>
		/// Füllt das Teil t mit den entsprechenden Segmenten
		/// </summary>
		/// <param name="t">Das zu füllende Teil</param>
		public static void fillTeil(TTeil t)
		{
			t.SegmentArray[0, 0] = true;
			t.SegmentArray[0, 1] = true;
			t.SegmentArray[0, 2] = true;
			t.SegmentArray[0, 3] = true;
		}
	} //Ende Klasse TTeil4erTurm

	/// <summary>
	/// Dieses Teil besteht aus drei eckig angeordneten Segmenten
	/// </summary>
	public static class TTeilEcke1
	{
		/// <summary>
		/// Füllt das Teil t mit den entsprechenden Segmenten
		/// </summary>
		/// <param name="t">Das zu füllende Teil</param>
		public static void fillTeil(TTeil t)
		{
			t.SegmentArray[0, 0] = true;
			t.SegmentArray[1, 0] = true;
			t.SegmentArray[0, 1] = true;
		}
	} //Ende Klasse TTeilEcke1

	/// <summary>
	/// Dieses Teil besteht aus drei eckig angeordneten Segmenten
	/// </summary>
	public static class TTeilEcke2
	{
		/// <summary>
		/// Füllt das Teil t mit den entsprechenden Segmenten
		/// </summary>
		/// <param name="t">Das zu füllende Teil</param>
		public static void fillTeil(TTeil t)
		{
			t.SegmentArray[0, 0] = true;
			t.SegmentArray[1, 0] = true;
			t.SegmentArray[1, 1] = true;
		}
	} //Ende Klasse TTeilEcke2
} //Ende namespace Ringtetris
