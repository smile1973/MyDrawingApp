using MyDrawing.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace MyDrawing.Model.command
{
    public class TextMoveCommand : ICommand
    {
        private readonly Shape _shape;
        private int _oldTextX, _oldTextY;
        private int _newTextX, _newTextY;

        public TextMoveCommand(Shape shape, List<int> pos)
        {
            _shape = shape;
            _newTextX = _shape.TextX;
            _newTextY = _shape.TextY;
            _oldTextX = pos[2];
            _oldTextY = pos[3];
        }

        public void Execute()
        {
            _shape.TextX = _newTextX;
            _shape.TextY = _newTextY;
            _shape.DragPointX = _shape.TextX + _shape.Text.Length * 5;
            _shape.DragPointY = _shape.TextY - 4;
        }

        public void UnExecute()
        {
            _shape.TextX = _oldTextX;
            _shape.TextY = _oldTextY;
            _shape.DragPointX = _shape.TextX + _shape.Text.Length * 5;
            _shape.DragPointY = _shape.TextY - 4;
        }
    }
}
