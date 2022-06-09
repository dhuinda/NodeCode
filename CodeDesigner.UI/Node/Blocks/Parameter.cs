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
        public object Value { get; set; }
        public BlockBase Block { get; set; }

        public enum ParameterType
        {
            String,
            Bool,
            Float,
            Double,
            Int,
            Object
        }
    }
}
