using System;
using System.Drawing;

namespace IM_AGES
{
    internal class Furkan
    {
        public static Bitmap Rotate90(Bitmap originalImage)
        {
            Bitmap rotatedImage = new Bitmap(originalImage.Height, originalImage.Width);
            for (int i = 0; i < originalImage.Width; i++)
            {
                for (int j = 0; j < originalImage.Height; j++)
                {
                    // Orijinal resmin (i, j) pikselini döndürülmüş resmin (j, orijinal genişlik - 1 - i) pikseline yerleştirme.
                    rotatedImage.SetPixel(j, originalImage.Width - 1 - i, originalImage.GetPixel(i, j));
                }
            }
            return rotatedImage;
        }

        public static Bitmap Rotate180(Bitmap originalImage)
        {
            Bitmap rotatedImage = new Bitmap(originalImage.Width, originalImage.Height);
            for (int i = 0; i < originalImage.Width; i++)
            {
                for (int j = 0; j < originalImage.Height; j++)
                {
                    // Orijinal resmin (i, j) pikselini döndürülmüş resmin (orijinal genişlik - 1 - i, orijinal yükseklik - 1 - j) pikseline yerleştirme.
                    rotatedImage.SetPixel(originalImage.Width - 1 - i, originalImage.Height - 1 - j, originalImage.GetPixel(i, j));
                }
            }
            return rotatedImage;
        }

        public static Bitmap Rotate270(Bitmap originalImage)
        {
            Bitmap rotatedImage = new Bitmap(originalImage.Height, originalImage.Width);
            for (int i = 0; i < originalImage.Width; i++)
            {
                for (int j = 0; j < originalImage.Height; j++)
                {
                    // Orijinal resmin (i, j) pikselini döndürülmüş resmin (orijinal yükseklik - 1 - j, i) pikseline yerleştirme.
                    rotatedImage.SetPixel(originalImage.Height - 1 - j, i, originalImage.GetPixel(i, j));
                }
            }
            return rotatedImage;
        }

        public static Bitmap ApplyAdaptiveThreshold(Bitmap originalImage, int blockSize, double C)
        {
            int width = originalImage.Width;
            int height = originalImage.Height;
            Bitmap resultImage = new Bitmap(width, height);

            // Görüntüyü gri tonlamalı yapma
            Bitmap grayScaleImage = ConvertToGrayscale(originalImage);

            // Blok boyutu içinde ortalama hesaplama için integral görüntüsü oluşturma
            int[,] integralImage = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                int sum = 0;
                for (int x = 0; x < width; x++)
                {
                    sum += grayScaleImage.GetPixel(x, y).R;
                    if (y == 0)
                    {
                        integralImage[x, y] = sum;
                    }
                    else
                    {
                        integralImage[x, y] = integralImage[x, y - 1] + sum;
                    }
                }
            }

            int halfBlockSize = blockSize / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int x1 = Math.Max(x - halfBlockSize, 0);
                    int x2 = Math.Min(x + halfBlockSize, width - 1);
                    int y1 = Math.Max(y - halfBlockSize, 0);
                    int y2 = Math.Min(y + halfBlockSize, height - 1);

                    int count = (x2 - x1 + 1) * (y2 - y1 + 1);
                    int sum = integralImage[x2, y2];

                    if (x1 > 0)
                        sum -= integralImage[x1 - 1, y2];
                    if (y1 > 0)
                        sum -= integralImage[x2, y1 - 1];
                    if (x1 > 0 && y1 > 0)
                        sum += integralImage[x1 - 1, y1 - 1];

                    int average = sum / count;
                    int pixelValue = grayScaleImage.GetPixel(x, y).R;
                    if (pixelValue < average - C)
                        resultImage.SetPixel(x, y, Color.Black);
                    else
                        resultImage.SetPixel(x, y, Color.White);
                }
            }

            return resultImage;
        }

        public static Bitmap ConvertToGrayscale(Bitmap originalImage)
        {
            Bitmap grayImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int i = 0; i < originalImage.Width; i++)
            {
                for (int j = 0; j < originalImage.Height; j++)
                {
                    // Her bir pikselin gri tonlamalı değerini hesaplama ve atama.
                    Color originalColor = originalImage.GetPixel(i, j);
                    //grileştirme için çarpanlr
                    int grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));
                    Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);
                    grayImage.SetPixel(i, j, grayColor);
                }
            }
            return grayImage;
        }

        // Morfolojik İşlemler
        public static Bitmap ApplyDilation(Bitmap input, int kernelSize)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);
            int margin = kernelSize / 2;

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    bool isDilated = false;
                    for (int dy = -margin; dy <= margin && !isDilated; dy++)
                    {
                        for (int dx = -margin; dx <= margin && !isDilated; dx++)
                        {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && ny >= 0 && nx < input.Width && ny < input.Height)
                            {
                                // Beyaz piksel bulunursa genişletme işlemini uygular.
                                Color pixel = input.GetPixel(nx, ny);
                                if (pixel.R == 255)
                                {
                                    isDilated = true;
                                }
                            }
                        }
                    }
                    output.SetPixel(x, y, isDilated ? Color.White : Color.Black);
                }
            }
            return output;
        }

        public static Bitmap ApplyErosion(Bitmap input, int kernelSize)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);
            int margin = kernelSize / 2;

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    bool isEroded = true;
                    for (int dy = -margin; dy <= margin && isEroded; dy++)
                    {
                        for (int dx = -margin; dx <= margin && isEroded; dx++)
                        {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && ny >= 0 && nx < input.Width && ny < input.Height)
                            {
                                // Siyah piksel bulunursa aşındırma işlemini uygular.
                                Color pixel = input.GetPixel(nx, ny);
                                if (pixel.R != 255)
                                {
                                    isEroded = false;
                                }
                            }
                        }
                    }
                    output.SetPixel(x, y, isEroded ? Color.White : Color.Black);
                }
            }
            return output;
        }

        public static Bitmap ApplyOpening(Bitmap input, int kernelSize)
        {
            // Açma işlemi: önce aşındırma sonra genişletme uygular.
            Bitmap erodedImage = ApplyErosion(input, kernelSize);
            return ApplyDilation(erodedImage, kernelSize);
        }

        public static Bitmap ApplyClosing(Bitmap input, int kernelSize)
        {
            // Kapama işlemi: önce genişletme sonra aşındırma uygular.
            Bitmap dilatedImage = ApplyDilation(input, kernelSize);
            return ApplyErosion(dilatedImage, kernelSize);
        }

        // Histogram İşlemleri
        public Bitmap StretchHistogram(Bitmap original)
        {
            Bitmap stretchedImage = new Bitmap(original.Width, original.Height);
            int width = original.Width;
            int height = original.Height;

            // Görüntüdeki minimum ve maksimum yoğunluk değerlerini belirle
            int min = 255;
            int max = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color = original.GetPixel(x, y);
                    int intensity = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    if (intensity < min) min = intensity;
                    if (intensity > max) max = intensity;
                }
            }

            // Piksel değerlerini genişletme
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color = original.GetPixel(x, y);
                    int r = Clamp(StretchValue(color.R, min, max));
                    int g = Clamp(StretchValue(color.G, min, max));
                    int b = Clamp(StretchValue(color.B, min, max));
                    stretchedImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return stretchedImage;
        }

        // Yoğunluk değerlerini genişletme
        private int StretchValue(int value, int min, int max)
        {
            if (max == min)
                return value; // Tüm piksel değerleri aynıysa, dönüşüm yapma
            return (value - min) * 255 / (max - min);
        }

        // Değerleri 0 ile 255 arasında sınırla
        private int Clamp(int value)
        {
            return Math.Max(0, Math.Min(255, value));
        }

        // Histogramı Hesaplama
        public int[] ComputeHistogram(Bitmap image)
        {
            int[] histogram = new int[256];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    // Her pikselin gri tonlamalı değerini hesaplayarak histogram dizisini günceller.
                    Color pixelColor = image.GetPixel(x, y);
                    int grayScale = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                    histogram[grayScale]++;
                }
            }
            return histogram;
        }
    }
}