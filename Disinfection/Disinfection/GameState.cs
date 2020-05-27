using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public GameState()
        {
            InitializeComponent();
        }

        private void LoadImage()
        {

        }

        public void OnTimerTick(object sender, EventArgs e)
        {

        }

        public override void StateDispose()
        {

        }

    }
}
