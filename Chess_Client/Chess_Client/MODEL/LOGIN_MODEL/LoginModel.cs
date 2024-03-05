using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Client.MODEL.LOGIN_MODEL
{
    public class LoginModel
    {
        private int index;
        private string name;
        private string ip;
        private string inGame;
        private string date;
        private ObservableCollection<string> invitedList;

        public LoginModel(string name, string ip, string inGame,DateTime dateTime)
        {
            this.name = name;
            this.ip = ip;
            this.inGame = inGame;
            this.date = dateTime.ToString("dd-MM-yyyy  HH:mm:ss");
            this.invitedList = new ObservableCollection<string>();
        }

        public LoginModel(int index, string name, string ip, string inGame, string dateTime)
        {
            this.index = index;
            this.name = name;
            this.ip = ip;
            this.inGame = inGame;
            this.date = dateTime;
        }

        public override string ToString()
        {
            return this.name+"|"+this.ip+"|"+this.inGame+"|"+this.date;
        }

        public void removeInvitedListIP(string clientName)
        {
            foreach (string client in this.invitedList)
                if (client.Equals(clientName) == true)
                {
                    this.invitedList.Remove(clientName);
                    break;
                }
        }

        public void addInvitedListIP(string clientName)
        {
            this.invitedList.Add(clientName);
        }


        public ObservableCollection<string> InvitedList
        {
            get => this.invitedList; set => this.invitedList = value;
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
