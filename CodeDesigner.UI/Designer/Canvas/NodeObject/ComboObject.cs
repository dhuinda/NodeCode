using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Designer.Canvas.NodeObject
{
    public class ComboObject : NodeObject
    {
        public string[] Options { get; set; }
        public ComboBox Box { get; set; }

        public ComboObject(string[] options)
        {
            Options = options;
        }

        public string GetSelectedOption()
        {
            return Box.SelectedText;
        }
    }
}
