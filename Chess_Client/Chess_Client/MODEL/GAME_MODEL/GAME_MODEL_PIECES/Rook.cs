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
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_PIECES
{
    public class Rook : Piece
    {
        private ChessGame chessGame;

        private bool alreadyMoved;

        public Rook(PieceColor pieceColor, Point point, ChessGame chessGame) : base(pieceColor, point)
        {
            this.chessGame = chessGame;
            this.alreadyMoved = false;
            this.CollisionMoves = new ObservableCollection<string>();
            string pathBackground = this.chessGame.PathTransparent;
            string pathImage = string.Empty;
            if (pieceColor == PieceColor.White)
                pathImage = Directory.GetCurrentDirectory() + @"\Images\RookW.png";
            if (pieceColor == PieceColor.Black)
                pathImage = Directory.GetCurrentDirectory() + @"\Images\RookB.png";
            MyImage myImage = new MyImage(pathBackground, pathImage, this.chessGame.MyColor);

            base.MyImage = myImage;
            base.MyImage.MouseDown += MyImage_MouseDown;
            base.MyImage.MouseUp += MyImage_MouseUp;
            base.MyImage.MouseMove += MyImage_MouseMove;


        }

        public override void MyImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.chessGame.mouseDown(this, e);
        }

        public override void MyImage_MouseMove(object sender, MouseEventArgs e)
        {
            this.chessGame.mouseMove(this, e);
        }

        public override void MyImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.chessGame.mouseUp(this, e);
        }

        public override void possiblePositions()
        {     
            base.ValidMoves = new ObservableCollection<string>();
            string moves;
            string[] movesSplit;


            moves = string.Empty;
            for (int i = (int)this.Point.Y + 1; i < 8; i++)
            {
                if (this.chessGame.Squares[(int)this.Point.X, i].Piece is Empty)
                {
                    moves += $"{(int)this.Point.X},{i}|";
                }
                else
                {
                    if ((this.chessGame.Squares[(int)this.Point.X, i].Piece as Piece).PieceColor == (this.chessGame.Squares[(int)this.Point.X, (int)this.Point.Y].Piece as Piece).PieceColor)
                        moves += $"{(int)this.Point.X},{i},myColorCollision|"; 
                    else
                    if ((this.chessGame.Squares[(int)this.Point.X, i].Piece as Piece).PieceColor != PieceColor.None)
                        moves += $"{(int)this.Point.X},{i},enemyColorCollision|";
                    break;
                }
            }
            if(moves!=string.Empty)
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
            for (int i = (int)this.Point.X + 1; i < 8; i++)
            {
                if (this.chessGame.Squares[i, (int)this.Point.Y].Piece is Empty)
                {
                    moves += $"{i},{(int)this.Point.Y}|";
                }
                else
                {
                    if ((this.chessGame.Squares[i, (int)this.Point.Y].Piece as Piece).PieceColor == (this.chessGame.Squares[(int)this.Point.X, (int)this.Point.Y].Piece as Piece).PieceColor)
                        moves += $"{i},{(int)this.Point.Y},myColorCollision|";
                    else
                    if ((this.chessGame.Squares[i, (int)this.Point.Y].Piece as Piece).PieceColor != PieceColor.None)
                        moves += $"{i},{(int)this.Point.Y},enemyColorCollision|";
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
            for (int i = (int)this.Point.Y - 1; i >= 0; i--) 
            {
                if (this.chessGame.Squares[(int)this.Point.X, i].Piece is Empty)
                {
                    moves += $"{(int)this.Point.X},{i}|";
                }
                else
                {
                    if ((this.chessGame.Squares[(int)this.Point.X, i].Piece as Piece).PieceColor == (this.chessGame.Squares[(int)this.Point.X, (int)this.Point.Y].Piece as Piece).PieceColor)
                        moves += $"{(int)this.Point.X},{i},myColorCollision|";
                    else
                    if ((this.chessGame.Squares[(int)this.Point.X, i].Piece as Piece).PieceColor != PieceColor.None)
                        moves += $"{(int)this.Point.X},{i},enemyColorCollision|";
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
            for (int i = (int)this.Point.X - 1; i >= 0; i--)
            {
                if (this.chessGame.Squares[i, (int)this.Point.Y].Piece is Empty)
                {
                    moves += $"{i},{(int)this.Point.Y}|";
                }
                else
                {
                    if ((this.chessGame.Squares[i, (int)this.Point.Y].Piece as Piece).PieceColor == (this.chessGame.Squares[(int)this.Point.X, (int)this.Point.Y].Piece as Piece).PieceColor)
                        moves += $"{i},{(int)this.Point.Y},myColorCollision|";
                    else
                    if ((this.chessGame.Squares[i, (int)this.Point.Y].Piece as Piece).PieceColor != PieceColor.None)
                        moves += $"{i},{(int)this.Point.Y},enemyColorCollision|";
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


        public bool AlreadyMoved
        {
            get => this.alreadyMoved; set => this.alreadyMoved = value;
        }
        public override void SetChessGame(ChessGame chessGame)
        {
            this.chessGame = chessGame;
        }
    }
}
