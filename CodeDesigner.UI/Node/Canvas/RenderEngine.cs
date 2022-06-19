using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO.Packaging;
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
        public static PointF MouseLocation;
        public static BlockBase ConnectingBlock;

        public static void DrawBlocks(Graphics g, PointF offset, Size view)
        {
            float zoom = Program.dash.DesignerCanvas.ZoomFactor;

            DrawConnectionLines(ConnectingBlock, g);

            foreach (BlockBase? block in Canvas.Blocks)
            {
                block.Coordinates.X += (offset.X - LastOffset.X) / zoom;
                block.Coordinates.Y += (offset.Y - LastOffset.Y) / zoom;

                g.DrawRectangle(new Pen(block.Properties.BorderColor, 1 * zoom), (block.Coordinates.X * zoom), (block.Coordinates.Y * zoom), block.Properties.Width * zoom, block.Properties.Height * zoom);
                g.FillRectangle(new SolidBrush(block.Properties.FillColor), block.Coordinates.X * zoom, block.Coordinates.Y * zoom, block.Properties.Width * zoom, block.Properties.Height * zoom);
                g.FillRectangle(new SolidBrush(block.Properties.SecondaryColor), block.Coordinates.X * zoom, block.Coordinates.Y * zoom, block.Properties.Width * zoom, 20 * zoom);
                g.DrawString(block.Properties.Name, new Font("Gilroy-Bold", 9f * zoom, FontStyle.Regular, GraphicsUnit.Point), new SolidBrush(block.Properties.TextColor), (block.Coordinates.X * zoom) + (5 * zoom), (block.Coordinates.Y * zoom) + (3.2f * zoom));

                DrawIO(block, g, offset, zoom);
                DrawWires(block, g, zoom);
            }

            LastOffset = offset;
        }

        public static void DrawIO(BlockBase? block, Graphics g, PointF offset, float zoom)
        {
            Color outputColor = block.Connecting ? Color.LightGray : Color.White;
            
            PointF[] outputPoints = new PointF[5];
            outputPoints[0] = new PointF((block.Coordinates.X + block.Properties.Width - 20) * zoom, (block.Coordinates.Y + block.Properties.Height - 22) * zoom);
            outputPoints[1] = new PointF((block.Coordinates.X + block.Properties.Width - 13) * zoom, (block.Coordinates.Y + block.Properties.Height - 22) * zoom);
            outputPoints[2] = new PointF((block.Coordinates.X + block.Properties.Width - 8) * zoom, (block.Coordinates.Y + block.Properties.Height - 16) * zoom);
            outputPoints[3] = new PointF((block.Coordinates.X + block.Properties.Width - 13) * zoom, (block.Coordinates.Y + block.Properties.Height - 10) * zoom);
            outputPoints[4] = new PointF((block.Coordinates.X + block.Properties.Width - 20) * zoom, (block.Coordinates.Y + block.Properties.Height - 10) * zoom);

            block.OutputPolygon = outputPoints;
            
            g.FillPolygon(new SolidBrush(GetParameterColor(block.Properties.OutputType)), outputPoints);

            float y = 25;
            
            foreach (Parameter? parameter in block.Parameters)
            {
                parameter.Coordinates = new PointF((block.Coordinates.X + 5) * zoom, (block.Coordinates.Y + y) * zoom);
                if (parameter.Connected)
                    g.FillEllipse(new SolidBrush(GetParameterColor(parameter.Type)), parameter.Coordinates.X, parameter.Coordinates.Y, 8 * zoom, 8 * zoom);
                else 
                    g.DrawEllipse(new Pen(GetParameterColor(parameter.Type)), parameter.Coordinates.X, parameter.Coordinates.Y, 8 * zoom, 8 * zoom);

                g.DrawString("(" + parameter.Type + ") " + parameter.Name, new Font("Gilroy-Bold", 5.5f * zoom, FontStyle.Regular, GraphicsUnit.Point), new SolidBrush(Color.White), parameter.Coordinates.X + (12 * zoom), parameter.Coordinates.Y);
                y += 16;
            }
        }
        
        public static Color GetParameterColor(Parameter.ParameterType type)
        {
            switch (type)
            {
                case Parameter.ParameterType.Int:
                    return Color.LightYellow;
                case Parameter.ParameterType.Double:
                    return Color.LightSlateGray;
                case Parameter.ParameterType.String:
                    return Color.LightSeaGreen;
                case Parameter.ParameterType.Bool:
                    return Color.LightGreen;
                case Parameter.ParameterType.Object:
                    return Color.LightBlue;
                default:
                    return Color.LightBlue;

            }
        }

        //Draws the wires from block to parameters
        public static void DrawWires(BlockBase block, Graphics g, float zoom)
        {
            foreach (Parameter parameter in block.Parameters)
            {
                if (parameter is { Connected: true })
                {
                    try
                    {
                        PointF[] points = new PointF[4];
                        points[0] = parameter.Block.OutputPolygon[2];
                        points[1] = new PointF(parameter.Block.OutputPolygon[2].X + 70,
                            parameter.Block.OutputPolygon[2].Y);
                        points[2] = new PointF(parameter.Coordinates.X - 70, parameter.Coordinates.Y + (4 * zoom));
                        points[3] = new PointF(parameter.Coordinates.X, parameter.Coordinates.Y + (4 * zoom));
                        g.DrawBezier(new Pen(Color.Gray), points[0], points[1], points[2], points[3]);
                    }
                    catch
                    {

                        }
                        
                    }
                }
            }
        }

        //Draws the connecting line between one block and the mouse point for connecting the block to other blocks
        public static void DrawConnectionLines(BlockBase block, Graphics g)
        {
            if (!Canvas.Connecting)
                return;

            PointF[] points = new PointF[4];
            points[0] = block.OutputPolygon[2];
            points[1] = new PointF(block.OutputPolygon[2].X + 70, block.OutputPolygon[2].Y);
            points[2] = new PointF(MouseLocation.X - 70, MouseLocation.Y);
            points[3] = MouseLocation;

            g.DrawBezier(new Pen(Color.Gray), points[0], points[1], points[2], points[3]);
        }
    }
}