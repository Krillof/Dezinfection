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
using System.Media;
using Disinfection.Properties;
using System.Windows.Media;

namespace Disinfection
{
    public partial class GameState : State
    {

        private int LeftOffset;//сколько отнять от координат экрана

        private Timer GameTimer;

        private Timer MonstTimer;

        private List<PictureBox> ListMonst;

        private Random Rnd;

        private PictureBox BackgroundPictureBox;

        private const int RightGameBorder = 3340;

        private const int BottomGameBorder = 500;

        private int UpdateMusicCounter = 0;

        private int Waiting;

        private Cursor GameCursor;

        private GameInfo CurrentGameInfo;

        private Panel AmmoPanel;

        private Label ScoreLabel, EnemiesLeftLabel, TimeLabel, InfoLabel1, InfoLabel2, InfoLabel3;

        private bool isEnd = false;

        private int alreadySpawned = 0;

        private MediaPlayer GameMusic, JustShot, GoodShot, Shutter;

        public GameState()
        {
            InitializeComponent();
            Waiting = 0;

            this.Size = new Size(1137, 654);
            this.BackColor = System.Drawing.Color.LightBlue;

            BackgroundPictureBox = new PictureBox();
            BackgroundPictureBox.MouseMove += BackgroundPictureBoxMouseMove;
            BackgroundPictureBox.MouseClick += BackgroundPictureBox_MouseClick;
            LoadImage();
            BackgroundPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            BackgroundPictureBox.Size = new Size(3340, 621);
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

            AmmoPanel = new Panel();
            AmmoPanel.Size = new Size(400, 100);
            AmmoPanel.Location = new Point(700, 500);
            AmmoPanel.BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(AmmoPanel);
            AmmoPanel.BringToFront();

            ScoreLabel = new Label();
            ScoreLabel.Location = new Point(16, 550);
            ScoreLabel.Size = new Size(150, 40);
            ScoreLabel.BackColor = System.Drawing.Color.Transparent;
            ScoreLabel.ForeColor = System.Drawing.Color.DarkBlue;
            ScoreLabel.Font = new Font(System.Drawing.FontFamily.GenericSansSerif, 20);
            Controls.Add(ScoreLabel);
            ScoreLabel.BringToFront();

            EnemiesLeftLabel = new Label();
            EnemiesLeftLabel.Location = new Point(190, 550);
            EnemiesLeftLabel.Size = new Size(150, 40);
            EnemiesLeftLabel.BackColor = System.Drawing.Color.Transparent;
            EnemiesLeftLabel.ForeColor = System.Drawing.Color.DarkBlue;
            EnemiesLeftLabel.Font = new Font(System.Drawing.FontFamily.GenericSansSerif, 20);
            Controls.Add(EnemiesLeftLabel);
            EnemiesLeftLabel.BringToFront();

            TimeLabel = new Label();
            TimeLabel.Location = new Point(430, 550);
            TimeLabel.Size = new Size(150, 40);
            TimeLabel.BackColor = System.Drawing.Color.Transparent;
            TimeLabel.ForeColor = System.Drawing.Color.DarkBlue;
            TimeLabel.Font = new Font(System.Drawing.FontFamily.GenericSansSerif, 20);
            Controls.Add(TimeLabel);
            TimeLabel.BringToFront();

            InfoLabel1 = new Label();
            InfoLabel1.Text = "Очки";
            InfoLabel1.Size = new Size(150, 40);
            InfoLabel1.Location = new Point(16, 500);
            InfoLabel1.BackColor = System.Drawing.Color.Transparent;
            InfoLabel1.ForeColor = System.Drawing.Color.DarkBlue;
            InfoLabel1.Font = new Font(System.Drawing.FontFamily.GenericSansSerif, 15);
            Controls.Add(InfoLabel1);
            InfoLabel1.BringToFront();

            InfoLabel2 = new Label();
            InfoLabel2.Text = "Осталось врагов";
            InfoLabel2.Size = new Size(225, 40);
            InfoLabel2.Location = new Point(190, 500);
            InfoLabel2.BackColor = System.Drawing.Color.Transparent;
            InfoLabel2.ForeColor = System.Drawing.Color.DarkBlue;
            InfoLabel2.Font = new Font(System.Drawing.FontFamily.GenericSansSerif, 15);
            Controls.Add(InfoLabel2);
            InfoLabel2.BringToFront();

            InfoLabel3 = new Label();
            InfoLabel3.Text = "Осталось времени";
            InfoLabel3.Size = new Size(260, 40);
            InfoLabel3.Location = new Point(430, 500);
            InfoLabel3.BackColor = System.Drawing.Color.Transparent;
            InfoLabel3.ForeColor = System.Drawing.Color.DarkBlue;
            InfoLabel3.Font = new Font(System.Drawing.FontFamily.GenericSansSerif, 15);
            Controls.Add(InfoLabel3);
            InfoLabel3.BringToFront();

            CurrentGameInfo = new GameInfo(30, 10, AmmoPanel, ScoreLabel, EnemiesLeftLabel, TimeLabel, this);

            GameTimer = new Timer();
            GameTimer.Interval = 1000;
            GameTimer.Tick += OnTimerTick;
            GameTimer.Start();

            GameMusic = new MediaPlayer();
            GameMusic.Open(new Uri("Sounds\\GameMusic.mp3", UriKind.Relative));
            GameMusic.Play();

            JustShot = new MediaPlayer();

            GoodShot = new MediaPlayer();

            Shutter = new MediaPlayer();

        }

        private void BackgroundPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (CurrentGameInfo.isReloading) return;
            if (e.Button == MouseButtons.Right)
            {
                Shutter.Open(new Uri("Sounds\\Shutter.mp3", UriKind.Relative));
                Shutter.Play();
                CurrentGameInfo.rightClick();
            }
            if (e.Button == MouseButtons.Left)
            {
                JustShot.Open(new Uri("Sounds\\JustShot.mp3", UriKind.Relative));
                JustShot.Play();
                CurrentGameInfo.leftClick(true);
            }
        }

        private void MonstTimerTick(object sender, EventArgs e)
        {
            if (isEnd) return;

            Invalidate();

            Waiting++;

            if (Waiting % 50 == 0)
            foreach (var monster in ListMonst)
            {
                MonstInfo monstInfo = (MonstInfo)monster.Tag; 

                switch (monstInfo.Type)
                {
                    case 1:
                        if (monstInfo.PictureState)
                        {
                            monster.Image = Resources.вирус1_1;
                        }
                        else
                        {
                            monster.Image = Resources.вирус_1_2;
                        }

                        monstInfo.PictureState = !monstInfo.PictureState;
                        break;
                    case 2:
                        if (monstInfo.PictureState)
                        {
                            monster.Image = Resources.вирус_2_1;
                        }
                        else
                        {
                            monster.Image = Resources.вирус_2_2;
                        }

                        monstInfo.PictureState = !monstInfo.PictureState;
                        break;
                    case 3:
                        if (monstInfo.PictureState)
                        {
                            monster.Image = Resources.вирус_3_1;
                        }
                        else
                        {
                            monster.Image = Resources.вирус_3_1;
                        }

                        monstInfo.PictureState = !monstInfo.PictureState;
                        break;
                }
            }

            if (Waiting == 100)
            {
                NewPB();
                Waiting = 0;
            }

            foreach (var monst in ListMonst)
            {
                MoveMonst(monst);
            }

        }

        public void OnTimerTick(object sender, EventArgs e)
        {
            CurrentGameInfo.refleshTime();
            UpdateMusicCounter++;
            if (UpdateMusicCounter == 70)
            {
                GameMusic.Open(new Uri("Sounds\\GameMusic.mp3", UriKind.Relative));
                GameMusic.Play();
                UpdateMusicCounter = 0;
            }
        }

        private void NewPB()
        {
            if (ListMonst.Count == 20) return;
            if (CurrentGameInfo.enemyMax == alreadySpawned) return;

            int tmp = Rnd.Next(1, 3), X, Y;

            if (tmp == 1)
            {
                X = 0;

            }
            else
            {
                X = RightGameBorder - 75;

            }
            Y = Rnd.Next(0, BottomGameBorder - 60);

            int type = Rnd.Next(1, 4);

            PictureBox PictureBox123 = new PictureBox();
            PictureBox123.Location = new Point(X, Y);
            PictureBox123.BackColor = System.Drawing.Color.Transparent;
            switch (type)
            {
                case 1:
                    PictureBox123.Image = Disinfection.Properties.Resources.вирус1_1;
                    PictureBox123.Size = new System.Drawing.Size(42, 55);
                    break;
                case 2:
                    PictureBox123.Image = Disinfection.Properties.Resources.вирус_2_1;
                    PictureBox123.Size = new System.Drawing.Size(46, 47);
                    break;
                case 3:
                    PictureBox123.Image = Disinfection.Properties.Resources.вирус_3_1;
                    PictureBox123.Size = new System.Drawing.Size(40, 43);
                    break;
            }
            PictureBox123.MouseMove += BackgroundPictureBoxMouseMove;
            PictureBox123.MouseClick += ShootMonst;
            PictureBox123.Cursor = GameCursor;

            

            MonstInfo monstInfo = new MonstInfo();
            monstInfo.Type = type;

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

            tmp = Rnd.Next(1, 4);
            monstInfo.deltX = tmp * monstInfo.deltX;


            tmp = Rnd.Next(1, 4);
            monstInfo.deltY = tmp * monstInfo.deltY;

            PictureBox123.Tag = monstInfo;

            ListMonst.Add(PictureBox123);
            Controls.Add(PictureBox123);
            PictureBox123.Parent = BackgroundPictureBox;
            PictureBox123.BringToFront();

            alreadySpawned++;
        }

        private void ShootMonst(object sender, MouseEventArgs e)
        {
            if (isEnd) return;
            if (e.Button == MouseButtons.Right)
            {
                Shutter.Open(new Uri("Sounds\\Shutter.mp3", UriKind.Relative));
                Shutter.Play();
                CurrentGameInfo.rightClick();
            }
            if (CurrentGameInfo.isReloading || (CurrentGameInfo.quantityAmmo == 0)) return;
            if (e.Button == MouseButtons.Left)
            {
                Shutter.Open(new Uri("Sounds\\GoodShot.mp3", UriKind.Relative));
                Shutter.Play();
                CurrentGameInfo.leftClick();
                PictureBox monster = (PictureBox)sender;
                ListMonst.Remove(monster);
                Controls.Remove(monster);
                monster.Dispose();
            }
        }

        public void MoveMonst(PictureBox monster)
        {

            MonstInfo monstInfo = (MonstInfo)monster.Tag;


            int newX = monster.Left + monstInfo.deltX;
            int newY = monster.Top + monstInfo.deltY;

            int maxX = RightGameBorder  - monster.Size.Width;
            int maxY = BottomGameBorder - monster.Size.Height;

            //foreach (var monst in ListMonst)
            //{
            //    if ((monst.Left <= newX || monst.Right >= newX) &&
            //        (monst.Top <= monster.Bottom || monst.Bottom >= monster.Top))
            //    {
            //        monstInfo.deltX = -monstInfo.deltX;
            //        newX = monster.Left + monstInfo.deltX;
            //    }

            //    if ((monst.Left <= monster.Right || monst.Right >= monster.Left) &&
            //        (monst.Top <= newY || monst.Bottom >= newY))
            //    {
            //        monstInfo.deltY = -monstInfo.deltY;
            //        newY = monster.Top + monstInfo.deltY;
            //    }
            //}

            if ((newX >= maxX) || (newX <= 0))
            {
                monstInfo.deltX = -monstInfo.deltX;
                newX = monster.Left + monstInfo.deltX;
            }

            if ((newY >= maxY) || (newY <= 0))
            {
                monstInfo.deltY = -monstInfo.deltY;
                newY = monster.Top + monstInfo.deltY;
            }

            monster.Left = newX;
            monster.Top = newY;

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
            if (isEnd) return;

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
            } else if ((PBCursorX > 947) && (PBCursorX < 1137)) // in right rectangle
            {
                if (BackgroundPictureBox.Location.X != -2220)
                {
                    BackgroundPictureBox.Location = new Point
                            (BackgroundPictureBox.Location.X - 10, BackgroundPictureBox.Location.Y);
                    LeftOffset -= 10;
                }
            }
        }

        private void LoadImage()
        {
            BackgroundPictureBox.Image = Disinfection.Properties.Resources.fon_pp;
        }

        public override void StateDispose()
        {
            GameMusic.Close();
            GameTimer.Stop();
            MonstTimer.Stop();
        }

        private void ShowEnd(bool isWon)
        {
            PictureBox CenterPicture = new PictureBox();
            CenterPicture.BackColor = System.Drawing.Color.Transparent;
            CenterPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            CenterPicture.Location = new System.Drawing.Point(100 - LeftOffset, 10);
            CenterPicture.Name = "CenterPicture";
            CenterPicture.Size = new System.Drawing.Size(900, 350);
            if (isWon)
            {
                CenterPicture.Image = Resources.Won;
            } else
            {
                CenterPicture.Image = Resources.Lose;
            }
            BackgroundPictureBox.Controls.Add(CenterPicture);
            CenterPicture.BringToFront();

            //button
            PictureBox ExitButton = new PictureBox();
            ExitButton.BackColor = System.Drawing.Color.Transparent;
            ExitButton.Location = new System.Drawing.Point(435 - LeftOffset, 400);
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(263, 83);
            ExitButton.Image = Resources.MainMenu;
            ExitButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            ExitButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExitButton_MouseClick);
            ExitButton.MouseEnter += new System.EventHandler(this.ExitButton_MouseEnter);
            ExitButton.MouseLeave += new System.EventHandler(this.ExitButton_MouseLeave);
            BackgroundPictureBox.Controls.Add(ExitButton);
            ExitButton.BringToFront();

            BackgroundPictureBox.Invalidate();
        }

        private void ExitButton_MouseEnter(object sender, EventArgs e)
        {
            PictureBox ExitButton = (PictureBox)sender;
            ExitButton.Image = Resources.MainMenu1;
        }
        private void ExitButton_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox ExitButton = (PictureBox)sender;
            ExitButton.Image = Resources.MainMenu2;
            MainForm.ChangeCurrentState(MainForm.States.menu);
        }
        private void ExitButton_MouseLeave(object sender, EventArgs e)
        {
            PictureBox ExitButton = (PictureBox)sender;
            ExitButton.Image = Resources.MainMenu;
        }

        public void StopGameLose()
        {
            if (!isEnd)
            {
                isEnd = true;
                ShowEnd(false);
            }
        }

        public void StopGameWon()
        {
            if (!isEnd)
            {
                isEnd = true;
                ShowEnd(true);
            }
        }
    }

    class GameInfo
    {
        public int quantityAmmo { get; private set; }
        public int score { get; private set; }
        public int enemyLeft { get; private set; }

        public int enemyMax { get; private set; }

        public int secLeft { get; private set; }
        public int ct { get; private set; }
        public bool isReloading { get; private set; }

        public Panel panelAmmo { get; private set; }
        public Label labelEnemyLeft { get; private set; }
        public Label labelTimeLeft { get; private set; }
        public Label labelScore { get; private set; }

        public List<Image> images = null;

        public GameState CurState;

        public GameInfo(int secLeft, int enemyLeft, Panel panelAmmo, Label labelScore, Label labelEnemyLeft, Label labelTimeLeft, GameState curState)
        {
            CurState = curState;
            quantityAmmo = 5;
            score = 0;
            ct = 0;
            isReloading = false;

            this.secLeft = secLeft;
            this.enemyLeft = enemyLeft;
            this.enemyMax = enemyLeft;

            this.panelAmmo = panelAmmo;
            this.labelEnemyLeft = labelEnemyLeft;
            this.labelTimeLeft = labelTimeLeft;
            this.labelScore = labelScore;

            images = new List<Image>();
            images.Add(Resources.ammo0);
            images.Add(Resources.ammo1);
            images.Add(Resources.ammo2);
            images.Add(Resources.ammo3);
            images.Add(Resources.ammo4);
            images.Add(Resources.ammo5);
            images.Add(Resources.ammo6);
            images.Add(Resources.ammo7);
            images.Add(Resources.ammo8);

            panelAmmo.BackgroundImage = images[quantityAmmo = 5];
            this.labelEnemyLeft.Text = enemyLeft.ToString();
            this.labelTimeLeft.Text = getTimeLeftString();
            this.labelScore.Text = score.ToString();
        }

        public void refleshTime()
        {
            labelTimeLeft.Text = getTimeLeftString();
            if (secLeft != 0) secLeft--;
            if (secLeft == 0)
            {
                CurState.StopGameLose();
            }
        }

        public string getTimeLeftString()
        {
            string extraZero = "";
            if (secLeft%60 <= 9) extraZero = "0";
            return $"{secLeft / 60}:{extraZero}{secLeft % 60}";
        }

        public void leftClick(bool isMissed = false)
        {
            if ((!isReloading) && quantityAmmo > 0)
                if (isMissed)
                {
                    panelAmmo.BackgroundImage = images[--quantityAmmo];
                }
                else
                {
                    panelAmmo.BackgroundImage = images[--quantityAmmo];
                    labelScore.Text = (score += 100).ToString();
                    labelEnemyLeft.Text = (--enemyLeft).ToString();

                    if (enemyLeft == 0) CurState.StopGameWon();
                }
        }

        public void rightClick()
        {
            Task.Run(async () =>
            {
                isReloading = true;
                for (int i = 6; i <= 8; i++)
                {
                    panelAmmo.BackgroundImage = images[i];
                    await Task.Delay(800);
                }
                panelAmmo.BackgroundImage = images[quantityAmmo = 5];
                isReloading = false;
            });
        }
    }

    public class MonstInfo
    {
        public int deltX = 0;
        public int deltY = 0;
        public int Type = 0;
        public bool PictureState = false;
    }
}
