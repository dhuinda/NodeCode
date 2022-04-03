using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Designer.Canvas.NodeObject
{
    public class LabelObject : NodeObject
    {
        public string Text { get; set; }

        public LabelObject(string labelText)
        {
            Text = labelText;
        }
    }
}
