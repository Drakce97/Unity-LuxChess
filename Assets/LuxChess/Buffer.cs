using System.Collections.Generic;

/// <summary>
/// Move buffer.
/// </summary>
public class Buffer{

    private List<Stack<int>> Moves = new List<Stack<int>>();

    /// <summary>
    /// Construct buffer based on given depth.
    /// </summary>
    public Buffer(int depth){
        Moves = new List<Stack<int>>();

        for (int i = 0; i < depth; i++) {
            Moves.Add(new Stack<int>());
        }
    }

    /// <summary>
    /// Add the move to the buffer - stack at the given depth.
    /// </summary>
    public void AddMove(int depth, int value) {
        Moves[depth].Push(value);
    }

    /// <summary>
    /// Pops the move from the stack at the given depth.
    /// </summary>
    public int PopMove(int depth) {
        return Moves[depth].Pop();
    }
	
}
