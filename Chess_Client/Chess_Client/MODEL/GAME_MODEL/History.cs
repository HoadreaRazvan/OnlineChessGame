using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Client.MODEL.GAME_MODEL
{
    public class History
    {
        private int index;
        private string firstMove;
        private string secondMove;

        public History(int index, string firstMove, string secondMove)
        {
            this.index = index;
            this.firstMove = firstMove;
            this.secondMove = secondMove;
        }

        public override string ToString()
        {
            return this.index + "|" + this.firstMove;
        }

        public int Index
        {
            get => this.index; set => this.index = value;
        }
        public string FirstMove
        {
            get => this.firstMove; set => this.firstMove = value;
        }
        public string SecondMove
        {
            get => this.secondMove; set => this.secondMove = value;
        }
    }
}
