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
        public Node Parent;

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
            deleteButton.Click += DeleteButtonOnClick;

            ToolStripItem unbindButton = new ToolStripButton("Unbind Node", null);

            unbindButton.BackColor = Color.FromArgb(14, 19, 29);
            unbindButton.ForeColor = Color.White;
            unbindButton.Width = 100;
            unbindButton.Click += delegate(object? sender, EventArgs args)
            {
                if (NodeHasParent)
                    Parent.RemoveChildNode(this);
            };

            ToolStripItem moveUpBtn = new ToolStripButton("Move Up", null);

            moveUpBtn.BackColor = Color.FromArgb(14, 19, 29);
            moveUpBtn.ForeColor = Color.White;
            moveUpBtn.Width = 100;
            moveUpBtn.Click += delegate(object? sender, EventArgs args)
            {
                MoveNodeUp();
            };

            ToolStripItem moveDownBtn = new ToolStripButton("Move Down", null);

            moveDownBtn.BackColor = Color.FromArgb(14, 19, 29);
            moveDownBtn.ForeColor = Color.White;
            moveDownBtn.Width = 100;
            moveDownBtn.Click += delegate(object? sender, EventArgs args)
            {
                MoveNodeDown();
            };

            cms.Items.Add(deleteButton);
            cms.Items.Add(unbindButton);
            cms.Items.Add(moveUpBtn);
            cms.Items.Add(moveDownBtn);

            cms.BackColor = Color.FromArgb(14, 19, 29);
            cms.ForeColor =  Color.FromArgb(14, 19, 29);

            cms.Opacity = 50;

            ContextMenuStrip = cms;

            cms.Width = 100;

            Children = new List<Node>();
        }

        public void MoveNodes(MouseEventArgs e, Point lastPoint)
        {
            Top += e.Y - lastPoint.Y;
            Left += e.X - lastPoint.X;

            foreach (Node c in Children)
            {
                c.MoveNodes(e, lastPoint);
            }

            _lastPoint = lastPoint;
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
            Children.Add(node);
            node.Parent = this;
            node.NodeHasParent = true;

            FormatNodes();
        }

        public void RemoveChildNode(Node node)
        {
            Children.Remove(node);
            node.Parent = null;
            node.NodeHasParent = false;
        }

        private void MoveNodeUp()
        {
            if (!NodeHasParent)
                return;

            if (Parent.Children.Count < 2)
                return;

            int nodeIndex = Parent.Children.IndexOf(this);

            Node[] nodes = new Node[Parent.Children.Count];

            for (int i = 0; i < nodes.Length; i++)
            {
                if (i == nodeIndex - 1)
                    nodes[i] = Parent.Children[i + 1];

                if (i == nodeIndex)
                    nodes[i] = Parent.Children[nodeIndex - 1];

                if (i != nodeIndex && i != nodeIndex - 1)
                {
                    nodes[i] = Parent.Children[i];
                }
            }

            Parent.Children.Clear();
            Parent.Children.AddRange(nodes);

            Parent.FormatNodes();
        }

        private void MoveNodeDown()
        {
            if (!NodeHasParent)
                return;

            if (Parent.Children.Count < 2)
                return;

            int nodeIndex = Parent.Children.IndexOf(this);

            Node[] nodes = new Node[Parent.Children.Count];

            for (int i = 0; i < nodes.Length; i++)
            {
                if (i == nodeIndex + 1)
                    nodes[i] = Parent.Children[i - 1];

                if (i == nodeIndex)
                    nodes[i] = Parent.Children[nodeIndex + 1];

                if (i != nodeIndex && i != nodeIndex - 1)
                {
                    nodes[i] = Parent.Children[i];
                }
            }

            Parent.Children.Clear();
            Parent.Children.AddRange(nodes);

            Parent.FormatNodes();
        }

        public void FormatNodes()
        {
            int heightFactor = 1;

            foreach (Node child in Children.ToArray())
            {
                child.Top = Top + heightFactor * 40;
                child.Left = Left + 30;

                heightFactor++;
            }
        }

        #region UI LOGIC

        private void DeleteButtonOnClick(object? sender, EventArgs e)
        {
            foreach (Node n in Children.ToArray())
            {
                RemoveChildNode(n);
            }

            if (NodeHasParent)
            {
                Node tempParent = Parent;
                Parent.RemoveChildNode(this);

                tempParent.FormatNodes();
            }


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
            if (NodeHasParent)
            {
                base.OnMouseMove(e);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                MoveNodes(e, _lastPoint);
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
