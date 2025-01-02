using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace projectIA
{
	public class Board
	{
		public int Size { get; private set; }
		public List<Piece> Pieces { get; private set; }

		public Board()
		{
			Size = 5;
			Pieces = new List<Piece>();

			Pieces.Add(new Piece(0, 0, "Rook", PlayerType.Computer));
			Pieces.Add(new Piece(4, 0, "Rook", PlayerType.Computer));
			Pieces.Add(new Piece(2, 0, "King", PlayerType.Computer));
			Pieces.Add(new Piece(1, 0, "Knight", PlayerType.Computer));
			Pieces.Add(new Piece(3, 0, "Bishop", PlayerType.Computer));

			for (int i = 0; i < Size; i++)
				Pieces.Add(new Piece(i, 1, "Pawn", PlayerType.Computer));

			Pieces.Add(new Piece(0, 4, "Rook", PlayerType.Human));
			Pieces.Add(new Piece(4, 4, "Rook", PlayerType.Human));
			Pieces.Add(new Piece(2, 4, "King", PlayerType.Human));
			Pieces.Add(new Piece(1, 4, "Knight", PlayerType.Human));
			Pieces.Add(new Piece(3, 4, "Bishop", PlayerType.Human));

			for (int i = 0; i < Size; i++)
				Pieces.Add(new Piece(i, 3, "Pawn", PlayerType.Human));
		}

		public Board(Board other)
		{
			Size = other.Size;
			Pieces = new List<Piece>();
			foreach (var piece in other.Pieces)
			{
				Pieces.Add(new Piece(piece.X, piece.Y, piece.Type, piece.Player));
			}
		}

        public Board MakeMove(Move move)
        {
            Board nextBoard = new Board(this);

            Piece piece = nextBoard.Pieces.First(p => p.X == move.StartX && p.Y == move.StartY);
            if (move.IsCapture)
            {
                var capturedPiece = nextBoard.Pieces.FirstOrDefault(p => p.X == move.EndX && p.Y == move.EndY && p.Player != piece.Player);
				if (capturedPiece != null)
				{
					nextBoard.Pieces.Remove(capturedPiece);
				}
            }

            piece.X = move.EndX;
            piece.Y = move.EndY;

            return nextBoard;
        }

        public void CheckFinish(out bool finished, out PlayerType winner)
		{
			if (!Pieces.Any(p => p.Type == "King" && p.Player == PlayerType.Human))
			{
				finished = true;
				winner = PlayerType.Computer;
				return;
			}

			if (!Pieces.Any(p => p.Type == "King" && p.Player == PlayerType.Computer))
			{
				finished = true;
				winner = PlayerType.Human;
				return;
			}

			finished = false;
			winner = PlayerType.None;
		}

        public double EvaluationFunction()
        {
            var pieceValues = new Dictionary<string, double>
			{
				{ "Pawn", 1.0 },
				{ "Knight", 3.0 },
				{ "Bishop", 3.0 },
				{ "Rook", 5.0 },
				{ "King", 100.0 }
			};

            double score = 0.0;

            foreach (Piece piece in Pieces)
            {
                double pieceValue = pieceValues.ContainsKey(piece.Type) ? pieceValues[piece.Type] : 0.0;

                if (piece.Player == PlayerType.Computer)
                {
                    score += pieceValue;

                    // Encourage central positions
                    score += 0.1 * (Math.Abs(Size / 2 - piece.X) + Math.Abs(Size / 2 - piece.Y));
                }
                else if (piece.Player == PlayerType.Human)
                {
                    score -= pieceValue;

                    // Penalize central positions for opponent
                    score -= 0.1 * (Math.Abs(Size / 2 - piece.X) + Math.Abs(Size / 2 - piece.Y));
                }
            }

            return score;
        }

    }
}
