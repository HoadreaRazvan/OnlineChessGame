using Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_PIECES;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS
{
    public class Board
    {
        private Square[,] squares;
        private ChessGame chessGame;
        private PieceColor myColor, enemyColor;

        public Board(int indexColor)
        {
            this.myColor = indexColor == 0 ? PieceColor.White : PieceColor.Black;
            this.enemyColor = indexColor == 1 ? PieceColor.White : PieceColor.Black;
            this.squares = new Square[8, 8];

        }


        public void setupBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    double leftPosition = j * 85;
                    double topPosition = i * 85;
                    if (i == 0)
                    {
                        if (j == 0 || j == 7)
                        {
                            squares[i, j] = new Square(new Rook(this.enemyColor, new Point(i, j), this.chessGame));
                            Canvas.SetLeft((squares[i, j].Piece as Rook).MyImage, leftPosition);
                            Canvas.SetTop((squares[i, j].Piece as Rook).MyImage, topPosition);
                        }
                        else if (j == 1 || j == 6)
                        {
                            squares[i, j] = new Square(new Knight(this.enemyColor, new Point(i, j), this.chessGame));
                            Canvas.SetLeft((squares[i, j].Piece as Knight).MyImage, leftPosition);
                            Canvas.SetTop((squares[i, j].Piece as Knight).MyImage, topPosition);
                        }
                        else if (j == 2 || j == 5)
                        {
                            squares[i, j] = new Square(new Bishop(this.enemyColor, new Point(i, j), this.chessGame));
                            Canvas.SetLeft((squares[i, j].Piece as Bishop).MyImage, leftPosition);
                            Canvas.SetTop((squares[i, j].Piece as Bishop).MyImage, topPosition);
                        }
                        else if (j == 3)
                        {
                            if (this.chessGame.IndexColor == 0)
                            {
                                squares[i, j] = new Square(new Queen(this.enemyColor, new Point(i, j), this.chessGame));
                                Canvas.SetLeft((squares[i, j].Piece as Queen).MyImage, leftPosition);
                                Canvas.SetTop((squares[i, j].Piece as Queen).MyImage, topPosition);
                            }
                            else
                            if (this.chessGame.IndexColor == 1)
                            {
                                squares[i, j] = new Square(new King(this.enemyColor, new Point(i, j), this.chessGame));
                                Canvas.SetLeft((squares[i, j].Piece as King).MyImage, leftPosition);
                                Canvas.SetTop((squares[i, j].Piece as King).MyImage, topPosition);
                            }

                        }
                        else if (j == 4)
                        {
                            if (this.chessGame.IndexColor == 1)
                            {
                                squares[i, j] = new Square(new Queen(this.enemyColor, new Point(i, j), this.chessGame));
                                Canvas.SetLeft((squares[i, j].Piece as Queen).MyImage, leftPosition);
                                Canvas.SetTop((squares[i, j].Piece as Queen).MyImage, topPosition);
                            }
                            else
                            if (this.chessGame.IndexColor == 0)
                            {
                                squares[i, j] = new Square(new King(this.enemyColor, new Point(i, j), this.chessGame));
                                Canvas.SetLeft((squares[i, j].Piece as King).MyImage, leftPosition);
                                Canvas.SetTop((squares[i, j].Piece as King).MyImage, topPosition);
                            }
                        }
                    } 
                    else
                    if (i == 1)
                    {
                        squares[i, j] = new Square(new Pawn(this.enemyColor, new Point(i, j), this.chessGame));
                        Canvas.SetLeft((squares[i, j].Piece as Pawn).MyImage, leftPosition);
                        Canvas.SetTop((squares[i, j].Piece as Pawn).MyImage, topPosition);
                    }else
                    if(i==6)
                    {
                        squares[i, j] = new Square(new Pawn(this.myColor, new Point(i, j), this.chessGame));
                        Canvas.SetLeft((squares[i, j].Piece as Pawn).MyImage, leftPosition);
                        Canvas.SetTop((squares[i, j].Piece as Pawn).MyImage, topPosition);
                    }
                    else
                    if (i == 7)
                    {
                        if (j == 0 || j == 7)
                        {
                            squares[i, j] = new Square(new Rook(this.myColor, new Point(i, j), this.chessGame));
                            Canvas.SetLeft((squares[i, j].Piece as Rook).MyImage, leftPosition);
                            Canvas.SetTop((squares[i, j].Piece as Rook).MyImage, topPosition);
                        }
                        else if (j == 1 || j == 6)
                        {
                            squares[i, j] = new Square(new Knight(this.myColor, new Point(i, j), this.chessGame));
                            Canvas.SetLeft((squares[i, j].Piece as Knight).MyImage, leftPosition);
                            Canvas.SetTop((squares[i, j].Piece as Knight).MyImage, topPosition);
                        }
                        else if (j == 2 || j == 5)
                        {
                            squares[i, j] = new Square(new Bishop(this.myColor, new Point(i, j), this.chessGame));
                            Canvas.SetLeft((squares[i, j].Piece as Bishop).MyImage, leftPosition);
                            Canvas.SetTop((squares[i, j].Piece as Bishop).MyImage, topPosition);
                        }
                        else if (j == 3)
                        {
                            if (this.chessGame.IndexColor == 0)
                            {
                                squares[i, j] = new Square(new Queen(this.myColor, new Point(i, j), this.chessGame));
                                Canvas.SetLeft((squares[i, j].Piece as Queen).MyImage, leftPosition);
                                Canvas.SetTop((squares[i, j].Piece as Queen).MyImage, topPosition);
                            }
                            else
                            if (this.chessGame.IndexColor == 1)
                            {
                                squares[i, j] = new Square(new King(this.myColor, new Point(i, j), this.chessGame));
                                Canvas.SetLeft((squares[i, j].Piece as King).MyImage, leftPosition);
                                Canvas.SetTop((squares[i, j].Piece as King).MyImage, topPosition);
                            }
                        }
                        else if (j == 4)
                        {
                            if (this.chessGame.IndexColor == 1)
                            {
                                squares[i, j] = new Square(new Queen(this.myColor, new Point(i, j), this.chessGame));
                                Canvas.SetLeft((squares[i, j].Piece as Queen).MyImage, leftPosition);
                                Canvas.SetTop((squares[i, j].Piece as Queen).MyImage, topPosition);
                            }
                            else
                            if (this.chessGame.IndexColor == 0)
                            {
                                squares[i, j] = new Square(new King(this.myColor, new Point(i, j), this.chessGame));
                                Canvas.SetLeft((squares[i, j].Piece as King).MyImage, leftPosition);
                                Canvas.SetTop((squares[i, j].Piece as King).MyImage, topPosition);
                            }
                        }
                    }
                    else
                    {
                        squares[i, j] = new Square(new Empty(PieceColor.None, new Point(i, j), this.chessGame));
                        Canvas.SetLeft((squares[i, j].Piece as Empty).MyImage, leftPosition);
                        Canvas.SetTop((squares[i, j].Piece as Empty).MyImage, topPosition);
                    }
                }
            }



        }


        public PieceColor MyColor
        {
            get => this.myColor; set => this.myColor = value;
        }
        public PieceColor EnemyColor
        {
            get => this.enemyColor; set => this.enemyColor = value;
        }
        public Square[,] Squares
        {
            get => this.squares; set => this.squares = value;
        }
        public ChessGame ChessGame
        {
            get => this.chessGame; set => this.chessGame = value;
        }
    }
}
