﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Accessibility;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Windows.Resources.Controls.Panels;

namespace CodeDesigner.UI.Node.Canvas
{
    public static class Canvas
    {
        public static CanvasPanel CanvasControl;
        public static PointF MousePosition;
        public static List<BlockBase?> Blocks = new();
        public static bool Connecting;
        public static bool IsOverParameter;
        public static Parameter? OverParameter;
        public static BlockBase ConnectingBlock;

        public static void Initialize(CanvasPanel panel)
        {
            CanvasControl = panel;
        }

        public static void AddNode(BlockBase? block)
        {
            Blocks.Add(block);
            CanvasControl.Refresh();
        }

        public static void Pan(PointF delta)
        {
            BlockBase? block = IsPointInBlock(delta);

            if (block != null)
            {
                MoveBlock(block, delta);
                return;
            }

            float offsetX = (delta.X - MousePosition.X);
            float offsetY = (delta.Y - MousePosition.Y);

            CanvasControl.OffsetX += offsetX;
            CanvasControl.OffsetY += offsetY;

            CanvasControl.Refresh();

            MousePosition = delta;
        }

        public static void ZoomOut(float value)
        {
            CanvasControl.ZoomFactor += value;
            CanvasControl.Refresh();
        }

        public static void ZoomIn(float value)
        {
            if (CanvasControl.ZoomFactor - value < 0.2)
                return;

            CanvasControl.ZoomFactor -= value;
            CanvasControl.Refresh();
        }

        public static void MoveBlock(BlockBase? block, PointF delta)
        {
            block.Coordinates.X += (delta.X - MousePosition.X) / CanvasControl.ZoomFactor;
            block.Coordinates.Y += (delta.Y - MousePosition.Y) / CanvasControl.ZoomFactor;

            MousePosition = delta;

            CanvasControl.Refresh();
        }

        public static BlockBase? IsPointInBlock(PointF testPoint)
        {
            foreach (BlockBase? block in Blocks)
            {
                RectangleF rect = new (block.Coordinates.X * CanvasControl.ZoomFactor, block.Coordinates.Y * CanvasControl.ZoomFactor, block.Properties.Width * CanvasControl.ZoomFactor, block.Properties.Height * CanvasControl.ZoomFactor);

                if (!rect.Contains(testPoint)) continue;

                if (Connecting && block != ConnectingBlock)
                {
                    OverParameter = PointInParameter(block, testPoint);
                    IsOverParameter = true;
                }
                
                return block;
            }

            return null;
        }

        public static Parameter? PointInParameter(BlockBase block, PointF testPoint)
        {
            foreach (Parameter? param in block.Parameters)
            {
                RectangleF rect = new(param.Coordinates.X * CanvasControl.ZoomFactor, param.Coordinates.Y * CanvasControl.ZoomFactor, 8 * CanvasControl.ZoomFactor, 8 * CanvasControl.ZoomFactor);
                if (!rect.Contains(testPoint)) continue;
                return param;
            }

            return null;
        }

        public static void ConnectParameter(BlockBase connectingBlock, Parameter? parameter)
        {
            parameter.ReferenceValue = connectingBlock;
            parameter.Connected = true;
        }

        public static void DeleteBlock(BlockBase block)
        {
            Blocks.Remove(block);
            CanvasControl.Refresh();
        }

        public static bool IsPointInPolygon(PointF[] polygon, PointF testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }
}
