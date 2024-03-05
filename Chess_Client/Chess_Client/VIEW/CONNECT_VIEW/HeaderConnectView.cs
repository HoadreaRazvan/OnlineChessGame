using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using Chess_Client.MODEL;
using Chess_Client.CONTROLLER.LOGIN_CONTROLLER;
using Chess_Client.VIEW.LOGIN_VIEW;

namespace Chess_Client.VIEW.CONNECT_VIEW
{
    public class HeaderConnectView : Border
    {
        private ConnectView connectView;

        public HeaderConnectView(ConnectView connectView)
        {
            this.connectView = connectView;
            this.InitializeComponent();
            this.Child = this.layout();
        }

        public void InitializeComponent()
        {
            this.Name = "BrdConnectHeader";
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = 570;
            this.Height = 45;
            this.Margin = new Thickness(0, 0, 0, 0);
            this.CornerRadius = new CornerRadius(30, 30, 0, 0);
            this.Background = Brushes.WhiteSmoke;
            this.BorderBrush = Brushes.Black;
            this.BorderThickness = new Thickness(1, 1, 1, 0);

            this.MouseDown += Border_MouseDown;
        }

        public Grid layout()
        {
            Grid grdConnectHeader = new Grid
            {
                Name = "GrdConnectHeader"
            };

            Button BtnExit = new Button
            {
                Name = "BtnExit",
                Content = "X",
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(520, 10, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16
            };
            BtnExit.Click += BtnExit_Click;

            Button BtnMinimize = new Button
            {
                Name = "BtnMinimize",
                Content = "_",
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(480, 10, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16
            };
            BtnMinimize.Click += BtnMinimize_Click;

            Button BtnBack = new Button
            {
                Name = "BtnBack",
                Content = "<",
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(20, 10, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16
            };
            BtnBack.Click += BtnBack_Click;

            grdConnectHeader.Children.Add(BtnExit);
            grdConnectHeader.Children.Add(BtnMinimize);
            grdConnectHeader.Children.Add(BtnBack);

            return grdConnectHeader;
        }

        private void Border_MouseDown(object sender, RoutedEventArgs e)
        {
            this.connectView.DragMove();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        public void close()
        {
            this.connectView.ConnectController.Network.close();
            this.connectView.ConnectController = null;
            this.connectView.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.connectView.WindowState = WindowState.Minimized;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            back(1);
        }
        public void back(int index)
        {
            this.connectView.ConnectController.Network.close();
            this.connectView.ConnectController = null;
      
            LoginView loginView = new LoginView();
            LoginController loginController = new LoginController(loginView);
            this.connectView.Close();
            if (index == 0)
            {
                foreach (object window in Application.Current.Windows) 
                    if (window is MyMessageBox) 
                        (window as MyMessageBox).Close();

                loginController.LoginView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("Disconnected!");
                myMessageBox.WindowStartupLocation= WindowStartupLocation.CenterScreen;
                myMessageBox.Closed += (sender, e) => { loginController.LoginView.IsEnabled = true; }; 
                myMessageBox.Show();
            }
        }


        public ConnectView ConnectView
        {
            get => this.connectView; set => this.connectView = value;
        }
    }
}
