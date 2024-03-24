using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Diagnostics;

namespace Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS
{
    public class MyImage : Grid
    {
        private Image image;
        private string backgroundPath, imagePath;
        private PieceColor myColor;


        public MyImage(string backgroundPath, string imagePath, PieceColor myColor)
        {
            this.backgroundPath = backgroundPath;
            this.imagePath = imagePath;
            this.myColor = myColor;
            base.Width = 85;
            base.Height = 85;
            this.setBackgroundPath(backgroundPath);

            this.image = new Image()
            {
                Margin = new Thickness(3),
                Stretch = Stretch.Fill
            };
            this.setImagePath(imagePath, myColor);
            base.Children.Add(this.image);
        }


        public void setImagePath(string imagePath, PieceColor myColor)
        {
            this.imagePath = imagePath;
            this.myColor = myColor;
            if ((this.imagePath.Split(".")[this.imagePath.Split(".").Length - 2][this.imagePath.Split(".")[this.imagePath.Split(".").Length - 2].Length - 1] + "").Equals(((this.myColor == PieceColor.White) ? "W" : "B")) == true)
                this.Cursor = Cursors.Hand;
            else
                this.Cursor = Cursors.Arrow;
            this.image.Source = new BitmapImage(new Uri(imagePath));
        }

        public void setBackgroundPath(string backgroundPath)
        {
            this.backgroundPath = backgroundPath;
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri(backgroundPath)));
            imageBrush.Stretch = Stretch.Fill;
            base.Background = imageBrush;
        }


        public string BackgroundPath
        {
            get => this.backgroundPath; set => this.backgroundPath = value;
        }
        public string ImagePath
        {
            get => this.imagePath; set => this.imagePath = value;
        }
        public Image Image
        {
            get => this.image; set => this.image = value;
        }
    }
}
