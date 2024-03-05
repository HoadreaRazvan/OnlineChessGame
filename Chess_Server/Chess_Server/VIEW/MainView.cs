using Chess_Server.MODEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess_Server.VIEW
{
    public class MainView : Border
    {
        private ObservableCollection<Client> clients;
        private ObservableCollection<LogEntry> logEntries;

        private View view;

        private DataGrid DGConnectedClients;
        private DataGrid DGLogActivity;

        public MainView(View view)
        {
            this.clients = new ObservableCollection<Client>();
            this.logEntries = new ObservableCollection<LogEntry>();
            this.view = view;
            InitializeComponent();
            this.Child = layout();
        }

        public void InitializeComponent()
        {
            this.Name = "BrdMain";
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = 750;
            this.Height = 765;
            this.Margin = new Thickness(0, 55, 0, 0);
            this.CornerRadius = new CornerRadius(0, 0, 30, 30);
            this.Background = Brushes.White;
            this.BorderBrush = Brushes.Black;
            this.BorderThickness = new Thickness(1, 1, 1, 1);
        }

        public Grid layout()
        {
            Grid grdMain = new Grid
            {
                Name = "GrdMain"
            };

            Label LblClients = new Label
            {
                Name = "LblClients",
                Content = "Connected Clients",
                VerticalAlignment = VerticalAlignment.Top,
                Height = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0),
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 20
            };

            DGConnectedClients = new DataGrid
            {
                Name = "DGConnectedClients",
                VerticalAlignment = VerticalAlignment.Top,
                Height = 300,
                Margin = new Thickness(10, 50, 10, 0),
                AutoGenerateColumns = false,
                IsReadOnly = true,
                CanUserSortColumns = false,
                CanUserReorderColumns = false,
                CanUserResizeColumns = false,
                SelectionMode = DataGridSelectionMode.Single,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                FontFamily = new FontFamily("Arial"),
                FontSize = 15,
                Cursor = Cursors.Hand
            };

            DGConnectedClients.Columns.Add(new DataGridTextColumn { Header = "#", Binding = new Binding("Index"), Width = 25 });
            DGConnectedClients.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            DGConnectedClients.Columns.Add(new DataGridTextColumn { Header = "IP", Binding = new Binding("IP"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            DGConnectedClients.Columns.Add(new DataGridTextColumn { Header = "In Game", Binding = new Binding("InGame"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            DGConnectedClients.Columns.Add(new DataGridTextColumn { Header = "Connected Date", Binding = new Binding("Date"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            this.DGConnectedClients.ItemsSource = this.clients;
            DGConnectedClients.SelectionChanged += DGConnectedClients_SelectionChanged;

            Label lblInfo = new Label
            {
                Name = "LblInfo",
                Content = "Click on the desired line to be able to disconnect the client.",
                FontStyle = FontStyles.Italic,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 30,
                Margin = new Thickness(0, 350, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };

            Label lblLog = new Label
            {
                Name = "LblLog",
                Content = "LOG Activity",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 50,
                Margin = new Thickness(0, 380, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 20
            };

            this.DGLogActivity = new DataGrid
            {
                Name = "DGLogActivity",
                VerticalAlignment = VerticalAlignment.Top,
                Height = 300,
                Margin = new Thickness(10, 430, 10, 0),
                AutoGenerateColumns = false,
                IsReadOnly = true,
                CanUserSortColumns = false,
                CanUserReorderColumns = false,
                CanUserResizeColumns = false,
                SelectionMode = DataGridSelectionMode.Single,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                FontFamily = new FontFamily("Arial"),
                FontSize = 15,
                Cursor = Cursors.Hand
            };

            this.DGLogActivity.Columns.Add(new DataGridTextColumn { Header = "#", Binding = new Binding("Index"), Width = 25 });
            DGLogActivity.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            DGLogActivity.Columns.Add(new DataGridTextColumn { Header = "Message", Binding = new Binding("Message"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) });
            DGLogActivity.Columns.Add(new DataGridTextColumn { Header = "Date", Binding = new Binding("Date"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            DGLogActivity.ItemsSource = this.logEntries;
            DGLogActivity.SelectionChanged += DGLogActivity_SelectionChanged;

            Label lblSignature = new Label
            {
                Name = "LblSignature",
                Content = "Hoadrea Razvan S&E",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 35,
                Margin = new Thickness(0, 730, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Arial"),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };

            grdMain.Children.Add(LblClients);
            grdMain.Children.Add(DGConnectedClients);
            grdMain.Children.Add(lblInfo);
            grdMain.Children.Add(lblLog);
            grdMain.Children.Add(DGLogActivity);
            grdMain.Children.Add(lblSignature);

            return grdMain;
        }

        private void DGConnectedClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGConnectedClients.SelectedItem != null)
            {
                string content = ((Client)DGConnectedClients.SelectedItem).IP;
                this.view.Controller.Network.disconnectIPClient(content);

                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.view.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox("The selected client has been disconnected!");
                myMessageBox.Left = this.view.Left + (this.view.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.view.Top + (this.view.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.view.IsEnabled = true; };
                myMessageBox.Show();

                DGConnectedClients.SelectedIndex = -1;
            }
        }

        private void DGLogActivity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGLogActivity.SelectedItem != null)
            {
                string content = ((LogEntry)DGLogActivity.SelectedItem).Message;

                foreach (object window in Application.Current.Windows)
                    if (window is MyMessageBox)
                        (window as MyMessageBox).Close();

                this.view.IsEnabled = false;
                MyMessageBox myMessageBox = new MyMessageBox(content);
                myMessageBox.Left = this.view.Left + (this.view.Width - myMessageBox.Width) / 2;
                myMessageBox.Top = this.view.Top + (this.view.Height - myMessageBox.Height) / 2;
                myMessageBox.Closed += (sender, e) => { this.view.IsEnabled = true; };
                myMessageBox.Show();

                DGLogActivity.SelectedIndex = -1;
            }
        }

        public DataGrid DGConnectedClientsTable
        {
            get => this.DGConnectedClients; set => this.DGConnectedClients = value;
        }
        public DataGrid DGLogActivityTable
        {
            get => this.DGLogActivity; set => this.DGLogActivity = value;
        }
        public ObservableCollection<Client> Clients
        {
            get => this.clients; set => this.clients = value;
        }
        public ObservableCollection<LogEntry> LogEntries
        {
            get => this.logEntries; set => this.logEntries = value;
        }
        public View View
        {
            get => this.view; set => this.view = value;
        }
    }
}
