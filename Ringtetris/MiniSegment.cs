using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Ringtetris
{
	/// <summary>
	/// Graphische Darstellung eines Segments
	/// </summary>
	public class TVisualMiniSegment : TSegmentBase
	{
		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		/// <param name="pa">Das Panel, auf dem die Segmente dargestellt werden</param>
		public TVisualMiniSegment(Panel pa) :
			base(pa)
		{
		}

		/// <summary>
		/// Berechnet die X-Koordinate des Kreiszentrums
		/// </summary>
		/// <param name="pa">Das Panel, auf dem die Segmente dargestellt werden</param>
		/// <returns>Die X-Koordinate</returns>
		protected override float calcCenterX(Panel pa)
		{
			return 0;
		}
		/// <summary>
		/// Berechnet die Y-Koordinate des Kreiszentrums
		/// </summary>
		/// <param name="pa">Das Panel, auf dem die Segmente dargestellt werden</param>
		/// <returns>Die Y-Koordinate</returns>
		protected override float calcCenterY(Panel pa)
		{
			return pa.ClientSize.Height / 2;
		}
		/// <summary>
		/// Bestimmt die Breite eines Segments in Pixel
		/// </summary>
		/// <returns>Die Breite eines Segments</returns>
		protected override float calcBreite()
		{
			return 10;
		}
	} //Ende Klasse TVisualMiniSegment
} //Ende namespace Ringtetris
