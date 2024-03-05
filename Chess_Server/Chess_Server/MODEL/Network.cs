using Chess_Server.CONTROLLER;
using Chess_Server.VIEW;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Chess_Server.MODEL
{
    public class Network 
    {
        private Controller controller;

        private TcpListener tcpListener;
        private Thread listenerThread;
        private ConcurrentDictionary<string, TcpClient> connectedClients = new ConcurrentDictionary<string, TcpClient>();
        private bool isClosing = false;

        public Network()
        {
            tcpListener = new TcpListener(IPAddress.Any, 8888);
            listenerThread = new Thread(new ThreadStart(ListenForClients));
            listenerThread.Start();
        }

        public string? GetLocalIPAddress()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
        }

        public void ListenForClients()
        {
            try
            {
                tcpListener.Start();
                while (!isClosing)
                {
                    try
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                        clientThread.Start(client);
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.Interrupted)
                            break;
                        else
                            throw;
                    }
                }
            }catch(SocketException e)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MyMessageBox myMessageBox = new MyMessageBox("There is already a server at this IP address!");
                        myMessageBox.Left = this.controller.View.Left + (this.controller.View.Width - myMessageBox.Width) / 2;
                        myMessageBox.Top = this.controller.View.Top + (this.controller.View.Height - myMessageBox.Height) / 2;
                        myMessageBox.Closed += (sender, e) => { this.controller.View.Close(); };
                        myMessageBox.Show();
                    });
                });
            }
        }

        public void HandleClientComm(object clientObj)
        {
            TcpClient tcpClient = (TcpClient)clientObj;
            string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            connectedClients.TryAdd(clientEndPoint, tcpClient);
            NetworkStream clientStream = tcpClient.GetStream();
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
                    this.controller.decode(receivedMessage);
                });
            }
            connectedClients.TryRemove(clientEndPoint, out _);
            tcpClient.Close();
        }

        public void BroadcastMessage(string ipClient, string message)
        {
            foreach (var clientPair in connectedClients)
            {
                if (clientPair.Key == ipClient)
                {
                    NetworkStream clientStream = clientPair.Value.GetStream();
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    clientStream.Write(data, 0, data.Length);
                    clientStream.Flush();
                    break;
                }
            }
        }

        public void disconnectIPClient(string ipClient)
        {
            foreach (var clientPair in connectedClients)
            {
                if (clientPair.Key == ipClient)
                {
                    NetworkStream clientStream = clientPair.Value.GetStream();
                    byte[] data = Encoding.ASCII.GetBytes("shutdown");
                    clientStream.Write(data, 0, data.Length);
                    clientStream.Flush();
                    break;
                }
            }
        }

        public void BroadcastAll(string message)
        {
            foreach (var clientPair in connectedClients)
            {
                bool ok = true;
                foreach (Client client in this.controller.View.MainView.Clients)
                    if (client.InGame.Equals("In Game") == true && clientPair.Key == client.IP)
                    {
                        ok = false;
                        break;
                    }
                if (ok == true)
                {
                    NetworkStream clientStream = clientPair.Value.GetStream();
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    clientStream.Write(data, 0, data.Length);
                    clientStream.Flush();
                }
            }
        }

        public void BroadcastAllExcept(string ipClient, string message)
        {
            foreach (var clientPair in connectedClients)
            {
                if (clientPair.Key != ipClient)
                {
                    bool ok = true;
                    foreach (Client client in this.controller.View.MainView.Clients)
                        if (client.InGame.Equals("In Game") == true && clientPair.Key == client.IP)
                        {
                            ok = false;
                            break;
                        }
                    if (ok == true)
                    {
                        NetworkStream clientStream = clientPair.Value.GetStream();
                        byte[] data = Encoding.ASCII.GetBytes(message);
                        clientStream.Write(data, 0, data.Length);
                        clientStream.Flush();
                    }
                }
            }
        }

        public void close()
        {
            isClosing = true;
            foreach (var clientPair in connectedClients)
                clientPair.Value.Close();
            connectedClients.Clear();
            tcpListener.Stop();
        }

        public ConcurrentDictionary<string, TcpClient> ConnectedClients
        {
            get => this.connectedClients; set => this.connectedClients = value;
        }

        public Controller Controller
        {
            get => this.controller; set => this.controller = value;
        }

    }
}
