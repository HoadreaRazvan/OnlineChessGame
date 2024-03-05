using Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS;
using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_PIECES
{
    public class Bishop : Piece
    {
        private ChessGame chessGame;

        public Bishop(PieceColor pieceColor, Point point, ChessGame chessGame) : base(pieceColor, point)
        {
            this.chessGame = chessGame;
            this.CollisionMoves = new ObservableCollection<string>();
            string pathBackground = this.chessGame.PathTransparent;
            string pathImage = string.Empty;
            if (pieceColor == PieceColor.White)
                pathImage = Directory.GetCurrentDirectory() + @"\Images\BishopW.png";
            if (pieceColor == PieceColor.Black)
                pathImage = Directory.GetCurrentDirectory() + @"\Images\BishopB.png";
            MyImage myImage = new MyImage(pathBackground, pathImage);

            base.MyImage = myImage;
            base.MyImage.MouseDown += MyImage_MouseDown;
            base.MyImage.MouseUp += MyImage_MouseUp;
            base.MyImage.MouseMove += MyImage_MouseMove;


        }

        public override void MyImage_MouseMove(object sender, MouseEventArgs e)
        {
            this.chessGame.mouseMove(this,e);
        }

        public override void MyImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.chessGame.mouseUp(this, e);
        }

        public override void MyImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.chessGame.mouseDown(this, e);
        }

        public override void possiblePositions()
        {
            base.ValidMoves = new ObservableCollection<string>();
            string moves;
            string[] movesSplit;


            moves = string.Empty;
            for (int i = (int)this.Point.X - 1, j = (int)this.Point.Y + 1; i >= 0 && j < 8; i--, j++)
            {
                if (this.chessGame.Squares[i, j].Piece is Empty)
                {
                    moves += $"{i},{j}|";
                }
                else
                {
                    if ((this.chessGame.Squares[i, j].Piece as Piece).PieceColor == (this.chessGame.Squares[(int)this.Point.X, (int)this.Point.Y].Piece as Piece).PieceColor)
                        moves += $"{i},{j},myColorCollision|";
                    else
                    if ((this.chessGame.Squares[i, j].Piece as Piece).PieceColor != PieceColor.None)
                        moves += $"{i},{j},enemyColorCollision|";
                    break;
                }
            }
            if (moves != string.Empty)
                moves = moves.Substring(0, moves.Length - 1);
            movesSplit = moves.Split("|");
            foreach (string move in movesSplit)
            {
                if (moves != String.Empty)
                {
                    base.ValidMoves.Add(move);
                    (this.chessGame.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).CollisionMoves.Add(((int)this.Point.X).ToString() + "," + ((int)this.Point.Y).ToString() + "," + this.PieceColor.ToString() + "|" + moves);
                }
            }


            moves = string.Empty;
            for (int i = (int)this.Point.X + 1, j = (int)this.Point.Y + 1; i < 8 && j < 8; i++, j++)
            {
                if (this.chessGame.Squares[i, j].Piece is Empty)
                {
                    moves += $"{i},{j}|";
                }
                else
                {
                    if ((this.chessGame.Squares[i, j].Piece as Piece).PieceColor == (this.chessGame.Squares[(int)this.Point.X, (int)this.Point.Y].Piece as Piece).PieceColor)
                        moves += $"{i},{j},myColorCollision|";
                    else
                    if ((this.chessGame.Squares[i, j].Piece as Piece).PieceColor != PieceColor.None)
                        moves += $"{i},{j},enemyColorCollision|";
                    break;
                }
            }
            if (moves != string.Empty)
                moves = moves.Substring(0, moves.Length - 1);
            movesSplit = moves.Split("|");
            foreach (string move in movesSplit)
            {
                if (moves != String.Empty)
                {
                    base.ValidMoves.Add(move);
                    (this.chessGame.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).CollisionMoves.Add(((int)this.Point.X).ToString() + "," + ((int)this.Point.Y).ToString() + "," + this.PieceColor.ToString() + "|" + moves);
                }
            }


            moves = string.Empty;
            for (int i = (int)this.Point.X + 1, j = (int)this.Point.Y - 1; i < 8 && j >= 0; i++, j--)
            {
                if (this.chessGame.Squares[i, j].Piece is Empty)
                {
                    moves += $"{i},{j}|";
                }
                else
                {
                    if ((this.chessGame.Squares[i, j].Piece as Piece).PieceColor == (this.chessGame.Squares[(int)this.Point.X, (int)this.Point.Y].Piece as Piece).PieceColor)
                        moves += $"{i},{j},myColorCollision|";
                    else
                    if ((this.chessGame.Squares[i, j].Piece as Piece).PieceColor != PieceColor.None)
                        moves += $"{i},{j},enemyColorCollision|";
                    break;
                }
            }
            if (moves != string.Empty)
                moves = moves.Substring(0, moves.Length - 1);
            movesSplit = moves.Split("|");
            foreach (string move in movesSplit)
            {
                if (moves != String.Empty)
                {
                    base.ValidMoves.Add(move);
                    (this.chessGame.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).CollisionMoves.Add(((int)this.Point.X).ToString() + "," + ((int)this.Point.Y).ToString() + "," + this.PieceColor.ToString() + "|" + moves);
                }
            }


            moves = string.Empty;
            for (int i = (int)this.Point.X - 1, j = (int)this.Point.Y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (this.chessGame.Squares[i, j].Piece is Empty)
                {
                    moves += $"{i},{j}|";
                }
                else
                {
                    if ((this.chessGame.Squares[i, j].Piece as Piece).PieceColor == (this.chessGame.Squares[(int)this.Point.X, (int)this.Point.Y].Piece as Piece).PieceColor)
                        moves += $"{i},{j},myColorCollision|";
                    else
                    if ((this.chessGame.Squares[i, j].Piece as Piece).PieceColor != PieceColor.None)
                        moves += $"{i},{j},enemyColorCollision|";
                    break;
                }
            }
            if (moves != string.Empty)
                moves = moves.Substring(0, moves.Length - 1);
            movesSplit = moves.Split("|");
            foreach (string move in movesSplit)
            {
                if (moves != String.Empty)
                {
                    base.ValidMoves.Add(move);
                    (this.chessGame.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).CollisionMoves.Add(((int)this.Point.X).ToString() + "," + ((int)this.Point.Y).ToString() + "," + this.PieceColor.ToString() + "|" + moves);
                }
            }
        }



        public override void SetChessGame(ChessGame chessGame)
        {
            this.chessGame = chessGame;
        }
    }
}
