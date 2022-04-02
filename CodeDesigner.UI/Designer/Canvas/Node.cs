using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeDesigner.UI.Resources.Controls;

namespace CodeDesigner.UI.Designer.Canvas
{
    public partial class Node : Panel
    {
        public Nodes.Node BindedNode { get; set; }
        public bool NodeIsBinded { get; set; }

        public bool Intersecting = false;
        public bool NodeHasParent = false;

        private Point _lastPoint;
        private CanvasCore _canvas;

        private Color _color = Color.FromArgb(24, 29, 39);

        public Node()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);

            Size = new Size(Size.Width, 35);

            ContextMenuStrip cms = new();

            ToolStripItem deleteButton = new ToolStripButton("Delete Node", null);

            deleteButton.BackColor = Color.FromArgb(14, 19, 29);
            deleteButton.ForeColor = Color.White;
            deleteButton.Width = 100;

            ToolStripItem unbindButton = new ToolStripButton("Unbind Node", null);

            unbindButton.BackColor = Color.FromArgb(14, 19, 29);
            unbindButton.ForeColor = Color.White;
            unbindButton.Width = 100;

            ToolStripItem moveUpBtn = new ToolStripButton("Move Up", null);

            moveUpBtn.BackColor = Color.FromArgb(14, 19, 29);
            moveUpBtn.ForeColor = Color.White;
            moveUpBtn.Width = 100;

            ToolStripItem moveDownBtn = new ToolStripButton("Move Down", null);

            moveDownBtn.BackColor = Color.FromArgb(14, 19, 29);
            moveDownBtn.ForeColor = Color.White;
            moveDownBtn.Width = 100;

            cms.Items.Add(deleteButton);
            cms.Items.Add(unbindButton);
            cms.Items.Add(moveUpBtn);
            cms.Items.Add(moveDownBtn);

            cms.BackColor = Color.FromArgb(14, 19, 29);
            cms.ForeColor =  Color.FromArgb(14, 19, 29);

            cms.Opacity = 50;

            ContextMenuStrip = cms;

            cms.Width = 100;
        }

        public void MoveNodes(MouseEventArgs e, Point lastPoint)
        {
        }

        public void SetColor(Color color)
        {
            _color = color;
            Invalidate();
        }

        public void SetCanvas(CanvasCore canvas)
        {
            _canvas = canvas;
        }

        public void FormatNodes()
        {
            
        }

        #region UI LOGIC

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _lastPoint = new Point(e.X, e.Y);

            BringToFront();

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (NodeHasParent)
            {
                base.OnMouseMove(e);
                return;
            }

            if (e.Button == MouseButtons.Left)
                if (!NodeIsBinded || !BindedNode.HasParent)
                {
                    Top += e.Y - _lastPoint.Y;
                    Left += e.X - _lastPoint.X;
                }
            

            _canvas.CheckOverlapping(this);

            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            try
            {
                Graphics graphics = pe.Graphics;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (SolidBrush brush =
                    new (_color))
                {
                    graphics.FillRoundedRectangle(brush, 0, 0, Width, Height, 10);
                    //graphics.FillRoundedRectangle(brush, 12, 12 + ((this.Height - 64) / 2), this.Width - 44, (this.Height - 64)/2, 10);
                }
            }
            catch { }

            base.OnPaint(pe);
        }

        #endregion
    }
}
