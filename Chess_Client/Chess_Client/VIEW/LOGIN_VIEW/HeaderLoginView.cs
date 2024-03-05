using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess_Client.VIEW.LOGIN_VIEW
{
    public class HeaderLoginView : Border
    {
        private LoginView loginView;

        public HeaderLoginView(LoginView loginView)
        {
            this.loginView = loginView;
            this.InitializeComponent();
            this.Child = this.layout();
        }

        public void InitializeComponent()
        {
            this.Name = "BrdLoginHeader";
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = 350;
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
            Grid grdLoginHeader = new Grid
            {
                Name = "GrdLoginHeader"
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
                Margin = new Thickness(300, 10, 0, 0),
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
                Margin = new Thickness(260, 10, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16
            };
            BtnMinimize.Click += BtnMinimize_Click;

            grdLoginHeader.Children.Add(BtnExit);
            grdLoginHeader.Children.Add(BtnMinimize);
            return grdLoginHeader;
        }


        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.loginView.Close();
        }

        private void Border_MouseDown(object sender, RoutedEventArgs e)
        {
            this.loginView.DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.loginView.WindowState = WindowState.Minimized;
        }

        public LoginView LoginView
        {
            get => this.loginView; set => this.loginView = value;
        }
    }
}
