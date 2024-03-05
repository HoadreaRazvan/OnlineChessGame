using Chess_Client.CONTROLLER.LOGIN_CONTROLLER;
using Chess_Client.VIEW.GAME_VIEW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess_Client.VIEW.LOGIN_VIEW
{
    public class LoginView :Window
    {
        private LoginController loginController;
        private HeaderLoginView headerLoginView;
        private MainLoginView mainLoginView;

        public LoginView()
        {
            InitializeComponent();
            this.Content = GridWindowLogin();
            this.Show();
        }

        public void InitializeComponent()
        {
            this.Width = 350;
            this.Height = 405;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowStyle = WindowStyle.None;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.NoResize;
            string path = Directory.GetCurrentDirectory() + @"\Images\QueenW.png";
            ImageSource imageSource = new BitmapImage(new Uri(path));
            this.Icon = imageSource;
        }

        public Grid GridWindowLogin()
        {
            this.headerLoginView = new HeaderLoginView(this);
            this.mainLoginView = new MainLoginView(this);
            Grid grdWindowLogin = new Grid { Name = "GrdWindowLogin" };
            grdWindowLogin.Children.Add(this.headerLoginView);
            grdWindowLogin.Children.Add(this.mainLoginView);
            return grdWindowLogin;
        }

        public HeaderLoginView HeaderLoginView
        {
            get => this.headerLoginView; set => this.headerLoginView = value;
        }
        public MainLoginView MainLoginView
        {
            get => this.mainLoginView; set => this.mainLoginView = value;
        }
        public LoginController LoginController
        {
            get => this.loginController; set => this.loginController = value;
        }
    }
}
