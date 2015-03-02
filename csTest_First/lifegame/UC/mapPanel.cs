using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lifegame
{
    public partial class mapPanel : Panel
    {
        public mapPanel() {
            InitializeComponent();
        }

        public mapPanel(IContainer container) {
            container.Add(this);

            InitializeComponent();
        }
    }
}
