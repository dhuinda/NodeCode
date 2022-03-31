using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CodeDesigner.Core;
using CodeDesigner.UI.Designer;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Windows.Resources.Controls.Panels;

namespace CodeDesigner.UI.Windows.Resources.Controls.Node
{
    public partial class ToolboxNode : InfoPanel
    {
        public NodeTypes NodeType { get; set; }

        public DesignerCore _core;

        private readonly Font ContentFont = new("Montserrat ExtraBold", 8F, System.Drawing.FontStyle.Bold,
            System.Drawing.GraphicsUnit.Point);

        public ToolboxNode(NodeTypes type, DesignerCore core)
        {
            InitializeComponent();
            _core = core;
            SetNodeType(type);
        }

        public void SetNodeType(NodeTypes nodeType)
        {
            NodeType = nodeType;

            string nodeName = Enum.GetName(nodeType);

            string[] nodeNames = nodeName.Split("_");

            StringBuilder sb = new();

            foreach (string s in nodeNames)
            {
                sb.Append(s + " ");
            }

            Font = ContentFont;

            Content = sb.ToString().Remove(sb.ToString().Length - 1);

            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            GradientOne = Color.FromArgb(44, 49, 59);
            GradientTwo = Color.FromArgb(44, 49, 59);

            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            GradientOne = Color.FromArgb(34, 39, 49);
            GradientTwo = Color.FromArgb(34, 39, 49);

            Invalidate();
            base.OnMouseEnter(e);
        }


        protected override void OnMouseDoubleClick(MouseEventArgs e)
         {
            _core.Canvas.GenerateNode(this);

            base.OnMouseDoubleClick(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
