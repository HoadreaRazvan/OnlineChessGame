using Chess_Client.CONTROLLER.CONNECT_CONTROLLER;
using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS;
using Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_PIECES;
using Chess_Client.VIEW.CONNECT_VIEW;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Chess_Client.VIEW.GAME_VIEW
{
    public class MainGameView : Border
    {
        private ObservableCollection<string> imagesCollection;
        private bool changePawn;
        private bool rematchInvitation;

        private GameView gameView;
        private ChatHistoryMainGameView chatHistoryMainGameView;
        private BoardMainGameView boardMainGameView;

        private Label lblOpponent;
        private StackPanel stPOpponentPiece;
        private StackPanel stPOpponentNumber;
        private Label lblTimerOpponent;

        private Label lblYou;
        private StackPanel stPYouPiece;
        private StackPanel stPYouNumber;
        private Label lblTimerYou;

        private Button btnResignMainMenu;
        private Button btnRematch;

        private DispatcherTimer timerYou;
        private TimeSpan timeRemainingYou;

        private DispatcherTimer timerOpponent;
        private TimeSpan timeRemainingOpponent;

        public MainGameView(GameView gameView)
        {
            this.gameView = gameView;
            this.changePawn = false;
            this.rematchInvitation = false;
            InitializeComponent();
            this.chatHistoryMainGameView = new ChatHistoryMainGameView(gameView);
            this.boardMainGameView = new BoardMainGameView(gameView);
            this.Child = layout();
            InitializeTimerYou();
            InitializeTimerOpponent();
            imagesCollection = new ObservableCollection<string>();      
        }

        public void InitializeComponent()
        {
            Name = "BrdGameMain";
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Width = 1080;
            Height = 900;
            Margin = new Thickness(0, 45, 0, 0);
            CornerRadius = new CornerRadius(0, 0, 30, 30);
            Background = Brushes.White;
            this.BorderBrush = Brushes.Black;
            this.BorderThickness = new Thickness(1, 1, 1, 1);
        }

        public Grid layout()
        {
            Grid mainContentGrid = new Grid
            {
                Name = "GrdMain"
            };


            Grid grdOpponent = new Grid
            {
                Name = "GrdOpponent",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 680,
                Height = 100,
                Margin = new Thickness(20, 0, 0, 0)
            };

            lblOpponent = new Label
            {
                Name = "LblOpponent",
                Content = "",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 170,
                Height = 100,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 20
            };

            stPOpponentPiece = new StackPanel
            {
                Name = "StPOpponentPiece",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Horizontal,
                Width = 340,
                Height = 70,
                Margin = new Thickness(165, 0, 0, 0)
            };

            stPOpponentNumber = new StackPanel
            {
                Name = "StPOpponentNumber",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Horizontal,
                Width = 340,
                Height = 30,
                Margin = new Thickness(165, 70, 0, 0)
            };

            lblTimerOpponent = new Label
            {
                Name = "LblTimerOpponent",
                Content = "10:00",
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 22,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(595, 0, 0, 0),
                Width = 85,
                Height = 100
            };

            grdOpponent.Children.Add(lblOpponent);
            grdOpponent.Children.Add(stPOpponentPiece);
            grdOpponent.Children.Add(stPOpponentNumber);
            grdOpponent.Children.Add(lblTimerOpponent);


            Grid grdYou = new Grid
            {
                Name = "GrdYou",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 680,
                Height = 100,
                Margin = new Thickness(20, 800, 0, 0)
            };

            lblYou = new Label
            {
                Name = "LblYou",
                Content = "",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 170,
                Height = 100,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 20
            };

            stPYouPiece = new StackPanel
            {
                Name = "StPYouPiece",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Horizontal,
                Width = 340,
                Height = 70,
                Margin = new Thickness(165, 0, 0, 0)
            };

            stPYouNumber = new StackPanel
            {
                Name = "StPYouNumber",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Orientation = Orientation.Horizontal,
                Width = 340,
                Height = 30,
                Margin = new Thickness(165, 70, 0, 0)
            };

            lblTimerYou = new Label
            {
                Name = "LblTimerYou",
                Content = "10:00",
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 22,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(595, 0, 0, 0),
                Width = 85,
                Height = 100
            };

            grdYou.Children.Add(lblYou);
            grdYou.Children.Add(stPYouPiece);
            grdYou.Children.Add(stPYouNumber);
            grdYou.Children.Add(lblTimerYou);


            Grid grdButtons = new Grid
            {
                Name = "GrdButtons",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 350,
                Height = 100,
                Margin = new Thickness(720, 800, 0, 0)
            };

            btnResignMainMenu = new Button
            {
                Name = "BtnResign",
                Content = "Resign",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 140,
                Height = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20, 25, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };
            btnResignMainMenu.Click += BtnResignMainMenu_Click;

            btnRematch = new Button
            {
                Name = "BtnRematch",
                Content = "Rematch",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 140,
                Height = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(185, 25, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };
            btnRematch.Click += BtnRematch_Click;

            grdButtons.Children.Add(btnResignMainMenu);
            grdButtons.Children.Add(btnRematch);


            mainContentGrid.Children.Add(this.boardMainGameView);
            mainContentGrid.Children.Add(this.chatHistoryMainGameView);
            mainContentGrid.Children.Add(grdOpponent);
            mainContentGrid.Children.Add(grdYou);
            mainContentGrid.Children.Add(grdButtons);

            return mainContentGrid;
        }

        public void setCaptureLayout()
        {
            if (this.gameView.GameController.ChessGame.IndexColor == 1)
            {
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\RookB.png");
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\KnightB.png");
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\BishopB.png");
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\QueenB.png");
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\PawnB.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\RookW.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\KnightW.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\BishopW.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\QueenW.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\PawnW.png");
            }
            else
            if (this.gameView.GameController.ChessGame.IndexColor == 0)
            {
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\RookW.png");
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\KnightW.png");
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\BishopW.png");
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\QueenW.png");
                this.addCapture(this.stPOpponentPiece, this.stPOpponentNumber, Directory.GetCurrentDirectory() + @"\Images\PawnW.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\RookB.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\KnightB.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\BishopB.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\QueenB.png");
                this.addCapture(this.stPYouPiece, this.stPYouNumber, Directory.GetCurrentDirectory() + @"\Images\PawnB.png");
            }
        }

        public void addCapture(StackPanel stackPanelImage, StackPanel stackPanelNumber, string pathImage)
        {
            imagesCollection.Add(pathImage);

            Image captureImage = new Image()
            {
                Source = new BitmapImage(new Uri(pathImage)),
                Width = 60,
                Height = 60,
                Margin = new Thickness(5),
            };
            if(stackPanelNumber == stPOpponentNumber)
                captureImage.MouseDown += CaptureImage_MouseDown;
            stackPanelImage.Children.Add(captureImage);

            Label captureNumber = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 70,
                Height = 30
            };
            captureNumber.Content = 0;
            stackPanelNumber.Children.Add(captureNumber);
        }

        public bool canChangePawn()
        {
            for (int i = 0; i < 4; i++)
                if((stPOpponentNumber.Children[i] as Label).Content.ToString() != "0")
                    return true;
            return false;
        }

        private void CaptureImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.changePawn == true)
            {
                for (int i = 0; i < 4; i++)
                {
                    if ((sender as Image) == (stPOpponentPiece.Children[i] as Image) && (stPOpponentNumber.Children[i] as Label).Content.ToString() != "0")
                    {
                        (stPOpponentNumber.Children[i] as Label).Content = (int.Parse((stPOpponentNumber.Children[i] as Label).Content.ToString()) - 1).ToString();
                        string color = this.gameView.GameController.ChessGame.MyColor == PieceColor.White ? "W" : "B";
                        this.addNumber(stPOpponentNumber, Directory.GetCurrentDirectory() + @$"\Images\Pawn{color}.png", 1);
                        for (int j = 0; j < 8; j++)
                        {
                            if (this.gameView.GameController.ChessGame.Squares[0, j].Piece is Pawn && (this.gameView.GameController.ChessGame.Squares[0, j].Piece as Piece).PieceColor == this.gameView.GameController.ChessGame.MyColor)
                            {
                                this.boardMainGameView.CanvasBoard.Children.Remove((this.gameView.GameController.ChessGame.Squares[0, j].Piece as Piece).MyImage);

                                if (this.imagesCollection[i].Equals(Directory.GetCurrentDirectory() + @"\Images\RookW.png") == true || this.imagesCollection[i].Equals(Directory.GetCurrentDirectory() + @"\Images\RookB.png") == true)
                                {
                                    this.gameView.GameController.ChessGame.Squares[0, j] = new Square(new Rook(this.gameView.GameController.ChessGame.MyColor, new Point(0, j), this.gameView.GameController.ChessGame));
                                    this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|changePawn|Rook");
                                }
                                else
                                if (this.imagesCollection[i].Equals(Directory.GetCurrentDirectory() + @"\Images\QueenW.png") == true || this.imagesCollection[i].Equals(Directory.GetCurrentDirectory() + @"\Images\QueenB.png") == true)
                                {
                                    this.gameView.GameController.ChessGame.Squares[0, j] = new Square(new Queen(this.gameView.GameController.ChessGame.MyColor, new Point(0, j), this.gameView.GameController.ChessGame));
                                    this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|changePawn|Queen");
                                }
                                else
                                if (this.imagesCollection[i].Equals(Directory.GetCurrentDirectory() + @"\Images\BishopW.png") == true || this.imagesCollection[i].Equals(Directory.GetCurrentDirectory() + @"\Images\BishopB.png") == true)
                                {
                                    this.gameView.GameController.ChessGame.Squares[0, j] = new Square(new Bishop(this.gameView.GameController.ChessGame.MyColor, new Point(0, j), this.gameView.GameController.ChessGame));
                                    this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|changePawn|Bishop");
                                }
                                else
                                if (this.imagesCollection[i].Equals(Directory.GetCurrentDirectory() + @"\Images\KnightW.png") == true || this.imagesCollection[i].Equals(Directory.GetCurrentDirectory() + @"\Images\KnightB.png") == true)
                                {
                                    this.gameView.GameController.ChessGame.Squares[0, j] = new Square(new Knight(this.gameView.GameController.ChessGame.MyColor, new Point(0, j), this.gameView.GameController.ChessGame));
                                    this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|changePawn|Knight");
                                }

                                Canvas.SetLeft((this.gameView.GameController.ChessGame.Squares[0, j].Piece as Piece).MyImage, j * 85);
                                Canvas.SetTop((this.gameView.GameController.ChessGame.Squares[0, j].Piece as Piece).MyImage, 0);

                                this.boardMainGameView.CanvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[0, j].Piece as Piece).MyImage);

                                for (int i1 = 0; i1 < 8; i1++) for (int j1 = 0; j1 < 8; j1++) { (this.gameView.GameController.ChessGame.Squares[i1, j1].Piece as Piece).CollisionMoves = new ObservableCollection<string>(); (this.gameView.GameController.ChessGame.Squares[i1, j1].Piece as Piece).SetChessGame(gameView.GameController.ChessGame); Panel.SetZIndex((this.gameView.GameController.ChessGame.Squares[i1, j1].Piece as Piece).MyImage, 2); }
                                for (int i1 = 0; i1 < 8; i1++) for (int j1 = 0; j1 < 8; j1++) (this.gameView.GameController.ChessGame.Squares[i1, j1].Piece as Piece).possiblePositions();

                                this.changePawn = false;
                                break;
                            }
                        }

                    }
                }
            }
        }

        public void addNumber(StackPanel stackPanel, string pathImage, int number)
        {
            for (int i = 0; i < this.imagesCollection.Count; i++)
            {
                if (imagesCollection[i].Equals(pathImage) == true)
                {
                    int textNumber = int.Parse((stackPanel.Children[i % 5] as Label).Content.ToString());
                    (stackPanel.Children[i % 5] as Label).Content = (textNumber + number).ToString();
                    break;
                }
            }
        }

        public void initializeNumbers()
        {
            StackPanel stackPanel = this.stPOpponentNumber;
            for (int i = 0; i < this.imagesCollection.Count; i++)
            {
                if (i > 4)
                    stackPanel = stPYouNumber;
                (stackPanel.Children[i % 5] as Label).Content = "0";
            }
        }

        public void InitializeTimerYou()
        {
            this.timerYou = null;
            timerYou = new DispatcherTimer();
            timerYou.Interval = TimeSpan.FromSeconds(1);
            timerYou.Tick += Timer_Tick_You;
            timeRemainingYou = TimeSpan.FromMinutes(10);
        }

        public void Timer_Tick_You(object sender, EventArgs e)
        {
            timeRemainingYou = timeRemainingYou.Subtract(TimeSpan.FromSeconds(1));
            LblTimerYou.Content = $"{timeRemainingYou:mm\\:ss}";

            if (timeRemainingYou.TotalSeconds <= 0)
            {
                timerYou.Stop();

                this.gameView.GameController.stopTheGame();
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("You lose! You can play again by pressing the Rematch button!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
                this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|youWin");
            }
        }

        public void InitializeTimerOpponent()
        {
            timerOpponent = null;
            timerOpponent = new DispatcherTimer();
            timerOpponent.Interval = TimeSpan.FromSeconds(1);
            timerOpponent.Tick += Timer_Tick_Opponent;
            timeRemainingOpponent = TimeSpan.FromMinutes(10);
        }

        public void Timer_Tick_Opponent(object sender, EventArgs e)
        {
            timeRemainingOpponent = timeRemainingOpponent.Subtract(TimeSpan.FromSeconds(1));
            LblTimerOpponent.Content = $"{timeRemainingOpponent:mm\\:ss}";

            if (timeRemainingOpponent.TotalSeconds <= 0)
            {
                timerOpponent.Stop();

                this.gameView.GameController.stopTheGame();
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("You Win! You can play again by pressing the Rematch button!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
                this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|youLose");
            }
        }



        private void BtnResignMainMenu_Click(object sender, RoutedEventArgs e)
        {
            if(this.btnResignMainMenu.Content.Equals("Resign"))
            {
                this.gameView.GameController.stopTheGame();
                this.BtnResignMainMenu.Content = "Main Menu";
                this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|resign");
                
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("You lose! You can send a new game request by pressing the Rematch button!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
            }
            else
            if (this.btnResignMainMenu.Content.Equals("Main Menu"))
            {
                if(this.gameView.GameController.Network.Opponent!=null)
                    this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|leave");


                ConnectView connectView = new ConnectView();
                ConnectController connectController = new ConnectController(this.gameView.GameController.Network, connectView);
                this.gameView.GameController.Network.ConnectController =connectController;
                this.gameView.GameController.Network.GameController = null;

                this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.You.IP + "|connectController|tableRequest|" + this.gameView.GameController.Network.You.ToString() + "|withoutAddTable");

                this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|all|gameController|gameOver");

                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("The game is over!");
                myMessageBox.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();

                this.gameView.Close();
            }
        }


        private void BtnRematch_Click(object sender, RoutedEventArgs e)
        {
            if (this.rematchInvitation == false)
            {
                this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|rematchRequest");
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("You have successfully submitted a rematch request!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
            }
            else
            if(this.rematchInvitation==true) 
            {
                this.rematchInvitation = false;
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("The match has started! Good luck!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();

                this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|rematchAccept");

                if (this.gameView.GameController.ChessGame.IndexColor == 0)
                    this.gameView.GameController.ChessGame.IsYourTurn = true;
                else
                if (this.gameView.GameController.ChessGame.IndexColor == 1)
                    this.gameView.GameController.ChessGame.IsYourTurn = false;

                this.gameView.GameController.rematch();                
            }
        }

        public bool RematchInvitation
        {
            get => this.rematchInvitation; set => this.rematchInvitation = value;
        }
        public bool ChangePawn
        {
            get => this.changePawn; set => this.changePawn = value;
        }
        public DispatcherTimer TimerYou
        {
            get => this.timerYou; set => this.timerYou = value;
        }
        public TimeSpan TimeRemainingYou
        {
            get => this.timeRemainingYou; set => this.timeRemainingYou = value;
        }
        public DispatcherTimer TimerOpponent
        {
            get => this.timerOpponent; set => this.timerOpponent= value;
        }
        public TimeSpan TimeRemainingOpponent
        {
            get => this.timeRemainingOpponent; set => this.timeRemainingOpponent = value;
        }
        public Button BtnRematch
        {
            get => this.btnRematch; set => this.btnRematch = value;
        }
        public Button BtnResignMainMenu
        {
            get => this.btnResignMainMenu; set => this.btnResignMainMenu = value;
        }
        public Label LblTimerYou
        {
            get => this.lblTimerYou; set => this.lblTimerYou = value;
        }
        public StackPanel StPYouNumber
        {
            get => this.stPYouNumber; set => this.stPYouNumber = value;
        }
        public StackPanel StPYouPiece
        {
            get => this.stPYouPiece; set => stPYouPiece = value;
        }
        public Label LblYou
        {
            get => this.lblYou; set => this.lblYou = value;
        }
        public Label LblTimerOpponent
        {
            get => this.lblTimerOpponent; set => this.lblTimerOpponent = value;
        }
        public StackPanel StPOpponentNumber
        {
            get => this.stPOpponentNumber; set => this.stPOpponentNumber = value;
        }
        public StackPanel StPOpponentPiece
        {
            get => this.stPOpponentPiece; set => this.stPOpponentPiece = value;
        }
        public Label LblOpponent
        {
            get => this.lblOpponent; set => this.lblOpponent = value;
        }
        public BoardMainGameView BoardMainGameView
        {
            get => this.boardMainGameView; set => this.boardMainGameView = value;
        }
        public ChatHistoryMainGameView ChatHistoryMainGameView
        {
            get => this.chatHistoryMainGameView; set => this.chatHistoryMainGameView = value;
        }
        public GameView GameView
        {
            get => this.gameView; set => this.gameView = value;
        }
    }
}
