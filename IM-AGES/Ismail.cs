using IM_AGES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM_AGES
{
    internal class Ismail
    {
        public static Bitmap ConvertToBinary(Bitmap originalImage)
        {
            // Yeni bir bitmap oluştur
            Bitmap binaryImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Her pikseli dolaşarak binary dönüşümü yap
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    // Pikselin renk değerini al
                    Color originalColor = originalImage.GetPixel(x, y);

                    // Renk yoğunluğunu hesapla (R+G+B)/3 formülü ile
                    int intensity = (originalColor.R + originalColor.G + originalColor.B) / 3;

                    // Eşik değerine göre pikseli siyah veya beyaz yap
                    Color binaryColor = (intensity < 128) ? Color.Black : Color.White;

                    // Yeni pikseli ata
                    binaryImage.SetPixel(x, y, binaryColor);
                }
            }
            return binaryImage;
        }
        public static Bitmap ConvertToHSV(Bitmap originalImage)
        {
            Bitmap hsvImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color originalColor = originalImage.GetPixel(x, y);

                    // RGB değerlerini 0-1 arasına ölçekle
                    float r = originalColor.R / 255.0f;
                    float g = originalColor.G / 255.0f;
                    float b = originalColor.B / 255.0f;

                    float max = Math.Max(r, Math.Max(g, b));
                    float min = Math.Min(r, Math.Min(g, b));
                    float delta = max - min;

                    float h = 0;
                    float s = (max == 0) ? 0 : delta / max;
                    float v = max;

                    if (delta != 0)
                    {
                        if (max == r)
                            h = 60 * (((g - b) / delta) % 6);
                        else if (max == g)
                            h = 60 * ((b - r) / delta + 2);
                        else
                            h = 60 * ((r - g) / delta + 4);
                    }

                    // Hue değeri negatif olamaz
                    h = (h < 0) ? h + 360 : h;

                    // HSV değerlerini 0-255 arasına dönüştür
                    int hue = (int)(h / 360 * 255);
                    int saturation = (int)(s * 255);
                    int value = (int)(v * 255);

                    // Yeni pikseli ata
                    Color hsvColor = Color.FromArgb(hue, saturation, value);
                    hsvImage.SetPixel(x, y, hsvColor);
                }
            }
            return hsvImage;
        }
        public static Bitmap ApplyGaussianBlur(Bitmap originalImage)
        {
            Bitmap blurredImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Gaussian çekirdeği (kernel) tanımla
            double[,] kernel = {
                                { 1, 2, 1 },
                                { 2, 4, 2 },
                                { 1, 2, 1 }
    };

            // Çekirdek boyutunu ve orta noktayı bul
            int kernelSize = 3;
            int kernelMid = kernelSize / 2;

            // Her piksel için konvolüsyon uygula
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    double rSum = 0, gSum = 0, bSum = 0, kSum = 0;

                    // Konvolüsyon yapılacak pikselin etrafında dön
                    for (int i = 0; i < kernelSize; i++)
                    {
                        for (int j = 0; j < kernelSize; j++)
                        {
                            // Resmin kenarlarını kontrol et
                            int posX = Math.Min(originalImage.Width - 1, Math.Max(0, x + j - kernelMid));
                            int posY = Math.Min(originalImage.Height - 1, Math.Max(0, y + i - kernelMid));

                            Color pixel = originalImage.GetPixel(posX, posY);

                            double kernelValue = kernel[i, j];

                            rSum += pixel.R * kernelValue;
                            gSum += pixel.G * kernelValue;
                            bSum += pixel.B * kernelValue;
                            kSum += kernelValue;
                        }
                    }

                    // Ortalama değerleri al
                    int r = (int)(rSum / kSum);
                    int g = (int)(gSum / kSum);
                    int b = (int)(bSum / kSum);

                    // Yeni pikseli ata
                    Color blurredColor = Color.FromArgb(r, g, b);
                    blurredImage.SetPixel(x, y, blurredColor);
                }
            }
            return blurredImage;
        }
        public static Bitmap ApplyAverageFilter(Bitmap originalImage)
        {
            Bitmap blurredImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Filtre boyutu
            int filterSize = 3;

            // Filtre toplamı
            int filterSum = filterSize * filterSize;

            // Her piksel için filtre uygula
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    int rSum = 0, gSum = 0, bSum = 0;

                    // Filtre boyutu içinde dolaş
                    for (int i = -filterSize / 2; i <= filterSize / 2; i++)
                    {
                        for (int j = -filterSize / 2; j <= filterSize / 2; j++)
                        {
                            int posX = Math.Min(originalImage.Width - 1, Math.Max(0, x + j));
                            int posY = Math.Min(originalImage.Height - 1, Math.Max(0, y + i));

                            Color pixel = originalImage.GetPixel(posX, posY);

                            rSum += pixel.R;
                            gSum += pixel.G;
                            bSum += pixel.B;
                        }
                    }

                    // Ortalama değerleri al
                    int r = rSum / filterSum;
                    int g = gSum / filterSum;
                    int b = bSum / filterSum;

                    // Yeni pikseli ata
                    Color blurredColor = Color.FromArgb(r, g, b);
                    blurredImage.SetPixel(x, y, blurredColor);
                }
            }
            return blurredImage;
        }
    }
}
