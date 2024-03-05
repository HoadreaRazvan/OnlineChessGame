using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;

namespace Chess_Client.VIEW
{
    public class MyMessageBox : Window
    {
        private Border brdHeader;
        private Border brdMain;
        private Label lblInformationMessage;

        private DispatcherTimer timer;

        public MyMessageBox(string message)
        {
            InitializeComponent();
            this.Content = GridWindowLogin();

            this.lblInformationMessage.Content = message;
            AdjustWindowSize();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();

            this.Topmost = true;
            this.Show();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void AdjustWindowSize()
        {
            FormattedText formattedText = new FormattedText(
                lblInformationMessage.Content.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(lblInformationMessage.FontFamily, lblInformationMessage.FontStyle, lblInformationMessage.FontWeight, lblInformationMessage.FontStretch),
                lblInformationMessage.FontSize,
                lblInformationMessage.Foreground
                );

            double textWidth = formattedText.Width + 10;
            double windowWidth = 125 + textWidth;

            if (textWidth > 120)
            {
                this.Width = windowWidth;
                this.lblInformationMessage.Width = textWidth;
                brdMain.Width = windowWidth;
                brdHeader.Width = windowWidth;
            }

        }

        public void InitializeComponent()
        {
            this.Height = 150;
            this.Width = 250;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowStyle = WindowStyle.None;
            this.Background = Brushes.Transparent;
            this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.NoResize;
            string path = Directory.GetCurrentDirectory() + @"\Images\InformationIcon.png";
            ImageSource imageSource = new BitmapImage(new Uri(path));
            this.Icon = imageSource;
        }

        public Grid GridWindowLogin()
        {

            Grid windowGrid = new Grid();

            brdHeader = new Border
            {
                Name = "BrdHeader",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 250,
                Height = 45,
                Margin = new Thickness(0, 0, 0, 0),
                CornerRadius = new CornerRadius(30, 30, 0, 0),
                Background = Brushes.WhiteSmoke,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1, 1, 1, 0)
            };
            brdHeader.MouseDown += Border_MouseDown;

            Grid grdHeader = new Grid
            {
                Name = "GrdHeader"
            };

            Label LblInformation = new Label
            {
                Name = "LblInformation",
                Content = "Information!",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 130,
                Height = 35,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20, 10, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 20
            };

            Button btnExit = new Button
            {
                Name = "BtnExit",
                Content = "X",
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(0, 10, 20, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16
            };
            btnExit.Click += BtnExit_Click;

            Button btnMinimize = new Button
            {
                Name = "BtnMinimize",
                Content = "_",
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 30,
                Margin = new Thickness(0, 10, 60, 0),
                FontFamily = new FontFamily("Arial"),
                FontSize = 16
            };
            btnMinimize.Click += BtnMinimize_Click;

            grdHeader.Children.Add(LblInformation);
            grdHeader.Children.Add(btnExit);
            grdHeader.Children.Add(btnMinimize);

            brdHeader.Child = grdHeader;

            brdMain = new Border
            {
                Name = "BrdMain",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 250,
                Height = 105,
                Margin = new Thickness(0, 45, 0, 0),
                CornerRadius = new CornerRadius(0, 0, 30, 30),
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1, 1, 1, 1)
            };

            Grid grdMain = new Grid
            {
                Name = "GrdMain"
            };

            string path = Directory.GetCurrentDirectory() + @"\Images\InformationImage.png";
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(path)),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 45,
                Height = 45,
                Margin = new Thickness(30, 10, 0, 0)
            };

            lblInformationMessage = new Label
            {
                Name = "LblInformationMessage",
                Content = "",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 125,
                Height = 65,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(105, 0, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 16
            };

            Button btnOK = new Button
            {
                Name = "btnOK",
                Content = "OK",
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 50,
                Height = 30,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 65, 20, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };
            btnOK.Click += BtnOK_Click;

            grdMain.Children.Add(image);
            grdMain.Children.Add(lblInformationMessage);
            grdMain.Children.Add(btnOK);

            brdMain.Child = grdMain;

            windowGrid.Children.Add(brdHeader);
            windowGrid.Children.Add(brdMain);
            return windowGrid;
        }


        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public Border BrdHeader
        {
            get => this.brdHeader; set => this.brdHeader = value;
        }
        public Border BrdMain
        {
            get => this.brdMain; set => this.brdMain = value;
        }
        public Label LblInformationMessage
        {
            get => this.lblInformationMessage; set => this.lblInformationMessage = value;
        }

    }
}
