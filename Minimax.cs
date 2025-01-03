using System.Linq;
using System;

namespace projectIA
{
    public class Minimax
    {
        public static Board FindNextBoard(Board currentBoard, int depth)
        {
            double alpha = double.MinValue;
            double beta = double.MaxValue;
            bool maximizingPlayer = true;

            double bestValue = double.MinValue;
            Board bestBoard = null;

            foreach (Piece piece in currentBoard.Pieces.Where(p => p.Player == PlayerType.Computer))
            {
                foreach (Move move in piece.GetValidMoves(currentBoard))
                {
                    Board nextBoard = currentBoard.MakeMove(move);
                    double value = MinimaxAlgorithm(nextBoard, depth - 1, alpha, beta, !maximizingPlayer);

                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestBoard = nextBoard;
                    }

                    alpha = Math.Max(alpha, value);
                    if (alpha >= beta)
                        break;
                }
            }

            return bestBoard;
        }

        private static double MinimaxAlgorithm(Board board, int depth, double alpha, double beta, bool maximizingPlayer)
        {
            bool finished;
            PlayerType winner;
            board.CheckFinish(out finished, out winner);

            if (depth == 0 || finished)
            {
                if (finished)
                {
                    return winner == PlayerType.Computer ? double.MaxValue : double.MinValue;
                }

                return board.EvaluationFunction();
            }

            if (maximizingPlayer)
            {
                double maxEval = double.MinValue;

                foreach (Piece piece in board.Pieces.Where(p => p.Player == PlayerType.Computer))
                {
                    foreach (Move move in piece.GetValidMoves(board))
                    {
                        Board nextBoard = board.MakeMove(move);
                        double eval = MinimaxAlgorithm(nextBoard, depth - 1, alpha, beta, false);
                        maxEval = Math.Max(maxEval, eval);
                        alpha = Math.Max(alpha, eval);
                        if (alpha >= beta)
                            break;
                    }
                }

                return maxEval;
            }
            else
            {
                double minEval = double.MaxValue;

                foreach (Piece piece in board.Pieces.Where(p => p.Player == PlayerType.Human))
                {
                    foreach (Move move in piece.GetValidMoves(board))
                    {
                        Board nextBoard = board.MakeMove(move);
                        double eval = MinimaxAlgorithm(nextBoard, depth - 1, alpha, beta, true);
                        minEval = Math.Min(minEval, eval);
                        beta = Math.Min(beta, eval);
                        if (beta <= alpha)
                            break;
                    }
                }

                return minEval;
            }
        }

    }
}
