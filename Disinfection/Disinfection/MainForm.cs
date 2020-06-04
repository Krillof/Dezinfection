using Disinfection.Properties;
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

        public MainForm()
        {
            InitializeComponent();
            MainFormObject = this;

            this.Size = new Size(1137, 654);
            this.MinimumSize = new Size(1137, 654);
            this.MaximumSize = new Size(1137, 654);

            ChangeCurrentState(States.menu);
            this.Controls.Add(CurrentState);
            CurrentState.Location = new Point(0, 0);

            this.AllowTransparency = true;

            BackColor = Color.Black;

            this.Icon = Resources.GameIco;
        }

        public static void ChangeCurrentState(States changeToState)
        {
            CurrentState?.StateDispose();
            MainFormObject.Controls.Remove(CurrentState);
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
            MainFormObject.Controls.Add(CurrentState);
        }

        public enum States
        {
            menu,
            credits,
            game
        }
    }
}
