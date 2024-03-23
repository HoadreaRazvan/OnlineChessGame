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
    public class Pawn : Piece
    {
        private ChessGame chessGame;
        private int indexFirstMove
;

        public Pawn(PieceColor pieceColor, Point point, ChessGame chessGame) : base(pieceColor, point)
        {
            this.chessGame = chessGame;
            this.indexFirstMove = 0;
            this.CollisionMoves = new ObservableCollection<string>();
            string pathBackground = this.chessGame.PathTransparent;
            string pathImage = string.Empty;
            if (pieceColor == PieceColor.White)
                pathImage = Directory.GetCurrentDirectory() + @"\Images\PawnW.png";
            if (pieceColor == PieceColor.Black)
                pathImage = Directory.GetCurrentDirectory() + @"\Images\PawnB.png";
            MyImage myImage = new MyImage(pathBackground, pathImage, this.chessGame.MyColor);

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
            bool continueMove = true; ;

            int[] dX = { 1, 2 };
            int[] dY = { 1, 0, -1 };

            moves = string.Empty;
            for (int i = 0; i < 2 - this.indexFirstMove && continueMove==true; i++)
            {
                int dXmyColorEnPassant = dX[i];
                if (this.PieceColor == this.chessGame.MyColor)
                    dXmyColorEnPassant *= -1;
                for (int j = 0; j < dY.Length; j++)
                {
                    if ((int)this.Point.X + dXmyColorEnPassant >= 0 && (int)this.Point.X + dXmyColorEnPassant < 8 && (int)this.Point.Y + dY[j] >= 0 && (int)this.Point.Y + dY[j] < 8)
                    {
                        if (this.chessGame.Squares[(int)this.Point.X + dXmyColorEnPassant, (int)this.Point.Y + dY[j]].Piece is Empty && dY[j] == 0)
                        {
                            moves += $"{(int)this.Point.X + dXmyColorEnPassant},{(int)this.Point.Y + dY[j]}|";
                        }
                        else
                        {
                            if (i == 0 && dY[j] == 0 && this.chessGame.Squares[(int)this.Point.X + dXmyColorEnPassant, (int)this.Point.Y + dY[j]].Piece is not Empty)
                                continueMove = false;
                            if (dY[j] != 0 && i != 1)
                            {
                                if ((this.chessGame.Squares[(int)this.Point.X + dXmyColorEnPassant, (int)this.Point.Y + dY[j]].Piece as Piece).PieceColor.ToString().Equals(this.chessGame.EnemyColor.ToString()))
                                    moves += $"{(int)this.Point.X + dXmyColorEnPassant},{(int)this.Point.Y + dY[j]},enemyColorCollision|";
                            }                          
                        }
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



        public int IndexFirstMove
        {
            get => this.indexFirstMove; set => this.indexFirstMove = value;
        }   
        public override void SetChessGame(ChessGame chessGame)
        {
            this.chessGame = chessGame;
        }
    }
}
