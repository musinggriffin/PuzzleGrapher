using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGraph
{
    /// <summary>
    /// This class takes an IPuzzle and builds its state graph, starting from the puzzles initial state.
    /// Currently I am building the graph in the constructor, but really this is something that should be
    /// performed on a separate thread or across multiple frames to retain application responsiveness.
    /// </summary>
    public class StateGraph
    {

        //private fields
        private Dictionary<IPuzzleState, StateNode> stateNodes;
        private IPuzzle mPuzzle;
        private Queue<StateNode> mNodesToBeExplored;


        /// <summary>
        /// Constructs the stategraph of a puzzle by performing a breadth-first search of its states.
        /// </summary>
        /// <param name="puzzle"></param>
        public StateGraph(IPuzzle puzzle)
        {
            mPuzzle = puzzle;
            stateNodes = new Dictionary<IPuzzleState, StateNode>();
            mNodesToBeExplored = new Queue<StateNode>();

            RecordState(mPuzzle.GetInitialState(), 0); //register the initial state and add it to the unexplored state queue.

            //Perform the breadth-first exploration.
            while (mNodesToBeExplored.Count > 0)
            {
                StateNode node = mNodesToBeExplored.Dequeue();
                if (mPuzzle.CheckForSolution(node.state))
                {
                    node.nodeType = StateNode.NodeType.solution;
                    Debug.Log($"Solution found. {node.depth} moves to reach the solution.");
                }
                else
                {
                    ExploreNode(node);
                }
            }
            Debug.Log($"StateGraph Construction complete. The graph contains {stateNodes.Count} nodes.");
        }

        /// <summary>
        /// //Get any unexplored states that can be reached from the current state add them to the queue.
        /// </summary>
        /// <param name="node"></param>
        private void ExploreNode(StateNode node)
        {
            IPuzzleState[] childStates = mPuzzle.GetReachableStates(node.state);
            foreach (IPuzzleState state in childStates)
            {
                RecordState(state, node.depth + 1);
            }
        }

        /// <summary>
        /// Register discovered states in stateNodes dictionary.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="depth"></param>
        private void RecordState(IPuzzleState state, int depth)
        {
            if (!stateNodes.ContainsKey(state))
            { //Only create, record, and queue state for exploration if it is new.
                StateNode node = new StateNode(state, depth);
                stateNodes.Add(state, node);
                mNodesToBeExplored.Enqueue(node);
            }
        }
    }
}

