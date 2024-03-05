using Chess_Client.CONTROLLER.CONNECT_CONTROLLER;
using Chess_Client.MODEL;
using Chess_Client.MODEL.LOGIN_MODEL;
using Chess_Client.VIEW;
using Chess_Client.VIEW.CONNECT_VIEW;
using Chess_Client.VIEW.LOGIN_VIEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess_Client.CONTROLLER.LOGIN_CONTROLLER
{
    public class LoginController
    {
        private LoginView loginView;
        private Network network;

        public LoginController(LoginView loginView)
        {
            this.loginView = loginView;
            this.loginView.LoginController = this;
        }


        public void decode(string code)
        {
            string []splitCode=code.Split("|");
            if (splitCode[3].Equals("notValid"))
            {
                this.network.You.Name += "#";
                this.network.close();

                foreach (object window in Application.Current.Windows) 
                    if (window is MyMessageBox) 
                        (window as MyMessageBox).Close();

                this.loginView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("There is someone with your name already or the server is full!");
                myMessageBox.Left = this.loginView.Left + (this.loginView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.loginView.Top + (this.loginView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.loginView.IsEnabled = true; this.loginView.MainLoginView.BtnConnect.IsEnabled = true; };
                myMessageBox.Show();

                
            }
            else
            if (splitCode[3].Equals("isValid"))
            {
                this.network.LoginController = null;
                ConnectView connectView = new ConnectView();
                ConnectController connectController = new ConnectController(this.network, connectView);
                this.network.SendMessage(this.network.You.IP + "|" + this.network.You.IP + "|connectController|tableRequest|"+this.network.You.ToString());
                this.LoginView.Close();
            }
        }

        public Network Network
        {
            get => this.network; set => this.network = value;
        }

        public LoginView LoginView
        {
            get => this.loginView; set => this.loginView = value;
        }
    }
}
