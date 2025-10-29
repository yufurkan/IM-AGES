using System.Net.NetworkInformation;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace IM_AGES
{
    //Eklediğiniz kodlara açıklama satırları koyun Sonuna baş harfinizi ekleyin ---------------------------------------   !!!!<<<<<<<<<<<<<<<<<<<<<<<<---HEYYYY
    public partial class Form1 : Form
    {



        private const string dosyaAdi = "Etkinlik.txt";
        private string dosyaYolu = Path.Combine(Application.StartupPath, dosyaAdi);//dosyanın yeri her bilgisayar için farklı olabilir bu yüzden proje dizinini kullanıyoruz-F
        public Form1()
        {
            InitializeComponent();
            EtkinlikDosyasiVarMi();
            VerileriDosyadanOku();
            this.DoubleBuffered = true;

        }
        private void EtkinlikDosyasiVarMi()//
        {

            if (!File.Exists(dosyaYolu))
            {

                File.WriteAllText(dosyaYolu, "");
            }
        }

        private void VerileriDosyadanOku()
        {

            string[] satirlar = File.ReadAllLines(dosyaYolu);
            if (satirlar.Count() == 0) label4.Visible = true;
            foreach (string satir in satirlar.Reverse()) // Son yapılan işlem en üste gelecek şekilde sıralama
            {
                string[] parcalar = satir.Split('$');
                if (parcalar.Length >= 2)
                {
                    string dosyaAdi = Path.GetFileNameWithoutExtension(parcalar[0]);
                    string tarih = parcalar[1];

                    Label dosyaAdiLabel = new Label();
                    dosyaAdiLabel.Text = dosyaAdi;
                    dosyaAdiLabel.MaximumSize = new Size(400, 500);
                    dosyaAdiLabel.Width = 400;
                    dosyaAdiLabel.Margin = new Padding(4);
                    dosyaAdiLabel.ForeColor = Color.Blue;
                    dosyaAdiLabel.Font = new Font("Arial", 14, FontStyle.Bold);
                    dosyaAdiLabel.Cursor = Cursors.Hand; // Üzerine gelindiğinde el işareti görünsün
                    dosyaAdiLabel.Click += (sender, e) =>
                    {
                        if (DosyaVarMı(parcalar[0]))
                        {
                            string dosyaYolu = parcalar[0];
                            Ana_Sayfa images = new Ana_Sayfa(dosyaYolu);
                            images.ShowDialog();
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("Dosya taşınmış yada silinmiş olabilir. Geçmişten kaldırmak ister misiniz?", "Dosya Bulunamadı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                try
                                {
                                    try
                                    {
                                        // Listeden silme işlemi-F
                                        File.WriteAllLines(dosyaYolu, satirlar.Where(s => s != satir).ToArray());
                                        MessageBox.Show("Dosya geçmişten kaldırıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        flowLayoutPanel1.Controls.Clear(); // Yeniden yüklemek için paneli temizle
                                        VerileriDosyadanOku(); // Verileri tekrar yükle
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Dosya kaldırılırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Dosya silinirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    };
                    flowLayoutPanel1.Controls.Add(dosyaAdiLabel);

                    Label tarihLabel = new Label();
                    tarihLabel.Text = tarih;
                    tarihLabel.MaximumSize = new Size(400, 20);
                    tarihLabel.Width = 400;
                    tarihLabel.Margin = new Padding(0, 0, 0, 20); // Altta bir boşluk bırak
                    tarihLabel.ForeColor = Color.Blue;
                    tarihLabel.Font = new Font("Arial", 10, FontStyle.Italic);
                    flowLayoutPanel1.Controls.Add(tarihLabel);
                }
            }
        }

        public bool DosyaVarMı(string yol)
        {
            string[] uzantılar = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff" };

            if (File.Exists(yol))
            {
                string uzantısı = Path.GetExtension(yol).ToLower();//dosya uzantısı-F
                foreach (string uzanti in uzantılar)if (uzantısı == uzanti) return true;//uzantı doğru mu-F   
            }

            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {//butonun transparent özelliği yok o yüzden groupbox kullandım-F
            panel1.BackColor = Color.FromArgb(130, Color.Gray);
            panel2.BackColor = Color.FromArgb(130, Color.Gray);
            panel1.Click += Panel_Click;
            panel2.Click += Panel_Click;
            panel2.MouseEnter += Panel_MouseEnter;
            panel1.MouseLeave += Panel_MouseLeave;
            panel2.MouseLeave += Panel_MouseLeave;
            panel1.MouseEnter += Panel_MouseEnter;
            label6.MouseEnter += (sender, e) => panel1.BackColor = Color.FromArgb(80, Color.Gray);
            label5.MouseEnter += (sender, e) => panel2.BackColor = Color.FromArgb(80, Color.Gray);

            string resimAdi = "uc.jpeg";
            string resimYolu = Path.Combine(Application.StartupPath, resimAdi);
            pictureBox1.Image = Image.FromFile(resimYolu);

            label6.Click += Panel_Click;
            label5.Click += Panel_Click;

        }
        private void Panel_MouseEnter(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BackColor = Color.FromArgb(80, Color.Gray);


        }
        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            panel.BackColor = Color.FromArgb(130, Color.Gray);
        }
        private void Panel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.Title = "Resim Seç";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Ana_Sayfa images = new Ana_Sayfa(openFileDialog.FileName);
                images.ShowDialog();
            }
        }



        private void InitializeFlowLayout()
        {


        }
        private void label_ClickOP(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
/*         Deneme amaçlı-F
             * Dictionary<string, string> fileEntries = new Dictionary<string, string>();
            fileEntries.Add(@"C:\Photos\photo1.jpg", "Photo 1");
            fileEntries.Add(@"C:\Photos\photo2.jpg", "Photo 2");
            fileEntries.Add(@"C:\Photos\photo3.jpg", "Photo 3");
            fileEntries.Add(@"C:\Photos\photcfgo3.jpg", "Photo 4");
            fileEntries.Add(@"C:\Photos\photcvo3.jpg", "Photo 36");
            fileEntries.Add(@"C:\Photos\photodg3.jpg", "Photo 35");
            fileEntries.Add(@"C:\Photos\photdggo3.jpg", "Photo 6");
            fileEntries.Add(@"C:\Photos\phocvto3.jpg", "Photo 7");
            fileEntries.Add(@"C:\Photos\photfgo3.jpg", "Photo 8");
            fileEntries.Add(@"C:\Photos\phocdgvto3.jpg", "Photo 39");
            InitializeFlowLayout();
            LoadFileEntries(fileEntries);
            */