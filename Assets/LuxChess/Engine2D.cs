using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Engine2D : MonoBehaviour {

    /// <summary>
    /// Piece set holds image for every piece type. One side only.
    /// </summary>
    [System.Serializable]
    public class PieceSet
    {
        public Texture2D Pawn; public Texture2D Knight; public Texture2D Bishop; public Texture2D Rook; public Texture2D Queen; public Texture2D King; public Texture2D Tile;
    }

    //Piece set
    public PieceSet WhiteSet;
    public PieceSet BlackSet;

    //Custom GUI skin
    public GUISkin skin;

    //Highlight texture
    public Texture2D HighlightTex;

    //Table rectangle
    public Rect TableRect;

    //Reference
    private Board ChessBoard;

    //Array of pieces
    private int[] Grid;

	void Start () {

        //Initialize magics
        Magics.Init();

        //Create board
        ChessBoard = new Board("6KQ/8/8/8/8/8/8/7k b - - 0 1");

        //Get array
        Grid = ChessBoard.ArrayFromBoard();

        Stack<int> staki = new Stack<int>();

        MoveGen.GenerateMoves(ChessBoard, staki);
        
        while(staki.Count >0)
        {
            int move = staki.Pop();

            Debug.Log("MOVING PIECE: " + (Piece) move.GetPiece() + " FROM: " + move.GetFrom() + " TO: " + move.GetTo() + " EATEN: " + (Piece)move.GetCapture() + " PROMO: " + move.GetPromo() );
        }

        Debug.Log(staki.Count);
	}


    void OnGUI() {

        GUI.skin = skin;

        //For every chess square
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                //Index
                int index = x + y * 8;

                //Get piece at array
                int piece = Grid[index];

                //Piece and square rectangle
                Rect squareRect = new Rect(TableRect.x + x * TableRect.width, TableRect.y + y * TableRect.height, TableRect.width, TableRect.height);
                Rect pieceRect = new Rect(TableRect.x + x * TableRect.width, TableRect.y + y * TableRect.height, TableRect.width, TableRect.height);

                //Every second square is black - (starting from a8)
                if ((index + y) % 2 == 0) GUI.DrawTexture(squareRect, WhiteSet.Tile);
                else GUI.DrawTexture(squareRect, BlackSet.Tile);

                //Label squares
                GUI.Label(squareRect, index.ToString());

                //Draw pieces
                switch (piece)
                {
                    //Button will shrink texture, which creates much better 'piece' effect
                    case (int)Piece.WPawn: GUI.Button(pieceRect, WhiteSet.Pawn); break;
                    case (int)Piece.WBishop: GUI.Button(pieceRect, WhiteSet.Bishop); break;
                    case (int)Piece.WKnight: GUI.Button(pieceRect, WhiteSet.Knight); break;
                    case (int)Piece.WRook: GUI.Button(pieceRect, WhiteSet.Rook); break;
                    case (int)Piece.WQueen: GUI.Button(pieceRect, WhiteSet.Queen); break;
                    case (int)Piece.WKing: GUI.Button(pieceRect, WhiteSet.King); break;
                    //Black
                    case (int)Piece.BPawn: GUI.Button(pieceRect, BlackSet.Pawn); break;
                    case (int)Piece.BBishop: GUI.Button(pieceRect, BlackSet.Bishop); break;
                    case (int)Piece.BKnight: GUI.Button(pieceRect, BlackSet.Knight); break;
                    case (int)Piece.BRook: GUI.Button(pieceRect, BlackSet.Rook); break;
                    case (int)Piece.BQueen: GUI.Button(pieceRect, BlackSet.Queen); break;
                    case (int)Piece.BKing: GUI.Button(pieceRect, BlackSet.King); break;

                    default: break;

                }
            }
        }
    }
}
