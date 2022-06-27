using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Node.Blocks;

namespace CodeDesigner.UI.Node.Interaction.Elements
{
    public class TextBoxElement : Element
    {
        public string Text = string.Empty;
        public string ButtonText = string.Empty;
        public Action Method;
        private Color _color;
        private Color _buttonColor;
        private Color _strokeColor;

        public TextBoxElement(ElementProperties properties, string buttonText, Color color, Color buttonColor, Color stroke, Action method)
        {
            ButtonText = buttonText;
            Properties = properties;
            _color = color;
            _buttonColor = buttonColor;
            _strokeColor = stroke;
            Method = method;
        }
        
        public override void Draw(BlockBase block, Graphics g, float zoom)
        {
            float x = (block.Coordinates.X + Properties.BlockCoordinates.X) * zoom;
            float y = (block.Coordinates.Y + Properties.BlockCoordinates.Y) * zoom;
            float width = Properties.Size.Width * zoom;
            float height = Properties.Size.Height * zoom;

            g.DrawRectangle(new Pen(_strokeColor, 1 * zoom), x, y, width, height);
            float buttonWidth = g.MeasureString(ButtonText,
                new Font("Gilroy-Bold", 9f * zoom, FontStyle.Regular, GraphicsUnit.Point)).Width;

            buttonWidth += 10 * zoom;
            g.FillRectangle(new SolidBrush(_buttonColor), x + width - buttonWidth, y, buttonWidth, height);

            float textWidth = width - buttonWidth;
            g.FillRectangle(new SolidBrush(_color), x, y, textWidth, height);

            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.WordBreak;

            TextRenderer.DrawText(g, ButtonText, new Font("Gilroy-Bold", 9f * zoom, FontStyle.Regular, GraphicsUnit.Point), new Rectangle((int)(x + width - buttonWidth), (int)y, (int)buttonWidth, (int)height), Color.White, flags);

            TextRenderer.DrawText(g, Text, new Font("Gilroy-Bold", 9f * zoom, FontStyle.Regular, GraphicsUnit.Point), new Rectangle((int)x, (int)y, (int)textWidth, (int)height), Color.White, flags);

            //g.FillRectangle(new SolidBrush());
        }
    }
}
