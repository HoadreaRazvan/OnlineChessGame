using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS
{
    public class Square //: ICloneable
    {
        private object piece;

        public Square(object piece)
        {
            this.piece = piece;
        }


        public object Piece
        {
            get => this.piece; set => this.piece = value;
        }

        public Square Clone()
        {
            return new Square(piece);
        }
    }
}
