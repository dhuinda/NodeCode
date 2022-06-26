using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Interaction;

namespace CodeDesigner.UI.Node.Blocks
{
    [Serializable]
    public class BlockBase
    {
        public BlockBase? InputBlock; //This is the block that invoked this block.
        public BlockBase? NextBlock; //This is the block that this block is invoking after completion.

        public List<Parameter?> Parameters;
        public List<Parameter> ConnectedParameters;
        public List<Element> Elements;

        public BlockProperties Properties;

        public PointF Coordinates;

        public PointF NextPolygon;
        public PointF[] OutputPolygon;
        public PointF[] SecondaryPolygon;
        
        public bool Connecting;
        public bool SecondaryConnecting;
        public bool UseSecondaryOutput;

        public NodeType NodeType = NodeType.DEFAULT;

        public BlockBase(BlockProperties properties)
        {
            Coordinates = new Point(0, 0);
            Properties = properties;
            Parameters = new List<Parameter?>();
            ConnectedParameters = new List<Parameter>();
            Elements = new List<Element>();
        }

        public BlockBase(BlockProperties properties, PointF coordinates)
        {
            Coordinates = coordinates;
            Properties = properties;
            Parameters = new List<Parameter?>();
            ConnectedParameters = new List<Parameter>();
            Elements = new List<Element>();
        }

        public BlockBase()
        {
            Parameters = new List<Parameter?>();
            ConnectedParameters = new List<Parameter>();
            Elements = new List<Element>();
        }

        public void DestroyConnections()
        {
            foreach (Parameter p in ConnectedParameters)
            {
                p.Connected = false;
                p.SecondaryConnected = false;
                p.ReferenceValue = null;
            }

            ConnectedParameters.Clear();
        }
    }
}
