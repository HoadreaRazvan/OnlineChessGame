using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using Chess_Server.MODEL;
using System.Windows.Threading;
using System.Threading;

namespace Chess_Server.VIEW
{
    public class HeaderView : Border
    {
        private View view;

        private Label lblServerIp;

        private DispatcherTimer timer;
        private int clientsCount;


        public HeaderView(View view)
        {
            this.view = view;
            InitializeComponent();
            this.Child = layout();
        }

        public void InitializeComponent()
        {
            this.Name = "BrdHeader";
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = 750;
            this.Height = 55;
            this.Margin = new Thickness(0);
            this.CornerRadius = new CornerRadius(30, 30, 0, 0);
            this.Background = Brushes.WhiteSmoke;
            this.BorderBrush = Brushes.Black;
            this.BorderThickness = new Thickness(1, 1, 1, 0);

            this.MouseDown += Grid_MouseDown;
        }

        public Grid layout()
        {
            Grid grdHeader = new Grid
            {
                Name = "GrdHeader"
            };

            lblServerIp = new Label
            {
                Name = "LblServerIp",
                Content = "Server IP: ",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 650,
                Height = 50,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Bottom,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 20
            };

            Button btnExit = new Button
            {
                Name = "BtnExit",
                Content = "X",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(705, 15, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16,
                Cursor = Cursors.Hand
            };
            btnExit.Click += BtnExit_Click;

            Button btnMinimize = new Button
            {
                Name = "BtnMinimize",
                Content = "_",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(665, 15, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16,
                Cursor = Cursors.Hand
            };
            btnMinimize.Click += BtnMinimize_Click;

            grdHeader.Children.Add(btnExit);
            grdHeader.Children.Add(btnMinimize);
            grdHeader.Children.Add(lblServerIp);

            return grdHeader;
        }

        private void Grid_MouseDown(object sender, RoutedEventArgs e)
        {
            this.view.DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.view.WindowState = WindowState.Minimized;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            foreach (object window in Application.Current.Windows)
                if (window is MyMessageBox)
                    (window as MyMessageBox).Close();

            this.view.IsEnabled = false;
            MyMessageBox myMessageBox = new MyMessageBox("All clients will be disconnected!");
            myMessageBox.Left = this.view.Left + (this.view.Width - myMessageBox.Width) / 2;
            myMessageBox.Top = this.view.Top + (this.view.Height - myMessageBox.Height) / 2;
            myMessageBox.Show();

            this.clientsCount = this.view.MainView.Clients.Count;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            bool emptyList = false;
            if (clientsCount == 0)
            {
                timer.Stop();
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                MyMessageBox myMessageBox = new MyMessageBox("The server will shut down!");
                myMessageBox.Left = this.view.Left + (this.view.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.view.Top + (this.view.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.view.Close(); this.view.Controller.Network.close(); };
                myMessageBox.Show();
                emptyList = true;
            }
            if (emptyList == false)
            {
                this.view.Controller.Network.disconnectIPClient(this.view.MainView.Clients[0].IP);
                clientsCount--;
            }
        }


        public Label LblServerIp
        {
            get => this.lblServerIp; set => this.lblServerIp = value;
        }

        public View View
        {
            get => this.view; set => this.view = value;
        }
           
    }
}
