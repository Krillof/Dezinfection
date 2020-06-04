using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Disinfection.Properties;
using System.Media;
using System.IO;
using System.Windows.Media;
using System.Diagnostics;

namespace Disinfection
{
    public partial class CreditsState : State
    {

        int timerValue = 0;
        Thread t;
        PictureBox pictureBox1, pictureBox2, pictureBox3, pictureBox4;
        System.Windows.Forms.Timer timer1;
        object monitor = new object();
        int pb1Height = 0;
        MediaPlayer CreditsMusic;
        System.Windows.Forms.Timer MusicTimer;
        public CreditsState()
        {
            InitializeComponent();
            DoubleBuffered = true;

            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();

            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = Resources.fon;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1007, 521);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(85, 94);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(795, 206);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Location = new System.Drawing.Point(883, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(121, 45);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            this.pictureBox3.MouseEnter += new System.EventHandler(this.pictureBox3_MouseEnter);
            this.pictureBox3.MouseLeave += new System.EventHandler(this.pictureBox3_MouseLeave);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Location = new System.Drawing.Point(85, 306);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(795, 206);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // UserControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "UserControl2";
            this.Size = new System.Drawing.Size(1007, 521);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);


            pictureBox2.Location = new Point(-300, -300);
            timer1.Interval = 100;
            timer1.Start();
            this.Dock = DockStyle.Fill;
            this.Cursor = LoadCreditsCursor();
            pictureBox3.Parent = pictureBox1;
            pictureBox3.BackColor = System.Drawing.Color.Transparent;
            pictureBox3.Image = Resources.MainMenu;

            pictureBox2.BringToFront();

            pictureBox2.Parent = pictureBox1;
            pb1Height = pictureBox1.Height;

            CreditsMusic = new MediaPlayer();
            CreditsMusic.Open(new Uri("Sounds\\CreditsMusic.mp3", UriKind.Relative));
            CreditsMusic.Play();

            MusicTimer = new System.Windows.Forms.Timer();
            MusicTimer.Tick += MusicTimer_Tick;
            MusicTimer.Interval = 60000;
            MusicTimer.Start();
        }

        private void MusicTimer_Tick(object sender, EventArgs e)
        {
            CreditsMusic.Open(new Uri("Sounds\\CreditsMusic.mp3", UriKind.Relative));
            CreditsMusic.Play();
        }

        private Cursor LoadCreditsCursor()
        {
            IconConverter ic = new IconConverter();
            var buffer = ic.ConvertTo(Properties.Resources.cursor, typeof(byte[])) as byte[];

            using (var m = new MemoryStream(buffer))
            {
                return new Cursor(m);
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            timerValue++;
            if (timerValue > 5)
            {
                timer1.Stop();
                t = new Thread(new ThreadStart(Show_Titr));
                t.IsBackground = true;
                t.Start();



            }
        }

        void Move_One_Titr(object y)
        {
            pictureBox2.Location = new Point(pb1Height / 10, (int)y);
        }

        delegate void Del(object y);

        void Show_Titr()
        {

            Image[] mas = { Resources._1t, Resources._2t, Resources._3t, Resources._4t, Resources._5t, Resources._6t,
                Resources._7t, Resources._8t, Resources._9t, Resources._10t};
            List<Image> list = new List<Image>(mas);
            lock (monitor)
            {
                pictureBox1.BackColor = System.Drawing.Color.Transparent;
                pictureBox2.BackColor = System.Drawing.Color.Transparent;
            }
            foreach (var el in list)
            {
                lock (monitor)
                {
                    pictureBox2.Image = el;
                }


                for (int y = (int)(pictureBox1.Height * 1.3); y >= -1 * pictureBox1.Height / 10 * 3; y -= 5)
                {
                    Thread.Sleep(50);
                    lock (monitor)
                    {
                        BeginInvoke(new Del(Move_One_Titr), new object[] { y });
                        
                    }
                    this.Invalidate();
                }

            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
            pictureBox3.Image = Resources.MainMenu2;
            MainForm.ChangeCurrentState(MainForm.States.menu);
            t.Abort();
            this.Dispose();

        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Image = Resources.MainMenu1;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = Resources.MainMenu;
        }

        private void LoadImage()
        {

        }

        public override void StateDispose()
        {
            CreditsMusic.Close();
            MusicTimer.Stop();
        }
    }
}
