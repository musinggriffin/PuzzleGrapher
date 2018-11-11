//copyright Quinn Griffin 2017

using System.Collections.Generic;
using UnityEngine;
using PuzzleGraph;

namespace KamiExample
{
    /// <summary>
    /// This class contains the information for a given kami puzzle: Its initial state and area adjacency rules.
    /// Its job is to provide information about state connectivity to the state graph.
    /// </summary>
    public class KamiPuzzle : IPuzzle
    {

        //private fields.
        private int mNumberOfColors; //the number of distinct colors featured in the puzzle. Typically 3 or 4.
        private int mOriginalAreaCount;
        private KamiState mInitialState;
        private Dictionary<uint, uint> mAdjacencyMasks; //for memoizing adjacency masks so that the don't need to be recomputed. Key is an inclusion mask, item is its adjacency mask.

        public KamiPuzzle(ColorArea[] startingAreas, int numberOfColors)
        { //Here, the areaMasks of base area data correspond to their adjacency.
            mNumberOfColors = numberOfColors;
            mOriginalAreaCount = startingAreas.Length;
            mAdjacencyMasks = new Dictionary<uint, uint>();

            //Seed the adjacency dictionary with the info of the starting areas.
            ColorArea[] initialAreas = new ColorArea[startingAreas.Length];
            for (int i = 0; i < startingAreas.Length; i++)
            {
                initialAreas[i] = new ColorArea((uint)(1 << i), startingAreas[i].color);
                mAdjacencyMasks.Add((uint)(1 << i), startingAreas[i].areaMask);
            }

            //Generate the initial state.
            mInitialState = new KamiState(mOriginalAreaCount, mNumberOfColors, initialAreas);

            Debug.Log($"Puzzle constructed. Number of colors = {mNumberOfColors}, Adjacency Mask count = {mAdjacencyMasks.Count}, Initial State Areas Count = {mInitialState.areas.Length}");
        }

        //Required by IPuzzle, returns the puzzle's initial state so that it can be used as the starting point for the breadth-first search.
        public IPuzzleState GetInitialState()
        {
            return mInitialState;
        }

        /// <summary>
        /// Takes in a state and returns all the states that can be reached from that state via player input.
        /// In our case, we have one transition for each color that the player can paint each area.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public IPuzzleState[] GetReachableStates(IPuzzleState state)
        {

            KamiState kamiState = state as KamiState; //Cast IPuzzleState instance to KamiState so that specific information can be extracted.
            if (kamiState == null) //If the state is not a KamiState, something has gone wrong. Print a debug message and return an empty array;
            {
                Debug.Log("State was not castable to KamiState.");
                return new KamiState[0];
            }

            //Make a list of states that can be reached from the current state by player input.
            //If I were optimizing I would use an array since we can calculate the exact number of transitions: (numberOfColors -1) * areas. Readability would suffer though
            List<IPuzzleState> reachableStates = new List<IPuzzleState>(); 
            foreach (ColorArea area in kamiState.areas) //foreach area...
            {
                for (int paintColor = 0; paintColor < mNumberOfColors; paintColor++) //foreach color that are could be painted...
                {
                    if (area.color != paintColor) //An area can't be painted its current color.
                    {
                        reachableStates.Add(TransitionState(kamiState, area, paintColor));  //Call TransitionState to change color of area and check adjacent areas for merge.
                    }
                }
            }

            return reachableStates.ToArray(); //reachableStates could be an array to begin with; more optimal but less readable.
        }

        //Check whether the state is a solution to the puzzle.
        public bool CheckForSolution(IPuzzleState state)
        {

            KamiState kamiState = state as KamiState;
            if (kamiState == null) //If the state is not a KamiState, something has gone wrong. Print a debug message and return an empty array;
            {
                Debug.Log("State was not castable to KamiState.");
                return false;
            }

            if (kamiState.areas.Length <= 1)
            {
                return true; //if there is only one area remaining, the puzzle is solved.
            }
            else
            { //Later levels have discontinous areas that must be set to the same color.
                int firstColor = kamiState.areas[0].color;
                for (int i = 1; i < kamiState.areas.Length; i++)
                {
                    if (kamiState.areas[i].color != firstColor)
                    {
                        return false; //If there is any mismatch, the state is not a solution.
                    }
                }
                return true; //If they all match, the solution has been reached.
            }
        }

        /// <summary>
        /// This method applies the "paintbucket" mechanic of Kami to move the puzzle from one state to another.
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="paintedArea"></param>
        /// <param name="paintedColor"></param>
        /// <returns></returns>
        private KamiState TransitionState(KamiState fromState, ColorArea paintedArea, int paintedColor)
        {
            List<ColorArea> areasToBeMerged = new List<ColorArea>();
            List<ColorArea> remainingAreas = new List<ColorArea>();
            foreach (ColorArea area in fromState.areas)
            {
                if ((TestAdjacency(paintedArea, area) && (area.color == paintedColor)) || (area.areaMask == paintedArea.areaMask))
                { //Add the painted area and adjacent areas of the paintedColor; 
                    areasToBeMerged.Add(area);
                }
                else
                {
                    remainingAreas.Add(area);
                }
            }

            remainingAreas.Add(MergeAreas(areasToBeMerged, paintedColor)); //Merge the painted area and append it to the list of unpainted areas.

            return new KamiState(mOriginalAreaCount, mNumberOfColors, remainingAreas.ToArray());
        }

        private bool TestAdjacency(ColorArea fromArea, ColorArea toArea)
        {
            return ((mAdjacencyMasks[fromArea.areaMask] & toArea.areaMask) > 0); //areas are adjacent if A's adjacency mask has overlap with B's area mask.
        }

        private ColorArea MergeAreas(List<ColorArea> areas, int paintedColor)
        {
            uint mergedInclusions = 0;
            uint mergedAdjacency = 0;
            foreach (ColorArea area in areas)
            {
                mergedInclusions = mergedInclusions | area.areaMask;
                mergedAdjacency = mergedAdjacency | mAdjacencyMasks[area.areaMask];
            }
            mAdjacencyMasks[mergedInclusions] = mergedAdjacency; // this is redundant if the merged area has already been found, but overwriting causes no harm and is probably faster than checking.
            return new ColorArea(mergedInclusions, paintedColor);
        }
    }  
}
