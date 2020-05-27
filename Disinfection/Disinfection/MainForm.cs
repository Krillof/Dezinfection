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
        private static State CurrentState;

        public MainForm()
        {
            InitializeComponent();

            
        }

        public static void ChangeCurrentState(States changeToState)
        {
            CurrentState.StateDispose();
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
