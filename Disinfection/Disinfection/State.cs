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
    public abstract partial class State : UserControl
    {
        public State()
        {
            InitializeComponent();
        }


        public abstract void StateDispose();
    }
}
