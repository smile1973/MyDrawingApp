using MyDrawing.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing.Model.command
{
    public class TextChangedCommand : ICommand
    {
        private readonly Shape _shape;
        private ShapeModel _model;
        private string _oldText, _newText;

        public TextChangedCommand(ShapeModel model, Shape shape, string oldText, string newText)
        {
            _model = model;
            _shape = shape;
            _oldText = oldText;
            _newText = newText;
        }

        public void Execute()
        {
            _model.UpdateSelectedShapeText(_shape, _newText);
        }

        public void UnExecute()
        {
            _model.UpdateSelectedShapeText(_shape, _oldText);
        }
    }

}
