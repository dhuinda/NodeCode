using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Node.Blocks
{
    [Serializable]
    public class Parameter
    {
        public ParameterType Type { get; set; }
        
        // Either RawValue or Reference value must be null
        public string? RawValue { get; set; } // A string representing a primitive literal; ex: "1", "false", "hi", "1.2" (coalesced via Type)
        public BlockBase? ReferenceValue { get; set; } // The type of this is based on Type
        
        public PointF Coordinates { get; set; }
        public string Name { get; set; }
        public bool Connected { get; set; }

        public enum ParameterType
        {
            String,
            Bool,
            Double,
            Int,
            Object,
            Void
        }

        public string? ObjectType;
    }
}
