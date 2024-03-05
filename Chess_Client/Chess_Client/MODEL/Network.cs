using Chess_Client.CONTROLLER.CONNECT_CONTROLLER;
using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using Chess_Client.CONTROLLER.LOGIN_CONTROLLER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Chess_Client.MODEL.LOGIN_MODEL;
using Chess_Client.VIEW.CONNECT_VIEW;
using Chess_Client.VIEW.LOGIN_VIEW;

namespace Chess_Client.MODEL
{
    public class Network
    {
        private LoginController loginController;
        private ConnectController connectController;
        private GameController gameController;

        private LoginModel you,opponent;

        private TcpClient tcpClient;
        private Thread clientThread;
        private bool isConnected;
        private string ipClient;

        public Network(string serverIp)
        {
            this.ConnectToServer(serverIp);
        }

        public string? GetLocalIPAddress()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
        }

        private void ConnectToServer(string serverIp)
        {
            try
            {
                if(serverIp.Equals("localHost") ==true)
                    this.tcpClient = new TcpClient(GetLocalIPAddress(), 8888);
                else
                    this.tcpClient = new TcpClient(serverIp, 8888);
                this.ipClient = this.GetLocalIPAddress() + ":" + tcpClient.Client.LocalEndPoint.ToString().Split(":")[tcpClient.Client.LocalEndPoint.ToString().Split(":").Length - 1];
                clientThread = new Thread(new ThreadStart(ListenForMessages));
                clientThread.Start();
                this.isConnected = true;
            }
            catch (Exception ex)
            {
                this.isConnected=false;
            }
        }

        private void ListenForMessages()
        {
            NetworkStream clientStream = this.tcpClient.GetStream();
            byte[] message = new byte[4096];
            int bytesRead;
            while (true)
            {
                bytesRead = 0;
                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }
                if (bytesRead == 0)
                    break;
                string receivedMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (receivedMessage.Equals("shutdown"))
                    {
                        if (this.connectController != null)
                        {
                            this.connectController.ConnectView.HeaderConnectView.back(0);
                        }else
                        if(this.gameController != null)
                        {
                            this.gameController.GameView.HeaderGameView.close(1);
                        }

                    }
                    else
                    if (receivedMessage.Split("|")[2].Equals("loginController") == true)
                    {
                        this.loginController.decode(receivedMessage);
                    }
                    else
                    if (receivedMessage.Split("|")[2].Equals("connectController") == true)
                    {
                        if (this.connectController != null)
                            this.connectController.decode(receivedMessage);
                    }
                    else
                    if (receivedMessage.Split("|")[2].Equals("gameController") == true)
                    {
                        this.gameController.decode(receivedMessage);
                    }
                }); 
            }
        }


        public void SendMessage(string message)
        {
            NetworkStream clientStream = tcpClient.GetStream();
            byte[] data = Encoding.ASCII.GetBytes(message);
            clientStream.Write(data, 0, data.Length);
            clientStream.Flush();
        }

        public void close()
        {
            this.SendMessage(this.you.IP + "|all|disconnected|" + this.you.Name);
            tcpClient.Close();
        }

        public TcpClient TcpClient
        {
            get => this.tcpClient; set => this.tcpClient = value;
        }
        public LoginModel You
        {
            get => this.you; set => this.you = value;
        }
        public LoginModel Opponent
        {
            get => this.opponent; set => this.opponent = value;
        }
        public string IpClient
        {
            get => this.ipClient; set => this.ipClient = value;
        }
        public bool IsConnected
        {
            get => this.isConnected; set => this.isConnected = value;
        }
        public LoginController LoginController
        {
            get => this.loginController; set => this.loginController = value;
        }
        public ConnectController ConnectController
        {
            get => this.connectController; set => this.connectController = value;
        }
        public GameController GameController
        {
            get => this.gameController; set => this.gameController = value;
        }
    }
}
