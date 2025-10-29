using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace IM_AGES
{
    internal class Alperen
    {
        // Gri tonlamaya dönüştürme
        public Bitmap ConvertToGrayscale(Bitmap original)
        {
            // Orijinal resimle aynı boyutlarda yeni bir boş resim oluştur
            Bitmap grayImage = new Bitmap(original.Width, original.Height);

            // Resimdeki her pikseli dolaş
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    // Mevcut pikselin rengini al
                    Color originalColor = original.GetPixel(x, y);

                    // RGB değerlerinin ağırlıklı toplamını kullanarak gri tonlama değeri hesapla
                    int grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));

                    // R, G ve B'nin hepsi gri tonlama değerine ayarlanmış yeni bir renk oluştur
                    Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);

                    // Yeni resimdeki pikseli bu gri tonlama rengi ile ayarla
                    grayImage.SetPixel(x, y, grayColor);
                }
            }

            // Yeni gri tonlamalı resmi döndür
            return grayImage;
        }

        // Ortalama filtresi uygula
        public Bitmap ApplyMeanFilter(Bitmap original)
        {
            // Orijinal resimle aynı boyutlarda yeni bir boş resim oluştur
            Bitmap meanImage = new Bitmap(original.Width, original.Height);

            // Ortalama filtre çekirdeğini tanımla
            int[,] filter = new int[3, 3]
            {
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 }
            };

            // Filtrenin boyutlarını tanımla
            int filterWidth = 3;
            int filterHeight = 3;
            int filterOffset = 1; // Filtrenin görüntü sınırları içinde kalması için ofset

            // Resimdeki her pikseli dolaş, kenarları atla çünkü filtre taşacaktır
            for (int y = filterOffset; y < original.Height - filterOffset; y++)
            {
                for (int x = filterOffset; x < original.Width - filterOffset; x++)
                {
                    int red = 0, green = 0, blue = 0;

                    // Çevredeki piksellere filtre uygula
                    for (int filterY = 0; filterY < filterHeight; filterY++)
                    {
                        for (int filterX = 0; filterX < filterWidth; filterX++)
                        {
                            // Komşu pikselin rengini al
                            Color imageColor = original.GetPixel(x + filterX - filterOffset, y + filterY - filterOffset);

                            // Renk değerlerini ağırlıklı olarak topla
                            red += imageColor.R * filter[filterX, filterY];
                            green += imageColor.G * filter[filterX, filterY];
                            blue += imageColor.B * filter[filterX, filterY];
                        }
                    }

                    // Toplanan değerleri ortalama
                    red /= 9;
                    green /= 9;
                    blue /= 9;

                    // Yeni resimdeki pikseli ortalanmış renk ile ayarla
                    meanImage.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }

            // Ortalama filtre uygulanmış yeni resmi döndür
            return meanImage;
        }

        // Median filtresi uygula
        public Bitmap ApplyMedianFilter(Bitmap original)
        {
            // Orijinal resimle aynı boyutlarda yeni bir boş resim oluştur
            Bitmap medianImage = new Bitmap(original.Width, original.Height);

            // Filtrenin boyutlarını tanımla
            int filterWidth = 3;
            int filterHeight = 3;
            int filterOffset = 1; // Filtrenin görüntü sınırları içinde kalması için ofset

            // Resimdeki her pikseli dolaş, kenarları atla çünkü filtre taşacaktır
            for (int y = filterOffset; y < original.Height - filterOffset; y++)
            {
                for (int x = filterOffset; x < original.Width - filterOffset; x++)
                {
                    int[] red = new int[filterWidth * filterHeight];
                    int[] green = new int[filterWidth * filterHeight];
                    int[] blue = new int[filterWidth * filterHeight];
                    int k = 0;

                    // Komşu piksellerin renklerini topla
                    for (int filterY = 0; filterY < filterHeight; filterY++)
                    {
                        for (int filterX = 0; filterX < filterWidth; filterX++)
                        {
                            // Komşu pikselin rengini al
                            Color imageColor = original.GetPixel(x + filterX - filterOffset, y + filterY - filterOffset);

                            // Renk değerlerini dizilere kaydet
                            red[k] = imageColor.R;
                            green[k] = imageColor.G;
                            blue[k] = imageColor.B;
                            k++;
                        }
                    }

                    // Dizileri sıralayarak medyan değeri bul
                    Array.Sort(red);
                    Array.Sort(green);
                    Array.Sort(blue);

                    // Yeni resimdeki pikseli medyan renk ile ayarla
                    medianImage.SetPixel(x, y, Color.FromArgb(red[4], green[4], blue[4])); // 3x3 çekirdek için medyan 4. elemandır
                }
            }

            // Median filtre uygulanmış yeni resmi döndür
            return medianImage;
        }

        // Tuz ve biber gürültüsü ekle
        public Bitmap ApplySaltAndPepperNoise(Bitmap original, double noiseLevel)
        {
            // Orijinal resimle aynı boyutlarda yeni bir boş resim oluştur
            Bitmap noisyImage = new Bitmap(original.Width, original.Height);
            Random rand = new Random();

            // Resimdeki her pikseli dolaş
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    // Mevcut piksele gürültü eklemeye karar ver
                    if (rand.NextDouble() < noiseLevel)
                    {
                        // Rastgele olarak tuz (beyaz) veya biber (siyah) gürültüsü seç
                        bool salt = rand.Next(0, 2) == 0;
                        noisyImage.SetPixel(x, y, salt ? Color.White : Color.Black);
                    }
                    else
                    {
                        // Orijinal piksel rengini kopyala
                        noisyImage.SetPixel(x, y, original.GetPixel(x, y));
                    }
                }
            }

            // Gürültü eklenmiş yeni resmi döndür
            return noisyImage;
        }

        // Parlaklık ayarlama
        public Bitmap AdjustBrightness(Bitmap image, int brightness)
        {
            // Orijinal resimle aynı boyutlarda yeni bir boş resim oluştur
            Bitmap adjustedImage = new Bitmap(image.Width, image.Height);

            // Tüm resmi kaplayan bir dikdörtgen tanımla
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            // Orijinal ve ayarlanmış resimlerin bitlerini kilitle
            BitmapData imageData = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData adjustedData = adjustedImage.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            // Piksel başına bayt sayısını ve toplam bayt sayısını hesapla
            int bytesPerPixel = Image.GetPixelFormatSize(image.PixelFormat) / 8;
            int byteCount = imageData.Stride * image.Height;

            // Piksel verilerini tutmak için diziler oluştur
            byte[] pixels = new byte[byteCount];
            byte[] adjustedPixels = new byte[byteCount];

            // İlk piksele işaretçiyi al
            IntPtr ptrFirstPixel = imageData.Scan0;
            IntPtr ptrFirstAdjustedPixel = adjustedData.Scan0;

            // Piksel verilerini resimden diziye kopyala
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

            // Resimdeki her pikseli dolaş
            for (int y = 0; y < imageData.Height; y++)
            {
                int currentLine = y * imageData.Stride;

                // Pikseldeki her renk bileşenini dolaş
                for (int x = 0; x < imageData.Width * bytesPerPixel; x += bytesPerPixel)
                {
                    // Orijinal renk değerlerini al
                    int oldBlue = pixels[currentLine + x];
                    int oldGreen = pixels[currentLine + x + 1];
                    int oldRed = pixels[currentLine + x + 2];

                    // Her renk bileşeninin parlaklığını ayarla
                    adjustedPixels[currentLine + x] = Clamp(oldBlue + brightness);
                    adjustedPixels[currentLine + x + 1] = Clamp(oldGreen + brightness);
                    adjustedPixels[currentLine + x + 2] = Clamp(oldRed + brightness);

                    // Sınır aşımı olmadığından emin ol
                    if (currentLine + x >= pixels.Length || currentLine + x + 1 >= pixels.Length || currentLine + x + 2 >= pixels.Length)
                    {
                        throw new IndexOutOfRangeException("Piksel verileri işlenirken sınır aşıldı.");
                    }
                }
            }

            // Ayarlanmış piksel verilerini ayarlanmış resme kopyala
            Marshal.Copy(adjustedPixels, 0, ptrFirstAdjustedPixel, adjustedPixels.Length);

            // Orijinal ve ayarlanmış resimlerin bitlerini kilidini aç
            image.UnlockBits(imageData);
            adjustedImage.UnlockBits(adjustedData);

            // Ayarlanmış parlaklık ile yeni resmi döndür
            return adjustedImage;
        }

        // Değer sınırlandırma işlevi
        private byte Clamp(int value)
        {
            // Değerin 0 ile 255 arasında olduğundan emin ol
            return (byte)Math.Max(0, Math.Min(255, value));
        }
    }
}
