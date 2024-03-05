using Chess_Client.MODEL.LOGIN_MODEL;
using Chess_Client.MODEL;
using Chess_Client.VIEW.LOGIN_VIEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess_Client.VIEW.CONNECT_VIEW;
using System.Windows;
using System.Collections.ObjectModel;
using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using Chess_Client.VIEW.GAME_VIEW;
using System.Reflection.Metadata;
using Chess_Client.VIEW;

namespace Chess_Client.CONTROLLER.CONNECT_CONTROLLER
{
    public class ConnectController
    {
        private Network network;
        private ConnectView connectView;

        public ConnectController(Network network, ConnectView connectView)
        {
            this.network = network;
            this.network.ConnectController = this;
            this.connectView = connectView;
            this.connectView.ConnectController = this;

            this.connectView.MainConnectView.LblName.Content += this.network.You.Name + " !";
        }

        public void decode(string code)
        {
            string[] splitCode = code.Split("|");
           
            if (splitCode[3].Equals("tableRequest") == true)
            {
                int k = 1;
                for (int i = 4; i < splitCode.Length; i++)
                {
                    if (splitCode[i].Equals(this.network.You.Name) == false)
                    {
                        this.connectView.MainConnectView.Clients.Add(new LoginModel(k++, splitCode[i], splitCode[i + 1], splitCode[i + 2], splitCode[i + 3]));
                    }
                    i += 3;
                }
                object focusRow = null;
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = this.connectView.MainConnectView.Clients;
                if(this.connectView.MainConnectView.DGConnectedClientsTable.Items.Count !=0)
                {
                    focusRow = this.connectView.MainConnectView.DGConnectedClientsTable.Items[this.connectView.MainConnectView.DGConnectedClientsTable.Items.Count - 1];
                    this.connectView.MainConnectView.DGConnectedClientsTable.ScrollIntoView(focusRow);
                }
            }
            else
            if (splitCode[3].Equals("addTable") == true)
            {
                object focusRow = null;
                this.connectView.MainConnectView.Clients.Add(new LoginModel(this.connectView.MainConnectView.Clients.Count + 1, splitCode[4], splitCode[5], splitCode[6], splitCode[7]));
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = this.connectView.MainConnectView.Clients;
                if (this.connectView.MainConnectView.DGConnectedClientsTable.Items.Count != 0)
                {
                    focusRow = this.connectView.MainConnectView.DGConnectedClientsTable.Items[this.connectView.MainConnectView.DGConnectedClientsTable.Items.Count - 1];
                    this.connectView.MainConnectView.DGConnectedClientsTable.ScrollIntoView(focusRow);
                }
            }
            else
            if (splitCode[3].Equals("removeTable") ==true)
            {
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = new ObservableCollection<LoginModel>();
                this.connectView.MainConnectView.DGInviteTable.ItemsSource = new ObservableCollection<LoginModel>();
                foreach (LoginModel client in this.connectView.MainConnectView.Clients)
                {
                    if (client.Name.Equals(splitCode[4])==true)
                    {
                        this.connectView.MainConnectView.Clients.Remove(client);
                        break;
                    }
                }
                for (int i = 0; i < this.connectView.MainConnectView.Clients.Count; i++)
                {
                    this.connectView.MainConnectView.Clients[i].Index = i+1;
                }
                foreach(LoginModel client in this.connectView.MainConnectView.Invites)
                {
                    if (client.Name.Equals(splitCode[4]) == true)
                    {
                        this.connectView.MainConnectView.Invites.Remove(client);
                        break;
                    }
                }
                for (int i = 0; i < this.connectView.MainConnectView.Invites.Count; i++)
                {
                    this.connectView.MainConnectView.Invites[i].Index = i + 1;
                }
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = this.connectView.MainConnectView.Clients;
                this.connectView.MainConnectView.DGInviteTable.ItemsSource = this.connectView.MainConnectView.Invites;
            }
            else
            if (splitCode[3].Equals("addInvite") ==true)
            {
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = new ObservableCollection<LoginModel>();
                this.connectView.MainConnectView.DGInviteTable.ItemsSource = new ObservableCollection<LoginModel>();
                this.connectView.MainConnectView.Invites.Add(new LoginModel(this.connectView.MainConnectView.Invites.Count + 1, splitCode[4], "", "", ""));
                this.connectView.MainConnectView.DGInviteTable.ItemsSource = this.connectView.MainConnectView.Invites;

                foreach (LoginModel client in this.connectView.MainConnectView.Clients)
                {
                    if (client.Name.Equals(splitCode[4]) == true)
                    {
                        client.InGame = "Cancel Invite";
                        break;
                    }
                }
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = this.connectView.MainConnectView.Clients;

                foreach (object window in Application.Current.Windows) 
                    if (window is MyMessageBox) 
                        (window as MyMessageBox).Close();

                this.connectView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("You received an invitation from " + splitCode[4]+" !");
                myMessageBox.Left = this.connectView.Left + (this.connectView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.connectView.Top + (this.connectView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.connectView.IsEnabled = true; };
                myMessageBox.Show();

            }
            else
            if (splitCode[3].Equals("cancelInvite") == true)
            {
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = new ObservableCollection<LoginModel>();
                this.connectView.MainConnectView.DGInviteTable.ItemsSource = new ObservableCollection<LoginModel>();

                foreach (LoginModel client in this.connectView.MainConnectView.Clients)
                {
                    if (client.Name.Equals(splitCode[5]) == true)
                    {
                        client.InGame = "Available";
                        break;
                    }
                }

                foreach (LoginModel client in this.connectView.MainConnectView.Invites)
                {
                    if (client.Name.Equals(splitCode[5]) == true)
                    {
                        this.connectView.MainConnectView.Invites.Remove(client);
                        break;
                    }
                }
                for (int i = 0; i < this.connectView.MainConnectView.Invites.Count; i++)
                {
                    this.connectView.MainConnectView.Invites[i].Index = i + 1;
                }

                this.connectView.MainConnectView.DGInviteTable.ItemsSource = this.connectView.MainConnectView.Invites;
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = this.connectView.MainConnectView.Clients;

                foreach (object window in Application.Current.Windows) 
                    if (window is MyMessageBox) 
                        (window as MyMessageBox).Close();
                
                this.connectView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox(splitCode[5] + " withdrew his invitation!");
                myMessageBox.Left = this.connectView.Left + (this.connectView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.connectView.Top + (this.connectView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.connectView.IsEnabled = true; };
                myMessageBox.Show();
            }
            else
            if (splitCode[3].Equals("prepareTheGame") == true)
            {
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = new ObservableCollection<LoginModel>();
                this.connectView.MainConnectView.DGInviteTable.ItemsSource = new ObservableCollection<LoginModel>();

                foreach (LoginModel client in this.connectView.MainConnectView.Clients)
                {
                    if (client.Name.Equals(splitCode[4]) == true)
                    {
                        client.InGame = "In Game";
                        break;
                    }
                }

                foreach (LoginModel client in this.connectView.MainConnectView.Invites)
                {
                    if (client.Name.Equals(splitCode[4]) == true)
                    {
                        this.connectView.MainConnectView.Invites.Remove(client);
                        break;
                    }
                }
                for (int i = 0; i < this.connectView.MainConnectView.Invites.Count; i++)
                {
                    this.connectView.MainConnectView.Invites[i].Index = i + 1;
                }

                this.connectView.MainConnectView.DGInviteTable.ItemsSource = this.connectView.MainConnectView.Invites;
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = this.connectView.MainConnectView.Clients;
            }else
            if (splitCode[3].Equals("prepareTheGameExceptGamers") == true)
            {
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = new ObservableCollection<LoginModel>();
                foreach (LoginModel client in this.connectView.MainConnectView.Clients)
                {
                    if (client.Name.Equals(splitCode[4]) == true || client.Name.Equals(splitCode[5])==true)
                    {
                        client.InGame = "In Game";
                    }
                }
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = this.connectView.MainConnectView.Clients;
            }
            else
            if (splitCode[3].Equals("startGame") == true)
            {
                this.Network.Opponent = new LoginModel(splitCode[4], splitCode[5], "In Game", DateTime.Now);
                this.ConnectView.Hide();
                GameView gameView = new GameView();
                GameController gameController = new GameController(this.Network, gameView,int.Parse(splitCode[6]),int.Parse(splitCode[7]));
                this.Network.ConnectController = null;
                this.Network.GameController = gameController;
                this.ConnectView.Close();
            }
            else
            if (splitCode[3].Equals("gameOver") == true)
            {
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = new ObservableCollection<LoginModel>();
                foreach (LoginModel client in this.connectView.MainConnectView.Clients)
                {
                    if (client.IP.Equals(splitCode[0]) == true)
                    {
                        client.InGame = "Available";
                    }
                }
                this.connectView.MainConnectView.DGConnectedClientsTable.ItemsSource = this.connectView.MainConnectView.Clients;
            }

        }
        
        public Network Network
        {
            get => this.network; set => this.network = value;
        }
        public ConnectView ConnectView
        {
            get => this.connectView; set => this.connectView = value;
        }
    }
}
