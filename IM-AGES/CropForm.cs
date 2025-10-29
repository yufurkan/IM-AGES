using System;
using System.Drawing;
using System.Windows.Forms;

namespace IM_AGES
{
    public partial class CropForm : Form
    {
        private Bitmap originalImage;
        public Bitmap CroppedImage { get; private set; }

        public CropForm(Bitmap image)
        {
            InitializeComponent();
            originalImage = image;
        }

        private void CropForm_Load(object sender, EventArgs e)
        {
            pictureBoxCrop.Image = originalImage;
        }

        private void pictureBoxCrop_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            cropArea = new Rectangle(e.X, e.Y, 0, 0);
        }

        private void pictureBoxCrop_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                cropArea.Width = e.X - cropArea.X;
                cropArea.Height = e.Y - cropArea.Y;
                pictureBoxCrop.Invalidate();
            }
        }

        private void pictureBoxCrop_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void pictureBoxCrop_Paint(object sender, PaintEventArgs e)
        {
            if (cropArea != Rectangle.Empty && cropArea.Width > 0 && cropArea.Height > 0)
            {
                e.Graphics.DrawRectangle(Pens.Red, cropArea);
            }
        }

        private void buttonCrop_Click(object sender, EventArgs e)
        {
            if (cropArea.Width > 0 && cropArea.Height > 0)
            {
                Bitmap sourceBitmap = new Bitmap(pictureBoxCrop.Image, pictureBoxCrop.Width, pictureBoxCrop.Height);
                CroppedImage = new Bitmap(cropArea.Width, cropArea.Height);

                using (Graphics g = Graphics.FromImage(CroppedImage))
                {
                    g.DrawImage(sourceBitmap, new Rectangle(0, 0, CroppedImage.Width, CroppedImage.Height),
                                cropArea,
                                GraphicsUnit.Pixel);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
