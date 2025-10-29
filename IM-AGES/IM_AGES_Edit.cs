using System;
using System.Drawing;
using System.Windows.Forms;

namespace IM_AGES    //Eklediğiniz kodlara açıklama satırları koyun Sonuna baş harfinizi ekleyin ---------------------------------------   !!!!<<<<<<<<<<<<<<<<<<<<<<<<---HEYYYY
{
    public partial class IM_AGES_Edit : Form
    {
        private string orjinalResimYolu;
        private Bitmap orijinalResim;  //burası önemli 3 resim var biri ekranda gösterdiğimiz ve diğeri asıl işlemleri yapacağımız ve ilk halini kontrol etmek için kullandığımız-F
        private Bitmap gösterilenResim;//editlenmiş ve gösterilen üzerinde aynı işemleri yapacaksınız fark zomm yapabilmek için
        private Bitmap editlenmişResim; //gösterilen resimi biraz bozuyoruz pixel kayması yaşayabilir kaydederken onu kullanmıyacağız

        double a = 1.0;
        double b = 1.0;
        double k= 1.0;
        float yatay = 1;
        float dikey = 1;




        public IM_AGES_Edit(string ilk)
        {
            orjinalResimYolu = ilk;
            InitializeComponent();
            ResmiYükle(orjinalResimYolu);
            Trackbarİşlemi();
            // Eğer resim boyutu 1280x720 boyutlarından büyükse Resimi sığdırmak için burası, öbür türlü resim boyutu işleri zorluyo-F

            a = 1.0;
            b = 1.0;

            if (gösterilenResim.Width > 1280 || gösterilenResim.Height > 720)
            {
                a = 1280.0 / gösterilenResim.Width;
                b = 720.0 / gösterilenResim.Height;
            }
            k = Math.Max(a, b);
            if (a == b) k = a;
            //k değerini 1 den küçük bir değer olarak atıyoruz ki resimi istediğimiz boyutta başlatalım-F
        }

        //Resime Hem Zoom hem sürükleme işlemi yapbilmek için resim aoutscrool=True özellikli bir panelin içindeki autosize özellikli bir pictureboxta. bu özellikleri değiştirmeyin-F
        private void IM_AGES_Edit_Load(object sender, EventArgs e)
        {
            EtkinlikEkle(orjinalResimYolu);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        private void Trackbarİşlemi()
        {
  
            trackBar1.UseWaitCursor = false;
          
            this.DoubleBuffered = false;
        }
        Image ZoomPicture(Image img, Size size)
        {
            //10x zoom özelliği düşünüyorum ancak büyük resimlerde çok zorlnıyo-F
            //k değerini küçük resimler için 1 den büyük yapabiliriz bu sayede küçük resimlerde tam boyutla hizalanır-F

            //burada gpu ile zoom boyutlarını ayarlayan işlevler-F
            Bitmap bmp = new Bitmap(img, Convert.ToInt32(img.Width*k * size.Width), Convert.ToInt32(img.Height*k * size.Height));//k
            Graphics gpu = Graphics.FromImage(bmp);
            gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }

        public void ResmiYükle(string orjinalResimYolu)
        {
            try { 
            Bitmap bm = new Bitmap(orjinalResimYolu);
            gösterilenResim = bm.Clone() as Bitmap;
            orijinalResim   = bm.Clone() as Bitmap;
            editlenmişResim = bm.Clone() as Bitmap;
            

            pictureBox2.Image = ZoomPicture(gösterilenResim, new Size(trackBar1.Value, trackBar1.Value));
            }catch(Exception ex)
            {
                MessageBox.Show("HATA OLUŞTU : " +ex);
                this.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)//yakınlaştırma yaptığımızda tetikleniyor
        {
            yatay = PaneliYatayAl(panel1);
            dikey = PaneliDikeyAl(panel1);// zoomu istdiğimiz bölgeye yapmak için-F

            if (trackBar1.Value != 0)
            {
                pictureBox2.Image = null;
                pictureBox2.Image = ZoomPicture(gösterilenResim, new Size(trackBar1.Value, trackBar1.Value));
            }
            KaydırmaOrantısı(panel1, yatay, dikey);
            panel1.Invalidate();
        }
        
        private void IM_AGES_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            orijinalResim?.Dispose();
            gösterilenResim?.Dispose();
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

        //Panel işleri kafanızı karıştırabilir resimi hem haraket ettirip hem zoomlamak zordu-F
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
            panel1.AutoScrollPosition = new Point( (int)(maxHorizontalScroll * yatayAl),((int)(maxVerticalScroll * dikeyAl)));//Panel scrollarının sıfırlanmasıyla ilgili bir hatayı düzeltir-F
            //bu bir mühendislik harikası
        }




    }
}
