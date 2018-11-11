//copyright Quinn Griffin 2017

using UnityEngine;
using PuzzleGraph;

namespace KamiExample
{
    /// <summary>
    /// This monobehavior serves as a rough interface in the Unity inspector.
    /// It lets me define a kami puzzle (through an ugly interface), generate the puzzle object, then generate the state graph object.
    /// </summary>

    [ExecuteInEditMode]
    public class KamiPuzzleBuilder : MonoBehaviour
    {

        //private fields
        private KamiPuzzle mPuzzle;
        private StateGraph mStateGraph;
        private const int maskBitCount = 32;

        //fields editable in inspector.
        [SerializeField, Range(2,32)] //A reasonable range for number of colors. A typical puzzle will have 3 or 4.
        int numberOfColors = 3;
        [SerializeField]
        ColorArea[] startingAreas = null; //Here, the areaMask of the startingAreas corresponds to their adjacencyMask.

        /// <summary>
        /// Construct a KamiPuzzle instance from the parameters set in this class.
        /// This is called from a button in the inspector window.
        /// </summary>
        public void GeneratePuzzle()
        {
            if (startingAreas.Length > maskBitCount)
            {
                Debug.Log($"Array of length {startingAreas.Length} exceeds maskBitCount of {maskBitCount}.");
            }
            else
            {
                mPuzzle = new KamiPuzzle(startingAreas, numberOfColors);
            }
        }


        /// <summary>
        /// Construct a state graph from the puzzle instance.
        /// This is called from a button in the inspector window.
        /// </summary>
        public void GenerateStateGraph()
        {
            if (mPuzzle != null)
            {
                mStateGraph = new StateGraph(mPuzzle);
            }
            else
            {
                Debug.Log("Puzzle model is null. Did you forget to generate a puzzle?");
            }
        }
    }
}
