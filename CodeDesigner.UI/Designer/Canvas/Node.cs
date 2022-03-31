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
        public List<Node> Children;

        public bool Intersecting = false;

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
            deleteButton.Click += DeleteButtonOnClick;

            cms.Items.Add(deleteButton);

            cms.BackColor = Color.FromArgb(14, 19, 29);
            cms.ForeColor =  Color.FromArgb(14, 19, 29);

            cms.Opacity = 50;

            ContextMenuStrip = cms;

            cms.Width = 100;

            Children = new List<Node>();
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

        public void SetNodeAsChild(Node node)
        {
            int top = 40 + 40 * Children.Count;
            node.Top = Top + top;
            node.Left = Left + 50;

            Children.Add(node);
        }

        #region UI LOGIC

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            drgevent.Effect = DragDropEffects.Move;

            _color = Color.FromArgb(14, 19, 29);

            base.OnDragEnter(drgevent);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
        }

        protected override void OnDragLeave(EventArgs eventArgs)
        {
            _color = Color.FromArgb(24, 29, 39);
            Invalidate();
        }

        private void DeleteButtonOnClick(object? sender, EventArgs e)
        {
            Hide();
            _canvas.Nodes.Remove(this);
            _canvas.Core.Form.DesignerCanvas.Controls.Remove(this);
            Dispose();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _lastPoint = new Point(e.X, e.Y);

            BringToFront();

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _canvas.CheckOverlapping(this);

            if (!Intersecting)
                SetColor(Color.FromArgb(24, 29, 39));

            _canvas.ReleaseOverlapping(this);
                

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
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
