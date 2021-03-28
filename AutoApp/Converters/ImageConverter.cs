using AutoApp.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using AutoApp.Contexts;
using GalaSoft.MvvmLight.Ioc;

namespace AutoApp.Converters
{
    public class ImageConverter 
    {
        public string PathImageToString(string path)
        {
            byte[] imageArray = File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }

        public BitmapImage StringToImage(string value)
        {
            var img = System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String((string)value)));
            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public string ImageToString(BitmapImage value)
        {
            MemoryStream ms = new MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(value));
            encoder.Save(ms);
            byte[] bitmapdata = ms.ToArray();
            return Convert.ToBase64String(bitmapdata);
        }

    }
}
