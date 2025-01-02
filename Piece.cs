using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace projectIA
{
	public class Piece
	{
		public int X { get; set; }
		public int Y { get; set; }
		public string Type { get; set; } 
		public PlayerType Player { get; set; } 

		public Piece(int x, int y, string type, PlayerType player)
		{
			X = x;
			Y = y;
			Type = type;
			Player = player;
		}

		public List<Move> GetValidMoves(Board board)
		{
			List<Move> validMoves = new List<Move>();
			bool[] directions = {true, true, true, true};

			switch (Type)
			{
				case "Pawn":
					int direction = Player == PlayerType.Human ? -1 : 1;
					int newY = Y + direction;
					if (newY >= 0 && newY < board.Size)
					{
						if (!board.Pieces.Any(p => p.X == X && p.Y == newY))
							validMoves.Add(new Move(X, Y, X, newY, false));

						if (X - 1 >= 0 && board.Pieces.Any(p => p.X == X - 1 && p.Y == newY && p.Player != Player))
							validMoves.Add(new Move(X, Y, X - 1, newY, true));

						if (X + 1 < board.Size && board.Pieces.Any(p => p.X == X + 1 && p.Y == newY && p.Player != Player))
							validMoves.Add(new Move(X, Y, X + 1, newY, true));
					}
					break;

				case "Rook":
					for (int i = 1; i < board.Size; i++)
					{
						if (directions[0])
							directions[0] = AddMoveIfValid(board, validMoves, X + i, Y);
                        if (directions[1])
                            directions[1] = AddMoveIfValid(board, validMoves, X - i, Y);
                        if (directions[2])
                            directions[2] = AddMoveIfValid(board, validMoves, X, Y + i);
                        if (directions[3])
                            directions[3] = AddMoveIfValid(board, validMoves, X, Y - i);
					}
					break;

				case "Bishop":
				
					for (int i = 1; i < board.Size; i++)
					{
                        if (directions[0])
                            directions[0] = AddMoveIfValid(board, validMoves, X + i, Y + i);
                        if (directions[1])
                            directions[1] = AddMoveIfValid(board, validMoves, X - i, Y + i);
                        if (directions[2])
                            directions[2] = AddMoveIfValid(board, validMoves, X + i, Y - i);
                        if (directions[3])
                            directions[3] = AddMoveIfValid(board, validMoves, X - i, Y - i);
					}
					break;

				case "King":
					for (int dx = -1; dx <= 1; dx++)
						for (int dy = -1; dy <= 1; dy++)
						{
							if (dx == 0 && dy == 0) continue;
							AddMoveIfValid(board, validMoves, X + dx, Y + dy);
						}
					break;

				case "Knight":
					var knightMoves = new List<(int dx, int dy)>
					{
						(2, 1), (1, 2), (-1, 2), (-2, 1), (-2, -1), (-1, -2), (1, -2), (2, -1)
					};

					foreach (var move in knightMoves)
					{
						AddMoveIfValid(board, validMoves, X + move.dx, Y + move.dy);
					}
					break;
			}

			return validMoves;
		}

		private bool AddMoveIfValid(Board board, List<Move> validMoves, int x, int y)
		{
			if (x >= 0 && x < board.Size && y >= 0 && y < board.Size)
			{
                var pieceAtPosition = board.Pieces.FirstOrDefault(p => p.X == x && p.Y == y);
				if (pieceAtPosition == null)
				{
					validMoves.Add(new Move(X, Y, x, y, false));
					return true;
				}
				else if (pieceAtPosition.Player != Player)
				{
					validMoves.Add(new Move(X, Y, x, y, true));
					return false;
				}
            }
			return false;
		}
	}
}
