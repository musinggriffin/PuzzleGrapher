namespace PuzzleGraph
{
    /// <summary>
    /// The interface for puzzle objects.
    /// It allows the state graph explorer to work with any puzzle, not just Kami Puzzle.
    /// </summary>
    public interface IPuzzle
    {
        IPuzzleState GetInitialState(); //Get the puzzle's starting state. This is used to begin the breadth-first search.
        IPuzzleState[] GetReachableStates(IPuzzleState state); //Get an array of the states reachable from the given state.
        bool CheckForSolution(IPuzzleState state); //Check if a state fulfills the requirements for solving the puzzle.
    }
}

