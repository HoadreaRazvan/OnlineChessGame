using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS
{
    public abstract class Piece : IPiece
    {
        private PieceColor pieceColor;
        private MyImage myImage;
        private Point point;
        private ObservableCollection<string> validMoves, collisionMoves;

        public Piece(PieceColor pieceColor, Point point)
        {
            this.pieceColor = pieceColor;
            this.point = point;
        }


        public PieceColor PieceColor
        {
            get => this.pieceColor; set => this.pieceColor = value;
        }
        public MyImage MyImage
        {
            get => this.myImage; set => this.myImage = value;
        }
        public Point Point
        {
            get => this.point; set => this.point = value;
        }
        public ObservableCollection<string> ValidMoves
        {
            get => this.validMoves; set => this.validMoves = value;
        }
        public ObservableCollection<string> CollisionMoves
        {
            get => this.collisionMoves; set => this.collisionMoves = value;
        }

        public abstract void MyImage_MouseDown(object sender, MouseButtonEventArgs e);
        public abstract void MyImage_MouseMove(object sender, MouseEventArgs e);
        public abstract void MyImage_MouseUp(object sender, MouseButtonEventArgs e);

        public abstract void possiblePositions();
        public abstract void SetChessGame(ChessGame chessGame);
    }
}
