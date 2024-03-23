using Chess_Client.CONTROLLER.CONNECT_CONTROLLER;
using Chess_Client.MODEL;
using Chess_Client.MODEL.LOGIN_MODEL;
using Chess_Client.VIEW.CONNECT_VIEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess_Client.VIEW.LOGIN_VIEW
{
    public class MainLoginView : Border
    {
        private LoginView loginView;

        private TextBox txbServerIP;
        private TextBox txbName;
        private CheckBox ckbLocalHost;
        private Button btnConnect;

        public MainLoginView(LoginView loginView)
        {
            this.loginView = loginView;
            InitializeComponent();
            this.Child = layout();
        }

        public void InitializeComponent()
        {
            this.Name = "BrdLoginMain";
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = 350;
            this.Height = 380;
            this.Margin = new Thickness(0, 45, 0, 0);
            this.CornerRadius = new CornerRadius(0, 0, 30, 30);
            this.Background = Brushes.White;
            this.BorderBrush = Brushes.Black;
            this.BorderThickness = new Thickness(1, 1, 1, 1);
        }

        public Grid layout()
        {
            Grid grdLoginMain = new Grid
            {
                Name = "GrdLoginMain"
            };

            Label LblTitle = new Label
            {
                Name = "LblTitle",
                Content = "Client",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 350,
                Height = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 20
            };

            Label LblName = new Label
            {
                Name = "LblName",
                Content = "Name:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 100,
                Height = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 70, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16
            };

            txbName = new TextBox
            {
                Name = "TxbName",
                MaxLength = 10,
                IsReadOnly = false,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 220,
                Height = 30,
                Margin = new Thickness(120, 80, 0, 0),
                TextWrapping = TextWrapping.NoWrap,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };
            txbName.TextChanged += TxbName_TextChanged;

            Label LblServerIP = new Label
            {
                Name = "LblServerIP",
                Content = "Server IP:",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 100,
                Height = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 140, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16
            };

            txbServerIP = new TextBox
            {
                Name = "TxbServerIP",
                MaxLength = 20,
                IsReadOnly = false,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 220,
                Height = 30,
                Margin = new Thickness(120, 150, 0, 0),
                TextWrapping = TextWrapping.NoWrap,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };
            txbServerIP.TextChanged += TxbServerIP_TextChanged;

            ckbLocalHost = new CheckBox
            {
                Name = "CkbLocalHost",
                Cursor = Cursors.Hand,
                Content = "Local Host",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 220,
                Height = 30,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(120, 190, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16
            };
            ckbLocalHost.Checked += CkbLocalHost_Checked;
            ckbLocalHost.Unchecked += CkbLocalHost_Unchecked;

            btnConnect = new Button
            {
                Name = "BtnConnect",
                Cursor = Cursors.Hand,
                Content = "Connect",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 150,
                Height = 40,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 270, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16
            };
            btnConnect.Click += BtnConnect_Click;

            Label LblSignature = new Label
            {
                Name = "LblSignature",
                Content = "Created and Developed by \n    Hoadrea Razvan S&E",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 55,
                Margin = new Thickness(0, 320, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontStyle = FontStyles.Italic,
                FontSize = 14
            };

            grdLoginMain.Children.Add(LblTitle);
            grdLoginMain.Children.Add(LblName);
            grdLoginMain.Children.Add(txbName);
            grdLoginMain.Children.Add(LblServerIP);
            grdLoginMain.Children.Add(txbServerIP);
            grdLoginMain.Children.Add(ckbLocalHost);
            grdLoginMain.Children.Add(btnConnect);
            grdLoginMain.Children.Add(LblSignature);

            return grdLoginMain;
        }

        private void TxbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = this.txbName.Text;
            string pattern = "^[a-zA-Z0-9]*$"; 
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(inputText))
            {
                this.txbName.Text = Regex.Replace(inputText, "[^a-zA-Z0-9]", "");
                this.txbName.CaretIndex = this.txbName.Text.Length;
            }
        }

        private void TxbServerIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = this.txbServerIP.Text;
            string pattern = "^[0-9.]*$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(inputText))
            {
                this.txbServerIP.Text = Regex.Replace(inputText, "[^0-9.]", "");
                this.txbServerIP.CaretIndex = this.txbServerIP.Text.Length;
            }
        }

        private void CkbLocalHost_Checked(object sender, RoutedEventArgs e)
        {
            this.txbServerIP.Text = "";
            this.txbServerIP.IsReadOnly = true;
        }

        private void CkbLocalHost_Unchecked(object sender, RoutedEventArgs e)
        {
            this.txbServerIP.IsReadOnly = false;
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {

            if (this.TxbName.Text.Equals("") == false && (this.ckbLocalHost.IsChecked == true || this.txbServerIP.Text.Equals("") == false))
            {
                this.btnConnect.IsEnabled = false;
                Network network = null;
                if (this.ckbLocalHost.IsChecked == true)
                    network = new Network("localHost");
                else
                    network = new Network(this.txbServerIP.Text);
                if (network.IsConnected == true)
                {
                    network.LoginController = this.loginView.LoginController;
                    LoginModel loginModel = new LoginModel(this.txbName.Text, network.IpClient, "Available", DateTime.Now);
                    network.You = loginModel;
                    this.loginView.LoginController.Network = network;
                    network.SendMessage(network.IpClient + "|" + network.IpClient + "|loginController|init|" + network.You.ToString());
                }
                else
                {
                    foreach (object window in Application.Current.Windows)
                        if (window is MyMessageBox)
                            (window as MyMessageBox).Close(); 
                    
                    this.loginView.IsEnabled = false;
                    MyMessageBox myMessageBox = new MyMessageBox("The connection to the server could not be established!");
                    myMessageBox.Left = this.loginView.Left + (this.loginView.Width - myMessageBox.Width) / 2;
                    myMessageBox.Top = this.loginView.Top + (this.loginView.Height - myMessageBox.Height) / 2;
                    myMessageBox.Closed += (sender, e) => { this.loginView.IsEnabled = true; this.btnConnect.IsEnabled = true; };
                    myMessageBox.Show();
                    
                }
            }
            else
            {
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.loginView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("Do not leave blank fields!");
                myMessageBox.Left = this.loginView.Left + (this.loginView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.loginView.Top + (this.loginView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.loginView.IsEnabled = true; }; 
                myMessageBox.Show();
            }

        }

        public Button BtnConnect
        {
            get=> this.btnConnect; set => this.btnConnect = value;
        }
        public TextBox TxbServerIP
        {
            get => this.txbServerIP; set => this.txbServerIP = value;
        }
        public TextBox TxbName
        {
            get => this.txbName; set => this.txbName = value;
        }
        public CheckBox CkbLocalHost
        {
            get => this.ckbLocalHost; set => this.ckbLocalHost = value;
        }
        public LoginView LoginView
        {
            get => this.loginView; set => this.loginView = value;
        }
    }
}
