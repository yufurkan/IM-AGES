using IM_AGES;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IM_AGES
{
    internal class Mustafa
    {
        public static Bitmap DetectEdges(Bitmap image)
        {
            Bitmap grayImage = Grayscale(image);
            Bitmap edgeImage = new Bitmap(image.Width, image.Height);
            // Sobel operatörü için Gx ve Gy matrislerini tanımlıyoruz
            int[,] Gx = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] Gy = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
            // Her pikselin kenarlık değerini hesaplamak için gri tonlamalı görüntü üzerinde döngü başlıyor
            for (int y = 1; y < grayImage.Height - 1; y++)
            {
                for (int x = 1; x < grayImage.Width - 1; x++)
                {
                    // Her pikselin x ve y yönlü türevlerini hesaplamak için Sobel operatörünü kullanıyoruz
                    int pixelX = (Gx[0, 0] * grayImage.GetPixel(x - 1, y - 1).R) + (Gx[0, 1] * grayImage.GetPixel(x, y - 1).R) + (Gx[0, 2] * grayImage.GetPixel(x + 1, y - 1).R)
                                + (Gx[1, 0] * grayImage.GetPixel(x - 1, y).R) + (Gx[1, 1] * grayImage.GetPixel(x, y).R) + (Gx[1, 2] * grayImage.GetPixel(x + 1, y).R)
                                + (Gx[2, 0] * grayImage.GetPixel(x - 1, y + 1).R) + (Gx[2, 1] * grayImage.GetPixel(x, y + 1).R) + (Gx[2, 2] * grayImage.GetPixel(x + 1, y + 1).R);

                    int pixelY = (Gy[0, 0] * grayImage.GetPixel(x - 1, y - 1).R) + (Gy[0, 1] * grayImage.GetPixel(x, y - 1).R) + (Gy[0, 2] * grayImage.GetPixel(x + 1, y - 1).R)
                                + (Gy[1, 0] * grayImage.GetPixel(x - 1, y).R) + (Gy[1, 1] * grayImage.GetPixel(x, y).R) + (Gy[1, 2] * grayImage.GetPixel(x + 1, y).R)
                                + (Gy[2, 0] * grayImage.GetPixel(x - 1, y + 1).R) + (Gy[2, 1] * grayImage.GetPixel(x, y + 1).R) + (Gy[2, 2] * grayImage.GetPixel(x + 1, y + 1).R);
                    // Pikselin kenarlık büyüklüğünü hesapla
                    int magnitude = (int)Math.Sqrt(pixelX * pixelX + pixelY * pixelY);
                    // Kenarlık büyüklüğünü 0 ile 255 arasında kısıtla
                    if (magnitude > 255)
                        magnitude = 255;
                    if (magnitude < 0)
                        magnitude = 0;
                    // Kenar pikselini oluşturmak için kenarlık büyüklüğünü kullanıyorum
                    edgeImage.SetPixel(x, y, Color.FromArgb(magnitude, magnitude, magnitude));
                }
            }

            return edgeImage;
        }

        private static Bitmap Grayscale(Bitmap image)
        {
            Bitmap grayImage = new Bitmap(image.Width, image.Height);
            // Her pikselin gri tonlamalı değerini hesaplamak için görüntü üzerinde döngü başlıyor
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    // Her pikselin RGB bileşenlerini alıyo
                    Color pixel = image.GetPixel(x, y);
                    // RGB bileşenlerini kullanarak gri tonlamalı bir değer hesaplıyorum.
                    int gray = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    // Gri pikseli oluşturmak için RGB bileşenlerinin aynı değerini kullanıyoruz
                    grayImage.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }

            return grayImage;
        }



        // Aritmetik işlemler (toplama, çarpma)
        //piksel değerlerini topluyoruz 
        public Bitmap AddImages(Bitmap image1, Bitmap image2)
        {
            // Her piksel değerini topluyoruz ve sonucu 0 ile 255 arasında sınırlıyoruz.
            return PerformArithmeticOperation(image1, image2, (v1, v2) => Math.Clamp(v1 + v2, 0, 255));
        }
        //piksel değerlerini çarpıyoruz 
        public Bitmap MultiplyImages(Bitmap image1, Bitmap image2)
        {
            return PerformArithmeticOperation(image1, image2, (v1, v2) => Math.Clamp((v1 * v2) / 255, 0, 255));
        }

        private Bitmap PerformArithmeticOperation(Bitmap image1, Bitmap image2, Func<int, int, int> operation)
        {
            Bitmap resultImage = new Bitmap(image1.Width, image1.Height);
            // Görüntülerin boyutlarına uygun bir dikdörtgen oluşturuyoruum
            Rectangle rect = new Rectangle(0, 0, image1.Width, image1.Height);
            // Görüntülerin veri kümelerini kilitleyerek işlem yapmak için BitmapData nesneleri oluşturuyor
            BitmapData imageData1 = image1.LockBits(rect, ImageLockMode.ReadOnly, image1.PixelFormat);
            BitmapData imageData2 = image2.LockBits(rect, ImageLockMode.ReadOnly, image2.PixelFormat);
            BitmapData resultData = resultImage.LockBits(rect, ImageLockMode.WriteOnly, resultImage.PixelFormat);
            // Her pikselin boyutunu ve byte sayısını hesaplıyoruz
            int bytesPerPixel = Image.GetPixelFormatSize(image1.PixelFormat) / 8;
            int byteCount = imageData1.Stride * image1.Height;
            // Her görüntü için bir dizi oluşturuyorum
            byte[] buffer1 = new byte[byteCount];
            byte[] buffer2 = new byte[byteCount];
            byte[] resultBuffer = new byte[byteCount];
            // Görüntü verilerini belleğe kopyalıyoruz
            Marshal.Copy(imageData1.Scan0, buffer1, 0, byteCount);
            Marshal.Copy(imageData2.Scan0, buffer2, 0, byteCount);
            // Her piksel için işlemi oluyorr
            for (int k = 0; k < byteCount; k += bytesPerPixel)
            {
                // İşlevi, her pikselin değerlerine uygulayarak sonuç tamponuna kaydediyoruz
                resultBuffer[k] = (byte)operation(buffer1[k], buffer2[k]);
                resultBuffer[k + 1] = (byte)operation(buffer1[k + 1], buffer2[k + 1]);
                resultBuffer[k + 2] = (byte)operation(buffer1[k + 2], buffer2[k + 2]);
            }
            // Görüntü verilerini kilitleyerek bellek sızıntısını önlemek için 
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, byteCount);
            image1.UnlockBits(imageData1);
            image2.UnlockBits(imageData2);
            resultImage.UnlockBits(resultData);

            return resultImage;
        }

    }
}