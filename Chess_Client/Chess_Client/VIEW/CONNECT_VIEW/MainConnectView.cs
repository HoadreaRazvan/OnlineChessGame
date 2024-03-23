using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using System.Reflection.Metadata;
using System.Windows.Media;
using System.Windows.Input;
using Chess_Client.VIEW.GAME_VIEW;
using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using System.Collections.ObjectModel;
using Chess_Client.MODEL.LOGIN_MODEL;

namespace Chess_Client.VIEW.CONNECT_VIEW
{
    public class MainConnectView : Border
    {
        private ConnectView connectView;

        private DataGrid DGConnectedClients;
        private DataGrid DGInvite;
        private ObservableCollection<LoginModel> clients;
        private ObservableCollection<LoginModel> invites;

        private Label lblName;

        public MainConnectView(ConnectView connectView)
        {
            this.clients = new ObservableCollection<LoginModel>();
            this.invites = new ObservableCollection<LoginModel>();
            this.connectView = connectView;
            InitializeComponent();
            this.Child = layout();
        }

        public void InitializeComponent()
        {
            Name = "BrdConnectMain";
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Width = 570;
            Height = 435;
            Margin = new Thickness(0, 45, 0, 0);
            CornerRadius = new CornerRadius(0, 0, 30, 30);
            Background = Brushes.White;
            this.BorderBrush = Brushes.Black;
            this.BorderThickness = new Thickness(1, 1, 1, 1);
        }

        public Grid layout()
        {
            Grid grdConnectMain = new Grid
            {
                Name = "GrdConnectMain"
            };

            lblName = new Label
            {
                Name = "LblName",
                Content = "Welcome ",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 570,
                Height = 40,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 20
            };

            Label LblTitle = new Label
            {
                Name = "LblTitle",
                Content = "Connected Clients",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 380,
                Height = 40,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 40, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16
            };

            Label LblInvite = new Label
            {
                Name = "LblInvite",
                Content = "Invitations",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 160,
                Height = 40,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(400, 40, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16
            };

            DGConnectedClients = new DataGrid
            {
                Name = "DGConnectedClients",
                Cursor = Cursors.Hand,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 250,
                Margin = new Thickness(10, 80, 180, 0),
                AutoGenerateColumns = false,
                IsReadOnly = true,
                CanUserSortColumns = false,
                CanUserReorderColumns = false,
                CanUserResizeColumns = false,
                SelectionMode = DataGridSelectionMode.Single,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                FontFamily = new FontFamily("Arial"),
                FontSize = 15
            };

            DGConnectedClients.Columns.Add(new DataGridTextColumn { Header = "#", Binding = new System.Windows.Data.Binding("Index"), Width = 25 });
            DGConnectedClients.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new System.Windows.Data.Binding("Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            DGConnectedClients.Columns.Add(new DataGridTextColumn { Header = "Status", Binding = new System.Windows.Data.Binding("InGame"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            
            DGConnectedClients.SelectionChanged += DGConnectedClients_SelectionChanged;


            DGInvite = new DataGrid
            {
                Name = "DGInvite",
                Cursor = Cursors.Hand,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 250,
                Margin = new Thickness(400, 80, 10, 0),
                AutoGenerateColumns = false,
                IsReadOnly = true,
                CanUserSortColumns = false,
                CanUserReorderColumns = false,
                CanUserResizeColumns = false,
                SelectionMode = DataGridSelectionMode.Single,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                FontFamily = new FontFamily("Arial"),
                FontSize = 15
            };

            DGInvite.Columns.Add(new DataGridTextColumn { Header = "#", Binding = new System.Windows.Data.Binding("Index"), Width = 25 });
            DGInvite.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new System.Windows.Data.Binding("Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });

            DGInvite.SelectionChanged += DGInvite_SelectionChanged;

            Label lblInfo = new Label
            {
                Name = "LblInfo",
                Content = "Click on the desired line to send a game request.\n\nClick on the desired line to cancel a game request.\n                 (if you have already sent one)",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 80,
                Margin = new Thickness(0, 340, 180, 0),
                VerticalContentAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontStyle = FontStyles.Italic,
                FontSize = 14
            };

            Label lblInviteInfo = new Label
            {
                Name = "LblInviteInfo",
                Content = " Click on the desired\nline to start the game.",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 80,
                Margin = new Thickness(400, 340, 10, 0),
                VerticalContentAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontStyle = FontStyles.Italic,
                FontSize = 14
            };

            grdConnectMain.Children.Add(lblName);
            grdConnectMain.Children.Add(LblTitle);
            grdConnectMain.Children.Add(LblInvite);
            grdConnectMain.Children.Add(DGConnectedClients);
            grdConnectMain.Children.Add(DGInvite);
            grdConnectMain.Children.Add(lblInfo);
            grdConnectMain.Children.Add(lblInviteInfo);

            return grdConnectMain;
        }

        private void DGConnectedClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGConnectedClients.SelectedItem != null)
            {
                LoginModel content = ((LoginModel)DGConnectedClients.SelectedItem);
                if(content.InGame.Equals("Available")==true)
                {
                    this.DGConnectedClients.ItemsSource = new ObservableCollection<LoginModel>();
                    foreach(LoginModel client in this.clients)
                        if(content.Name.Equals(client.Name) == true)
                        {
                            client.InGame = "Cancel Invite";
                        }
                    this.DGConnectedClients.ItemsSource=this.clients;

                    this.ConnectView.ConnectController.Network.SendMessage(this.ConnectView.ConnectController.Network.You.IP + "|" + content.IP + "|connectController|addInvite|" + this.ConnectView.ConnectController.Network.You.Name);
                    
                    foreach (object window in Application.Current.Windows) 
                        if (window is MyMessageBox) 
                            (window as MyMessageBox).Close();

                    this.connectView.IsEnabled = false;
                    MyMessageBox myMessageBox = new MyMessageBox("Request sent successfully!");
                    myMessageBox.Left = this.connectView.Left + (this.connectView.Width - myMessageBox.Width) / 2;
                    myMessageBox.Top = this.connectView.Top + (this.connectView.Height - myMessageBox.Height) / 2;
                    myMessageBox.Closed += (sender, e) => { this.connectView.IsEnabled = true; };
                    myMessageBox.Show();

                }
                else
                if (content.InGame.Equals("Cancel Invite") == true)
                {
                    this.DGConnectedClients.ItemsSource = new ObservableCollection<LoginModel>();
                    foreach (LoginModel client in this.clients)
                        if (content.Name.Equals(client.Name) == true)
                        {
                            client.InGame = "Available";
                        }
                    this.DGConnectedClients.ItemsSource = this.clients;

                    foreach (LoginModel client in this.Invites)
                    {
                        if (client.Name.Equals(content.Name) == true)
                        {
                            this.Invites.Remove(client);
                            break;
                        }
                    }
                    for (int i = 0; i < this.Invites.Count; i++)
                    {
                        this.Invites[i].Index = i + 1;
                    }
                    this.DGInvite.ItemsSource = new ObservableCollection<LoginModel>();
                    this.DGInvite.ItemsSource = this.invites;

                    this.ConnectView.ConnectController.Network.SendMessage(this.ConnectView.ConnectController.Network.You.IP + "|" + content.IP + "|connectController|cancelInvite|" + content.Name + "|" + this.ConnectView.ConnectController.Network.You.Name);
                    
                    foreach (object window in Application.Current.Windows) 
                        if (window is MyMessageBox) 
                            (window as MyMessageBox).Close();

                    this.connectView.IsEnabled = false;
                    MyMessageBox myMessageBox = new MyMessageBox("The request was successfully canceled!");
                    myMessageBox.Left = this.connectView.Left + (this.connectView.Width - myMessageBox.Width) / 2;
                    myMessageBox.Top = this.connectView.Top + (this.connectView.Height - myMessageBox.Height) / 2;
                    myMessageBox.Closed += (sender, e) => { this.connectView.IsEnabled = true; };
                    myMessageBox.Show();
                }
                else
                if (content.InGame.Equals("In Game") == true)
                {
                    foreach (object window in Application.Current.Windows) 
                        if (window is MyMessageBox) 
                            (window as MyMessageBox).Close();

                    this.connectView.IsEnabled = false;
                    MyMessageBox myMessageBox = new MyMessageBox("The player is already in the game!");
                    myMessageBox.Left = this.connectView.Left + (this.connectView.Width - myMessageBox.Width) / 2;
                    myMessageBox.Top = this.connectView.Top + (this.connectView.Height - myMessageBox.Height) / 2;
                    myMessageBox.Closed += (sender, e) => { this.connectView.IsEnabled = true; };
                    myMessageBox.Show();
                }
                DGConnectedClients.SelectedIndex = -1;
            }
        }

        private void DGInvite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGInvite.SelectedItem != null)
            {
                LoginModel content = ((LoginModel)DGInvite.SelectedItem);
                foreach (LoginModel client in this.Clients)
                {
                    if (client.Name.Equals(content.Name) == false && client.InGame.Equals("Cancel Invite") == true)
                    {
                        this.ConnectView.ConnectController.Network.SendMessage(this.ConnectView.ConnectController.Network.You.IP + "|" + client.IP + "|connectController|prepareTheGame|" + this.ConnectView.ConnectController.Network.You.Name);
                    }
                }
                int indexColor = new Random().Next() % 2, indexMirror = new Random().Next() % 2, indexTurn = new Random().Next() % 2;
                string contentIP = String.Empty;
                foreach (LoginModel client in this.Clients)
                {
                    if(client.Name.Equals(content.Name)==true)
                    {
                        contentIP = client.IP;
                        break;
                    }
                }
                this.ConnectView.ConnectController.Network.SendMessage(this.ConnectView.ConnectController.Network.You.IP + "|" + "all|connectController|prepareTheGameExceptGamers|" + content.Name + "|" + this.ConnectView.ConnectController.Network.You.Name);
                
                this.ConnectView.ConnectController.Network.SendMessage(this.ConnectView.ConnectController.Network.You.IP + "|" + contentIP + "|connectController|startGame|" + this.ConnectView.ConnectController.Network.You.Name + "|" + this.ConnectView.ConnectController.Network.You.IP + "|" + Math.Abs(indexColor - 1) + "|" + Math.Abs(indexMirror - 1));

                this.ConnectView.ConnectController.Network.Opponent = new LoginModel(content.Name, contentIP, "In Game", DateTime.Now);
                this.ConnectView.Hide();
                GameView gameView = new GameView();
                GameController gameController = new GameController(this.ConnectView.ConnectController.Network, gameView, indexColor, indexMirror);
                this.ConnectView.ConnectController.Network.GameController = gameController;
                this.ConnectView.ConnectController.Network.ConnectController = null;
                this.ConnectView.Close();
                DGInvite.SelectedIndex = -1;
            }
        }
        public Label LblName
        {
            get => this.lblName; set => this.lblName = value;
        }
        public ObservableCollection<LoginModel> Invites
        {
            get => this.invites; set => this.invites = value;
        }
        public ObservableCollection<LoginModel> Clients
        {
            get => this.clients; set => this.clients = value;
        }
        public DataGrid DGConnectedClientsTable
        {
            get => this.DGConnectedClients; set => this.DGConnectedClients = value;
        }
        public DataGrid DGInviteTable
        {
            get => this.DGInvite; set => this.DGInvite = value;
        }
        public ConnectView ConnectView
        {
            get => this.connectView; set => this.connectView = value;
        }
    }
}
