using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using static Chess_Client.VIEW.GAME_VIEW.MainGameView;
using Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS;
using Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_PIECES;
using System.Windows.Input;

namespace Chess_Client.VIEW.GAME_VIEW
{
    public class BoardMainGameView : Grid
    {
        private GameView gameView;

        private Grid grdBoard;
        private Canvas canvasBoard;

        public BoardMainGameView(GameView gameView)
        {
            this.gameView = gameView;
            InitializeComponent();
            this.Children.Add(layout());
        }
        public void InitializeComponent()
        {

            this.Name = "GrdBorderBoard";
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = 700;
            this.Height = 700;
            this.Margin = new Thickness(10, 100, 0, 0);
            this.Background = Brushes.Black;
        }
        public Grid layout()
        {
            grdBoard = new Grid
            {
                Name = "GrdBoard",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 680,
                Height = 680,
                Margin = new Thickness(10, 10, 0, 0)
            };



            canvasBoard = new Canvas
            {
                Name = "CanvasBoard",
                Width = 680,
                Height = 680
            };

            grdBoard.Children.Add(canvasBoard);
            return grdBoard;
        }

        public void initializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0 || i == 7)
                    {
                        if (j == 0 || j == 7)
                        {
                            (this.gameView.GameController.ChessGame.Squares[i, j].Piece as Rook).possiblePositions();
                            canvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[i, j].Piece as Rook).MyImage);
                        }
                        else if (j == 1 || j == 6)
                        {
                            (this.gameView.GameController.ChessGame.Squares[i, j].Piece as Knight).possiblePositions();
                            canvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[i, j].Piece as Knight).MyImage);
                        }
                        else if (j == 2 || j == 5)
                        {
                            (this.gameView.GameController.ChessGame.Squares[i, j].Piece as Bishop).possiblePositions();
                            canvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[i, j].Piece as Bishop).MyImage);
                        }
                        else if (j == 3)
                        {
                            if (this.gameView.GameController.ChessGame.IndexColor == 0)
                            {
                                (this.gameView.GameController.ChessGame.Squares[i, j].Piece as Queen).possiblePositions();
                                canvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[i, j].Piece as Queen).MyImage);
                            }
                            else
                            if (this.gameView.GameController.ChessGame.IndexColor == 1)
                            {
                                (this.gameView.GameController.ChessGame.Squares[i, j].Piece as King).possiblePositions();
                                canvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[i, j].Piece as King).MyImage);
                            }
                        }
                        else if (j == 4)
                        {
                            if (this.gameView.GameController.ChessGame.IndexColor == 1)
                            {
                                (this.gameView.GameController.ChessGame.Squares[i, j].Piece as Queen).possiblePositions();
                                canvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[i, j].Piece as Queen).MyImage);
                            }
                            else
                            if (this.gameView.GameController.ChessGame.IndexColor == 0)
                            {
                                (this.gameView.GameController.ChessGame.Squares[i, j].Piece as King).possiblePositions();
                                canvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[i, j].Piece as King).MyImage);
                            }
                        }
                    }
                    else
                    if (i == 1 || i == 6)
                    {
                        (this.gameView.GameController.ChessGame.Squares[i, j].Piece as Pawn).possiblePositions();
                        canvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[i, j].Piece as Pawn).MyImage);
                    }
                    else
                    {
                        canvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[i, j].Piece as Empty).MyImage);
                    }
                }
            }
          



        }

        public Canvas CanvasBoard
        {
            get => this.canvasBoard; set => this.canvasBoard = value;
        }
        public Grid GrdBoard
        {
            get => this.grdBoard; set => this.grdBoard = value;
        }
        public GameView GameView
        {
            get => this.gameView; set => this.gameView = value;
        }
    }
}
