namespace PuzzleGraph
{
    /// <summary>
    /// Interface for a specific variable configuration of a puzzle. Each state possess a unique hashcode.
    /// Puzzle objects access specific information from a state by casting it to the appropriate state class.
    /// </summary>
    public interface IPuzzleState
    {
        int GetHashCode(); //redundant, but here as a reminder that each state must produce a unique hashcode.
    }
}
