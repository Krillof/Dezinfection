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
using System.Security.Policy;

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

        private Timer MonstTimer;

        private List<PictureBox> ListMonst;

        private Random Rnd;

        private Image GameImage;

        private List<AdvButton> Buttons;

        private PictureBox BackgroundPictureBox;

        private const int RightGameBorder = 1920;

        private const int BottomGameBorder = 360;

        private int Waiting;

        private Cursor GameCursor;

        public GameState()
        {
            InitializeComponent();
            Waiting = 0;

            this.Size = new Size(640, 360);
            BackgroundPictureBox = new PictureBox();
            BackgroundPictureBox.MouseMove += BackgroundPictureBoxMouseMove;
            BackgroundPictureBox.Size = new Size(1920, 360);
            BackgroundPictureBox.Location = new Point(0, 0);
            GameCursor = LoadGameCursor();
            BackgroundPictureBox.Cursor = GameCursor;
            Controls.Add(BackgroundPictureBox);

            ListMonst = new List<PictureBox>();

            Rnd = new Random();

            DoubleBuffered = true;//от мерцаний

            MonstTimer = new Timer();
            MonstTimer.Interval = 3;
            MonstTimer.Tick += new EventHandler(MonstTimerTick);
            MonstTimer.Start();

            LoadImage();
        }

        private void MonstTimerTick(object sender, EventArgs e)
        {
            Invalidate();

            Waiting++;

            if (Waiting == 300)
            {
                NewPB();
                Waiting = 0;
            }

            foreach (var monst in ListMonst)
            {
                MoveMonst(monst);
            }

        }

        private void NewPB()
        {
            if (ListMonst.Count == 20) return;

            int tmp = Rnd.Next(1, 3), X, Y;

            if (tmp == 1)
            {
                X = 0;

            }
            else
            {
                X = RightGameBorder - 75;

            }
            Y = Rnd.Next(0, BottomGameBorder - 44);


            PictureBox PictureBox123 = new PictureBox();
            PictureBox123.Location = new Point(X, Y);
            PictureBox123.Size = new System.Drawing.Size(55, 44);
            PictureBox123.BackColor = Color.Red;
            PictureBox123.MouseMove += BackgroundPictureBoxMouseMove;
            PictureBox123.MouseClick += ShootMonst;
            PictureBox123.Cursor = GameCursor;
            

            MonstInfo monstInfo = new MonstInfo();
            monstInfo.X = X;

            tmp = Rnd.Next(1, 3);

            if (tmp == 1)
            {
                monstInfo.deltX = -1;

            }
            else
            {
                monstInfo.deltX = 1;
            }
            tmp = Rnd.Next(1, 3);
            if (tmp == 1)
            {
                monstInfo.deltY = -1;

            }
            else
            {
                monstInfo.deltY = 1;
            }

            tmp = Rnd.Next(1, 3);
            monstInfo.deltX = tmp * monstInfo.deltX;


            tmp = Rnd.Next(1, 3);
            monstInfo.deltY = tmp * monstInfo.deltY;

            PictureBox123.Tag = monstInfo;

            ListMonst.Add(PictureBox123);
            Controls.Add(PictureBox123);
            PictureBox123.BringToFront();
        }

        private void ShootMonst(object sender, EventArgs e)
        {
            PictureBox monster = (PictureBox)sender;
            ListMonst.Remove(monster);
            Controls.Remove(monster);
            monster.Dispose();
        }

        public void MoveMonst(PictureBox monster)
        {

            MonstInfo monstInfo = (MonstInfo)monster.Tag;


            int newX = monstInfo.X + monstInfo.deltX + LeftOffset;
            int newY = monster.Top + monstInfo.deltY;



            int maxX = RightGameBorder  - 75;
            int maxY = BottomGameBorder - 64;


            if ((monstInfo.X + monstInfo.deltX >= maxX) || (monstInfo.X + monstInfo.deltX <= 0))
            {
                monstInfo.deltX = -monstInfo.deltX;

                newX = monstInfo.X + monstInfo.deltX + LeftOffset;
            }
            if ((newY >= maxY) || (newY <= 0))
            {
                monstInfo.deltY = -monstInfo.deltY;

                newY = monster.Top + monstInfo.deltY;
            }

            monster.Left = newX;
            monster.Top = newY;
            monstInfo.X = monstInfo.X + monstInfo.deltX;

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
                {
                    BackgroundPictureBox.Location = new Point
                            (BackgroundPictureBox.Location.X + 10, BackgroundPictureBox.Location.Y);
                    LeftOffset += 10;
                }

            } else if ((PBCursorX > 535) && (PBCursorX < 640)) // in right rectangle
            {
                if (BackgroundPictureBox.Location.X != -1280)
                {
                    BackgroundPictureBox.Location = new Point
                            (BackgroundPictureBox.Location.X - 10, BackgroundPictureBox.Location.Y);
                    LeftOffset -= 10;
                }
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

    public class MonstInfo
    {
        public int deltX = 0;
        public int deltY = 0;
        public int Type = 0;
        public int X = 0;
    }
}
