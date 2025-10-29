using OxyPlot.Series;
using OxyPlot;
using OxyPlot.WindowsForms;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using IM_AGES;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IM_AGES
{


    public partial class Ana_Sayfa : Form
    {
        private Panel historyPanel;
        private Queue<Tuple<string, Bitmap>> historyQueue; // İşlem adı ve görsel için Tuple kullanılır
        private const int ThumbnailWidth = 170;
        private const int ThumbnailHeight = 120;

        private Point cropStartPoint = Point.Empty;
        private Rectangle cropArea = Rectangle.Empty;
        private bool isMouseDown = false;

        private Bitmap kayıtResim;
        private Bitmap gösterilenResim;//zoom için-F
        private Bitmap orjinalResim; // Orijinal resm
        private Bitmap temporaryImage; // Parlaklık değişikliklerini saklamak için
        private Bitmap currentImage; // Geçerli resim
        private int currentBrightness = 0;
        private string ilkResimYolu;
        //zoom ile ilgili kısım-F
        double a = 1.0;
        double b = 1.0;
        double k = 1.0;
        float yatay = 1;
        float dikey = 1;

        private HScrollBar hScrollBarBrightness;

        public Ana_Sayfa(string ilk)
        {
            ilkResimYolu = ilk;

            InitializeComponent();
            historyQueue = new Queue<Tuple<string, Bitmap>>();
            InitializeHScrollBar();

            LoadOriginalImage(ilkResimYolu);


        }

        public void EtkinlikEkle(string yeni)//Yeni Etkinlik yani yeni açlan dosyayı son açılanlar kısmına ekliyorum-F
        {
            string dosyaAdi = "Etkinlik.txt";
            string dosyaYolu = Path.Combine(Application.StartupPath, dosyaAdi);
            bool eklemeYapildi = false;
            string[] satirlar = File.ReadAllLines(dosyaAdi);
            string yeniEtkinlik = $"{yeni}${DateTime.Now}";
            for (int i = 0; i < satirlar.Length; i++)
            {
                string[] parcalar = satirlar[i].Split('$');
                if (parcalar[0] == yeni)
                {

                    satirlar[i] = yeniEtkinlik; // eğer geçmişte varsa tarihini değiştiriyoz-F
                    File.WriteAllLines(dosyaYolu, satirlar);
                    eklemeYapildi = true;
                    break;
                }
            }
            if (!eklemeYapildi)
            {
                // Yeni etkinliği dosyaya ekle
                using (StreamWriter sw = File.AppendText(dosyaAdi))
                {
                    sw.WriteLine(yeniEtkinlik);
                    sw.Close();
                }
            }

        }

        private void ResetBrightness()
        {
            // Parlaklık ayarını sıfırla
            hScrollBar1.Value = 0;
            currentBrightness = 0;

            // Geçerli resmi geri yükle
            if (currentImage != null)
            {
                temporaryImage = new Bitmap(currentImage);
                gösterilenResim = temporaryImage;
            }
        }

        /*     private void InitializeHistoryPanel()
             {
                 // Geçmiş işlemleri gösterecek Panel oluştur
                 historyPanel = new Panel();
                 historyPanel.Dock = DockStyle.Bottom;
                 historyPanel.Height = 100; // Panel yüksekliğini ayarlayın
                 Controls.Add(historyPanel);
             }*/

        private void InitializeHScrollBar()
        {//-İ
            // HScrollBar'ı ayarla
            hScrollBar1.Minimum = -255;
            hScrollBar1.Maximum = 255;
            hScrollBar1.Value = 0; // Ortada başlat
            hScrollBar1.SmallChange = 1;
            hScrollBar1.LargeChange = 10;
            hScrollBar1.Scroll += new ScrollEventHandler(hScrollBar1_Scroll);
        }

        private void AddToHistory(string operationName, Bitmap image)
        {
            if (historyQueue == null)
            {
                historyQueue = new Queue<Tuple<string, Bitmap>>();
            }

            // Geçmiş sırasına yeni bir girdi ekler
            if (operationName != "Orijinal Resim")
            {
                historyQueue.Enqueue(new Tuple<string, Bitmap>(operationName, new Bitmap(image)));
            }

            // Geçmiş kuyruğu maksimum kapasiteye ulaşırsa, en eski girdiyi çıkar
            int maxCapacity = 10;
            while (historyQueue.Count > maxCapacity)
            {
                historyQueue.Dequeue();
            }

            UpdateHistoryPanel(); // Geçmiş panelini güncelle
        }


        private void UpdateHistoryPanel()
        {
            panel2.Controls.Clear();
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                
                WrapContents = false,
                AutoScroll = true
            };

            // Orijinal resmi ilk sıraya ekle
            if (orjinalResim != null)
            {
                AddImageToPanel(flowLayoutPanel, "Orijinal Resim", orjinalResim);
            }

            // Geçmişteki diğer resimleri ekle
            foreach (var historyItem in historyQueue)
            {
                AddImageToPanel(flowLayoutPanel, historyItem.Item1, historyItem.Item2);
            }

            panel2.Controls.Add(flowLayoutPanel);
        }

        private void AddImageToPanel(FlowLayoutPanel panel, string title, Bitmap image)
        {
            Bitmap thumbnail = new Bitmap(image, ThumbnailWidth, ThumbnailHeight);
            Panel itemPanel = new Panel
            {
                Size = new Size(ThumbnailWidth, ThumbnailHeight + 20),
                Dock = DockStyle.Top
            };
            Label label = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                AutoSize = false,
                Height = 32
            };
            PictureBox pictureBox = new PictureBox
            {
                Image = thumbnail,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            pictureBox.Click += (sender, e) =>
            {
                kayıtResim = new Bitmap(image);
                gösterilenResim = new Bitmap(image);
                // Ana PictureBox'a tıklanan resmi yükle
                temporaryImage = new Bitmap(image);  // Geçici resmi güncelle
                currentImage = new Bitmap(image); // Geçerli resmi güncelle
                ResetBrightness();  // Parlaklık ayarını sıfırla
                ResmiYükle(image);

                // Tıklanan resmin histogramını güncelle
                int[] histogram = ComputeHistogram(image);
                DrawHistogram(histogram, panel3);
                panel1.Invalidate();
            };

            itemPanel.Controls.Add(pictureBox);
            itemPanel.Controls.Add(label);
            panel.Controls.Add(itemPanel);
        }

        private void AdjustBrightness(int brightnessValue)
        {
            if (temporaryImage == null)
            {
                return;
            }

            // Geçici resmi kullanarak yeni bir Bitmap oluştur
            Bitmap adjustedImage = new Bitmap(temporaryImage.Width, temporaryImage.Height);

            // Renk matrisi oluştur
            float brightness = brightnessValue / 500f;
            float[][] matrixElements = {
                new float[] {1, 0, 0, 0, 0}, // Kırmızı bileşen
                new float[] {0, 1, 0, 0, 0}, // Yeşil bileşen
                new float[] {0, 0, 1, 0, 0}, // Mavi bileşen
                new float[] {0, 0, 0, 1, 0}, // Alfa bileşen (opaqueness)
                new float[] {brightness, brightness, brightness, 0, 1} // Konstanta
            };
            ColorMatrix colorMatrix = new ColorMatrix(matrixElements);

            // Resmi işle
            using (Graphics graphics = Graphics.FromImage(adjustedImage))
            {
                using (ImageAttributes imageAttributes = new ImageAttributes())
                {
                    imageAttributes.SetColorMatrix(colorMatrix);
                    graphics.DrawImage(temporaryImage, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height),
                        0, 0, temporaryImage.Width, temporaryImage.Height, GraphicsUnit.Pixel, imageAttributes);
                }
            }

            // İşlem tamamlandıktan sonra PictureBox'a güncellenmiş resmi ata
            kayıtResim = adjustedImage.Clone() as Bitmap;
            gösterilenResim = adjustedImage;
        }


        private void LoadOriginalImage(string imagePath)
        {
            EtkinlikEkle(imagePath);
            // Seçilen resmi yükle ve pictureBox1'e ayarla
            orjinalResim = new Bitmap(imagePath);
            kayıtResim = orjinalResim.Clone() as Bitmap;
            temporaryImage = new Bitmap(orjinalResim);
            currentImage = new Bitmap(orjinalResim);
            gösterilenResim = new Bitmap(orjinalResim);



            ResmiYükle(gösterilenResim);

            pictureBox4.Image = orjinalResim;

            // Eğer "Orijinal Resim" geçmişte yoksa ekle
            if (historyQueue.All(item => item.Item1 != "Orijinal Resim"))
            {
                AddToHistory("Orijinal Resim", new Bitmap(orjinalResim));
            }
        }



        private int[] ComputeHistogram(Bitmap image)
        {
            int[] histogram = new int[256];
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int grayScale = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                    histogram[grayScale]++;
                }
            }
            return histogram;
        }

        private void DrawHistogram(int[] histogram, Panel panel)
        {
            int max = histogram.Max(); // En yüksek sıklık değeri
            Bitmap bmp = new Bitmap(panel.Width, panel.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White); // Arka planı temizle

                // Grafiğin çerçevesini çiz
                int margin = 30;
                Rectangle graphArea = new Rectangle(margin, margin, panel.Width - 2 * margin, panel.Height - 2 * margin);
                g.FillRectangle(Brushes.LightGray, graphArea);
                g.DrawRectangle(Pens.Black, graphArea);

                // X ve Y ekseni çizgilerini çiz
                g.DrawLine(Pens.Black, margin, panel.Height - margin, panel.Width - margin, panel.Height - margin);
                g.DrawLine(Pens.Black, margin, margin, margin, panel.Height - margin);

                // Histogram çubuklarını çiz
                float columnWidth = (float)graphArea.Width / histogram.Length;
                for (int i = 0; i < histogram.Length; i++)
                {
                    float columnHeight = ((float)histogram[i] / max) * graphArea.Height;
                    RectangleF columnRect = new RectangleF(
                        margin + i * columnWidth,
                        graphArea.Bottom - columnHeight,
                        columnWidth,
                        columnHeight);

                    g.FillRectangle(Brushes.RoyalBlue, columnRect);
                    g.DrawRectangle(Pens.Black, columnRect.X, columnRect.Y, columnRect.Width, columnRect.Height); // Çubuk çerçevesi
                }

                // Eksen etiketleri ve başlığı ekle
                string title = "Gri Tonlama Histogramı";
                Font font = new Font("Arial", 12);
                SizeF titleSize = g.MeasureString(title, font);
                g.DrawString(title, font, Brushes.Black, (panel.Width - titleSize.Width) / 2, 5);

                // X ekseni etiketleri
                string xAxisLabel = "Piksel Değerleri (0-255)";
                SizeF xAxisLabelSize = g.MeasureString(xAxisLabel, font);
                g.DrawString(xAxisLabel, font, Brushes.Black, (panel.Width - xAxisLabelSize.Width) / 2, panel.Height - 25);
            }

            panel.BackgroundImage = bmp; // Panelin arka planına çizilen bitmap'i ata
        }

        private void toolStripMenuItem90dondur_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap rotatedImage = Furkan.Rotate90(currentImage);
                kayıtResim = rotatedImage.Clone() as Bitmap;
                gösterilenResim = rotatedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("90 derece döndürüldü", rotatedImage);  // Açıklama ilk sırada, resim ikinci sırada
                temporaryImage = new Bitmap(rotatedImage); // Geçici resmi güncelle
                currentImage = new Bitmap(rotatedImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
            }
        }

        private void toolStripMenuItemdondur180_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap rotatedImage = Furkan.Rotate180(currentImage);
                kayıtResim = rotatedImage.Clone() as Bitmap;
                gösterilenResim = rotatedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("180 derece döndürüldü", rotatedImage);  // Açıklama ilk sırada, resim ikinci sırada
                temporaryImage = new Bitmap(rotatedImage); // Geçici resmi güncelle
                currentImage = new Bitmap(rotatedImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
            }
        }

        private void toolStripMenuItemdondur270_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap rotatedImage = Furkan.Rotate270(currentImage);
                kayıtResim = rotatedImage.Clone() as Bitmap;
                gösterilenResim = rotatedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("270 derece döndürüldü", rotatedImage);  // Açıklama ilk sırada, resim ikinci sırada
                temporaryImage = new Bitmap(rotatedImage); // Geçici resmi güncelle
                currentImage = new Bitmap(rotatedImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
            }
        }


        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Alperen alperen = new Alperen();
                Bitmap medianImage = alperen.ApplyMedianFilter(currentImage);
                kayıtResim = medianImage.Clone() as Bitmap;
                gösterilenResim = medianImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Median Filtre", medianImage);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(medianImage);
                currentImage = new Bitmap(medianImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim seçin.");
            }
        }

        private void tuzBiberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Alperen alperen = new Alperen();
                double noiseLevel = 0.05; // Gürültü seviyesi (örnek olarak %5 gürültü)
                Bitmap noisyImage = alperen.ApplySaltAndPepperNoise(currentImage, noiseLevel);
                kayıtResim = noisyImage.Clone() as Bitmap;
                gösterilenResim = noisyImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Tuz Biber Gürültüsü", noisyImage);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(noisyImage);
                currentImage = new Bitmap(noisyImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim seçin.");
            }
        }

        private void meanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Alperen alperen = new Alperen();
                Bitmap meanImage = alperen.ApplyMeanFilter(currentImage);
                kayıtResim = meanImage.Clone() as Bitmap;
                gösterilenResim = meanImage;
                AddToHistory("Mean Filtre", meanImage);

                ResmiYükle(gösterilenResim);
                // Geçici resmi güncelle
                temporaryImage = new Bitmap(meanImage);
                currentImage = new Bitmap(meanImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim seçin.");
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (temporaryImage != null)
            {
                currentBrightness = hScrollBar1.Value;

                try
                {
                    AdjustBrightness(currentBrightness); // Parlaklığı ayarla
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Parlaklık ayarlarken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            trackBar2_Scroll_1(trackBar2, EventArgs.Empty);
        }



        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }//kullanım dışı bırakıldı-F

        private void acToolStripMenuItem_Click_1(object sender, EventArgs e)//resim açıldığında
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                ilkResimYolu = selectedImagePath;
                // Seçilen resmi yükle ve pictureBox1'e ayarla
                LoadOriginalImage(selectedImagePath);
                pictureBox1.Image = ZoomPicture(gösterilenResim, new Size(trackBar2.Value, trackBar2.Value));

                pictureBox4.Image = new Bitmap(openFileDialog.FileName);
                pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;

                // Parlaklık ayarını sıfırla
                ResetBrightness();
            }
        }

        private void histogramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen bir resim seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Bitmap currentImage = new Bitmap(pictureBox1.Image);
            int[] histogram = ComputeHistogram(currentImage);
            DrawHistogram(histogram, panel3); // Panel3'te histogramı çiz
        }

        private void kenarBulmaSobelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen bir resim seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Bitmap currentImage = new Bitmap(kayıtResim);
            Mustafa mustafaInstance = new Mustafa();
            Bitmap edgeImage = Mustafa.DetectEdges(currentImage);
            AddToHistory("Sobel Kenar Tespiti", edgeImage); // İşlem görselini geçmişe ekle
            kayıtResim = edgeImage.Clone() as Bitmap;
            gösterilenResim = edgeImage;
            ResmiYükle(gösterilenResim);
            // Geçici resmi güncelle
            temporaryImage = new Bitmap(edgeImage);
            currentImage = new Bitmap(edgeImage); // Geçerli resmi güncelle
        }

        private void hSVDonusumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Ismail IsmailInstance = new Ismail();
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap hsvImage = Ismail.ConvertToHSV(currentImage);
                kayıtResim = hsvImage.Clone() as Bitmap;
                gösterilenResim = hsvImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("HSV Dönüşüm", hsvImage);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(hsvImage);
                currentImage = new Bitmap(hsvImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim seçin.");
            }
        }

        private void binaryDonusumToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Ismail IsmailInstance = new Ismail();
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap transformedImage = Ismail.ConvertToBinary(currentImage);
                kayıtResim = transformedImage.Clone() as Bitmap;
                gösterilenResim = transformedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Binary Dönüşüm", transformedImage);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(transformedImage);
                currentImage = new Bitmap(transformedImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim seçin.");
            }
        }

        private void adaptifEsiklemetoolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap thresholdedImage = Furkan.ApplyAdaptiveThreshold(currentImage, 21, 5); // blockSize ve C değerleri örnek olarak verilmiştir
                kayıtResim = thresholdedImage.Clone() as Bitmap;
                gösterilenResim = thresholdedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Adaptif Eşikleme Uygulandı", thresholdedImage);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(thresholdedImage);
                currentImage = new Bitmap(thresholdedImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
            }
        }

        private void GenislemetoolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap dilatedImage = Furkan.ApplyDilation(currentImage, 3);  // Kernel boyutu örneğin 3 olarak belirlenmiştir
                kayıtResim = dilatedImage.Clone() as Bitmap;
                gösterilenResim = dilatedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Genişleme Uygulandı", dilatedImage);


                // Geçici resmi güncelle
                temporaryImage = new Bitmap(dilatedImage);
                currentImage = new Bitmap(dilatedImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
            }
        }

        private void AsinmatoolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap erodedImage = Furkan.ApplyErosion(currentImage, 3);  // Kernel boyutu örneğin 3 olarak belirlenmiştir
                kayıtResim = erodedImage.Clone() as Bitmap;
                gösterilenResim = erodedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Aşınma Uygulandı", erodedImage);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(erodedImage);
                currentImage = new Bitmap(erodedImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
            }
        }

        private void AcmatoolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap openedImage = Furkan.ApplyOpening(currentImage, 3);  // Kernel boyutu  3 
                kayıtResim = openedImage.Clone() as Bitmap;
                gösterilenResim = openedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Açma Uygulandı", openedImage);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(openedImage);
                currentImage = new Bitmap(openedImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
            }
        }

        private void kapamaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap closedImage = Furkan.ApplyClosing(currentImage, 3);  // Kernel boyutu örneğin 3 olarak belirlenmiştir
                kayıtResim = closedImage.Clone() as Bitmap;
                gösterilenResim = closedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Kapama Uygulandı", closedImage);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(closedImage);
                currentImage = new Bitmap(closedImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
            }
        }

        private void germeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap currentImage = new Bitmap(kayıtResim);
                Furkan FurkanInstance = new Furkan();
                Bitmap stretchedImage = FurkanInstance.StretchHistogram(currentImage);
                kayıtResim = stretchedImage.Clone() as Bitmap;
                gösterilenResim = stretchedImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Histogram Germe", stretchedImage);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(stretchedImage);
                currentImage = new Bitmap(stretchedImage); // Geçerli resmi güncelle

                // Yeni histogramı hesapla ve göster
                int[] newHistogram = FurkanInstance.ComputeHistogram(stretchedImage);
                DrawHistogram(newHistogram, panel3);
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim seçin.");
            }
        }

        private void gaussToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Ismail IsmailInstance = new Ismail();
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap blurredImage = Ismail.ApplyGaussianBlur(currentImage);
                kayıtResim = blurredImage.Clone() as Bitmap;
                gösterilenResim = blurredImage;
                AddToHistory("Gauss Blur", blurredImage);
                ResmiYükle(gösterilenResim);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(blurredImage);
                currentImage = new Bitmap(blurredImage); // Geçerli resmi güncelle
            }
        }

        private void aritmetikIslemModuToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ImageSelectionForm imageSelectionForm = new ImageSelectionForm();
            if (imageSelectionForm.ShowDialog() == DialogResult.OK)
            {
                Bitmap resultImage = imageSelectionForm.ResultImage;
                kayıtResim = resultImage.Clone() as Bitmap;
                gösterilenResim = resultImage;
                AddToHistory("Aritmetik İşlem", resultImage);
                ResmiYükle(resultImage);
                // Geçici resmi güncelle
                temporaryImage = new Bitmap(resultImage);
                currentImage = new Bitmap(resultImage);
            }
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kayıtResim != null)
            {
                string dosyaYolu = ilkResimYolu; // Kaydedilecek dosya yolunu ve adını belirleyin.
                direkKaydet(kayıtResim, dosyaYolu, System.Drawing.Imaging.ImageFormat.Jpeg);

                MessageBox.Show("Başarıyla Kaydedildi");
            }
        }
        private void direkKaydet(Image image, string filePath, System.Drawing.Imaging.ImageFormat format)
        {
            image.Save(filePath, format);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png";
                saveFileDialog.Title = "Resmi Kaydet";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    using (System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile())
                    {
                        switch (saveFileDialog.FilterIndex)
                        {
                            case 1:
                                kayıtResim.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                                break;
                            case 2:
                                kayıtResim.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                                break;
                            case 3:
                                kayıtResim.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                                break;
                        }
                    }

                    MessageBox.Show("Başarıyla Kaydedildi");
                }
            }
        }


        private void kapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void griDonusumToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Alperen alperen = new Alperen();
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap grayscaleImage = alperen.ConvertToGrayscale(currentImage);
                kayıtResim = grayscaleImage.Clone() as Bitmap;
                gösterilenResim = grayscaleImage;
                ResmiYükle(gösterilenResim);
                AddToHistory("Gri Dönüşüm", grayscaleImage);


                // Geçici görüntüyü güncelle
                temporaryImage = new Bitmap(grayscaleImage);
                currentImage = new Bitmap(grayscaleImage); // Geçerli resmi güncelle
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin.");
            }
        }

        private void kirpToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (CropForm cropForm = new CropForm((Bitmap)kayıtResim))
                {
                    if (cropForm.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap croppedImage = cropForm.CroppedImage;
                        if (croppedImage != null)
                        {
                            // Kırpılmış resmi ana paneldeki pictureBox'a yeniden boyutlandırarak ekle
                            kayıtResim = croppedImage.Clone() as Bitmap;

                            gösterilenResim = croppedImage;
                            ResmiYükle(croppedImage);

                            AddToHistory("Kırpma", croppedImage);

                            // Geçici resmi güncelle
                            temporaryImage = new Bitmap(croppedImage);
                            currentImage = new Bitmap(croppedImage); // Geçerli resmi güncelle
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim seçin.");
            }
        }

        /* private Bitmap ResizeImageToFitPanel(Bitmap image, Control panel)
         {
             int panelWidth = pictureBox1.Width;
             int panelHeight = pictureBox1.Height;
             return ResizeImage(image, panelWidth, panelHeight);
         }*/


        private void yardımToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Yardım yd = new Yardım();
            yd.ShowDialog();
        }


        //ZOOM KISMI------------------------------------------------------------------------------------------------------------------------------------------------------<<<<<<<<<<<<<<<<<<<<<<<<

        public void ResmiYükle(Bitmap resim)
        {

            // Eğer resim boyutu panel boyutlarından büyükse Resimi sığdırmak için burası, öbür türlü resim boyutu işleri zorluyo-F

            a = 1.0;
            b = 1.0;

            double genişlik = panel1.Size.Width;
            double yükseklik = panel1.Size.Height;
            a = genişlik / gösterilenResim.Width;
            b = yükseklik / gösterilenResim.Height;

            k = Math.Max(a, b);
            if (a == b) k = a;
            //k değerini 1 den küçük bir değer olarak atıyoruz ki resimi istediğimiz boyutta başlatalım-F

            try
            {
                gösterilenResim = resim.Clone() as Bitmap;


                pictureBox1.Image = ZoomPicture(gösterilenResim, new Size(trackBar2.Value, trackBar2.Value));
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA OLUŞTU : " + ex);

            }
        }
        Image ZoomPicture(Image img, Size size)
        {
            //10x zoom özelliği düşünüyorum ancak büyük resimlerde çok zorlnıyo-F
            //k değerini küçük resimler için 1 den büyük yapabiliriz bu sayede küçük resimlerde tam boyutla hizalanır-F

            //burada gpu ile zoom boyutlarını ayarlayan işlevler-F
            Bitmap bmp = new Bitmap(img, Convert.ToInt32(img.Width * k * size.Width), Convert.ToInt32(img.Height * k * size.Height));//k
            Graphics gpu = Graphics.FromImage(bmp);
            gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }

        float PaneliYatayAl(ScrollableControl panel)
        {
            if (panel.HorizontalScroll.Visible)
            {
                int maxScroll = panel.HorizontalScroll.Maximum - panel.HorizontalScroll.LargeChange;
                if (maxScroll != 0)
                {
                    return (float)panel.HorizontalScroll.Value / maxScroll;
                }
            }
            return 0.5f;
        }//yatay panel konumunu bulur-F

        float PaneliDikeyAl(ScrollableControl panel)
        {
            if (panel.VerticalScroll.Visible)
            {
                int maxScroll = panel.VerticalScroll.Maximum - panel.VerticalScroll.LargeChange;
                if (maxScroll != 0)
                {
                    return (float)panel.VerticalScroll.Value / maxScroll;
                }
            }
            return 0.5f;
        }//dikey panel konumunu bulur-F

        void KaydırmaOrantısı(ScrollableControl panel, float yatayAl, float dikeyAl)//zoom yapıldıktan sonra eski konumu yenisine uygular-F
        {// baktığını yere zoom yapmak için

            int maxVerticalScroll = panel.VerticalScroll.Maximum - panel.VerticalScroll.LargeChange;
            int maxHorizontalScroll = panel.HorizontalScroll.Maximum - panel.HorizontalScroll.LargeChange;
            if (panel.HorizontalScroll.Visible)
            {

                panel.HorizontalScroll.Value = (int)(maxHorizontalScroll * yatayAl);
            }

            if (panel.VerticalScroll.Visible)
            {

                panel.VerticalScroll.Value = (int)(maxVerticalScroll * dikeyAl);

            }
            panel1.AutoScrollPosition = new Point((int)(maxHorizontalScroll * yatayAl), ((int)(maxVerticalScroll * dikeyAl)));//Panel scrollarının sıfırlanmasıyla ilgili bir hatayı düzeltir-F
            //bu bir mühendislik harikası
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void trackBar2_Scroll_1(object sender, EventArgs e)
        {
            yatay = PaneliYatayAl(panel1);
            dikey = PaneliDikeyAl(panel1);// zoomu istdiğimiz bölgeye yapmak için-F

            if (trackBar2.Value != 0)
            {
                pictureBox1.Image = null;
                pictureBox1.Image = ZoomPicture(gösterilenResim, new Size(trackBar2.Value, trackBar2.Value));
            }
            KaydırmaOrantısı(panel1, yatay, dikey);
            panel1.Invalidate();
        }

        private void averageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Ismail IsmailInstance = new Ismail();
                Bitmap currentImage = new Bitmap(kayıtResim);
                Bitmap blurredImage = Ismail.ApplyAverageFilter(currentImage);
                kayıtResim = blurredImage.Clone() as Bitmap;
                gösterilenResim = blurredImage;
                AddToHistory("Average Blur", blurredImage);
                ResmiYükle(gösterilenResim);

                // Geçici resmi güncelle
                temporaryImage = new Bitmap(blurredImage);
                currentImage = new Bitmap(blurredImage); // Geçerli resmi güncelle
            }
        }
    }
}
