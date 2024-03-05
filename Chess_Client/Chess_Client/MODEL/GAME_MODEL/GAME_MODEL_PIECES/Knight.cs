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
    public class Knight : Piece
    {
        private ChessGame chessGame;


        public Knight(PieceColor pieceColor, Point point, ChessGame chessGame) : base(pieceColor, point)
        {
            this.chessGame = chessGame; 
            this.CollisionMoves = new ObservableCollection<string>();
            string pathBackground = this.chessGame.PathTransparent;
            string pathImage = string.Empty;
            if (pieceColor == PieceColor.White)
                pathImage = Directory.GetCurrentDirectory() + @"\Images\KnightW.png";
            if (pieceColor == PieceColor.Black)
                pathImage = Directory.GetCurrentDirectory() + @"\Images\KnightB.png";
            MyImage myImage = new MyImage(pathBackground, pathImage);

            base.MyImage = myImage;
            base.MyImage.MouseDown += MyImage_MouseDown;
            base.MyImage.MouseUp += MyImage_MouseUp;
            base.MyImage.MouseMove += MyImage_MouseMove;

        }

        public override void MyImage_MouseMove(object sender, MouseEventArgs e)
        {
            this.chessGame.mouseMove(this, e);
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

            int[] dX = { -2, -1, 1, 2, 2, 1, -1, -2 }; 
            int[] dY = { 1, 2, 2, 1, -1, -2, -2, -1 };

            moves = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                if ((int)this.Point.X + dX[i] >= 0 && (int)this.Point.X + dX[i] < 8 && (int)this.Point.Y + dY[i] >= 0 && (int)this.Point.Y + dY[i] < 8) 
                {
                    if (this.chessGame.Squares[(int)this.Point.X + dX[i], (int)this.Point.Y + dY[i]].Piece is Empty)
                            moves += $"{(int)this.Point.X + dX[i]},{(int)this.Point.Y + dY[i]}|";
                    else
                    {
                        if ((this.chessGame.Squares[(int)this.Point.X + dX[i], (int)this.Point.Y + dY[i]].Piece as Piece).PieceColor == (this.chessGame.Squares[(int)this.Point.X, (int)this.Point.Y].Piece as Piece).PieceColor)
                            moves += $"{(int)this.Point.X + dX[i]},{(int)this.Point.Y + dY[i]},myColorCollision|";
                        else
                        if ((this.chessGame.Squares[(int)this.Point.X + dX[i], (int)this.Point.Y + dY[i]].Piece as Piece).PieceColor != PieceColor.None)
                            moves += $"{(int)this.Point.X + dX[i]},{(int)this.Point.Y + dY[i]},enemyColorCollision|";                
                    }
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
