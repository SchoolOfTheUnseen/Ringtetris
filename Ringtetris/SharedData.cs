using System;
using System.Drawing;

namespace Ringtetris
{
	/// <summary>
	/// Delegate für den Event in der Klasse TSharedData
	/// </summary>
	public delegate void DataChanged();

	/// <summary>
	/// Speichert Daten, die an verschiedenen Stellen benötigt werden
	/// </summary>
	public class TSharedData
	{
		/// <summary>
		/// Das fallende Teil
		/// </summary>
		public TTeil FallingTeil;
		/// <summary>
		/// Das nächste fallende Teil
		/// </summary>
		public TTeil NextFallingTeil;

		/// <summary>
		/// Die Farbe des fallenden Teils
		/// </summary>
		public Color FallingColor;
		/// <summary>
		/// Die Farbe des nächsten fallenden Teils
		/// </summary>
		public Color NextFallingColor;

		/// <summary>
		/// Random-Objekt für Zufall
		/// </summary>
		private Random _MyRandom;

		/// <summary>
		/// Dieser Event wird aufgerufen, wenn die Daten änder
		/// </summary>
		public event DataChanged OnDataChanged;

		/// <summary>
		/// Standard-Konstruktor
		/// </summary>
		public TSharedData()
		{
			this._MyRandom = new Random();

			this.FallingTeil = new TTeil();
			this.FallingTeil.randomize(this._MyRandom);
			this.FallingTeil.move(0, TArena.AnzahlSchichten - 1);

			this.NextFallingTeil = new TTeil();
			this.NextFallingTeil.randomize(this._MyRandom);
			this.NextFallingTeil.move(0, TTeil.MaxHoehe);

			this.FallingColor = TTeil.createRandomColor(this._MyRandom);
			this.NextFallingColor = TTeil.createRandomColor(this._MyRandom);
		}

		/// <summary>
		/// Das fallende Teil wird zufällig zu einem neuen 
		/// Teil und an die Startposition gesetzt
		/// </summary>
		public void startNewFallingTeil()
		{
			this.NextFallingTeil.copyTo(this.FallingTeil);
			this.NextFallingTeil.Clear();
			this.NextFallingTeil.randomize(this._MyRandom);
			this.NextFallingTeil.move(0, TTeil.MaxHoehe);
			this.FallingTeil.move(0, TArena.AnzahlSchichten - 1);
			this.FallingColor = this.NextFallingColor;
			this.NextFallingColor = TTeil.createRandomColor(this._MyRandom);

			if (this.OnDataChanged != null)
				this.OnDataChanged();
		}

		/// <summary>
		/// Setzt alles zurück
		/// </summary>
		public void reset()
		{
			this.NextFallingTeil = new TTeil();
			this.NextFallingTeil.randomize(this._MyRandom);
			this.NextFallingTeil.move(0, TTeil.MaxHoehe);

			this.FallingTeil = new TTeil();
			this.FallingTeil.randomize(this._MyRandom);
			this.FallingTeil.move(0, TArena.AnzahlSchichten - 1);

			this.NextFallingColor = TTeil.createRandomColor(this._MyRandom);
			this.FallingColor = TTeil.createRandomColor(this._MyRandom);
		}
	} //Ende Klasse TSharedData
} //Ende namespace Ringtetris
