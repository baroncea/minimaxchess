using System;
using System.Collections.Generic;
using System.Linq;

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
	}

}
