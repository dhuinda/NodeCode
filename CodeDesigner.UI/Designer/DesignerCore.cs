using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Designer.Canvas;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Windows.Resources.Controls.Node;

namespace CodeDesigner.UI.Designer
{
    public class DesignerCore
    {
        public readonly Windows.Designer Form;
        public readonly CanvasCore Canvas;

        private List<ToolboxNode> _nodes;
        private int _blockTypeIndex = 0;

        private readonly string[] _blockTypes =
        {
            "LOGIC",
            "VARIABLES",
            "LISTS",
            "LOOPS",
            "OUTPUT"
        };

        public DesignerCore(Windows.Designer form)
        {
            Form = form;
            _nodes = new List<ToolboxNode>();
            Canvas = new CanvasCore(this);
            UpdateToolbox();
        }

        #region Toolbox

        public void IncrementBlockType(int direction = 1)
        {
            if (direction == 1)
                _blockTypeIndex++;
            else
                _blockTypeIndex--;

            if (_blockTypeIndex < 0)
                _blockTypeIndex = 4;
            
            if (_blockTypeIndex > 4)
                _blockTypeIndex = 0;

            Form.BlockTypePanel.Content = _blockTypes[_blockTypeIndex];
            Form.BlockTypePanel.Invalidate();

            UpdateToolbox();
        }

        public void UpdateToolbox()
        {
            switch (_blockTypeIndex)
            {
                case 0:
                    AddLogicNodes();
                    break;
                case 1:
                    AddVariableNodes();
                    break;
                case 2:
                    AddListNodes();
                    break;
                case 3:
                    AddLoopNodes();
                    break;
                case 4:
                    AddOutputNodes();
                    break;
            }
        }

        private void AddLogicNodes()
        {
            DisposeNodes();
            _nodes.Add(new ToolboxNode(NodeTypes.FUNCTION_DEFINITION, this));
            PlaceNodes();
        }

        private void AddVariableNodes()
        {
            DisposeNodes();

            _nodes.AddRange(new []
            {
                new ToolboxNode(NodeTypes.NEW_VARIABLE, this),
                new ToolboxNode(NodeTypes.NEW_VARIABLE, this),
                new ToolboxNode(NodeTypes.NEW_VARIABLE, this),
                new ToolboxNode(NodeTypes.NEW_VARIABLE, this)
            });

            PlaceNodes();
        }

        private void AddListNodes()
        {
            DisposeNodes();
            PlaceNodes();
        }

        private void AddLoopNodes()
        {
            DisposeNodes();
            PlaceNodes();
        }

        private void AddOutputNodes()
        {
            DisposeNodes();
            PlaceNodes();
        }

        private void PlaceNodes()
        {
            int top = 0;
            foreach (ToolboxNode node in _nodes)
            {
                Form.BlockPanel.Controls.Add(node);
                node.Size = new Size(152, 27);

                node.BorderRadius = 10;

                node.GradientOne = Color.FromArgb(34, 39, 49);
                node.GradientTwo =Color.FromArgb(34, 39, 49);
                node.TextColor = Color.White;

                node.Left = (Form.BlockPanel.ClientRectangle.Width - node.Width) / 2;

                node.Location = new Point(node.Location.X, top);

                top += 35;
            }
        }

        private void DisposeNodes()
        {
            foreach (ToolboxNode node in _nodes)
            {
                node.Hide();
                node.Dispose();
            }

            _nodes.Clear();
        }

        #endregion
    }
}
