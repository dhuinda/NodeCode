using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Node.Blocks
{
    [Serializable]
    public class BlockBase
    {
        public BlockBase InputBlock; //This is the block that invoked this block.
        public BlockBase OutputBlock; //This is the block that this block is invoking after completion.

        public List<Parameter?> Parameters;

        public BlockProperties Properties;

        public PointF Coordinates;

        public PointF[] OutputPolygon;
        
        public bool Connecting;

        public BlockBase(BlockProperties properties)
        {
            Coordinates = new Point(0, 0);
            Properties = properties;
            Parameters = new List<Parameter?>();
        }

        public BlockBase(BlockProperties properties, PointF coordinates)
        {
            Coordinates = coordinates;
            Properties = properties;
            Parameters = new List<Parameter?>();
        }

        public BlockBase()
        {
            Parameters = new List<Parameter?>();
        }
    }
}
