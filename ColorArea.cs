//copyright Quinn Griffin 2017

namespace KamiExample
{
    /// <summary>
    /// ColorArea is a useful struct for transfering information about color areas and super areas when handling logic for the Kami puzzle.
    /// In most situations the areaMask field refers to the areas included in the area,
    /// but to initialize the puzzle I do use it to store the adjacency mask of the starting areas,
    /// which I then pass from the KamiPuzzleBuilder to the KamiPuzzle constructor.
    /// </summary>
    [System.Serializable]
    public struct ColorArea
    {
        public uint areaMask; //A bitmask for describing a set of subareas or adjacent areas.
        public int color; //Kami puzzles involve a small handful of color states, I believe never more than six.

        public ColorArea(uint mask, int paintedColor)
        {
            areaMask = mask;
            color = paintedColor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            ColorArea otherArea = (ColorArea)obj;
            return this.color == otherArea.color && this.areaMask == otherArea.areaMask;
        }
    }
}


