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

namespace Chess_Client.MODEL.GAME_MODEL.GAME_MODEL_LOGISTICS
{
    public class MyImage : Grid
    {
        private Image image;
        private string backgroundPath, imagePath;


        public MyImage(string backgroundPath, string imagePath)
        {
            this.backgroundPath = backgroundPath;
            this.imagePath = imagePath;
            base.Width = 85;
            base.Height = 85;
            this.setBackgroundPath(backgroundPath);

            this.image = new Image()
            {
                Margin = new Thickness(3),
                Stretch = Stretch.Fill
            };
            this.setImagePath(imagePath);
            base.Children.Add(this.image);
        }


        public void setImagePath(string imagePath)
        {
            this.imagePath = imagePath;
            try
            {
                this.image.Source = new BitmapImage(new Uri(imagePath));
            }catch (Exception ex) { }
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
