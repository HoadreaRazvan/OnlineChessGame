using Chess_Server.CONTROLLER;
using Chess_Server.MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Chess_Server.VIEW
{
    public class View : Window
    {
        private Controller controller;
        private HeaderView headerView;
        private MainView mainView;

        public View()
        {
            InitializeComponent();
            this.Content = GridWindow();
            this.Show();
        }

        public void InitializeComponent()
        {
            this.Width = 750;
            this.Height = 820;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowStyle = WindowStyle.None;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.NoResize;
            string path = Directory.GetCurrentDirectory() + @"\Images\ServerIcon.png";
            ImageSource imageSource = new BitmapImage(new Uri(path));
            this.Icon = imageSource;
        }

        public Grid GridWindow()
        {
            this.headerView = new HeaderView(this);
            this.mainView = new MainView(this);
            Grid grdWindow = new Grid { Name = "GrdWindow" };
            grdWindow.Children.Add(this.headerView);
            grdWindow.Children.Add(this.mainView);
            return grdWindow;
        }

        public MainView MainView
        {
            get => this.mainView; set => this.mainView = value;
        }
        public HeaderView HeaderView
        {
            get => this.headerView; set => this.headerView = value;
        }
        public Controller Controller
        {
            get => this.controller; set => this.controller = value;
        }
    }
}
