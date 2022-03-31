using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeDesigner.UI.Designer;
using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Windows
{
    public partial class Designer : Form
    {
        public DesignerCore Core;

        public Designer()
        {
            InitializeComponent();
            Core = new DesignerCore(this);
        }

        private void pictureBoxButton6_Click(object sender, EventArgs e)
        {
            Core.IncrementBlockType();
        }

        private void pictureBoxButton5_Click(object sender, EventArgs e)
        {
            Core.IncrementBlockType(0);
        }
    }
}
