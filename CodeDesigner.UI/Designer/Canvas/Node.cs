using System.Drawing.Drawing2D;
using CodeDesigner.UI.Resources.Controls;

namespace CodeDesigner.UI.Designer.Canvas;

public partial class Node : Panel
{

    public ParentNode Parent;
    public int HeightFactor = 1;

    public bool Intersecting = false;
    public bool NodeHasParent = false;

    protected Point _lastPoint;
    protected CanvasCore _canvas;

    protected Color _color = Color.FromArgb(24, 29, 39);
    
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

    }

    public virtual bool CanHaveChildren()
    {
        return false;
    }

    public virtual void MoveNodes(MouseEventArgs e, Point lastPoint)
    {
        Top += e.Y - lastPoint.Y;
        Left += e.X - lastPoint.X;


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
    
    public virtual void FormatNodes()
    {
        HeightFactor = 1;
    }
    
    #region UI LOGIC

    protected virtual void DeleteButtonOnClick(object? sender, EventArgs e)
    {
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