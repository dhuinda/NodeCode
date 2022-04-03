using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Designer.Canvas.NodeObject
{
    public class TextboxObject : NodeObject
    {
        public int Width { get; set; }
        public TextboxObject(int width)
        {
            Width = width;
        }
    }
}
