using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace projectIA
{
	public partial class Form1 : Form
	{
		private Board _board;
		private int _selectedPieceIndex = -1;
		private const int CellSize = 100;
		private const int BoardSize = 5;
        private int _minimaxDepth = 3;

        public Form1()
		{
			InitializeComponents();
			_board = new Board();
			this.DoubleBuffered = true;
		}

		private void InitializeComponents()
		{
			this.Text = "Chess";
			this.ClientSize = new Size(BoardSize * CellSize, BoardSize * CellSize + 30);

            var menuStrip = new MenuStrip();
            var jocMenu = new ToolStripMenuItem("Game");
            var startMenuItem = new ToolStripMenuItem("Start", null, StartGame);
            var exitMenuItem = new ToolStripMenuItem("Exit", null, (s, e) => this.Close());
            var settingsMenu = new ToolStripMenuItem("Settings");
            var easyMenuItem = new ToolStripMenuItem("Easy", null, (s, e) => SetDepth(1));
            var mediumMenuItem = new ToolStripMenuItem("Medium", null, (s, e) => SetDepth(2));
            var hardMenuItem = new ToolStripMenuItem("Hard", null, (s, e) => SetDepth(3));

            settingsMenu.DropDownItems.Add(easyMenuItem);
            settingsMenu.DropDownItems.Add(mediumMenuItem);
            settingsMenu.DropDownItems.Add(hardMenuItem);

            jocMenu.DropDownItems.Add(startMenuItem);
            jocMenu.DropDownItems.Add(exitMenuItem);
            menuStrip.Items.Add(jocMenu);
            menuStrip.Items.Add(settingsMenu);

            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;


            this.Controls.Add(menuStrip);
			this.MainMenuStrip = menuStrip;

			this.Paint += PaintTable;
			this.MouseClick += ClickPiece;
		}

		private void StartGame(object sender, EventArgs e)
		{
			_board = new Board();
			_selectedPieceIndex = -1;
			this.Invalidate();
		}

		private void PaintTable(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			string boardImagePath = "board.png";
			if (System.IO.File.Exists(boardImagePath))
			{
				Image boardImage = Image.FromFile(boardImagePath);
				g.DrawImage(boardImage, new Rectangle(0, 30, BoardSize * CellSize, BoardSize * CellSize));
			}
			else
			{
				throw new Exception("Game assets not found.");
			}

			foreach (var piece in _board.Pieces)
			{
				DrawPiece(g, piece);
			}

			if (_selectedPieceIndex != -1)
			{
				var selectedPiece = _board.Pieces[_selectedPieceIndex];
				Rectangle highlight = new Rectangle(selectedPiece.X * CellSize, (selectedPiece.Y * CellSize) + 30, CellSize, CellSize);
				g.DrawRectangle(new Pen(Color.Yellow, 3), highlight);
			}
		}

		private void DrawPiece(Graphics g, Piece piece)
		{
			string imagePath = piece.Type.ToLower() + (piece.Player == PlayerType.Human ? "_white" : "_black") + ".png";

			if (System.IO.File.Exists(imagePath))
			{
				Image pieceImage = Image.FromFile(imagePath);
				Rectangle rect = new Rectangle(piece.X * CellSize + 10, piece.Y * CellSize + 40, CellSize - 20, CellSize - 20);
				g.DrawImage(pieceImage, rect);
			}
			else
			{
				throw new Exception("Game assets not found.");
			}
		}


		private void ClickPiece(object sender, MouseEventArgs e)
		{
			int x = e.X / CellSize;
			int y = (e.Y - 30) / CellSize;

			if (x < 0 || x >= BoardSize || y < 0 || y >= BoardSize)
				return;

			if (_selectedPieceIndex == -1)
			{
				_selectedPieceIndex = _board.Pieces.FindIndex(p => p.X == x && p.Y == y && p.Player == PlayerType.Human);
			}
			else
			{
				var selectedPiece = _board.Pieces[_selectedPieceIndex];
				var move = selectedPiece.GetValidMoves(_board).FirstOrDefault(m => m.EndX == x && m.EndY == y);


                if (move != null)
				{
					_board = _board.MakeMove(move);
					_selectedPieceIndex = -1;
					_board.CheckFinish(out bool finished, out PlayerType winner);
					if (finished)
					{
						MessageBox.Show(winner == PlayerType.Human ? "You won!" : "The computer has won!", "Game has finished.");
						_board = new Board();
					}
					else
						ComputerMove();
				}
				else
				{
					_selectedPieceIndex = -1;
				}
			}

			this.Invalidate();
		}

        private void ComputerMove()
        {
            Board nextBoard = Minimax.FindNextBoard(_board, _minimaxDepth);
            _board = nextBoard;
            this.Invalidate();

            _board.CheckFinish(out bool finished, out PlayerType winner);
            if (finished)
            {
                MessageBox.Show(winner == PlayerType.Human ? "You won!" : "The computer has won!", "Game has finished.");
                _board = new Board();
            }
        }

        private void SetDepth(int depth)
        {
            _minimaxDepth = depth;
            MessageBox.Show($"AI difficulty set to {(depth == 1 ? "Easy" : depth == 2 ? "Medium" : "Hard")}", "Difficulty Updated");
        }
    }
}
