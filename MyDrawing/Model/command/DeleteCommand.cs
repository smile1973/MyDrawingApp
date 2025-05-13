using MyDrawing.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing.Model.command
{
    public class DeleteCommand : ICommand
    {
        private readonly ShapeModel _model;
        private readonly int _shapeId;
        private Shape _deletedShape;
        private List<(Line line, int id)> _connectedLines;

        public DeleteCommand(ShapeModel model, int shapeId)
        {
            _model = model;
            _shapeId = shapeId;
            _connectedLines = new List<(Line line, int id)>();
        }

        public void Execute()
        {
            var shapes = _model.GetShapes();
            _deletedShape = shapes.FirstOrDefault(s => s.ID == _shapeId);

            if (_deletedShape != null)
            {
                // 儲存所有相連的線條及其ID
                _connectedLines = shapes
                    .OfType<Line>()
                    .Where(line =>
                        line.StartPoint.Shape == _deletedShape ||
                        line.EndPoint.Shape == _deletedShape)
                    .Select(line => (line, line.ID))
                    .ToList();

                // 先刪除所有相連的線條
                foreach (var (_, lineId) in _connectedLines)
                {
                    _model.DeleteShape(lineId);
                }

                // 刪除選中的圖形
                _model.DeleteShape(_shapeId);
            }
        }

        public void UnExecute()
        {
            if (_deletedShape != null)
            {
                // 先恢復被刪除的圖形
                _model.AddNewShape(_deletedShape);

                // 恢復所有相連的線條
                foreach (var (line, _) in _connectedLines)
                {
                    _model.AddNewShape(line);
                }
            }
        }
    }
}
