namespace PuzzleGraph
{
    /// <summary>
    /// This is the node class for the state graph.
    /// It holds an IPuzzleState plus some metadata about that state.
    /// </summary>
    public class StateNode
    {

        public IPuzzleState state;
        public int depth; //Minimum number of moves from the starting state.
        public StateNode[] reachableStates;
        public NodeType nodeType = NodeType.normal;

        public StateNode(IPuzzleState puzzleState, int depthInGraph)
        {
            state = puzzleState;
            depth = depthInGraph;
        }

        public enum NodeType { normal, solution, failure } //Kami puzzles don't have a concept of failure (since I'm not counting moves), but other types of puzzles might.
    }
}

