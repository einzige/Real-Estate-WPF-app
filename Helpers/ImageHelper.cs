using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;
using System.Windows.Data;

namespace EstateHelpers.Imaging
{
    public static class ImageHelper
    {
        public static byte[] GetImageData(BitmapImage bi)
        {
            bi.StreamSource.Seek(0, SeekOrigin.Begin);

            byte[] bytes = new byte[bi.StreamSource.Length];
            int res = bi.StreamSource.Read(bytes, 0, (int)bi.StreamSource.Length);

            return bytes;
        }

        public static BitmapImage GetBitmapImage(byte[] imageBytes, int w, int h)
        {
            return (BitmapImage)CreateImage(imageBytes, w, h);
        }

        public static ImageSource GetResizedImageSource(string filePath, int width, int height)
        {
            byte[] imageBytes = LoadImageData(@filePath);
            return CreateImage(imageBytes, width, height);
        }

        private static byte[] LoadImageData(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] imageBytes = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return imageBytes;
        }

        private static void SaveImageData(byte[] imageData, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(imageData);

            bw.Close();
            fs.Close();
        }

        private static ImageSource CreateImage(byte[] imageData, int decodePixelWidth, int decodePixelHeight)
        {
            if (imageData == null) return null;
            BitmapImage result = new BitmapImage();

            result.BeginInit();

            if (decodePixelWidth > 0)
            {
                result.DecodePixelWidth = decodePixelWidth;
            }

            if (decodePixelHeight > 0)
            {
                result.DecodePixelHeight = decodePixelHeight;
            }

            result.StreamSource = new MemoryStream(imageData);
            result.CreateOptions = BitmapCreateOptions.None;
            result.CacheOption = BitmapCacheOption.Default;

            result.EndInit();

            return result;
        }

        internal static byte[] GetEncodedImageData(ImageSource image, string preferredFormat)
        {
            byte[] result = null;
            BitmapEncoder encoder = null;

            switch (preferredFormat.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    encoder = new JpegBitmapEncoder();
                    break;

                case ".bmp":
                    encoder = new BmpBitmapEncoder();
                    break;

                case ".png":

                    encoder = new PngBitmapEncoder();
                    break;

                case ".tif":
                case ".tiff":
                    encoder = new TiffBitmapEncoder();
                    break;

                case ".gif":
                    encoder = new GifBitmapEncoder();
                    break;

                case ".wmp":
                    encoder = new WmpBitmapEncoder();
                    break;
            }

            if (image is BitmapSource)
            {
                MemoryStream stream = new MemoryStream();

                encoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
                encoder.Save(stream);

                stream.Seek(0, SeekOrigin.Begin);
                result = new byte[stream.Length];

                BinaryReader br = new BinaryReader(stream);
                br.Read(result, 0, (int)stream.Length);

                br.Close();
                stream.Close();
            }

            return result;
        }
    }
}
