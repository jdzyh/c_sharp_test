using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lifegame
{
    public partial class WorldForm : Form
    {
        public WorldForm() {
            InitializeComponent();
            //
        }

        private void timer1_Tick(object sender, EventArgs e) {

            map.Invalidate();
        }
    }
}
