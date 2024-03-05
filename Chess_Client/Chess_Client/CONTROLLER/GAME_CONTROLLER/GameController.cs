using Chess_Client.MODEL;
using Chess_Client.MODEL.GAME_MODEL;
using Chess_Client.VIEW.CONNECT_VIEW;
using Chess_Client.VIEW.GAME_VIEW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Threading;
using Chess_Client.VIEW;
using System.Collections;
using Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS;
using Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_PIECES;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Chess_Client.CONTROLLER.GAME_CONTROLLER
{
    public class GameController
    {
        private Network network;
        private GameView gameView;

        private ChessGame chessGame;

        private bool enableChat;

        public GameController(Network network, GameView gameView, int indexColor, int indexMirror)
        {
            this.chessGame = new ChessGame(this, indexColor, indexMirror);

            this.network = network;
            this.network.GameController = this;
            this.gameView = gameView;
            this.gameView.GameController = this;

            this.enableChat = true;

            this.gameView.MainGameView.LblOpponent.Content = this.network.Opponent.Name;
            this.gameView.MainGameView.LblYou.Content = this.network.You.Name;

            if (this.chessGame.IndexMirror == 0)
            {
                string path = Directory.GetCurrentDirectory() + @"\Images\Board.png";
                ImageBrush imageBrush = new ImageBrush { ImageSource = new BitmapImage(new Uri(path)) };
                this.gameView.MainGameView.BoardMainGameView.GrdBoard.Background = imageBrush;
            }
            else
            if (this.chessGame.IndexMirror == 7)
            {
                string path = Directory.GetCurrentDirectory() + @"\Images\BoardMirrored.png";
                ImageBrush imageBrush = new ImageBrush { ImageSource = new BitmapImage(new Uri(path)) };
                this.gameView.MainGameView.BoardMainGameView.GrdBoard.Background = imageBrush;
            }
            this.gameView.MainGameView.setCaptureLayout();

            if (this.chessGame.IsYourTurn == true)
            {
                this.gameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.Columns.Add(new DataGridTextColumn { Header = this.network.You.Name, Binding = new Binding("FirstMove"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                this.gameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.Columns.Add(new DataGridTextColumn { Header = this.network.Opponent.Name, Binding = new Binding("SecondMove"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                this.gameView.MainGameView.TimerYou.Start();

                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("The match has started! It's your turn! Good luck!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
            }
            else
            if (this.chessGame.IsYourTurn == false)
            {
                this.gameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.Columns.Add(new DataGridTextColumn { Header = this.network.Opponent.Name, Binding = new Binding("FirstMove"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                this.gameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.Columns.Add(new DataGridTextColumn { Header = this.network.You.Name, Binding = new Binding("SecondMove"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                this.gameView.MainGameView.TimerOpponent.Start();

                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("The match has started! It's the opponent's turn! Good luck!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
            }
            this.startTheGame();
        }



        public void decode(string code)
        {
            string[] splitCode = code.Split("|");
            if (splitCode[3].Equals("sendMessage") == true)
            {
                this.gameView.MainGameView.ChatHistoryMainGameView.sendMessage(this.network.Opponent.Name, splitCode[4], 1);
            }
            else
            if (splitCode[3].Equals("leave") == true)
            {
                this.network.Opponent = null;

                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("The opponent has disconnected!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();

                loseConnection();
                stopTheGame();
            }
            else
            if (splitCode[3].Equals("resign") == true)
            {
                this.gameView.MainGameView.BtnResignMainMenu.Content = "Main Menu";

                stopTheGame();

                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("You win! You can send a new game request by pressing the Rematch button!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
            }
            else
            if (splitCode[3].Equals("movePiece") == true)
            {
                if (this.chessGame.Squares[int.Parse(splitCode[6]), int.Parse(splitCode[7])].Piece is Empty)
                    this.chessGame.NumberOfMovesWithoutCapture++;
                if (this.chessGame.Squares[int.Parse(splitCode[6]), int.Parse(splitCode[7])].Piece is not Empty)
                    this.chessGame.NumberOfMovesWithoutCapture = 0;

                this.gameView.MainGameView.TimerYou.Start();
                this.gameView.MainGameView.TimerOpponent.Stop();

                this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.chessGame.LastMoveDark1);
                this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.chessGame.LastMoveDark2);
                this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.chessGame.LastMoveLight1);
                this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.chessGame.LastMoveLight2);

                this.chessGame.setHistory(this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece, this.chessGame.Squares[int.Parse(splitCode[6]), int.Parse(splitCode[7])].Piece);
                this.chessGame.setStackPanelCapturePiece(this.chessGame.Squares[int.Parse(splitCode[6]), int.Parse(splitCode[7])].Piece, 1);
                if ((int.Parse(splitCode[4]) + int.Parse(splitCode[5])) % 2 == 0)
                {
                    Canvas.SetLeft(this.chessGame.LastMoveLight3, int.Parse(splitCode[5]) * 85);
                    Canvas.SetTop(this.chessGame.LastMoveLight3, int.Parse(splitCode[4]) * 85);
                    this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.chessGame.LastMoveLight3);
                }
                else
                if ((int.Parse(splitCode[4]) + int.Parse(splitCode[5])) % 2 == 1)
                {
                    Canvas.SetLeft(this.chessGame.LastMoveDark3, int.Parse(splitCode[5]) * 85);
                    Canvas.SetTop(this.chessGame.LastMoveDark3, int.Parse(splitCode[4]) * 85);
                    this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.chessGame.LastMoveDark3);
                }

                if ((int.Parse(splitCode[6]) + int.Parse(splitCode[7])) % 2 == 0)
                {
                    Canvas.SetLeft(this.chessGame.LastMoveLight4, int.Parse(splitCode[7]) * 85);
                    Canvas.SetTop(this.chessGame.LastMoveLight4, int.Parse(splitCode[6]) * 85);
                    this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.chessGame.LastMoveLight4);
                }
                else
                if ((int.Parse(splitCode[6]) + int.Parse(splitCode[7])) % 2 == 1)
                {
                    Canvas.SetLeft(this.chessGame.LastMoveDark4, int.Parse(splitCode[7]) * 85);
                    Canvas.SetTop(this.chessGame.LastMoveDark4, int.Parse(splitCode[6]) * 85);
                    this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.chessGame.LastMoveDark4);
                }

                if (this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece is Pawn && (this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as Pawn).IndexFirstMove == 0)
                    (this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as Pawn).IndexFirstMove = 1;
                else
                if (this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece is Rook && (this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as Rook).AlreadyMoved == false)
                    (this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as Rook).AlreadyMoved = true;
                else
                if (this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece is King && (this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as King).AlreadyMoved == false)
                    (this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as King).AlreadyMoved = true;

                Canvas.SetLeft((this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as Piece).MyImage, int.Parse(splitCode[7]) * 85);
                Canvas.SetTop((this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as Piece).MyImage, int.Parse(splitCode[6]) * 85);
                object piece = this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece;
                this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove((this.chessGame.Squares[int.Parse(splitCode[6]), int.Parse(splitCode[7])].Piece as Piece).MyImage);
                this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])] = new Square(new Empty(PieceColor.None, new Point(int.Parse(splitCode[4]), int.Parse(splitCode[5])), this.chessGame));
                this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add((this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as Piece).MyImage);
                Canvas.SetLeft((this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as Piece).MyImage, int.Parse(splitCode[5]) * 85);
                Canvas.SetTop((this.chessGame.Squares[int.Parse(splitCode[4]), int.Parse(splitCode[5])].Piece as Piece).MyImage, int.Parse(splitCode[4]) * 85);
                this.chessGame.Squares[int.Parse(splitCode[6]), int.Parse(splitCode[7])].Piece = piece;
                (this.chessGame.Squares[int.Parse(splitCode[6]), int.Parse(splitCode[7])].Piece as Piece).Point = new Point(int.Parse(splitCode[6]), int.Parse(splitCode[7]));


                for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) { (this.chessGame.Squares[i, j].Piece as Piece).CollisionMoves = new ObservableCollection<string>(); (this.chessGame.Squares[i, j].Piece as Piece).SetChessGame(this.chessGame); Panel.SetZIndex((this.chessGame.Squares[i, j].Piece as Piece).MyImage, 2); }
                for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) (this.chessGame.Squares[i, j].Piece as Piece).possiblePositions();


                this.chessGame.IsYourTurn = true;
            }
            else
            if (splitCode[3].Equals("changePawn") == true)
            {
                if (splitCode[4].Equals("wait") == true)
                {
                    this.chessGame.IsYourTurn = false;
                    foreach (object window in Application.Current.Windows)
                        if (window is MyMessageBox)
                            (window as MyMessageBox).Close();
                    this.gameView.IsEnabled = false;
                    MyMessageBox myMessageBox = new MyMessageBox("Your opponent can convert a pawn! Wait until he chooses the piece he wants to transform!");
                    myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                    myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                    myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                    myMessageBox.Show();
                }
                else
                {
                    foreach (object window in Application.Current.Windows)
                        if (window is MyMessageBox)
                            (window as MyMessageBox).Close();
                    this.chessGame.IsYourTurn = true;
                    string color = this.chessGame.MyColor == PieceColor.White ? "W" : "B";
                    this.gameView.MainGameView.addNumber(this.gameView.MainGameView.StPYouNumber, Directory.GetCurrentDirectory() + @$"\Images\{splitCode[4]}{color}.png", -1);
                    this.gameView.MainGameView.addNumber(this.gameView.MainGameView.StPYouNumber, Directory.GetCurrentDirectory() + @$"\Images\Pawn{color}.png", 1);

                    for (int j = 0; j < 8; j++)
                    {
                        if (this.gameView.GameController.ChessGame.Squares[7, j].Piece is Pawn && (this.chessGame.Squares[7, j].Piece as Piece).PieceColor == this.gameView.GameController.ChessGame.EnemyColor)
                        {
                            this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove((this.chessGame.Squares[7, j].Piece as Piece).MyImage);

                            if (splitCode[4].Equals("Knight") == true)
                                this.gameView.GameController.ChessGame.Squares[7, j] = new Square(new Knight(this.gameView.GameController.ChessGame.EnemyColor, new Point(7, j), this.gameView.GameController.ChessGame));
                            else
                            if (splitCode[4].Equals("Queen") == true)
                                this.gameView.GameController.ChessGame.Squares[7, j] = new Square(new Queen(this.gameView.GameController.ChessGame.EnemyColor, new Point(7, j), this.gameView.GameController.ChessGame));
                            else
                            if (splitCode[4].Equals("Rook") == true)
                                this.gameView.GameController.ChessGame.Squares[7, j] = new Square(new Rook(this.gameView.GameController.ChessGame.EnemyColor, new Point(7, j), this.gameView.GameController.ChessGame));
                            else
                            if (splitCode[4].Equals("Bishop") == true)
                                this.gameView.GameController.ChessGame.Squares[7, j] = new Square(new Bishop(this.gameView.GameController.ChessGame.EnemyColor, new Point(7, j), this.gameView.GameController.ChessGame));

                            Canvas.SetLeft((this.gameView.GameController.ChessGame.Squares[7, j].Piece as Piece).MyImage, j * 85);
                            Canvas.SetTop((this.gameView.GameController.ChessGame.Squares[7, j].Piece as Piece).MyImage, 595);

                            this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add((this.gameView.GameController.ChessGame.Squares[7, j].Piece as Piece).MyImage);

                            for (int i1 = 0; i1 < 8; i1++) for (int j1 = 0; j1 < 8; j1++) { (this.chessGame.Squares[i1, j1].Piece as Piece).CollisionMoves = new ObservableCollection<string>(); (this.chessGame.Squares[i1, j1].Piece as Piece).SetChessGame(this.chessGame); Panel.SetZIndex((this.chessGame.Squares[i1, j1].Piece as Piece).MyImage, 2); }
                            for (int i1 = 0; i1 < 8; i1++) for (int j1 = 0; j1 < 8; j1++) (this.chessGame.Squares[i1, j1].Piece as Piece).possiblePositions();

                            break;
                        }
                    }

                }

            }
            else
            if (splitCode[3].Equals("moveCastle") == true)
            {
                this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove((this.chessGame.Squares[0, int.Parse(splitCode[4])].Piece as Piece).MyImage);
                Canvas.SetLeft((this.chessGame.Squares[0, int.Parse(splitCode[5])].Piece as Piece).MyImage, int.Parse(splitCode[4]) * 85);
                Canvas.SetTop((this.chessGame.Squares[0, int.Parse(splitCode[5])].Piece as Piece).MyImage, 0);
                this.chessGame.Squares[0, int.Parse(splitCode[4])].Piece = this.chessGame.Squares[0, int.Parse(splitCode[5])].Piece;
                (this.chessGame.Squares[0, int.Parse(splitCode[4])].Piece as Rook).Point = new Point(0, int.Parse(splitCode[4]));
                (this.chessGame.Squares[0, int.Parse(splitCode[4])].Piece as Rook).AlreadyMoved = true;

                this.chessGame.Squares[0, int.Parse(splitCode[5])] = new Square(new Empty(PieceColor.None, new Point(0, int.Parse(splitCode[5])), this.chessGame));
                Canvas.SetLeft((this.chessGame.Squares[0, int.Parse(splitCode[5])].Piece as Piece).MyImage, int.Parse(splitCode[5]) * 85);
                Canvas.SetTop((this.chessGame.Squares[0, int.Parse(splitCode[5])].Piece as Piece).MyImage, 0);
                this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add((this.chessGame.Squares[0, int.Parse(splitCode[5])].Piece as Piece).MyImage);

                for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) { (this.chessGame.Squares[i, j].Piece as Piece).CollisionMoves = new ObservableCollection<string>(); (this.chessGame.Squares[i, j].Piece as Piece).SetChessGame(this.chessGame); Panel.SetZIndex((this.chessGame.Squares[i, j].Piece as Piece).MyImage, 2); }
                for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) (this.chessGame.Squares[i, j].Piece as Piece).possiblePositions();
            }
            else
            if (splitCode[3].Equals("draw") == true)
            {
                this.stopTheGame();

                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox(splitCode[4]);
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
            }
            else
            if (splitCode[3].Equals("youWin") == true)
            {
                this.stopTheGame();
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("You win! You can play again by pressing the Rematch button!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
            }
            else
            if (splitCode[3].Equals("youLose") == true)
            {
                this.stopTheGame();
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("You lose! You can play again by pressing the Rematch button!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();
            }
            else
            if (splitCode[3].Equals("rematchRequest") == true)
            {
                this.gameView.MainGameView.RematchInvitation = true;
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("You have received a rematch request!");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; }; ;
                myMessageBox.Show();
            }
            else
            if (splitCode[3].Equals("rematchAccept") == true)
            {
                this.gameView.MainGameView.RematchInvitation = false;
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("The match has started! Good luck! asta");
                myMessageBox.Left = this.gameView.Left + (this.gameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameView.Top + (this.gameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameView.IsEnabled = true; };
                myMessageBox.Show();

                if (this.chessGame.IndexColor == 0)
                    this.chessGame.IsYourTurn = true;
                else
                if (this.chessGame.IndexColor == 1)
                    this.chessGame.IsYourTurn = false;
                this.rematch();
            }

        }

        public void rematch()
        {
            this.gameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Clear();
            this.gameView.MainGameView.TimerOpponent.Stop();
            this.gameView.MainGameView.TimerYou.Stop();

            this.gameView.MainGameView.InitializeTimerOpponent();
            this.gameView.MainGameView.InitializeTimerYou();
            this.gameView.MainGameView.LblTimerOpponent.Content = "10:00";
            this.gameView.MainGameView.LblTimerYou.Content = "10:00";

            if (this.chessGame.IndexColor == 0)
            {
                this.gameView.MainGameView.TimerYou.Start();
                this.gameView.MainGameView.TimerOpponent.Stop();
            }
            else
            if (this.chessGame.IndexColor == 1)
            {
                this.gameView.MainGameView.TimerYou.Stop();
                this.gameView.MainGameView.TimerOpponent.Start();
            }
            this.gameView.MainGameView.initializeNumbers();
            this.gameView.MainGameView.ChatHistoryMainGameView.History = new ObservableCollection<History>();
            this.gameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.ItemsSource = this.gameView.MainGameView.ChatHistoryMainGameView.History;
            this.gameView.MainGameView.BtnResignMainMenu.Content = "Resign";
            this.startTheGame();
        }

        public void loseConnection()
        {
            this.enableChat = false;
            this.gameView.MainGameView.BtnResignMainMenu.Content = "Main Menu";
            this.gameView.MainGameView.BtnRematch.IsEnabled = false;
            this.gameView.MainGameView.ChatHistoryMainGameView.BtnSend.IsEnabled = false;
        }

        public void stopTheGame()
        {
            this.chessGame.stopTheGame();
        }

        public void startTheGame()
        {
            this.chessGame.startTheGame();
            this.gameView.MainGameView.BoardMainGameView.initializeBoard();
        }




        public bool EnableChat
        {
            get => this.enableChat; set => this.enableChat = value;
        }
        public Network Network
        {
            get => this.network; set => this.network = value;
        }
        public GameView GameView
        {
            get => this.gameView; set => this.gameView = value;
        }
        public ChessGame ChessGame
        {
            get => this.chessGame; set => this.chessGame = value;
        }
    }
}
