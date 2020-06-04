using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using Disinfection.Properties;
using System.IO;
using System.Threading;
using System.Windows.Media;

namespace Disinfection
{
    public partial class MenuState : State
    {

        System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer MusicTimer = new System.Windows.Forms.Timer();
        PictureBox Picture1 = new PictureBox();
        PictureBox Picture2 = new PictureBox();
        PictureBox Picture3 = new PictureBox();
        bool a = true;
        double number = 0;
        PictureBox pictureBox1, pictureBox2, pictureBox3, pictureBox4;

        MediaPlayer MenuMusic, ButtonHover, ButtonChoice;

        public MenuState()
        {
            InitializeComponent();

            DoubleBuffered = true;

            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = Resources.fon;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1137, 654);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Location = new System.Drawing.Point(435, 230);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(263, 83);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseClick);
            this.pictureBox2.MouseEnter += new System.EventHandler(this.pictureBox2_MouseEnter);
            this.pictureBox2.MouseLeave += new System.EventHandler(this.pictureBox2_MouseLeave);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Location = new System.Drawing.Point(435, 331);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(263, 83);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseClick);
            this.pictureBox3.MouseEnter += new System.EventHandler(this.pictureBox3_MouseEnter);
            this.pictureBox3.MouseLeave += new System.EventHandler(this.pictureBox3_MouseLeave);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Location = new System.Drawing.Point(435, 448);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(263, 83);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseClick);
            this.pictureBox4.MouseEnter += new System.EventHandler(this.pictureBox4_MouseEnter);
            this.pictureBox4.MouseLeave += new System.EventHandler(this.pictureBox4_MouseLeave);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(1137, 654);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

            pictureBox2.Image = Resources.Start;
            pictureBox3.Image = Resources.Titri;
            pictureBox4.Image = Resources.exit;

            MenuMusic = new MediaPlayer();
            MenuMusic.Open(new Uri("Sounds\\MainMenuMusic.mp3",UriKind.Relative));
            MenuMusic.Play();


            ButtonHover = new MediaPlayer();
            ButtonChoice = new MediaPlayer();
          

            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            pictureBox2.Parent = pictureBox1;
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
            pictureBox3.Parent = pictureBox1;
            pictureBox3.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.Parent = pictureBox1;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.Cursor = LoadMenuCursor();

            Picture1.Image = Resources.virus1_1;
            Picture2.Image = Resources.virus1_2;
            Picture3.Image = Resources.virus1_3;

            pictureBox1.Controls.Add(Picture1);
            pictureBox1.Controls.Add(Picture2);
            pictureBox1.Controls.Add(Picture3);

            Timer.Interval = 10;
            Timer.Tick += tm1_Tick;
            Timer.Start();

            MusicTimer = new System.Windows.Forms.Timer();
            MusicTimer.Tick += MusicTimer_Tick;
            MusicTimer.Interval = 70000;
            MusicTimer.Start();
        }

        private void MusicTimer_Tick(object sender, EventArgs e)
        {
            MenuMusic.Open(new Uri("Sounds\\MainMenuMusic.mp3", UriKind.Relative));
            MenuMusic.Play();
        }

        private Cursor LoadMenuCursor()
        {
            IconConverter ic = new IconConverter();
            var buffer = ic.ConvertTo(Properties.Resources.cursor, typeof(byte[])) as byte[];

            using (var m = new MemoryStream(buffer))
            {
                return new Cursor(m);
            }
        }

        private void tm1_Tick(object sender, EventArgs e)
        {
            if (a)
            {
                number++;
            }
            else
            {
                number--;
            }
            Picture1.Location = new Point(350, 500 - Convert.ToInt32(150 * (1 + Math.Cos(Math.PI * number / 400))));
            Picture2.Location = new Point(800, 200 + Convert.ToInt32(150 * (1 + Math.Cos(Math.PI * number / 300))));
            Picture3.Location = new Point(250, 200 + Convert.ToInt32(150 * (1 + Math.Cos(Math.PI * number / 350))));

            if (number == 200)
            {
                a = false;
            }
            if (number == 0)
            {
                a = true;
            }
        }



        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.Image = Resources.Start1;
            ButtonHover.Open(new Uri("Sounds\\MenuHover.mp3", UriKind.Relative));
            ButtonHover.Play();
        }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox2.Image = Resources.Start2;
            ButtonChoice.Open(new Uri("Sounds\\MenuChoice.mp3", UriKind.Relative));
            ButtonChoice.Play();
            MainForm.ChangeCurrentState(MainForm.States.game);
        }
        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Resources.Start;

        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Image = Resources.Titri1;
            ButtonHover.Open(new Uri("Sounds\\MenuHover.mp3", UriKind.Relative));
            ButtonHover.Play();
        }
        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox3.Image = Resources.Titri2;
            ButtonChoice.Open(new Uri("Sounds\\MenuChoice.mp3", UriKind.Relative));
            ButtonChoice.Play();
            MainForm.ChangeCurrentState(MainForm.States.credits);
        }
        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = Resources.Titri;
        }
        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.Image = Resources.exit1;
            ButtonHover.Open(new Uri("Sounds\\MenuHover.mp3", UriKind.Relative));
            ButtonHover.Play();
        }

        private void pictureBox4_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox4.Image = Resources.exit2;
            ButtonChoice.Open(new Uri("Sounds\\MenuChoice.mp3", UriKind.Relative));
            ButtonChoice.Play();
            Application.Exit();
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = Resources.exit;
        }

        private void LoadImage()
        {

        }

        public override void StateDispose()
        {
            MenuMusic.Close();
            MusicTimer.Stop();
        }
    }
}
