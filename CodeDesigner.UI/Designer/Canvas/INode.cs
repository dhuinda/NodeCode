using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Designer.Canvas
{
    public interface INode
    {
        public Control BindedControl { get; set; }
    }
}
