using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disinfection
{
    public partial class MainForm : Form
    {
        private static State CurrentState = null;

        public static MainForm MainFormObject;

        public MainForm()//Показываю как коммитить 2
        {
            InitializeComponent();
            MainFormObject = this;

            this.Size = new Size(652, 390);
            this.MinimumSize = new Size(652, 390);
            this.MaximumSize = new Size(652, 390);

            ChangeCurrentState(States.game);
            this.Controls.Add(CurrentState);
            CurrentState.Location = new Point(0, 0);

            BackColor = Color.Black;
        }

        public static void ChangeCurrentState(States changeToState)
        {
            CurrentState?.StateDispose();
            switch (changeToState)
            {
                case States.menu:
                    CurrentState = new MenuState();
                    break;
                case States.game:
                    CurrentState = new GameState();
                    break;
                case States.credits:
                    CurrentState = new CreditsState();
                    break;
            }
        }

        public enum States
        {
            menu,
            credits,
            game
        }
    }
}
