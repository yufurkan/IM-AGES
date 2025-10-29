namespace IM_AGES
{
    partial class CropForm
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pictureBoxCrop;
        private Button buttonCrop;
        private Rectangle cropArea;
        private bool isMouseDown;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBoxCrop = new PictureBox();
            buttonCrop = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCrop).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxCrop
            // 
            pictureBoxCrop.Dock = DockStyle.Fill;
            pictureBoxCrop.Location = new Point(0, 0);
            pictureBoxCrop.Margin = new Padding(4, 3, 4, 3);
            pictureBoxCrop.Name = "pictureBoxCrop";
            pictureBoxCrop.Size = new Size(933, 484);
            pictureBoxCrop.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxCrop.TabIndex = 0;
            pictureBoxCrop.TabStop = false;
            pictureBoxCrop.Paint += pictureBoxCrop_Paint;
            pictureBoxCrop.MouseDown += pictureBoxCrop_MouseDown;
            pictureBoxCrop.MouseMove += pictureBoxCrop_MouseMove;
            pictureBoxCrop.MouseUp += pictureBoxCrop_MouseUp;
            // 
            // buttonCrop
            // 
            buttonCrop.Dock = DockStyle.Bottom;
            buttonCrop.Location = new Point(0, 484);
            buttonCrop.Margin = new Padding(4, 3, 4, 3);
            buttonCrop.Name = "buttonCrop";
            buttonCrop.Size = new Size(933, 35);
            buttonCrop.TabIndex = 1;
            buttonCrop.Text = "Kırp";
            buttonCrop.UseVisualStyleBackColor = true;
            buttonCrop.Click += buttonCrop_Click;
            // 
            // CropForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 519);
            Controls.Add(pictureBoxCrop);
            Controls.Add(buttonCrop);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            Name = "CropForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Kırpma";
            Load += CropForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxCrop).EndInit();
            ResumeLayout(false);
        }
    }
}
