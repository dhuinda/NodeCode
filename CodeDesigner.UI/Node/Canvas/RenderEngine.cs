using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Node.Blocks;
using CodeRunner.UI;

namespace CodeDesigner.UI.Node.Canvas
{
    public static class RenderEngine
    {
        public static PointF LastOffset = new (0, 0);

        public static void DrawBlocks(Graphics g, PointF offset, Size view)
        {
            float zoom = Program.dash.DesignerCanvas.ZoomFactor;

            foreach (BlockBase block in Canvas.Blocks)
            {
                block.Coordinates.X += (offset.X - LastOffset.X) / zoom;
                block.Coordinates.Y += (offset.Y - LastOffset.Y) / zoom;

                g.DrawRectangle(new Pen(block.Properties.BorderColor, 1 * zoom), (block.Coordinates.X * zoom), (block.Coordinates.Y * zoom), block.Properties.Width * zoom, block.Properties.Height * zoom);
                g.FillRectangle(new SolidBrush(block.Properties.FillColor), block.Coordinates.X * zoom, block.Coordinates.Y * zoom, block.Properties.Width * zoom, block.Properties.Height * zoom);
                g.FillRectangle(new SolidBrush(block.Properties.SecondaryColor), block.Coordinates.X * zoom, block.Coordinates.Y * zoom, block.Properties.Width * zoom, 20 * zoom);
                g.DrawString(block.Properties.Name, new Font("Gilroy-Bold", 9f * zoom, FontStyle.Regular, GraphicsUnit.Point), new SolidBrush(block.Properties.TextColor), (block.Coordinates.X * zoom) + (5 * zoom), (block.Coordinates.Y * zoom) + (3.2f * zoom));

                DrawIO(block, g, offset, zoom);
            }

            LastOffset = offset;
        }

        public static void DrawIO(BlockBase block, Graphics g, PointF offset, float zoom)
        {
            Color outputColor = block.Connecting ? Color.LightGray : Color.White;
            
            PointF[] outputPoints = new PointF[5];
            outputPoints[0] = new PointF((block.Coordinates.X + block.Properties.Width - 20) * zoom, (block.Coordinates.Y + block.Properties.Height - 22) * zoom);
            outputPoints[1] = new PointF((block.Coordinates.X + block.Properties.Width - 13) * zoom, (block.Coordinates.Y + block.Properties.Height - 22) * zoom);
            outputPoints[2] = new PointF((block.Coordinates.X + block.Properties.Width - 8) * zoom, (block.Coordinates.Y + block.Properties.Height - 16) * zoom);
            outputPoints[3] = new PointF((block.Coordinates.X + block.Properties.Width - 13) * zoom, (block.Coordinates.Y + block.Properties.Height - 10) * zoom);
            outputPoints[4] = new PointF((block.Coordinates.X + block.Properties.Width - 20) * zoom, (block.Coordinates.Y + block.Properties.Height - 10) * zoom);

            block.OutputPolygon = outputPoints;

            g.FillPolygon(new SolidBrush(outputColor), outputPoints);
        }
    }
}