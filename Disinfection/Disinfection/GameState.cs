using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Disinfection
{
    public partial class GameState : State
    {
        private List<Monster> Monsters;

        private int Syringes;

        private int MonstersAmount;

        private int Score;

        private int LeftOffset;//сколько отнять от координат экрана

        private Timer GameTimer;

        private Image GameImage;

        private List<Image> MonstersImages;

        private List<AdvButton> Buttons;

        private PictureBox BackgroundPictureBox;

        public GameState()
        {
            InitializeComponent();
            this.Size = new Size(640, 360);
            BackgroundPictureBox = new PictureBox();
            BackgroundPictureBox.MouseMove += BackgroundPictureBoxMouseMove;
            BackgroundPictureBox.Size = new Size(1920, 360);
            BackgroundPictureBox.Location = new Point(0, 0);
            BackgroundPictureBox.Cursor = LoadGameCursor();
            Controls.Add(BackgroundPictureBox);


            LoadImage();
        }

        private Cursor LoadGameCursor()
        {
            IconConverter ic = new IconConverter();
            var buffer = ic.ConvertTo(Properties.Resources.Aim, typeof(byte[])) as byte[];

            using (var m = new MemoryStream(buffer))
            {
                return new Cursor(m);
            }
        }

        public void BackgroundPictureBoxMouseMove(object sender, EventArgs e)
        {
            int PBCursorX = Cursor.Position.X - MainForm.MainFormObject.Location.X;
            //coords on this user control

            if ((PBCursorX > 0) && (PBCursorX < 105)) // in left rectangle
            {
                if (BackgroundPictureBox.Location.X != 0)
                    BackgroundPictureBox.Location = new Point
                            (BackgroundPictureBox.Location.X + 10, BackgroundPictureBox.Location.Y);
                LeftOffset += 10;

            } else if ((PBCursorX > 535) && (PBCursorX < 640)) // in right rectangle
            {
                if (BackgroundPictureBox.Location.X != -1280)
                    BackgroundPictureBox.Location = new Point
                            (BackgroundPictureBox.Location.X - 10, BackgroundPictureBox.Location.Y);
                LeftOffset -= 10;
            }
        }

        private void LoadImage()
        {
            BackgroundPictureBox.Image = Disinfection.Properties.Resources.BackgroundTemp;
        }



        public void OnTimerTick(object sender, EventArgs e)
        {

        }

        public override void StateDispose()
        {

        }

    }
}
