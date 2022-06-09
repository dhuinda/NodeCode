using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Canvas;

namespace CodeDesigner.UI.Utility.Project
{
    [Serializable]
    public class NodeMap
    {
        public string Name { get; set; }
        public System.Drawing.Image Thumbnail { get; set; }
        public List<BlockBase?> Blocks { get; set; }
    }
}
