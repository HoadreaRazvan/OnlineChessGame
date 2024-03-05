using Chess_Server.MODEL;
using Chess_Server.VIEW;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Chess_Server.CONTROLLER
{
    public class Controller
    {
        private Network network;
        private View view;

        public Controller(Network network, View view)
        {
            this.network = network;
            this.network.Controller = this;
            this.view = view;
            this.view.Controller = this;
            if (this.network.GetLocalIPAddress().Equals("127.0.0.1") == true)
                view.HeaderView.LblServerIp.Content += this.network.GetLocalIPAddress() + " (No Internet)";
            else
                view.HeaderView.LblServerIp.Content += this.network.GetLocalIPAddress();
        }

        public void decode(string code)
        {
            string[] splitCode = code.Split("|");
            if (splitCode[2].Equals("loginController") == true)
                loginControllerDecode(splitCode);
            else
            if (splitCode[2].Equals("connectController") == true)
                connectControllerDecode(splitCode);
            else
            if (splitCode[2].Equals("gameController") == true)
                gameControllerDecode(splitCode);
            else
            if (splitCode[2].Equals("disconnected") == true)
                disconnectedDecode(splitCode);




        }

        public void gameControllerDecode(string[] splitCode)
        {
            if (splitCode[3].Equals("sendMessage") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|sendMessage|" + splitCode[4]);
            }
            else
            if (splitCode[3].Equals("leave") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|leave");
            }
            else
            if (splitCode[3].Equals("resign") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|resign");
            }
            else
            if (splitCode[3].Equals("gameOver") == true)
            {
                this.view.MainView.DGConnectedClientsTable.ItemsSource = new ObservableCollection<Client>();
                foreach (Client client in this.view.MainView.Clients)
                {
                    if (client.IP.Equals(splitCode[0]) == true)
                    {
                        client.InGame = "Available";
                    }
                }
                this.view.MainView.DGConnectedClientsTable.ItemsSource = this.view.MainView.Clients;

                foreach (Client client in this.view.MainView.Clients)
                    if (client.InGame.Equals("Available") == true)
                        this.network.BroadcastAllExcept(client.IP, splitCode[0] + "|all|connectController|gameOver");
            }
            else
            if (splitCode[3].Equals("movePiece") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|movePiece|" + splitCode[4] + "|" + splitCode[5] + "|" + splitCode[6] + "|" + splitCode[7] + "|" + splitCode[8] + "|" + splitCode[9]);
            }
            else
            if (splitCode[3].Equals("changePawn") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|changePawn|" + splitCode[4]);
            }
            else
            if (splitCode[3].Equals("moveCastle") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|moveCastle|" + splitCode[4] + "|" + splitCode[5]);
            }
            else
            if (splitCode[3].Equals("draw") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|draw|" + splitCode[4]);
            }
            else
            if (splitCode[3].Equals("youLose") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|youLose");
            }
            else
            if (splitCode[3].Equals("youWin") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|youWin");
            }
            else
            if (splitCode[3].Equals("rematchRequest") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|rematchRequest");
            }
            if (splitCode[3].Equals("rematchAccept") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|gameController|rematchAccept");
            }
            object focusRow = null;
            string encode = String.Empty, clientName = String.Empty;
            foreach (string code in splitCode) encode += code + "|"; encode = encode.Substring(0, encode.Length - 1);
            foreach (Client client in this.view.MainView.Clients) if (client.IP.Equals(splitCode[0]) == true) { clientName = client.Name; break; }
            view.MainView.LogEntries.Add(new LogEntry(view.MainView.LogEntries.Count + 1, clientName, encode));
            focusRow = view.MainView.DGLogActivityTable.Items[view.MainView.DGLogActivityTable.Items.Count - 1];
            view.MainView.DGLogActivityTable.ScrollIntoView(focusRow);
            view.MainView.DGLogActivityTable.ItemsSource = view.MainView.LogEntries;
        }

        public void connectControllerDecode(string[] splitCode)
        {
            object focusRow = null;
            string encode = String.Empty, clientName = String.Empty;
            if (splitCode[3].Equals("tableRequest") == true)
            {
                string table = string.Empty;
                foreach (Client client in this.view.MainView.Clients)
                    table += client.ToString() + "|";
                if (table.Length != 0)
                    table = table.Substring(0, table.Length - 1);

                this.network.BroadcastMessage(splitCode[0], splitCode[0] + "|" + splitCode[1] + "|" + splitCode[2] + "|tableRequest|" + table);

                if (splitCode.Length == 8)
                {
                    this.network.BroadcastAllExcept(splitCode[0], "server|allExcept" + splitCode[0] + "|connectController|addTable|" + splitCode[4] + "|" + splitCode[5] + "|" + splitCode[6] + "|" + splitCode[7]);
                }
            }
            else
            if (splitCode[3].Equals("addInvite") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|" + splitCode[2] + "|addInvite|" + splitCode[4]);
            }
            else
            if (splitCode[3].Equals("cancelInvite") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|" + splitCode[2] + "|cancelInvite|" + splitCode[4] + "|" + splitCode[5]);
            }
            else
            if (splitCode[3].Equals("prepareTheGame") == true)
            {
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|" + splitCode[2] + "|prepareTheGame|" + splitCode[4]);
            }
            else
            if (splitCode[3].Equals("prepareTheGameExceptGamers") == true)
            {
                this.network.BroadcastAll(splitCode[0] + "|" + splitCode[1] + "|" + splitCode[2] + "|prepareTheGameExceptGamers|" + splitCode[4] + "|" + splitCode[5]);
            }
            else
            if (splitCode[3].Equals("startGame") == true)
            {
                this.view.MainView.DGConnectedClientsTable.ItemsSource = new ObservableCollection<Client>();
                foreach (Client client in this.view.MainView.Clients)
                {
                    if (client.IP.Equals(splitCode[1]) == true || client.IP.Equals(splitCode[0]) == true)
                    {
                        client.InGame = "In Game";
                    }
                }
                this.view.MainView.DGConnectedClientsTable.ItemsSource = this.view.MainView.Clients;
                this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|" + splitCode[2] + "|startGame|" + splitCode[4] + "|" + splitCode[5] + "|" + splitCode[6] + "|" + splitCode[7]);
            }


            foreach (string code in splitCode) encode += code + "|"; encode = encode.Substring(0, encode.Length - 1);
            foreach (Client client in this.view.MainView.Clients) if (client.IP.Equals(splitCode[0]) == true) { clientName = client.Name; break; }
            view.MainView.LogEntries.Add(new LogEntry(view.MainView.LogEntries.Count + 1, clientName, encode));
            focusRow = view.MainView.DGLogActivityTable.Items[view.MainView.DGLogActivityTable.Items.Count - 1];
            view.MainView.DGLogActivityTable.ScrollIntoView(focusRow);
            view.MainView.DGLogActivityTable.ItemsSource = view.MainView.LogEntries;
        }

        public void disconnectedDecode(string[] splitCode)
        {
            object focusRow = null;
            for (int i = 0; i < view.MainView.Clients.Count; i++)
                if (view.MainView.Clients[i].IP.Equals(splitCode[0]) == true)
                {
                    view.MainView.Clients.RemoveAt(i);
                    break;
                }
            ObservableCollection<Client> newClient = new ObservableCollection<Client>();
            for (int i = 0; i < view.MainView.Clients.Count; i++)
                newClient.Add(new Client(i + 1, view.MainView.Clients[i].Name, view.MainView.Clients[i].IP, view.MainView.Clients[i].InGame, view.MainView.Clients[i].Date));
            view.MainView.Clients.Clear();
            view.MainView.Clients = newClient;
            string encode = String.Empty;

            foreach (string code in splitCode) encode += code + "|"; encode = encode.Substring(0, encode.Length - 1);

            view.MainView.LogEntries.Add(new LogEntry(view.MainView.LogEntries.Count + 1, splitCode[3], encode));
            view.MainView.DGLogActivityTable.ItemsSource = view.MainView.LogEntries;
            focusRow = view.MainView.DGLogActivityTable.Items[view.MainView.DGLogActivityTable.Items.Count - 1];
            view.MainView.DGLogActivityTable.ScrollIntoView(focusRow);

            view.MainView.DGConnectedClientsTable.ItemsSource = view.MainView.Clients;
            this.network.BroadcastAllExcept(splitCode[0], splitCode[0] + "|" + splitCode[0] + "|connectController|removeTable|" + splitCode[3]);
        }

        public void loginControllerDecode(string[] splitCode)
        {
            object focusRow = null;

            if (splitCode[3].Equals("init") == true)
            {
                bool isValid = true;
                for (int i = 0; i < view.MainView.Clients.Count; i++)
                {
                    if (view.MainView.Clients[i].Name.Equals(splitCode[4]) == true)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (this.network.ConnectedClients.Count == 11)
                {
                    isValid = false;
                }

                if (isValid == true)
                {
                    view.MainView.Clients.Add(new Client(view.MainView.Clients.Count + 1, splitCode[4], splitCode[5], splitCode[6], splitCode[7]));
                    focusRow = view.MainView.DGConnectedClientsTable.Items[view.MainView.DGConnectedClientsTable.Items.Count - 1];
                    view.MainView.DGConnectedClientsTable.ScrollIntoView(focusRow);
                    view.MainView.DGConnectedClientsTable.ItemsSource = view.MainView.Clients;
                    this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|" + splitCode[2] + "|isValid");
                }
                if (isValid == false)
                {
                    this.network.BroadcastMessage(splitCode[1], splitCode[0] + "|" + splitCode[1] + "|" + splitCode[2] + "|notValid");
                }
            }

            string encode = String.Empty;
            foreach (string code in splitCode) encode += code + "|"; encode = encode.Substring(0, encode.Length - 1);

            view.MainView.LogEntries.Add(new LogEntry(view.MainView.LogEntries.Count + 1, splitCode[4], encode));
            focusRow = view.MainView.DGLogActivityTable.Items[view.MainView.DGLogActivityTable.Items.Count - 1];
            view.MainView.DGLogActivityTable.ScrollIntoView(focusRow);
            view.MainView.DGLogActivityTable.ItemsSource = view.MainView.LogEntries;

        }


        public Network Network
        {
            get => this.network; set => this.network = value;
        }
        public View View
        {
            get => this.view; set => this.view = value;
        }
    }
}
