using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using Chess_Client.MODEL.GAME_MODEL;
using Chess_Client.VIEW.LOGIN_VIEW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Chess_Client.VIEW.GAME_VIEW.MainGameView;

namespace Chess_Client.VIEW.GAME_VIEW
{
    public class GameView : Window
    {
        private GameController gameController;
        private HeaderGameView headerGameView;
        private MainGameView mainGameView;

        public GameView()
        {
            InitializeComponent();
            this.Content = GridWindowGame();
            this.Show();
        }

        public void InitializeComponent()
        {
            this.Height = 945;
            this.Width = 1080;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowStyle = WindowStyle.None;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.NoResize;
            this.PreviewKeyDown += this.OnPreviewKeyDown;
            string path = Directory.GetCurrentDirectory() + @"\Images\QueenW.png";
            ImageSource imageSource = new BitmapImage(new Uri(path));
            this.Icon = imageSource;
        }

        public Grid GridWindowGame()
        {
            Grid grdWindowGame = new Grid { Name = "GrdWindowGame" };
            this.headerGameView = new HeaderGameView(this);
            this.mainGameView = new MainGameView(this);
            grdWindowGame.Children.Add(this.headerGameView);
            grdWindowGame.Children.Add(this.mainGameView);
            return grdWindowGame;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.gameController.EnableChat==true && this.mainGameView.ChatHistoryMainGameView.TxbChat.Text.Equals("")==false)
            {
                this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|sendMessage|" + this.mainGameView.ChatHistoryMainGameView.TxbChat.Text);
                this.mainGameView.ChatHistoryMainGameView.sendMessage(this.gameController.Network.You.Name,this.mainGameView.ChatHistoryMainGameView.TxbChat.Text,0);
            }
        }

        public HeaderGameView HeaderGameView
        {
            get => this.headerGameView; set => this.headerGameView = value;
        }
        public MainGameView MainGameView
        { 
            get => this.mainGameView; set => this.mainGameView = value; 
        }
        public GameController GameController
        {
            get => this.gameController; set => this.gameController = value; 
        }
    }
}
