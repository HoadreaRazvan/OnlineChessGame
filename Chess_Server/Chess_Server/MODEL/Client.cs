using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess_Server.MODEL
{
    public class Client
    {
        private int index;
        private string name;
        private string ip;
        private string inGame;
        private string date;


        public Client(int index, string name, string ip, string inGame,string dateTime)
        {
            this.index = index;
            this.name = name;
            this.ip = ip;
            this.inGame = inGame;
            this.date = dateTime;
        }


        public override string ToString()
        {
            return this.name + "|" + this.ip + "|" + this.inGame + "|" + this.date;
        }


        public int Index
        {
            get => this.index; set => this.index = value;
        }
        public string Name
        {
            get => this.name; set => this.name = value;
        }
        public string IP
        {
            get => this.ip; set => this.ip = value;
        }
        public string InGame
        {
            get => this.inGame; set => this.inGame = value;
        }
        public string Date
        {
            get => this.date; set => this.date = value;
        }
    }
}
