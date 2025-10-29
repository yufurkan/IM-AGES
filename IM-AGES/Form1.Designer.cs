namespace IM_AGES
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel1 = new Panel();
            label6 = new Label();
            panel2 = new Panel();
            label5 = new Label();
            label7 = new Label();
            pictureBox1 = new PictureBox();
            flowLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 36F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            label1.ForeColor = Color.DarkSlateBlue;
            label1.Location = new Point(421, 12);
            label1.Name = "label1";
            label1.Size = new Size(279, 81);
            label1.TabIndex = 0;
            label1.Text = "IM-AGES";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.Cyan;
            label2.Location = new Point(274, 93);
            label2.Name = "label2";
            label2.Size = new Size(602, 31);
            label2.TabIndex = 1;
            label2.Text = "Interactive Multimedia Artistic Graphics Editing Studio";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.Cyan;
            label3.Location = new Point(34, 172);
            label3.Name = "label3";
            label3.Size = new Size(172, 31);
            label3.TabIndex = 2;
            label3.Text = "Son Çalışmalar";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            label4.ForeColor = Color.Fuchsia;
            label4.Location = new Point(3, 0);
            label4.Name = "label4";
            label4.Size = new Size(378, 62);
            label4.TabIndex = 5;
            label4.Text = "Görünüşe göre yeni bir başlangıç. \r\nHadi bir proje oluşturalım";
            label4.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.Transparent;
            flowLayoutPanel1.Controls.Add(label4);
            flowLayoutPanel1.Location = new Point(34, 224);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(434, 373);
            flowLayoutPanel1.TabIndex = 6;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(label6);
            panel1.Location = new Point(885, 224);
            panel1.Name = "panel1";
            panel1.Size = new Size(217, 69);
            panel1.TabIndex = 11;
            panel1.Paint += panel1_Paint;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            label6.ForeColor = Color.Cyan;
            label6.Location = new Point(46, 18);
            label6.Name = "label6";
            label6.Size = new Size(121, 31);
            label6.TabIndex = 9;
            label6.Text = "Yeni Proje";
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(label5);
            panel2.Location = new Point(885, 340);
            panel2.Name = "panel2";
            panel2.Size = new Size(217, 69);
            panel2.TabIndex = 12;
            panel2.Paint += panel2_Paint;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            label5.ForeColor = Color.Cyan;
            label5.Location = new Point(55, 19);
            label5.Name = "label5";
            label5.Size = new Size(112, 31);
            label5.TabIndex = 9;
            label5.Text = "Dosya Aç";
            label5.Click += label5_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            label7.ForeColor = Color.Red;
            label7.Location = new Point(787, 572);
            label7.Name = "label7";
            label7.Size = new Size(233, 25);
            label7.TabIndex = 13;
            label7.Text = "Powered by Still Undercover";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(1056, 548);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(46, 49);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 14;
            pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1145, 657);
            Controls.Add(pictureBox1);
            Controls.Add(label7);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hoşgeldiniz";
            Load += Form1_Load;
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
      
        private Label label4;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
        private Label label6;
        private Panel panel2;
        private Label label5;
        private Label label7;
        private PictureBox pictureBox1;
    }
}
