using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_PIECES;
using Chess_Client.VIEW;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS
{
    public class ChessGame : Board
    {
        private GameController gameController;

        private int indexColor, indexMirror, indexAttention, numberOfMovesWithoutCapture;
        private bool isYourTurn, isDragging;
        private DispatcherTimer timer;
        private object lastPickedPiece;
        private MyImage lastMoveDark1, lastMoveLight1, lastMoveDark2, lastMoveLight2, lastMoveDark3, lastMoveLight3, lastMoveDark4, lastMoveLight4, lastBorderedPiece, attentionMyImage;
        private string pathTransparent, pathPossibleMove, pathPossibleCapture, pathLastMoveDark, pathLastMoveLight, pathBorderMove, pathAttention, moveBigCastle, moveSmallCastle;

        public ChessGame(GameController gameController, int indexColor, int indexMirror) : base(indexColor)
        {
            this.gameController = gameController;
            this.indexColor = indexColor;
            this.indexMirror = indexMirror * 7;
            this.isYourTurn = indexColor == 0 ? true : false;
            this.numberOfMovesWithoutCapture = 0;
            base.ChessGame = this;
            this.isDragging = false;
            this.lastPickedPiece = null;

            this.pathTransparent = Directory.GetCurrentDirectory() + @"\Images\Transparent.png";
            this.pathPossibleMove = Directory.GetCurrentDirectory() + @"\Images\PossibleMove.png";
            this.pathPossibleCapture = Directory.GetCurrentDirectory() + @"\Images\PossibleCapture.png";
            this.pathLastMoveDark = Directory.GetCurrentDirectory() + @"\Images\LastMoveDark.png";
            this.pathLastMoveLight = Directory.GetCurrentDirectory() + @"\Images\LastMoveLight.png";
            this.pathBorderMove = Directory.GetCurrentDirectory() + @"\Images\BorderMove.png";
            this.pathAttention = Directory.GetCurrentDirectory() + @"\Images\Attention.png";

            this.lastMoveDark1 = new MyImage(pathLastMoveDark, pathTransparent, this.MyColor);
            this.lastMoveDark2 = new MyImage(pathLastMoveDark, pathTransparent, this.MyColor);
            this.lastMoveDark3 = new MyImage(pathLastMoveDark, pathTransparent, this.MyColor);
            this.lastMoveDark4 = new MyImage(pathLastMoveDark, pathTransparent, this.MyColor);

            this.lastMoveLight1 = new MyImage(pathLastMoveLight, pathTransparent, this.MyColor);
            this.lastMoveLight2 = new MyImage(pathLastMoveLight, pathTransparent, this.MyColor);
            this.lastMoveLight3 = new MyImage(pathLastMoveLight, pathTransparent, this.MyColor);
            this.lastMoveLight4 = new MyImage(pathLastMoveLight, pathTransparent, this.MyColor);

            this.lastBorderedPiece = new MyImage(pathBorderMove, pathTransparent, this.MyColor);
            this.attentionMyImage = new MyImage(pathAttention, pathTransparent, this.MyColor);

            Panel.SetZIndex(this.attentionMyImage, 1);
            Panel.SetZIndex(this.lastMoveDark1, 0);
            Panel.SetZIndex(this.lastMoveDark2, 0);
            Panel.SetZIndex(this.lastMoveDark3, 0);
            Panel.SetZIndex(this.lastMoveDark4, 0);
            Panel.SetZIndex(this.lastMoveLight1, 0);
            Panel.SetZIndex(this.lastMoveLight2, 0);
            Panel.SetZIndex(this.lastMoveLight3, 0);
            Panel.SetZIndex(this.lastMoveLight4, 0);
            Panel.SetZIndex(this.lastBorderedPiece, 1);


            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(0.4);
            this.timer.Tick += Timer_Tick;
        }

        public void startTheGame()
        {
            base.setupBoard();
            this.isYourTurn = indexColor == 0 ? true : false;
        }

        public void stopTheGame()
        {
            this.gameController.GameView.MainGameView.BtnResignMainMenu.Content = "Main Menu";
            this.gameController.GameView.MainGameView.ChangePawn = true;
            this.isYourTurn = false;
            this.gameController.GameView.MainGameView.TimerYou.Stop();
            this.gameController.GameView.MainGameView.TimerOpponent.Stop();
        }


        public void attention(Point point)
        {
            this.indexAttention = 0;

            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.attentionMyImage);
            Canvas.SetLeft(this.attentionMyImage, point.Y * 85);
            Canvas.SetTop(this.attentionMyImage, point.X * 85);
            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.attentionMyImage);

            this.timer.Stop();
            this.timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.indexAttention == 4)
            {
                this.timer.Stop();

            }
            if (this.indexAttention % 2 == 0)
            {
                this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.attentionMyImage);
            }
            else
            if (this.indexAttention % 2 == 1)
            {
                this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.attentionMyImage);
            }
            this.indexAttention++;
        }

        public void mouseDown(object piece, MouseButtonEventArgs e)
        {
            if (this.isYourTurn == true)
            {
                if ((piece as Piece).PieceColor == this.MyColor)
                {
                    this.GameController.GameView.Cursor = Cursors.Hand;

                    this.isDragging = true;
                    (piece as Piece).MyImage.CaptureMouse();
                    this.movePiece(piece, e);

                    Panel.SetZIndex((piece as Piece).MyImage, 3);
                    double leftPosition = (piece as Piece).Point.Y * 85;
                    double topPosition = (piece as Piece).Point.X * 85;
                    Canvas.SetLeft(this.lastMoveDark1, leftPosition);
                    Canvas.SetTop(this.lastMoveDark1, topPosition);
                    Canvas.SetLeft(this.lastMoveLight1, leftPosition);
                    Canvas.SetTop(this.lastMoveLight1, topPosition);

                    if (this.lastPickedPiece != null)
                    {
                        foreach (string move in (this.lastPickedPiece as Piece).ValidMoves)
                        {
                            (this.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
                        }
                        (this.Squares[7, 1].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
                        (this.Squares[7, 6].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
                        (this.Squares[7, 2].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
                        (this.Squares[7, 5].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);

                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveLight1);
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveDark1);
                    }
                    this.lastPickedPiece = piece;

                    if (((piece as Piece).Point.X + (piece as Piece).Point.Y) % 2 == 0)
                    {
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.lastMoveLight1);
                    }
                    else
                    if (((piece as Piece).Point.X + (piece as Piece).Point.Y) % 2 == 1)
                    {
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.lastMoveDark1);
                    }

                    foreach (string move in (piece as Piece).ValidMoves)
                    {
                        Panel.SetZIndex((this.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).MyImage.Image, 2);
                        Panel.SetZIndex((this.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).MyImage, 1);

                        if (move.Split(",").Length == 3 && move.Split(",")[2].Equals("enemyColorCollision") == true)
                            (this.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).MyImage.setBackgroundPath(pathPossibleCapture);
                        else
                        if (move.Split(",").Length == 2)
                            (this.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).MyImage.setBackgroundPath(pathPossibleMove);
                    }
                    if (piece is King)
                    {
                        moveBigCastle = this.bigCastleCheck();
                        moveSmallCastle = this.smallCastleCheck();
                        if (moveBigCastle.Equals(string.Empty) == false)
                        {
                            Panel.SetZIndex((this.Squares[7, int.Parse(moveBigCastle)].Piece as Piece).MyImage.Image, 2);
                            Panel.SetZIndex((this.Squares[7, int.Parse(moveBigCastle)].Piece as Piece).MyImage, 1);
                            (this.Squares[7, int.Parse(moveBigCastle)].Piece as Piece).MyImage.setBackgroundPath(pathPossibleMove);
                        }
                        if (moveSmallCastle.Equals(string.Empty) == false)
                        {
                            Panel.SetZIndex((this.Squares[7, int.Parse(moveSmallCastle)].Piece as Piece).MyImage.Image, 2);
                            Panel.SetZIndex((this.Squares[7, int.Parse(moveSmallCastle)].Piece as Piece).MyImage, 1);
                            (this.Squares[7, int.Parse(moveSmallCastle)].Piece as Piece).MyImage.setBackgroundPath(pathPossibleMove);
                        }
                    }
                }
                else
                if (((this.Squares[(int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y].Piece as Piece).MyImage.BackgroundPath.Equals(this.pathPossibleCapture) == true || (this.Squares[(int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y].Piece as Piece).MyImage.BackgroundPath.Equals(this.pathPossibleMove) == true))
                {
                    if (this.myKingInCheck(this.lastPickedPiece, (int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y) == false)
                    {
                        this.makeMove(this.lastPickedPiece, (int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y);
                    }
                    else
                    {
                        this.attention((this.lastPickedPiece as Piece).Point);
                    }
                }
                else
                if ((piece as Piece).PieceColor != this.MyColor && this.lastPickedPiece != null)
                {
                    foreach (string move in (this.lastPickedPiece as Piece).ValidMoves)
                    {
                        (this.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
                    }

                    (this.Squares[7, 1].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
                    (this.Squares[7, 6].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
                    (this.Squares[7, 2].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
                    (this.Squares[7, 5].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);

                    this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveLight1);
                    this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveDark1);
                }
            }
        }

        public void mouseUp(object piece, MouseButtonEventArgs e)
        {
            if (this.isDragging == true)
            {
                this.GameController.GameView.Cursor = Cursors.Arrow;
                Point currentPos = e.GetPosition(this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard);
                int dY = (int)currentPos.X / 85;
                int dX = (int)currentPos.Y / 85;
                if (dX < 0)
                    dX = 0;
                if (dX > 7)
                    dX = 7;
                if (dY < 0)
                    dY = 0;
                if (dY > 7)
                    dY = 7;
                isDragging = false;
                (piece as Piece).MyImage.ReleaseMouseCapture();


                this.timer.Stop();
                this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.attentionMyImage);

                if (((this.Squares[dX, dY].Piece as Piece).MyImage.BackgroundPath.Equals(this.pathPossibleCapture) == true || (this.Squares[dX, dY].Piece as Piece).MyImage.BackgroundPath.Equals(this.pathPossibleMove) == true))
                {
                    if (this.myKingInCheck(piece, dX, dY) == false)
                    {
                        this.makeMove(piece, dX, dY);
                    }
                    else
                    {
                        for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) Panel.SetZIndex((this.Squares[i, j].Piece as Piece).MyImage, 2);
                        if ((piece as Piece).Point.X != dX || (piece as Piece).Point.Y != dY)
                            this.attention((piece as Piece).Point);
                        Canvas.SetLeft((piece as Piece).MyImage, (piece as Piece).Point.Y * 85);
                        Canvas.SetTop((piece as Piece).MyImage, (piece as Piece).Point.X * 85);
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) Panel.SetZIndex((this.Squares[i, j].Piece as Piece).MyImage, 2);
                    if ((piece as Piece).Point.X != dX || (piece as Piece).Point.Y != dY)
                        this.attention((piece as Piece).Point);
                    Canvas.SetLeft((piece as Piece).MyImage, (piece as Piece).Point.Y * 85);
                    Canvas.SetTop((piece as Piece).MyImage, (piece as Piece).Point.X * 85);
                }

                this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastBorderedPiece);

            }

        }

        public bool myKingInCheck(object piece, int dX, int dY)
        {
            Square[,] newSquares = new Square[8, 8];
            ChessGame chessGame = new ChessGame(null, this.indexColor, this.indexMirror);
            for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++)
                    newSquares[i, j] = (Square)this.Squares[i, j].Clone();

            newSquares[(int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y] = new Square(new Empty(PieceColor.None, new Point((piece as Piece).Point.X, (piece as Piece).Point.Y), chessGame));

            if (piece is Pawn)
                newSquares[dX, dY] = new Square(new Pawn((piece as Piece).PieceColor, new Point(dX, dY), chessGame));
            else
            if (piece is King)
                newSquares[dX, dY] = new Square(new King((piece as Piece).PieceColor, new Point(dX, dY), chessGame));
            else
            if (piece is Queen)
                newSquares[dX, dY] = new Square(new Queen((piece as Piece).PieceColor, new Point(dX, dY), chessGame));
            else
            if (piece is Bishop)
                newSquares[dX, dY] = new Square(new Bishop((piece as Piece).PieceColor, new Point(dX, dY), chessGame));
            else
            if (piece is Knight)
                newSquares[dX, dY] = new Square(new Knight((piece as Piece).PieceColor, new Point(dX, dY), chessGame));
            else
            if (piece is Rook)
                newSquares[dX, dY] = new Square(new Rook((piece as Piece).PieceColor, new Point(dX, dY), chessGame));

            chessGame.Squares = newSquares;
            for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) (newSquares[i, j].Piece as Piece).CollisionMoves = new ObservableCollection<string>();
            for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) (newSquares[i, j].Piece as Piece).possiblePositions();

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (newSquares[i, j].Piece is King)
                        if ((newSquares[i, j].Piece as King).PieceColor.ToString().Equals(this.MyColor.ToString()) == true && (newSquares[i, j].Piece as King).check() == true)
                            return true;

            return false;
        }

        public void mouseMove(object piece, MouseEventArgs e)
        {
            this.movePiece(piece, e);
        }

        public void movePiece(object piece, MouseEventArgs e)
        {
            if (this.isDragging)
            {
                Point currentPos = e.GetPosition(this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard);
                if (currentPos.X <= this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Width - 1 && currentPos.X >= 0)
                {
                    Canvas.SetLeft((piece as Piece).MyImage, currentPos.X - (piece as Piece).MyImage.Width / 2);

                    this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastBorderedPiece);
                    double leftPosition = ((int)currentPos.X / 85) * 85;
                    Canvas.SetLeft(this.lastBorderedPiece, leftPosition);
                    this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.lastBorderedPiece);
                }
                if (currentPos.Y <= this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Height - 1 && currentPos.Y >= 0)
                {
                    Canvas.SetTop((piece as Piece).MyImage, currentPos.Y - (piece as Piece).MyImage.Height / 2);

                    this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastBorderedPiece);
                    double topPosition = ((int)currentPos.Y / 85) * 85;
                    Canvas.SetTop(this.lastBorderedPiece, topPosition);
                    this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.lastBorderedPiece);
                }

            }
        }


        public void makeMove(object piece, int dX, int dY)
        {
            this.gameController.GameView.MainGameView.TimerYou.Stop();
            this.gameController.GameView.MainGameView.TimerOpponent.Start();

            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveDark2);
            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveDark3);
            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveDark4);
            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveLight2);
            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveLight3);
            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove(this.lastMoveLight4);

            if ((this.Squares[dX, dY].Piece as Piece).MyImage.BackgroundPath == this.pathPossibleMove)
                this.numberOfMovesWithoutCapture++;
            if ((this.Squares[dX, dY].Piece as Piece).MyImage.BackgroundPath == this.pathPossibleCapture)
                this.numberOfMovesWithoutCapture = 0;

            foreach (string move in (piece as Piece).ValidMoves)
                (this.Squares[int.Parse(move.Split(",")[0]), int.Parse(move.Split(",")[1])].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);

            (this.Squares[7, 1].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
            (this.Squares[7, 6].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
            (this.Squares[7, 2].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);
            (this.Squares[7, 5].Piece as Piece).MyImage.setBackgroundPath(pathTransparent);

            double leftPosition = dY * 85;
            double topPosition = dX * 85;
            Canvas.SetLeft(this.lastMoveDark2, leftPosition);
            Canvas.SetTop(this.lastMoveDark2, topPosition);
            Canvas.SetLeft(this.lastMoveLight2, leftPosition);
            Canvas.SetTop(this.lastMoveLight2, topPosition);

            if ((dX + dY) % 2 == 0)
            {
                this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.lastMoveLight2);
            }
            else
            if ((dX + dY) % 2 == 1)
            {
                this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add(this.lastMoveDark2);
            }
            Canvas.SetLeft((piece as Piece).MyImage, dY * 85);
            Canvas.SetTop((piece as Piece).MyImage, dX * 85);

            this.setStackPanelCapturePiece(this.Squares[dX, dY].Piece, 0);
            this.setHistory(piece, this.Squares[dX, dY].Piece);

            if (piece is Pawn && (piece as Pawn).IndexFirstMove == 0)
                (piece as Pawn).IndexFirstMove = 1;
            else
            if (piece is Rook && (piece as Rook).AlreadyMoved == false)
                (piece as Rook).AlreadyMoved = true;
            else
            if (piece is King && (piece as King).AlreadyMoved == false)
                (piece as King).AlreadyMoved = true;
            this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|movePiece|" + (7 - (int)(piece as Piece).Point.X) + "|" + (7 - (int)(piece as Piece).Point.Y) + "|" + (7 - (int)(this.Squares[dX, dY].Piece as Piece).Point.X) + "|" + (7 - (int)(this.Squares[dX, dY].Piece as Piece).Point.Y) + "|" + this.gameController.GameView.MainGameView.ChatHistoryMainGameView.History[this.gameController.GameView.MainGameView.ChatHistoryMainGameView.History.Count - 1].ToString());
            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove((this.Squares[dX, dY].Piece as Piece).MyImage);
            this.Squares[(int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y] = new Square(new Empty(PieceColor.None, new Point((int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y), this));
            Canvas.SetLeft((this.Squares[(int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y].Piece as Piece).MyImage, ((piece as Piece)).Point.Y * 85);
            Canvas.SetTop((this.Squares[(int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y].Piece as Piece).MyImage, ((piece as Piece)).Point.X * 85);
            this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add((this.Squares[(int)(piece as Piece).Point.X, (int)(piece as Piece).Point.Y].Piece as Piece).MyImage);
            this.Squares[dX, dY].Piece = piece;
            (this.Squares[dX, dY].Piece as Piece).Point = new Point(dX, dY);

            if (this.Squares[dX, dY].Piece is Pawn && (this.Squares[dX, dY].Piece as Piece).Point.X == 0)
            {
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameController.GameView.IsEnabled = false;
                MyMessageBox myMessageBox;
                if (this.gameController.GameView.MainGameView.canChangePawn() == true)
                {
                    myMessageBox = new MyMessageBox("You can transform the pawn! Choose the piece you want to convert!");
                    this.gameController.GameView.MainGameView.ChangePawn = true;
                    this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|changePawn|wait");
                }
                else
                    myMessageBox = new MyMessageBox("There is no piece to replace the pawn!");
                myMessageBox.Left = this.gameController.GameView.Left + (this.gameController.GameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameController.GameView.Top + (this.gameController.GameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameController.GameView.IsEnabled = true; };
                myMessageBox.Show();
            }


            if (this.Squares[dX, dY].Piece is King)
            {
                if (moveBigCastle.Equals(string.Empty) == false && int.Parse(moveBigCastle) == dY && dX == 7)
                {
                    if (moveBigCastle.Equals("2") == true)
                    {
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove((this.Squares[7, 3].Piece as Piece).MyImage);
                        Canvas.SetLeft((this.Squares[7, 0].Piece as Piece).MyImage, 255);
                        Canvas.SetTop((this.Squares[7, 0].Piece as Piece).MyImage, 595);
                        this.Squares[7, 3].Piece = this.Squares[7, 0].Piece;
                        (this.Squares[7, 3].Piece as Rook).Point = new Point(7, 3);
                        (this.Squares[7, 3].Piece as Rook).AlreadyMoved = true;

                        this.Squares[7, 0] = new Square(new Empty(PieceColor.None, new Point(7, 0), this));
                        Canvas.SetLeft((this.Squares[7, 0].Piece as Piece).MyImage, 0);
                        Canvas.SetTop((this.Squares[7, 0].Piece as Piece).MyImage, 595);
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add((this.Squares[7, 0].Piece as Piece).MyImage);

                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|moveCastle|4|7");
                    }
                    else
                    if (moveBigCastle.Equals("5") == true)
                    {
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove((this.Squares[7, 4].Piece as Piece).MyImage);
                        Canvas.SetLeft((this.Squares[7, 7].Piece as Piece).MyImage, 340);
                        Canvas.SetTop((this.Squares[7, 7].Piece as Piece).MyImage, 595);
                        this.Squares[7, 4].Piece = this.Squares[7, 7].Piece;
                        (this.Squares[7, 4].Piece as Rook).Point = new Point(7, 4);
                        (this.Squares[7, 4].Piece as Rook).AlreadyMoved = true;

                        this.Squares[7, 7] = new Square(new Empty(PieceColor.None, new Point(7, 7), this));
                        Canvas.SetLeft((this.Squares[7, 7].Piece as Piece).MyImage, 595);
                        Canvas.SetTop((this.Squares[7, 7].Piece as Piece).MyImage, 595);
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add((this.Squares[7, 7].Piece as Piece).MyImage);

                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|moveCastle|3|0");
                    }
                }
                if (moveSmallCastle.Equals(string.Empty) == false && int.Parse(moveSmallCastle) == dY && dX == 7)
                {
                    if (moveSmallCastle.Equals("6") == true)
                    {

                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove((this.Squares[7, 5].Piece as Piece).MyImage);
                        Canvas.SetLeft((this.Squares[7, 7].Piece as Piece).MyImage, 425);
                        Canvas.SetTop((this.Squares[7, 7].Piece as Piece).MyImage, 595);
                        this.Squares[7, 5].Piece = this.Squares[7, 7].Piece;
                        (this.Squares[7, 5].Piece as Rook).Point = new Point(7, 5);
                        (this.Squares[7, 5].Piece as Rook).AlreadyMoved = true;

                        this.Squares[7, 7] = new Square(new Empty(PieceColor.None, new Point(7, 7), this));
                        Canvas.SetLeft((this.Squares[7, 7].Piece as Piece).MyImage, 595);
                        Canvas.SetTop((this.Squares[7, 7].Piece as Piece).MyImage, 595);
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add((this.Squares[7, 7].Piece as Piece).MyImage);

                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|moveCastle|2|0");
                    }
                    else
                    if (moveSmallCastle.Equals("1") == true)
                    {
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Remove((this.Squares[7, 2].Piece as Piece).MyImage);
                        Canvas.SetLeft((this.Squares[7, 0].Piece as Piece).MyImage, 170);
                        Canvas.SetTop((this.Squares[7, 0].Piece as Piece).MyImage, 595);
                        this.Squares[7, 2].Piece = this.Squares[7, 0].Piece;
                        (this.Squares[7, 2].Piece as Rook).Point = new Point(7, 2);
                        (this.Squares[7, 2].Piece as Rook).AlreadyMoved = true;

                        this.Squares[7, 0] = new Square(new Empty(PieceColor.None, new Point(7, 0), this));
                        Canvas.SetLeft((this.Squares[7, 0].Piece as Piece).MyImage, 0);
                        Canvas.SetTop((this.Squares[7, 0].Piece as Piece).MyImage, 595);
                        this.gameController.GameView.MainGameView.BoardMainGameView.CanvasBoard.Children.Add((this.Squares[7, 0].Piece as Piece).MyImage);

                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|moveCastle|5|7");
                    }
                }
                moveBigCastle = string.Empty;
                moveSmallCastle = string.Empty;
            }

            for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) { (this.Squares[i, j].Piece as Piece).CollisionMoves = new ObservableCollection<string>(); (this.Squares[i, j].Piece as Piece).SetChessGame(this); Panel.SetZIndex((this.Squares[i, j].Piece as Piece).MyImage, 2); }
            for (int i = 0; i < 8; i++) for (int j = 0; j < 8; j++) (this.Squares[i, j].Piece as Piece).possiblePositions();
            this.isYourTurn = false;

            this.statusGame();

        }


        public void statusGame()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (this.Squares[i, j].Piece is King && (this.Squares[i, j].Piece as King).PieceColor.ToString().Equals(this.EnemyColor.ToString()) && (this.Squares[i, j].Piece as King).checkmate() == true)
                    {
                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|youLose");
                        this.stopTheGame();
                        foreach (object window in Application.Current.Windows)
                            if (window is MyMessageBox)
                                (window as MyMessageBox).Close();

                        this.gameController.GameView.IsEnabled = false;
                        MyMessageBox myMessageBox = new MyMessageBox("You win! You can play again by pressing the Rematch button!");
                        myMessageBox.Left = this.gameController.GameView.Left + (this.gameController.GameView.Width - myMessageBox.Width) / 2;
                        myMessageBox.Top = this.gameController.GameView.Top + (this.gameController.GameView.Height - myMessageBox.Height) / 2;
                        myMessageBox.Closed += (sender, e) => { this.gameController.GameView.IsEnabled = true; };
                        myMessageBox.Show();
                    }
                    else
                    if (this.Squares[i, j].Piece is King && (this.Squares[i, j].Piece as King).PieceColor.ToString().Equals(this.MyColor.ToString()) && (this.Squares[i, j].Piece as King).checkmate() == true)
                    {
                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|youWin");
                        foreach (object window in Application.Current.Windows)
                            if (window is MyMessageBox)
                                (window as MyMessageBox).Close();

                        this.gameController.GameView.IsEnabled = false;
                        MyMessageBox myMessageBox = new MyMessageBox("You lose! You can play again by pressing the Rematch button!");
                        myMessageBox.Left = this.gameController.GameView.Left + (this.gameController.GameView.Width - myMessageBox.Width) / 2;
                        myMessageBox.Top = this.gameController.GameView.Top + (this.gameController.GameView.Height - myMessageBox.Height) / 2;
                        myMessageBox.Closed += (sender, e) => { this.gameController.GameView.IsEnabled = true; };
                        myMessageBox.Show();
                        this.stopTheGame();
                    }


            if (this.numberOfMovesWithoutCapture == 50)
            {
                this.stopTheGame();
                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.gameController.GameView.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("Draw! We had 50 moves without a catch! You can play again by pressing the Rematch button!");
                myMessageBox.Left = this.gameController.GameView.Left + (this.gameController.GameView.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.gameController.GameView.Top + (this.gameController.GameView.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.gameController.GameView.IsEnabled = true; };
                myMessageBox.Show();
                this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|draw|Draw! We had 50 moves without a catch! You can play again by pressing the Rematch button!");
            }
            else
            {
                int indexPiece = 0, kingWhite = 0, kingBlack = 0, knightWhite = 0, knightBlack = 0, bishopWhite = 0, bishopBlack = 0;
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if ((this.Squares[i, j].Piece is not Empty))
                        {
                            if (this.Squares[i, j].Piece is King && (this.Squares[i, j].Piece as Piece).PieceColor == PieceColor.White)
                                kingWhite++;
                            else
                            if (this.Squares[i, j].Piece is King && (this.Squares[i, j].Piece as Piece).PieceColor == PieceColor.Black)
                                kingBlack++;
                            else
                            if (this.Squares[i, j].Piece is Knight && (this.Squares[i, j].Piece as Piece).PieceColor == PieceColor.White)
                                knightWhite++;
                            else
                            if (this.Squares[i, j].Piece is Knight && (this.Squares[i, j].Piece as Piece).PieceColor == PieceColor.Black)
                                knightBlack++;
                            else
                            if (this.Squares[i, j].Piece is Bishop && (this.Squares[i, j].Piece as Piece).PieceColor == PieceColor.White)
                                bishopWhite++;
                            else
                            if (this.Squares[i, j].Piece is Bishop && (this.Squares[i, j].Piece as Piece).PieceColor == PieceColor.Black)
                                bishopBlack++;
                            indexPiece++;
                        }
                if (indexPiece == 3)
                {
                    bool validDraw = false;
                    if (kingWhite == 1 && kingBlack == 1 && knightWhite == 1)
                    {
                        validDraw = true;
                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|draw|Draw! Not enough pieces! You can play again by pressing the Rematch button!");
                    }
                    else
                    if (kingWhite == 1 && kingBlack == 1 && knightBlack == 1)
                    {
                        validDraw = true;
                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|draw|Draw! Not enough pieces! You can play again by pressing the Rematch button!");
                    }
                    else
                    if (kingWhite == 1 && kingBlack == 1 && bishopWhite == 1)
                    {
                        validDraw = true;
                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|draw|Draw! Not enough pieces! You can play again by pressing the Rematch button!");
                    }
                    else
                    if (kingWhite == 1 && kingBlack == 1 && bishopBlack == 1)
                    {
                        validDraw = true;
                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|draw|Draw! Not enough pieces! You can play again by pressing the Rematch button!");
                    }
                    if (validDraw == true)
                    {
                        this.stopTheGame();

                        foreach (object window in Application.Current.Windows)
                            if (window is MyMessageBox)
                                (window as MyMessageBox).Close();

                        this.gameController.GameView.IsEnabled = false;
                        MyMessageBox myMessageBox = new MyMessageBox("Draw! Not enough pieces! You can play again by pressing the Rematch button!");
                        myMessageBox.Left = this.gameController.GameView.Left + (this.gameController.GameView.Width - myMessageBox.Width) / 2;
                        myMessageBox.Top = this.gameController.GameView.Top + (this.gameController.GameView.Height - myMessageBox.Height) / 2;
                        myMessageBox.Closed += (sender, e) => { this.gameController.GameView.IsEnabled = true; };
                        myMessageBox.Show();
                    }
                }
                else
                if (indexPiece == 2)
                    if (kingWhite == 1 && kingBlack == 1)
                    {
                        this.stopTheGame();

                        this.gameController.Network.SendMessage(this.gameController.Network.You.IP + "|" + this.gameController.Network.Opponent.IP + "|gameController|draw|Draw! Not enough pieces! You can play again by pressing the Rematch button!");

                        foreach (object window in Application.Current.Windows)
                            if (window is MyMessageBox)
                                (window as MyMessageBox).Close();

                        this.gameController.GameView.IsEnabled = false;
                        MyMessageBox myMessageBox = new MyMessageBox("Draw! Not enough pieces! You can play again by pressing the Rematch button!");
                        myMessageBox.Left = this.gameController.GameView.Left + (this.gameController.GameView.Width - myMessageBox.Width) / 2;
                        myMessageBox.Top = this.gameController.GameView.Top + (this.gameController.GameView.Height - myMessageBox.Height) / 2;
                        myMessageBox.Closed += (sender, e) => { this.gameController.GameView.IsEnabled = true; };
                        myMessageBox.Show();
                    }
            }

        }


        public void setHistory(object piece1, object piece2)
        {
            string text = string.Empty;
            if (piece1 is Rook)
            {
                text += "R";
            }
            else
            if (piece1 is Queen)
            {
                text += "Q";

            }
            else
            if (piece1 is Pawn)
            {
                text += "";

            }
            else
            if (piece1 is Knight)
            {
                text += "N";

            }
            else
            if (piece1 is Bishop)
            {
                text += "B";
            }
            if (piece2 is not Empty)
                text += "x";

            if (Math.Abs((this.indexMirror == 7 ? 9 : 0) - ((int)(piece2 as Piece).Point.Y + 1)) == 1) text += "a";
            if (Math.Abs((this.indexMirror == 7 ? 9 : 0) - ((int)(piece2 as Piece).Point.Y + 1)) == 2) text += "b";
            if (Math.Abs((this.indexMirror == 7 ? 9 : 0) - ((int)(piece2 as Piece).Point.Y + 1)) == 3) text += "c";
            if (Math.Abs((this.indexMirror == 7 ? 9 : 0) - ((int)(piece2 as Piece).Point.Y + 1)) == 4) text += "d";
            if (Math.Abs((this.indexMirror == 7 ? 9 : 0) - ((int)(piece2 as Piece).Point.Y + 1)) == 5) text += "e";
            if (Math.Abs((this.indexMirror == 7 ? 9 : 0) - ((int)(piece2 as Piece).Point.Y + 1)) == 6) text += "f";
            if (Math.Abs((this.indexMirror == 7 ? 9 : 0) - ((int)(piece2 as Piece).Point.Y + 1)) == 7) text += "g";
            if (Math.Abs((this.indexMirror == 7 ? 9 : 0) - ((int)(piece2 as Piece).Point.Y + 1)) == 8) text += "h";



            text += Math.Abs((this.indexMirror == 7 ? 0 : 9) - ((int)(piece2 as Piece).Point.X + 1));

            this.gameController.GameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.ItemsSource = new ObservableCollection<History>();
            if ((piece1 as Piece).PieceColor == PieceColor.White)
                this.gameController.GameView.MainGameView.ChatHistoryMainGameView.History.Add(new History(this.gameController.GameView.MainGameView.ChatHistoryMainGameView.History.Count + 1, text, ""));
            else
            if ((piece1 as Piece).PieceColor == PieceColor.Black)
                this.gameController.GameView.MainGameView.ChatHistoryMainGameView.History[this.gameController.GameView.MainGameView.ChatHistoryMainGameView.History.Count - 1].SecondMove = text;
            this.gameController.GameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.ItemsSource = this.gameController.GameView.MainGameView.ChatHistoryMainGameView.History;

            object focusRow = null;
            focusRow = this.gameController.GameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.Items[this.gameController.GameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.Items.Count - 1];
            this.gameController.GameView.MainGameView.ChatHistoryMainGameView.DGHistoryTable.ScrollIntoView(focusRow);

        }

        public void setStackPanelCapturePiece(object piece, int indexStackPanel)
        {
            string pieceName = string.Empty;
            if (piece is Rook)
            {
                if (this.indexColor == 0)
                    pieceName = "RookB";
                else
                    if (this.indexColor == 1)
                    pieceName = "RookW";
            }
            else
            if (piece is Queen)
            {
                if (this.indexColor == 0)
                    pieceName = "QueenB";
                else
                    if (this.indexColor == 1)
                    pieceName = "QueenW";
            }
            else
            if (piece is Pawn)
            {
                if (this.indexColor == 0)
                    pieceName = "PawnB";
                else
                    if (this.indexColor == 1)
                    pieceName = "PawnW";
            }
            else
            if (piece is Knight)
            {
                if (this.indexColor == 0)
                    pieceName = "KnightB";
                else
                    if (this.indexColor == 1)
                    pieceName = "KnightW";
            }
            else
            if (piece is Bishop)
            {
                if (this.indexColor == 0)
                    pieceName = "BishopB";
                else
                    if (this.indexColor == 1)
                    pieceName = "BishopW";
            }

            if (piece is not Empty)
                if (indexStackPanel == 0)
                    this.gameController.GameView.MainGameView.addNumber(this.gameController.GameView.MainGameView.StPYouNumber, Directory.GetCurrentDirectory() + @$"\Images\{pieceName}.png", 1);
                else
                if (indexStackPanel == 1)
                    this.gameController.GameView.MainGameView.addNumber(this.gameController.GameView.MainGameView.StPOpponentNumber, Directory.GetCurrentDirectory() + @$"\Images\{pieceName}.png", 1);

        }


        public string bigCastleCheck()
        {
            if (this.MyColor == PieceColor.White)
            {
                bool canMakeMove = true;
                for (int i = 0; i < 5; i++)
                    foreach (string move in (this.Squares[7, i].Piece as Piece).CollisionMoves)
                        if (move.Split("|")[0].Split(",")[2].Equals("Black") == true)
                            canMakeMove = false;
                for (int i = 1; i < 4; i++)
                    if (this.Squares[7, i].Piece is not Empty)
                        canMakeMove = false;
                if (this.Squares[7, 0].Piece is Rook && (this.Squares[7, 0].Piece as Rook).AlreadyMoved == false && this.Squares[7, 4].Piece is King && (this.Squares[7, 4].Piece as King).AlreadyMoved == false && canMakeMove == true)
                    return "2";
            }
            else
            if (this.MyColor == PieceColor.Black)
            {
                bool canMakeMove = true;
                for (int i = 3; i < 8; i++)
                    foreach (string move in (this.Squares[7, i].Piece as Piece).CollisionMoves)
                        if (move.Split("|")[0].Split(",")[2].Equals("White") == true)
                            canMakeMove = false;
                for (int i = 4; i < 7; i++)
                    if (this.Squares[7, i].Piece is not Empty)
                        canMakeMove = false;
                if (this.Squares[7, 7].Piece is Rook && (this.Squares[7, 7].Piece as Rook).AlreadyMoved == false && this.Squares[7, 3].Piece is King && (this.Squares[7, 3].Piece as King).AlreadyMoved == false && canMakeMove == true)
                    return "5";
            }
            return string.Empty;
        }

        public string smallCastleCheck()
        {
            if (this.MyColor == PieceColor.White)
            {
                bool canMakeMove = true;
                for (int i = 4; i < 8; i++)
                    foreach (string move in (this.Squares[7, i].Piece as Piece).CollisionMoves)
                        if (move.Split("|")[0].Split(",")[2].Equals("Black") == true)
                            canMakeMove = false;
                for (int i = 5; i < 7; i++)
                    if (this.Squares[7, i].Piece is not Empty)
                        canMakeMove = false;
                if (this.Squares[7, 7].Piece is Rook && (this.Squares[7, 7].Piece as Rook).AlreadyMoved == false && this.Squares[7, 4].Piece is King && (this.Squares[7, 4].Piece as King).AlreadyMoved == false && canMakeMove == true)
                    return "6";
            }
            else
            if (this.MyColor == PieceColor.Black)
            {
                bool canMakeMove = true;
                for (int i = 0; i < 4; i++)
                    foreach (string move in (this.Squares[7, i].Piece as Piece).CollisionMoves)
                        if (move.Split("|")[0].Split(",")[2].Equals("White") == true)
                            canMakeMove = false;
                for (int i = 1; i < 3; i++)
                    if (this.Squares[7, i].Piece is not Empty)
                        canMakeMove = false;
                if (this.Squares[7, 0].Piece is Rook && (this.Squares[7, 0].Piece as Rook).AlreadyMoved == false && this.Squares[7, 3].Piece is King && (this.Squares[7, 3].Piece as King).AlreadyMoved == false && canMakeMove == true)
                    return "1";
            }
            return string.Empty;
        }







        public int NumberOfMovesWithoutCapture
        {
            get => this.numberOfMovesWithoutCapture; set => this.numberOfMovesWithoutCapture = value;
        }
        public string PathTransparent
        {
            get => this.pathTransparent; set => this.pathTransparent = value;
        }
        public string PathPossibleMove
        {
            get => this.pathPossibleMove; set => this.pathPossibleMove = value;
        }
        public string PathPossibleCapture
        {
            get => this.pathPossibleCapture; set => this.pathPossibleCapture = value;
        }
        public string PathLastMoveDark
        {
            get => this.pathLastMoveDark; set => this.pathLastMoveDark = value;
        }
        public string PathLastMoveLight
        {
            get => this.pathLastMoveLight; set => this.pathLastMoveLight = value;
        }
        public string PathBorderMove
        {
            get => this.pathBorderMove; set => this.pathBorderMove = value;
        }
        public string PathAttention
        {
            get => this.pathAttention; set => this.pathAttention = value;
        }
        public GameController GameController
        {
            get => this.gameController; set => this.gameController = value;
        }
        public bool IsYourTurn
        {
            get => this.isYourTurn; set => this.isYourTurn = value;
        }
        public int IndexColor
        {
            get => this.indexColor; set => this.indexColor = value;
        }
        public int IndexMirror
        {
            get => this.indexMirror; set => this.indexMirror = value;
        }
        public MyImage LastMoveLight1
        {
            get => this.lastMoveLight1; set => this.lastMoveLight1 = value;
        }
        public MyImage LastMoveLight2
        {
            get => this.lastMoveLight2; set => this.lastMoveLight2 = value;
        }
        public MyImage LastMoveDark1
        {
            get => this.lastMoveDark1; set => this.lastMoveDark1 = value;
        }
        public MyImage LastMoveDark2
        {
            get => this.lastMoveDark2; set => this.lastMoveDark2 = value;
        }
        public MyImage LastMoveLight3
        {
            get => this.lastMoveLight3; set => this.lastMoveLight3 = value;
        }
        public MyImage LastMoveLight4
        {
            get => this.lastMoveLight4; set => this.lastMoveLight4 = value;
        }
        public MyImage LastMoveDark3
        {
            get => this.lastMoveDark3; set => this.lastMoveDark3 = value;
        }
        public MyImage LastMoveDark4
        {
            get => this.lastMoveDark4; set => this.lastMoveDark4 = value;
        }

    }
}
