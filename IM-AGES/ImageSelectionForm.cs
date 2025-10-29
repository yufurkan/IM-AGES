using System;
using System.Drawing;
using System.Windows.Forms;

namespace IM_AGES
{
    public partial class ImageSelectionForm : Form
    {
        private Button buttonResimSec1;
        private Button buttonResimSec2;
        private Button buttonTopla;
        private Button buttonCarp;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBoxSonuc;

        private Bitmap resim1;
        private Bitmap resim2;
        public Bitmap ResultImage { get; private set; }

        public ImageSelectionForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            buttonResimSec1 = new Button();
            buttonResimSec2 = new Button();
            buttonTopla = new Button();
            buttonCarp = new Button();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBoxSonuc = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSonuc).BeginInit();
            SuspendLayout();
            // 
            // buttonResimSec1
            // 
            buttonResimSec1.Location = new Point(81, 280);
            buttonResimSec1.Name = "buttonResimSec1";
            buttonResimSec1.Size = new Size(87, 30);
            buttonResimSec1.TabIndex = 0;
            buttonResimSec1.Text = "Resim 1 Seç";
            buttonResimSec1.Click += buttonResimSec1_Click;
            // 
            // buttonResimSec2
            // 
            buttonResimSec2.Location = new Point(194, 280);
            buttonResimSec2.Name = "buttonResimSec2";
            buttonResimSec2.Size = new Size(87, 30);
            buttonResimSec2.TabIndex = 1;
            buttonResimSec2.Text = "Resim 2 Seç";
            buttonResimSec2.Click += buttonResimSec2_Click;
            // 
            // buttonTopla
            // 
            buttonTopla.Location = new Point(291, 280);
            buttonTopla.Name = "buttonTopla";
            buttonTopla.Size = new Size(94, 30);
            buttonTopla.TabIndex = 2;
            buttonTopla.Text = "Topla";
            buttonTopla.Click += buttonTopla_Click;
            // 
            // buttonCarp
            // 
            buttonCarp.Location = new Point(397, 280);
            buttonCarp.Name = "buttonCarp";
            buttonCarp.Size = new Size(94, 30);
            buttonCarp.TabIndex = 3;
            buttonCarp.Text = "Çarp";
            buttonCarp.Click += buttonCarp_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Location = new Point(81, 57);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(200, 200);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            pictureBox2.Location = new Point(291, 57);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(200, 200);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 5;
            pictureBox2.TabStop = false;
            // 
            // pictureBoxSonuc
            // 
            pictureBoxSonuc.BorderStyle = BorderStyle.Fixed3D;
            pictureBoxSonuc.Location = new Point(625, 20);
            pictureBoxSonuc.Name = "pictureBoxSonuc";
            pictureBoxSonuc.Size = new Size(200, 200);
            pictureBoxSonuc.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSonuc.TabIndex = 6;
            pictureBoxSonuc.TabStop = false;
            // 
            // ImageSelectionForm
            // 
            BackgroundImage = Properties.Resources.desktop_wallpaper_blur_blue_gradient_cool_background;
            ClientSize = new Size(576, 369);
            Controls.Add(buttonResimSec1);
            Controls.Add(buttonResimSec2);
            Controls.Add(buttonTopla);
            Controls.Add(buttonCarp);
            Controls.Add(pictureBox1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBoxSonuc);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ImageSelectionForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Resim Seçimi ve Aritmetik İşlemler";
            Load += ImageSelectionForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSonuc).EndInit();
            ResumeLayout(false);
        }

        private void buttonResimSec1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tüm Dosyalar (*.*)|*.*";
            openFileDialog.Title = "Resim 1 Seç";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string resimYolu = openFileDialog.FileName;
                resim1 = new Bitmap(resimYolu);
                pictureBox1.Image = resim1;
            }
        }

        private void buttonResimSec2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tüm Dosyalar (*.*)|*.*";
            openFileDialog.Title = "Resim 2 Seç";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string resimYolu = openFileDialog.FileName;
                resim2 = new Bitmap(resimYolu);
                pictureBox2.Image = resim2;
            }
        }

        private void buttonTopla_Click(object sender, EventArgs e)
        {
            if (resim1 == null || resim2 == null)
            {
                MessageBox.Show("Lütfen iki resmi de seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Bitmap sonuc = Toplama(resim1, resim2);
            ResultImage = sonuc;
            pictureBoxSonuc.Image = sonuc;
            this.DialogResult = DialogResult.OK; // İşlem tamamlandığında formu kapat ve sonucu ana formda göster
            this.Close();
        }

        private void buttonCarp_Click(object sender, EventArgs e)
        {
            if (resim1 == null || resim2 == null)
            {
                MessageBox.Show("Lütfen iki resmi de seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Bitmap sonuc = Carpma(resim1, resim2);
            ResultImage = sonuc;
            pictureBoxSonuc.Image = sonuc;
            this.DialogResult = DialogResult.OK; // İşlem tamamlandığında formu kapat ve sonucu ana formda göster
            this.Close();
        }

        private Bitmap Toplama(Bitmap resim1, Bitmap resim2)
        {
            int genislik = Math.Min(resim1.Width, resim2.Width);
            int yukseklik = Math.Min(resim1.Height, resim2.Height);

            Bitmap sonuc = new Bitmap(genislik, yukseklik);

            for (int y = 0; y < yukseklik; y++)
            {
                for (int x = 0; x < genislik; x++)
                {
                    Color piksel1 = resim1.GetPixel(x, y);
                    Color piksel2 = resim2.GetPixel(x, y);

                    int red = Math.Min(255, piksel1.R + piksel2.R);
                    int green = Math.Min(255, piksel1.G + piksel2.G);
                    int blue = Math.Min(255, piksel1.B + piksel2.B);

                    sonuc.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }

            return sonuc;
        }

        private Bitmap Carpma(Bitmap resim1, Bitmap resim2)
        {
            int genislik = Math.Min(resim1.Width, resim2.Width);
            int yukseklik = Math.Min(resim1.Height, resim2.Height);

            Bitmap sonuc = new Bitmap(genislik, yukseklik);

            for (int y = 0; y < yukseklik; y++)
            {
                for (int x = 0; x < genislik; x++)
                {
                    Color piksel1 = resim1.GetPixel(x, y);
                    Color piksel2 = resim2.GetPixel(x, y);

                    int red = (int)Math.Min(255, piksel1.R * piksel2.R / 255.0);
                    int green = (int)Math.Min(255, piksel1.G * piksel2.G / 255.0);
                    int blue = (int)Math.Min(255, piksel1.B * piksel2.B / 255.0);

                    sonuc.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }

            return sonuc;
        }

        private void ImageSelectionForm_Load(object sender, EventArgs e)
        {

        }
    }
}
