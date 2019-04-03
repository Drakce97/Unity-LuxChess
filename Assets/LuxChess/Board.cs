using System.Collections.Generic;

/// <summary>
/// Board class.
/// </summary>
public sealed class Board{

    //64 bit ulong for each square and piece
    //Two elements in an array for black(0) and white(1) side
    public ulong[] Pawns = new ulong[2];
    public ulong[] Bishops = new ulong[2];
    public ulong[] Knights = new ulong[2];
    public ulong[] Rooks = new ulong[2];
    public ulong[] Queens = new ulong[2];
    public ulong[] Kings = new ulong[2];
    public ulong[] Occupancy = new ulong[2];

    /// <summary>
    /// En passant square.
    /// </summary>
    public int EnPassantSq;

    /// <summary>
    /// Current side to play.
    /// </summary>
    public int SideToPlay{get; set;}


    /// <summary>
    /// Castling permission.
    /// </summary>
    public int CastlePermission;


    /// <summary>
    /// History moves.
    /// </summary>
    public Stack<HMove> History = new Stack<HMove>();


    /// <summary>
    /// Construct board based on FEN.
    /// </summary>
    public Board(string Fen) {
        //Init board from FEN
        this.BoardFromFen(Fen);
    }


    /// <summary>
    /// Updates the occupancy BB.
    /// </summary>
    public void UpdateOccupancy() {

        Occupancy[0] = Pawns[0] | Bishops[0] | Knights[0] | Rooks[0] | Queens[0] | Kings[0];
        Occupancy[1] = Pawns[1] | Bishops[1] | Knights[1] | Rooks[1] | Queens[1] | Kings[1];

    }


    /// <summary>
    /// Returns an array of pieces in the current board.
    /// </summary>
    /// <returns></returns>
    public int[] ArrayFromBoard() {

        int[] data = new int[64];

        for (int i = 0; i < 64; i++) { 
            //Get piece, both
            int black = GetPieceAt(i, 0);
            int piece = black == 0 ? GetPieceAt(i, 1) : black;
            data[i] = piece;
        }

        return data;
    }


    /// <summary>
    /// Gets the piece at given side in the given square.
    /// </summary>
    public int GetPieceAt(int index,int side) {

        ulong bb = Ops.Pow2[index];
        int correct = ((1-side) << 3); //Corrects the piece, in case black

        if ((bb & Pawns[side]) != 0)
            return ((int)Piece.WPawn | correct);

        if ((bb & Bishops[side]) != 0)
            return ((int)Piece.WBishop | correct);

        if ((bb & Knights[side]) != 0)
            return ((int)Piece.WKnight | correct);

        if ((bb & Rooks[side]) != 0)
            return ((int)Piece.WRook | correct);

        if ((bb & Queens[side]) != 0)
            return ((int)Piece.WQueen | correct);

        if ((bb & Kings[side]) != 0)
            return ((int)Piece.WKing | correct);

        return 0;
    }


    /// <summary>
    /// Returns true if the position BB is attacked by the given side.
    /// </summary>
    public bool IsAttacked(ulong pos, int attackedBySide) {
        //Pawn attacks
        ulong Attacks;

        if(attackedBySide == 1)//White pawn attacks
            Attacks = ((Pawns[1] << 7) & 0xfefefefefefefefe) | (Pawns[1] << 9 & 0x7f7f7f7f7f7f7f7f);
        else //Black pawn attacks
            Attacks = ((Pawns[0] >> 7) & 0x7f7f7f7f7f7f7f7f) | (Pawns[0] >> 9 & 0xfefefefefefefefe);

        //Check if there is any attack on a given pos
        if ((pos & Attacks) != 0)
            return true;

        //Set all occupancy
        ulong AllOccupancy = Occupancy[0] | Occupancy[1];

        while (pos != 0) {

            //Square index
            int index = Ops.PopFirstBit(ref pos);

            //Bishop attack & queen
            ulong attack = MoveGen.BishopAttack(index, AllOccupancy) & (Bishops[attackedBySide] | Queens[attackedBySide]);

            if (attack != 0)
                return true;

            //Rook attack & queen
            attack = MoveGen.RookAttack(index, AllOccupancy) & (Rooks[attackedBySide] | Queens[attackedBySide]);

            if (attack != 0)
                return true;

            //Knight attack
            attack = MoveGen.KnightAttacksDatabase[index] & Knights[attackedBySide];

            if (attack != 0)
                return true;

            //King attack
            attack = MoveGen.KingAttacksDatabase[index] & Kings[attackedBySide];

            if (attack != 0)
                return true;
        }


        return false;
    }


}
