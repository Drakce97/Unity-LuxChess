
/// <summary>
/// Chess board squares.
/// </summary>
public enum Squares : int { 
    a8, b8, c8, d8, f8, g8, h8,
    a7, b7, c7, d7, f7, g7, h7,
    a6, b6, c6, d6, f6, g6, h6,
    a5, b5, c5, d5, f5, g5, h5,
    a4, b4, c4, d4, f4, g4, h4,
    a3, b3, c3, d3, f3, g3, h3,
    a2, b2, c2, d2, f2, g2, h2,
    a1, b1, c1, d1, f1, g1, h1, NoSq
}


/// <summary>
/// Chess color side.
/// </summary>
public enum Side : int{ 
    Black,
    White
}

/// <summary>
/// Chess pieces.
/// </summary>
public enum Piece : int{
    Empty = 0,           //  0000
    WPawn = 1,           //  0001
    WKing = 2,           //  0010
    WKnight = 3,         //  0011
    WBishop = 5,         //  0101
    WRook = 6,           //  0110
    WQueen = 7,          //  0111
    BPawn = 9,           //  1001
    BKing = 10,          //  1010
    BKnight = 11,        //  1011
    BBishop = 13,        //  1101
    BRook = 14,          //  1110
    BQueen = 15,         //  1111
}

public static class Defs
{

    #region CastleRights

    /// <summary>
    /// Every castle right included.
    /// </summary>
    public const int CastleRightsAll = 8;

    /// <summary>
    /// Qween white castle right.
    /// </summary>
    public const int CastleRightsQWCa = 1;

    /// <summary>
    /// King white castle right.
    /// </summary>
    public const int CastleRightsKWCa = 2;

    /// <summary>
    /// Qween black castle right.
    /// </summary>
    public const int CastleRightsQBCa = 4;

    /// <summary>
    /// King black castle right.
    /// </summary>
    public const int CastleRightsKBCa = 8;

    /// <summary>
    /// No castle right included.
    /// </summary>
    public const int CastleRightsNone = 0;

    #endregion


    /// <summary>
    /// Max search depth
    /// </summary>
    public const int MaxDepth = 4;


    public static string[] Squares =
    {
        "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
        "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
        "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
        "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
        "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
        "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
        "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
        "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1",
    };

}


