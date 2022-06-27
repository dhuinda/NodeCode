using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Canvas;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Utility.Project;

namespace CodeDesigner.UI.Windows
{
    public partial class Dashboard : Form
    {
        private bool _mouseDown;
        public NodeMap Map;
        private bool hasErrors = false; // todo: can probably change this to just check list.empty() once a list of errors is added for the UI

        public Dashboard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Canvas.Initialize(DesignerCanvas);
        }

        public string[] ListItems = new[]
            { "test 1", "test 2", "test 3", "test 4", "test 5", "test 6", "test 7", "test 8", "test 9", "test 10", };

        public void AddError(String message, Guid? nodeId = null)
        {
            hasErrors = true;
        }

        public void ClearErrors()
        {
            hasErrors = false;
        }

        public bool HasErrors()
        {
            return hasErrors;
        }
        
        private void BlockSearchBox_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (string item in ListItems)
            {
                if (item.Contains(BlockSearchBox.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    listBox1.Items.Add(item);
                }
            }
        }

        private void ZoomOutBtn_Click(object sender, EventArgs e)
        {
            Canvas.ZoomOut(0.2f);
        }

        private void ZoomInBtn_Click(object sender, EventArgs e)
        {
            Canvas.ZoomIn(0.2f);
        }

        private void DesignerCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            Canvas.MousePosition = e.Location;

            BlockBase? block = Canvas.IsPointInBlock(e.Location);

            if (block != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    Canvas.DeleteBlock(block);
                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    Element? element = Canvas.IsPointInElement(block, e.Location);

                    if (element != null)
                    {
                        Canvas.Interact(element);
                        return;
                    }

                    Parameter? p = Canvas.PointInParameter(block, e.Location);

                    if (p != null)
                    {
                        p.Connected = false;
                        p.SecondaryConnected = false;
                        p.NextConnected = false;
                        p.ReferenceValue = null;
                        DesignerCanvas.Refresh();
                        return;
                    }
                }
                
                if (block.UseOutput && Canvas.IsPointInPolygon(block.OutputPolygon, e.Location))
                {
                    block.Connecting = true;
                    Canvas.Connecting = true;
                    Canvas.ConnectingBlock = block;
                    RenderEngine.MouseLocation = e.Location;
                    RenderEngine.ConnectingBlock = block;
                    DesignerCanvas.Refresh();
                    return;
                } 
                if (block.UseSecondaryOutput && Canvas.IsPointInPolygon(block.SecondaryPolygon, e.Location))
                { 
                    block.SecondaryConnecting = true;
                    Canvas.Connecting = true;
                    Canvas.ConnectingBlock = block;
                    RenderEngine.MouseLocation = e.Location;
                    RenderEngine.ConnectingBlock = block;
                    DesignerCanvas.Refresh();
                    return;
                }
                
                if (Canvas.IsPointInPolygon(block.NextPolygon, e.Location))
                {
                    block.NextConnecting = true;
                    Canvas.Connecting = true;
                    Canvas.ConnectingBlock = block;
                    RenderEngine.MouseLocation = e.Location;
                    RenderEngine.ConnectingBlock = block;
                    return;
                }
            }
            
            _mouseDown = true;
        }
        private void DesignerCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            Canvas.MousePosition = e.Location;

            Canvas.IsPointInBlock(e.Location);

            if (Canvas.ElementClickedOn)
            {
                Canvas.ElementClickedOn = false;
                if (Canvas.ElementClicked != null) Canvas.ElementClicked.IsClickedOn = false;
            }
            
            if (Canvas.Connecting)
            {
                if (Canvas.IsOverParameter)
                {
                    Canvas.IsOverParameter = false;
                    Canvas.ConnectParameter(Canvas.ConnectingBlock, Canvas.OverParameter);
                }
                Canvas.ConnectingBlock.Connecting = false;
                Canvas.ConnectingBlock.SecondaryConnecting = false;
                Canvas.ConnectingBlock.NextConnecting = false;

                Canvas.Connecting = false;
                
                DesignerCanvas.Refresh();
            }

            Canvas.IsOverParameter = false;
            
            _mouseDown = false;
        }
        
        private void DesignerCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (Canvas.Connecting)
            {
                RenderEngine.MouseLocation = e.Location;
                Canvas.IsPointInBlock(e.Location);
                
                DesignerCanvas.Refresh();
                return;
            }
            
            if (!_mouseDown)
                return;

            Canvas.Pan(e.Location);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(DesignerCanvas.Width, DesignerCanvas.Height);
            label1.ForeColor = Color.Transparent;
            DesignerCanvas.DrawToBitmap(bitmap, new Rectangle(0, 0, DesignerCanvas.Width, DesignerCanvas.Height));
            label1.ForeColor = Color.FromArgb(69, 69, 69);
            ProjectUtil.Save(Canvas.Blocks, Map.Name, bitmap );
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
