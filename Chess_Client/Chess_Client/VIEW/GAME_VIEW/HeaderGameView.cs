using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using Chess_Client.CONTROLLER.LOGIN_CONTROLLER;
using Chess_Client.VIEW.LOGIN_VIEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess_Client.VIEW.GAME_VIEW
{
    public class HeaderGameView : Border
    {
        private GameView gameView;

        public HeaderGameView(GameView gameView)
        {
            this.gameView = gameView;
            this.InitializeComponent();
            this.Child = this.layout();
        }

        public void InitializeComponent()
        {
            this.Name = "BrdGameHeader";
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = 1080;
            this.Height = 45;
            this.Margin = new Thickness(0);
            this.CornerRadius = new CornerRadius(30, 30, 0, 0);
            this.Background = Brushes.WhiteSmoke;
            this.BorderBrush = Brushes.Black;
            this.BorderThickness = new Thickness(1, 1, 1, 0);
            this.MouseDown += Border_MouseDown;
        }
        public Grid layout()
        {
            Grid headerGrid = new Grid
            {
                Name = "GrdHeader"
            };

            Button exitButton = new Button
            {
                Name = "BtnExit",
                Content = "X",
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(1030, 10, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16
            };
            exitButton.Click += this.BtnExit_Click;

            Button minimizeButton = new Button
            {
                Name = "BtnMinimize",
                Content = "_",
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(990, 10, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16
            };
            minimizeButton.Click += this.BtnMinimize_Click;

            headerGrid.Children.Add(exitButton);
            headerGrid.Children.Add(minimizeButton);

            return headerGrid;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.GameView.DragMove();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.close(0);
        }

        public void close(int index)
        {
            if (this.gameView.GameController.Network.Opponent != null)
                this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|leave");
            this.gameView.GameController.Network.close();
            this.gameView.GameController = null;
            this.gameView.Hide();
            if (index == 1)
            {
                LoginView loginView = new LoginView();
                LoginController loginController = new LoginController(loginView);

                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                loginController.LoginView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("Disconnected!");
                myMessageBox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                myMessageBox.Closed += (sender, e) => { loginController.LoginView.IsEnabled = true; };
                myMessageBox.Show();
            }
            this.gameView.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.GameView.WindowState = WindowState.Minimized;
        }

        public GameView GameView
        {
            get => this.gameView; set => this.gameView = value;
        }
    }
}
