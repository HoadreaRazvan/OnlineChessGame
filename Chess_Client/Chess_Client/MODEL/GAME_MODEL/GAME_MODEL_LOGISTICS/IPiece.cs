using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS
{
    public enum PieceColor
    {
        White,
        Black,
        None
    }

    public interface IPiece
    {
        public abstract void MyImage_MouseMove(object sender, MouseEventArgs e);
        public abstract void MyImage_MouseUp(object sender, MouseButtonEventArgs e);
        public abstract void MyImage_MouseDown(object sender, MouseButtonEventArgs e);

        public abstract void possiblePositions();
        public abstract void SetChessGame(ChessGame chessGame);

    }
}
