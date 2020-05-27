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
    public partial class CreditsState : State
    {
        private Image CreditsImage;

        private List<AdvButton> Buttons;


        public CreditsState()
        {
            InitializeComponent();
        }

        private void LoadImage()
        {

        }

        public override void StateDispose()
        {
            
        }
    }
}
