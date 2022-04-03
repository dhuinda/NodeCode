namespace CodeDesigner.UI.Designer.Canvas.NodeObject
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

        public void CreatePreviewString()
        {
            List<LabelObject> nodeObjects = new ());

            string preview = "Nothing to Display";

            if (AttachedNode.NodeObjects.Count > 0)
            {
                foreach (NodeObject obj in AttachedNode.NodeObjects)
                {
                    if (NodeObject.GetType() == typeof(LabelObject))
                    {
                        nodeObjects.Add((LabelObject)obj);
                    }
                }

                if (nodeObjects.Count > 1)
                {
                    preview = nodeObjects[0].Text + " " + nodeObjects[1].Text;
                } else 
                {
                    preview = nodeObjects[0].Text;
                }
            }

            Label label = new ();
            label.Text = preview;
            label.ForeColor = Color.White();

            DropPanel.Controls.Add(label);
        }
    }
}