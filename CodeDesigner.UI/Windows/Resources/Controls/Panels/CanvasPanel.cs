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
using Accessibility;
using CodeDesigner.UI.Node.Canvas;
using CodeRunner.UI;

namespace CodeDesigner.UI.Windows.Resources.Controls.Panels
{
    public partial class CanvasPanel : Panel
    {
        public float ZoomFactor = 1;
        public float OffsetY = 0;
        public float OffsetX = 0;
        
        public CanvasPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            DrawGrid(pe, Size);
            
            if (!DesignMode)
            {
                RenderEngine.DrawBlocks(pe.Graphics, new PointF(OffsetX, OffsetY), Size);
            }

            base.OnPaint(pe);
        }

        public void DrawGrid(PaintEventArgs pe, Size size)
        {
            float width = Size.Width;
            float height = Size.Height;

            float x = 0;
            float y = 0;
            
            Graphics g = pe.Graphics;

            Pen pen = new (Color.FromKnownColor(KnownColor.ControlLight));
            
            while (x < width)
            {
                g.DrawLine(pen, x + OffsetX, 0, x + OffsetX, height);
                x += (20 * ZoomFactor);
            }

            while (y < height)
            {
                g.DrawLine(pen, 0, y + OffsetY, width, y + OffsetY);
                y += (20 * ZoomFactor);
            }

            bool offXNeg = false;
            bool offYNeg = false;

            x = 0;
            y = 0;

            if (OffsetX < 0)
                offXNeg = true;

            if (OffsetY < 0)
                offYNeg = true;

            switch (offXNeg)
            {
                case false when Math.Abs(OffsetX) > (20 * ZoomFactor):
                {
                    int lines = (int)(OffsetX / (20 * ZoomFactor));
                
                    while (x < lines)
                    {
                        g.DrawLine(pen, OffsetX - ((20 * x) * ZoomFactor), 0, OffsetX - ((20 * x) * ZoomFactor), height);
                        x += 1;
                    }

                    break;
                }
                case true when Math.Abs(OffsetX) > (20 * ZoomFactor):
                {
                    int lines = (int)Math.Ceiling(OffsetX / (20 * ZoomFactor));
                
                    while (x < Math.Abs(lines))
                    {
                        g.DrawLine(pen, (Width + OffsetX) + ((20 * x) * ZoomFactor), 0, (Width + OffsetX) + ((20 * x) * ZoomFactor), height);
                        x += 1;
                    }

                    break;
                }
            }

            switch (offYNeg)
            {
                case false when Math.Abs(OffsetY) > (20 * ZoomFactor):
                {
                    int lines = (int)Math.Ceiling(OffsetY / (20 * ZoomFactor));
                
                    while (y < lines)
                    {
                        g.DrawLine(pen, 0, OffsetY - ((20 * y) * ZoomFactor), width, OffsetY - ((20 * y) * ZoomFactor));
                        y += 1;
                    }

                    break;
                }
                case true when Math.Abs(OffsetY) > (20 * ZoomFactor):
                {
                    int lines = (int)(OffsetY / (20 * ZoomFactor));
                
                    while (y < Math.Abs(lines))
                    {
                        g.DrawLine(pen, 0, (height + OffsetY) + ((20 * y) * ZoomFactor), width, (height + OffsetY) + ((20 * y) * ZoomFactor));
                        y += 1;
                    }

                    break;
                }
            }
        }
    }
}
