using MyDrawing.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing.Model.command
{
    public class AddCommand : ICommand
    {
        private readonly ShapeModel _model;
        private readonly Shape _shape;

        public AddCommand(ShapeModel model, Shape shape)
        {
            _model = model;
            _shape = shape;
        }

        public void Execute()
        {
            _model.AddNewShape(_shape);
        }

        public void UnExecute()
        {
            _model.DeleteShape(_shape.ID);
        }
    }
}
