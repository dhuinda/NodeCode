using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Node.Interaction.Elements
{
    [Serializable]
    public class ButtonElement : Element
    {
        public Color ButtonColor { get; set; }
        public Color BorderColor { get; set; }
        public Action Method { get; set; }
        public string ButtonText { get; set; }


        public ButtonElement(ElementProperties properties, string text, Color color, Color borderColor, Action action)
        {
            Properties = properties;
            ButtonColor = color;
            BorderColor = borderColor;
            Method = action;
            ButtonText = text;
        }
    }
}
