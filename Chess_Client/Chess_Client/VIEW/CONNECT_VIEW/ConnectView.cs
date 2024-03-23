using Chess_Client.VIEW.LOGIN_VIEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Chess_Client.CONTROLLER.CONNECT_CONTROLLER;
using Chess_Client.VIEW.GAME_VIEW;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

namespace Chess_Client.VIEW.CONNECT_VIEW
{
    public class ConnectView : Window
    {
        private ConnectController connectController;
        private HeaderConnectView headerConnectView;
        private MainConnectView mainConnectView;

        public ConnectView()
        {
            InitializeComponent();
            this.Content = GridWindowConnect();
            this.Show();
        }

        public void InitializeComponent()
        {
            this.Width = 570;
            this.Height = 480;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowStyle = WindowStyle.None;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.NoResize;
            string path = Directory.GetCurrentDirectory() + @"\Images\QueenW.png";
            ImageSource imageSource = new BitmapImage(new Uri(path));
            this.Icon = imageSource;
        }

        public Grid GridWindowConnect()
        {
            this.headerConnectView = new HeaderConnectView(this);
            this.mainConnectView = new MainConnectView(this);
            Grid grdWindowConnect = new Grid { Name = "GrdWindowConnect" };
            grdWindowConnect.Children.Add(headerConnectView);
            grdWindowConnect.Children.Add(mainConnectView);
            return grdWindowConnect;
        }

        public HeaderConnectView HeaderConnectView
        {
            get => this.headerConnectView; set => this.headerConnectView = value;
        }
        public MainConnectView MainConnectView
        {
            get => this.mainConnectView; set => this.mainConnectView = value;
        }
        public ConnectController ConnectController
        {
            get => this.connectController; set => this.connectController = value;
        }
    }
}
