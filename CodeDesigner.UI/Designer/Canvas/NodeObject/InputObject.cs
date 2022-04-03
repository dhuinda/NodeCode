﻿namespace CodeDesigner.UI.Designer.Canvas.NodeObject
{
    public class InputObject : NodeObject
    {
        public int Width { get; set; }

        public Node AttachedNode {get; set;}

        public Panel DropPanel { get; set; }

        public InputObject(int width)
        {
            Width = width;
            DropPanel = new Panel();
            DropPanel.BackColor = Color.White;
            DropPanel.Width = width;
            DropPanel.Height = 20;
        }

        
    }
}