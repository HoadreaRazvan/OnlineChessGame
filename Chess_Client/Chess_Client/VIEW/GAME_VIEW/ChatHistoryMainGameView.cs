using Chess_Client.CONTROLLER.GAME_CONTROLLER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Chess_Client.VIEW.GAME_VIEW.MainGameView;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Chess_Client.MODEL.GAME_MODEL;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Chess_Client.VIEW.GAME_VIEW
{
    public class ChatHistoryMainGameView : Grid
    {
        private GameView gameView;

        private ObservableCollection<Message> messages;
        private ObservableCollection<History> history;
        private TextBox txbChat;
        private ListBox LBChat;
        private DataGrid DGHistory;
        private Button btnSend;

        public ChatHistoryMainGameView(GameView gameView)
        {
            this.messages = new ObservableCollection<Message>();
            this.history = new ObservableCollection<History>();
            this.gameView = gameView;
            InitializeComponent();
            this.layout();
        }

        public void InitializeComponent()
        {
            this.Name = "GrdHistoryChat";
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = 350;
            this.Height = 680;
            this.Margin = new Thickness(720, 100, 0, 0);
        }

        public void layout()
        {
            Grid GrdHistory = new Grid
            {
                Name = "GrdHistory",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 350,
                Height = 255,
                Margin = new Thickness(0, 0, 0, 0)
            };

            Label LblHistory = new Label
            {
                Name = "LblHistory",
                Content = "History",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 350,
                Height = 30,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 18
            };

            DGHistory = new DataGrid
            {
                Name = "DGHistory",
                Cursor = Cursors.Hand,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 200,
                Margin = new Thickness(0, 40, 0, 0),
                AutoGenerateColumns = false,
                IsReadOnly = true,
                CanUserSortColumns = false,
                CanUserReorderColumns = false,
                CanUserResizeColumns = false,
                SelectionMode = DataGridSelectionMode.Single,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                FontFamily = new FontFamily("Arial"),
                FontSize = 15
            };

            DGHistory.Columns.Add(new DataGridTextColumn { Header = "#", Binding = new Binding("Index"), Width = 25 });


            GrdHistory.Children.Add(LblHistory);
            GrdHistory.Children.Add(DGHistory);
            this.Children.Add(GrdHistory);

            Grid grdChat = new Grid
            {
                Name = "GrdChat",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 350,
                Height = 425,
                Margin = new Thickness(0, 255, 0, 0)
            };



            txbChat = new TextBox
            {
                Name = "TxbChat",
                MaxLength = 30,
                IsReadOnly = false,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 260,
                Height = 30,
                Margin = new Thickness(0, 380, 0, 0),
                TextWrapping = TextWrapping.NoWrap,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };
            txbChat.TextChanged += TxbChat_TextChanged;

            btnSend = new Button
            {
                Name = "BtnSend",
                Content = "Send",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 80,
                Height = 30,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(270, 380, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14,
                Cursor = Cursors.Hand
            };
            btnSend.Click += BtnSend_Click;

            grdChat.Children.Add(txbChat);
            grdChat.Children.Add(btnSend);

            LBChat = new ListBox
            {
                Name = "LBChat",
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 350,
                Height = 365,
                Margin = new Thickness(0),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Background = Brushes.White
            };

            LBChat.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Visible);

            VirtualizingStackPanel virtualizingStackPanel = new VirtualizingStackPanel();
            virtualizingStackPanel.VerticalAlignment = VerticalAlignment.Bottom;

            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
            factory.SetValue(VirtualizingStackPanel.VerticalAlignmentProperty, VerticalAlignment.Bottom);

            ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(factory);
            LBChat.ItemsPanel = itemsPanelTemplate;
            itemsPanelTemplate.Seal();
            LBChat.ItemsPanel = itemsPanelTemplate;

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.MarginProperty, new Thickness(3));
            borderFactory.SetValue(Border.PaddingProperty, new Thickness(5));
            borderFactory.SetValue(Border.BorderBrushProperty, Brushes.Black);
            borderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
            borderFactory.SetValue(Border.BackgroundProperty, Brushes.WhiteSmoke);

            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            FrameworkElementFactory senderStackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            senderStackPanelFactory.SetValue(StackPanel.HorizontalAlignmentProperty, new Binding("Alignment"));

            FrameworkElementFactory senderLabelFactory = new FrameworkElementFactory(typeof(Label));
            senderLabelFactory.SetValue(Label.ContentProperty, new Binding("Sender"));
            senderLabelFactory.SetValue(Label.FontFamilyProperty, new FontFamily("Arial"));
            senderLabelFactory.SetValue(Label.FontWeightProperty, FontWeights.SemiBold);
            senderLabelFactory.SetValue(Label.FontSizeProperty, 14.0);
            senderStackPanelFactory.AppendChild(senderLabelFactory);

            stackPanelFactory.AppendChild(senderStackPanelFactory);
            FrameworkElementFactory messageStackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            messageStackPanelFactory.SetValue(StackPanel.HorizontalAlignmentProperty, new Binding("Alignment"));

            FrameworkElementFactory messageLabelFactory = new FrameworkElementFactory(typeof(Label));
            messageLabelFactory.SetValue(Label.ContentProperty, new Binding("Text"));
            messageLabelFactory.SetValue(Label.FontFamilyProperty, new FontFamily("Arial"));
            messageLabelFactory.SetValue(Label.FontSizeProperty, 12.0);
            messageStackPanelFactory.AppendChild(messageLabelFactory);

            stackPanelFactory.AppendChild(messageStackPanelFactory);

            borderFactory.AppendChild(stackPanelFactory);

            DataTemplate dataTemplate = new DataTemplate();
            dataTemplate.VisualTree = borderFactory;

            LBChat.ItemTemplate = dataTemplate;
            LBChat.ItemsSource = Messages;

            grdChat.Children.Add(LBChat);
            this.Children.Add(grdChat);

        }





        private void TxbChat_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputText = this.txbChat.Text;
            string pattern = "^[a-zA-Z0-9.!?, =-]*$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(inputText))
            {
                this.txbChat.Text = Regex.Replace(inputText, "[^a-zA-Z0-9.!?, =-]", "");
                this.txbChat.CaretIndex = this.txbChat.Text.Length;
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            string message = txbChat.Text, client = this.gameView.GameController.Network.You.Name;
            if (message.Equals("") == false)
            {
                sendMessage(client, message,0);
                this.gameView.GameController.Network.SendMessage(this.gameView.GameController.Network.You.IP + "|" + this.gameView.GameController.Network.Opponent.IP + "|gameController|sendMessage|" + message);
            }
        }

        public void sendMessage(string client, string message, int indexMessage)
        {

            HorizontalAlignment alignment;
            if (client.Equals(this.gameView.GameController.Network.You.Name) == true)
                alignment = HorizontalAlignment.Right;
            else
                alignment = HorizontalAlignment.Left;
            messages.Add(new Message(client, message, alignment));
            LBChat.ScrollIntoView(messages[messages.Count - 1]);
            if (indexMessage == 0)
                txbChat.Text = "";

        }

        public TextBox TxbChat
        {
            get => this.txbChat; set => this.txbChat = value;
        }
        public Button BtnSend
        {
            get => this.btnSend; set => this.btnSend = value;
        }
        public DataGrid DGHistoryTable
        {
            get => this.DGHistory; set => this.DGHistory = value;
        }
        public ObservableCollection<Message> Messages
        {
            get => this.messages; set => this.messages = value;
        }
        public ObservableCollection<History> History
        {
            get => this.history; set => this.history = value;
        }
        public GameView GameView
        {
            get => this.gameView; set => this.gameView = value;
        }
    }
}
