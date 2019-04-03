using System.Collections.Generic;


/// <summary>
/// All about move generation.
/// </summary>
public static class MoveGen {


    #region Table


    /// <summary>
    /// Knight occupancy bitboard on every square.
    /// </summary>
    public static ulong[] KnightAttacksDatabase = {
		0x20400, 0x50800, 0xa1100, 0x142200, 0x284400, 0x508800, 0xa01000, 0x402000,
		0x2040004, 0x5080008, 0xa110011, 0x14220022, 0x28440044, 0x50880088, 0xa0100010, 0x40200020,
		0x204000402, 0x508000805, 0xa1100110a, 0x1422002214, 0x2844004428, 0x5088008850, 0xa0100010a0, 0x4020002040,
		0x20400040200, 0x50800080500, 0xa1100110a00, 0x142200221400, 0x284400442800, 0x508800885000, 0xa0100010a000, 0x402000204000,
		0x2040004020000, 0x5080008050000,  0xa1100110a0000, 0x14220022140000, 0x28440044280000, 0x50880088500000, 0xa0100010a00000, 0x40200020400000,
		0x204000402000000, 0x508000805000000, 0xa1100110a000000, 0x1422002214000000, 0x2844004428000000, 0x5088008850000000, 0xa0100010a0000000, 0x4020002040000000,
		0x400040200000000, 0x800080500000000, 0x1100110a00000000, 0x2200221400000000, 0x4400442800000000, 0x8800885000000000, 0x100010a000000000, 0x2000204000000000,
		0x4020000000000, 0x8050000000000, 0x110a0000000000, 0x22140000000000, 0x44280000000000, 0x88500000000000, 0x10a00000000000, 0x20400000000000

	};


    /// <summary>
    /// King occupancy bitboard on every square.
    /// </summary>
    public static ulong[] KingAttacksDatabase = {
		0x302, 0x705, 0xe0a, 0x1c14, 0x3828, 0x7050, 0xe0a0, 0xc040,
		0x30203, 0x70507, 0xe0a0e, 0x1c141c, 0x382838, 0x705070, 0xe0a0e0, 0xc040c0,
		0x3020300, 0x7050700, 0xe0a0e00, 0x1c141c00, 0x38283800, 0x70507000, 0xe0a0e000, 0xc040c000,
		0x302030000, 0x705070000, 0xe0a0e0000,  0x1c141c0000, 0x3828380000, 0x7050700000, 0xe0a0e00000, 0xc040c00000,
		0x30203000000, 0x70507000000, 0xe0a0e000000, 0x1c141c000000, 0x382838000000, 0x705070000000, 0xe0a0e0000000, 0xc040c0000000,
		0x3020300000000, 0x7050700000000, 0xe0a0e00000000, 0x1c141c00000000, 0x38283800000000, 0x70507000000000, 0xe0a0e000000000, 0xc040c000000000,
		0x302030000000000, 0x705070000000000, 0xe0a0e0000000000, 0x1c141c0000000000, 0x3828380000000000, 0x7050700000000000, 0xe0a0e00000000000, 0xc040c00000000000,
		0x203000000000000, 0x507000000000000, 0xa0e000000000000, 0x141c000000000000, 0x2838000000000000, 0x5070000000000000, 0xa0e0000000000000, 0x40c0000000000000

	};



    #endregion


    /// <summary>
    /// Returns legal rook attack bitboard.
    /// </summary>
    public static ulong RookAttack(int sq, ulong occupancy)
    {
        ulong blockers = occupancy & Magics.OccupancyMaskRook[sq];
        ulong index = (blockers * Magics.MagicNumberRook[sq]) >> Magics.MagicNumberShiftsRook[sq];
        return Magics.MagicMoveRook[sq][index];
    }

    /// <summary>
    /// Returns legal bishop attack bitboard.
    /// </summary>
    public static ulong BishopAttack(int bit, ulong occupancy)
    {
        ulong blockers = occupancy & Magics.OccupancyMaskBishop[bit];
        ulong index = (blockers * Magics.MagicNumberBishop[bit]) >> Magics.MagicNumberShiftsBishop[bit];
        return Magics.MagicMoveBishop[bit][index];
    }

    /// <summary>
    /// Generates all moves.
    /// </summary>
    public static void GenerateMoves(Board board, Stack<int> moves) {

        //Updates occupancy
        board.UpdateOccupancy();

        //Occupancy
        ulong Occupancy = board.Occupancy[0] | board.Occupancy[1];

        if (board.SideToPlay == (int)Side.White) //WHITE
        {

            #region White kings move generation

            ulong King = board.Kings[1];

            while (King != 0) {

                int From = Ops.PopFirstBit(ref King);
                ulong Moves = KingAttacksDatabase[From] & ~board.Occupancy[1]; //Can't step on a friendly pieces

                while (Moves != 0) {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 0);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.WKing, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region Castling move generation
            
            //Have right to castle
            if ((board.CastlePermission & Defs.CastleRightsKWCa) != 0) //King side
            {
                if ((Occupancy & 0x6000000000000000) == 0) //Not occupied
                {
                    if (!board.IsAttacked(0x7000000000000000, 0)) //Not attacked
                    {
                        //Add move
                        int move = Move.CreateMove(60, 62, (int)Piece.WKing, 0, (int)Piece.WKing);
                        moves.Push(move);
                    }
                }
            }

            if ((board.CastlePermission & Defs.CastleRightsQWCa) != 0)//Queen side
            {
                if ((Occupancy & 0xe00000000000000) == 0) //Not occupied
                {
                    if (!board.IsAttacked(0x1c00000000000000, 0)) //Not attacked
                    {
                        //Add move
                        int move = Move.CreateMove(60, 58, (int)Piece.WKing, 0, (int)Piece.WKing);
                        moves.Push(move);
                    }
                }
            }

            #endregion

            #region White rooks move generation

            ulong Rooks = board.Rooks[1];

            while (Rooks != 0)
            {

                int From = Ops.PopFirstBit(ref Rooks);
                ulong Moves = RookAttack(From, Occupancy) & ~board.Occupancy[1]; //Can't step on a friendly pieces

                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 0);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.WRook, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region White knights move generation

            ulong Knights = board.Knights[1];

            while (Knights != 0)
            {

                int From = Ops.PopFirstBit(ref Knights);
                ulong Moves = KnightAttacksDatabase[From] & ~board.Occupancy[1]; //Can't step on a friendly pieces

                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 0);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.WKnight, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region White bishops move generation

            ulong Bishops = board.Bishops[1];

            while (Bishops != 0)
            {

                int From = Ops.PopFirstBit(ref Bishops);
                ulong Moves = BishopAttack(From, Occupancy) & ~board.Occupancy[1]; //Can't step on a friendly pieces

                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 0);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.WBishop, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region White queens move generation

            ulong Queens = board.Queens[1];

            while (Queens != 0)
            {

                int From = Ops.PopFirstBit(ref Queens);
                ulong Moves = (BishopAttack(From, Occupancy) | RookAttack(From, Occupancy)) & ~board.Occupancy[1]; //Can't step on a friendly pieces

                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 0);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.WQueen, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region White pawn move generation

            //PAWNS
            ulong Pawns = board.Pawns[1]; //Copied

            while (Pawns != 0)
            {

                int From = Ops.PopFirstBit(ref Pawns);
                ulong Pawn = Ops.Pow2[From];

                ulong Pushes = Pawn >> 8;
                Pushes &= ~Occupancy; //Can't move on a piece

                if (Pushes != 0) { //We have pawn push to add
                
                    //ADD MOVE
                    int To = From - 8;

                    //Check for promotion
                    if ((Ops.Pow2[To] & 0xff) != 0)
                    {
                        //Add all 4 possible promotion
                        int move = Move.CreateMove(From, To, (int)Piece.WPawn, 0, (int)Piece.WQueen);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.WPawn, 0, (int)Piece.WRook);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.WPawn, 0, (int)Piece.WBishop);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.WPawn, 0, (int)Piece.WKnight);
                        moves.Push(move);

                    }
                    else
                    {
                        int move = Move.CreateMove(From, To, (int)Piece.WPawn, 0, 0);
                        moves.Push(move);
                    }
                }

                ulong ThirdRow = 0xff0000000000; //White side

                ulong DoublePushes = (Pushes & ThirdRow) >> 8;
                DoublePushes &= ~Occupancy;

                if (DoublePushes != 0) { //We have double push to add

                    //ADD MOVE
                    int To = From - 16;

                    int move = Move.CreateMove(From, To, (int)Piece.WPawn, 0, 0);
                    moves.Push(move);
                }

                ulong Attacks = ((Pawn >> 7) & 0xfefefefefefefefe) | (Pawn >> 9 & 0x7f7f7f7f7f7f7f7f);

                //Check en passant SQ
                if (board.EnPassantSq < 64)
                {
                    ulong EpSq = Ops.Pow2[board.EnPassantSq];

                    if ((Attacks & EpSq) != 0) { //Add en passant attack

                        //ADD MOVE
                        int To = board.EnPassantSq;

                        int move = Move.CreateMove(From, To, (int)Piece.WPawn, 0, (int)Piece.BPawn);
                        moves.Push(move);
                    
                    }
                }

                Attacks &= board.Occupancy[0]; //Can attack enemy pieces only

                while (Attacks != 0) {
                    int To = Ops.PopFirstBit(ref Attacks);

                    int capturePiece = board.GetPieceAt(To, 0);

                    //Check for promotion
                    if ((Ops.Pow2[To] & 0xff) != 0)
                    {
                        //Add all 4 possible promotion
                        int move = Move.CreateMove(From, To, (int)Piece.WPawn, capturePiece, (int)Piece.WQueen);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.WPawn, capturePiece, (int)Piece.WRook);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.WPawn, capturePiece, (int)Piece.WBishop);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.WPawn, capturePiece, (int)Piece.WKnight);
                        moves.Push(move);

                    }
                    else
                    {
                        //ADD MOVE
                        int move = Move.CreateMove(From, To, (int)Piece.WPawn, capturePiece, 0);
                        moves.Push(move);
                    }
                }

            }


            #endregion

        }
        else { //BLACK

            #region Black kings move generation

            ulong King = board.Kings[0];

            while (King != 0)
            {

                int From = Ops.PopFirstBit(ref King);
                ulong Moves = KingAttacksDatabase[From] & ~board.Occupancy[0]; //Can't step on a friendly pieces

                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 1);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.BKing, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region Castling move generation

            //Have right to castle
            if ((board.CastlePermission & Defs.CastleRightsKBCa) != 0) //King side
            {
                if ((Occupancy & 0x60) == 0) //Not occupied
                {
                    if (!board.IsAttacked(0x70, 1)) //Not attacked
                    {
                        //Add move
                        int move = Move.CreateMove(4, 6, (int)Piece.BKing, 0, (int)Piece.BKing);
                        moves.Push(move);
                    }
                }
            }

            if ((board.CastlePermission & Defs.CastleRightsQBCa) != 0)//Queen side
            {
                if ((Occupancy & 0xe) == 0) //Not occupied
                {
                    if (!board.IsAttacked(0x1c, 0)) //Not attacked
                    {
                        //Add move
                        int move = Move.CreateMove(4, 2, (int)Piece.BKing, 0, (int)Piece.BKing);
                        moves.Push(move);
                    }
                }
            }

            #endregion

            #region Black rooks move generation

            ulong Rooks = board.Rooks[0];

            while (Rooks != 0)
            {

                int From = Ops.PopFirstBit(ref Rooks);
                ulong Moves = RookAttack(From, Occupancy) & ~board.Occupancy[0]; //Can't step on a friendly pieces

                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 1);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.BRook, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region Black knights move generation

            ulong Knights = board.Knights[0];

            while (Knights != 0)
            {

                int From = Ops.PopFirstBit(ref Knights);
                ulong Moves = KnightAttacksDatabase[From] & ~board.Occupancy[0]; //Can't step on a friendly pieces

                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 1);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.BKnight, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region White bishops move generation

            ulong Bishops = board.Bishops[0];

            while (Bishops != 0)
            {

                int From = Ops.PopFirstBit(ref Bishops);
                ulong Moves = BishopAttack(From, Occupancy) & ~board.Occupancy[0]; //Can't step on a friendly pieces

                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 1);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.BBishop, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region White queens move generation

            ulong Queens = board.Queens[0];

            while (Queens != 0)
            {

                int From = Ops.PopFirstBit(ref Queens);
                ulong Moves = (BishopAttack(From, Occupancy) | RookAttack(From, Occupancy)) & ~board.Occupancy[0]; //Can't step on a friendly pieces

                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int capturedPiece = board.GetPieceAt(To, 1);

                    //Add move
                    int move = Move.CreateMove(From, To, (int)Piece.BQueen, capturedPiece, 0);
                    moves.Push(move);

                }

            }

            #endregion

            #region Black pawn move generation

            //PAWNS
            ulong Pawns = board.Pawns[0]; //Copied

            while (Pawns != 0)
            {

                int From = Ops.PopFirstBit(ref Pawns);
                ulong Pawn = Ops.Pow2[From];

                ulong Pushes = Pawn << 8;
                Pushes &= ~Occupancy; //Can't move on a piece

                if (Pushes != 0)
                { //We have pawn push to add

                    //ADD MOVE
                    int To = From + 8;

                    //Check for promotion
                    if ((Ops.Pow2[To] & 0xff00000000000000) != 0)
                    {
                        //Add all 4 possible promotion
                        int move = Move.CreateMove(From, To, (int)Piece.BPawn, 0, (int)Piece.BQueen);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.BPawn, 0, (int)Piece.BRook);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.BPawn, 0, (int)Piece.BBishop);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.BPawn, 0, (int)Piece.BKnight);
                        moves.Push(move);

                    }
                    else
                    {
                        int move = Move.CreateMove(From, To, (int)Piece.BPawn, 0, 0);
                        moves.Push(move);
                    }
                }

                ulong ThirdRow = 0xff0000; //Black side

                ulong DoublePushes = (Pushes & ThirdRow) << 8;
                DoublePushes &= ~Occupancy;

                if (DoublePushes != 0)
                { //We have double push to add

                    //ADD MOVE
                    int To = From + 16;

                    int move = Move.CreateMove(From, To, (int)Piece.BPawn, 0, 0);
                    moves.Push(move);
                }

                ulong Attacks = ((Pawn << 7) & 0x7f7f7f7f7f7f7f7f) | (Pawn << 9 & 0xfefefefefefefefe);

                //Check en passant SQ
                if (board.EnPassantSq < 64)
                {
                    ulong EpSq = Ops.Pow2[board.EnPassantSq];

                    if ((Attacks & EpSq) != 0)
                    { //Add en passant attack

                        //ADD MOVE
                        int To = board.EnPassantSq;

                        int move = Move.CreateMove(From, To, (int)Piece.BPawn, 0, (int)Piece.WPawn);
                        moves.Push(move);

                    }
                }

                Attacks &= board.Occupancy[1]; //Can attack enemy pieces only

                while (Attacks != 0)
                {
                    int To = Ops.PopFirstBit(ref Attacks);

                    int capturePiece = board.GetPieceAt(To, 0);

                    //Check for promotion
                    if ((Ops.Pow2[To] & 0xff) != 0)
                    {
                        //Add all 4 possible promotion
                        int move = Move.CreateMove(From, To, (int)Piece.BPawn, capturePiece, (int)Piece.BQueen);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.BPawn, capturePiece, (int)Piece.BRook);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.BPawn, capturePiece, (int)Piece.BBishop);
                        moves.Push(move);
                        move = Move.CreateMove(From, To, (int)Piece.BPawn, capturePiece, (int)Piece.BKnight);
                        moves.Push(move);

                    }
                    else
                    {
                        //ADD MOVE
                        int move = Move.CreateMove(From, To, (int)Piece.BPawn, capturePiece, 0);
                        moves.Push(move);
                    }
                }
            }

            #endregion

        }
    }



}
