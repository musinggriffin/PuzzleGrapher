//copyright Quinn Griffin 2017

using PuzzleGraph;

namespace KamiExample
{    
    /// <summary>
    /// Implements IPuzzleState to store a single Kami puzzle.
    /// </summary>
    public class KamiState : IPuzzleState
    {
        //public field
        public ColorArea[] areas; //area of the existing super-areas. 

        //private fields
        private int mHashCode; //precomputed hashcode generated from the states of individual puzzle variables.

        public KamiState(int originalAreaCount, int numberOfColors, ColorArea[] colorAreas)
        {
            areas = colorAreas;

            //Calculate the puzzle state hashcode.

            //Step 1: Create an int[] to describe the current colors of each initial area.
            int[] state = new int[originalAreaCount];
            for (int i = 0; i < areas.Length; i++) //foreach color super-area...
            {
                for (int j = 0; j < originalAreaCount; j++) //foreach initial area in the puzzle...
                {
                    if (((areas[i].areaMask >> j) & 1) == 1) //check if the super-area encompasses that initial area...
                    {
                        state[j] = areas[i].color; //and if it does, mark the initial area with the super-area's color.
                    }
                }
            }

            //Step 2: generate and store the hashcode.
            // The process for generating a puzzle state hashcode is essentially the same as generating an index for an element in a multidimensional array.
            int hashCode = 0;
            for (int i = 0; i < state.Length; i++)
            {
                hashCode *= numberOfColors; //the length of each "dimension" is equal to the number of colors.
                hashCode += state[i];
            }
            mHashCode = hashCode;
        }

        public override int GetHashCode()
        {
            return mHashCode;
        }

        public override bool Equals(object obj)
        {
            KamiState kamiState = (KamiState)obj;
            if (kamiState != null)
            {
                return mHashCode == kamiState.GetHashCode();
            }
            else
            {
                return false;
            }
        }
    }
}
