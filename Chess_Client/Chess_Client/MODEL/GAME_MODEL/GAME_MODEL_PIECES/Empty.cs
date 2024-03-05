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
    public class Empty : Piece
    {
        private ChessGame chessGame;

        public Empty(PieceColor pieceColor, Point point,ChessGame chessGame) : base(pieceColor, point)
        {
            this.chessGame = chessGame;
            this.CollisionMoves = new ObservableCollection<string>();
            string path = Directory.GetCurrentDirectory() + @"\Images\Transparent.png";
            MyImage myImage = new MyImage(path, path);

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

        }



        public override void SetChessGame(ChessGame chessGame)
        {
            this.chessGame = chessGame;
        }
    }
}
