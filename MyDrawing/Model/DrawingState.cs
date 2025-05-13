using MyDrawing.Model.command;
using MyDrawing.shapes;
using System;

namespace MyDrawing
{
    public class DrawingState : IState
    {
        private readonly CommandManager _commandManager;
        private readonly ShapeModel _model;
        private Coordinate _firstPoint;
        private bool _isPressed = false;
        private Shape _previewShape;
        private readonly string _selectedShapeType;

        public DrawingState(ShapeModel model, string shapeType)
        {
            _model = model;
            _selectedShapeType = shapeType;
        }

        public void HandlePointerPressed(double x, double y)
        {
            if (x > 0 && y > 0)
            {
                _firstPoint = new Coordinate((int)x, (int)y);
                _isPressed = true;

                _previewShape = ShapeFactory.CreateShape(_selectedShapeType);
                if (_previewShape != null)
                {
                    _previewShape.X = (int)x;
                    _previewShape.Y = (int)y;
                    _previewShape.Width = 0;
                    _previewShape.Height = 0;
                }
            }
        }

        public void HandlePointerMoved(double x, double y)
        {
            if (_isPressed && _previewShape != null)
            {
                if (_previewShape.ShapeType == "Terminator")
                {
                    UpdateTerminatorSize(_previewShape, x, y);
                }
                else
                {
                    UpdateShapeSize(_previewShape, x, y);
                }
                _model.NotifyModelChanged();
            }
        }

        public void HandlePointerReleased(double x, double y)
        {
            if (_isPressed && _previewShape != null)
            {
                _isPressed = false;
                Shape final = ShapeFactory.CreateShape(_selectedShapeType);
                if (final != null)
                {
                    final.ID = _model.GetNextId();
                    final.X = _previewShape.X;
                    final.Y = _previewShape.Y;
                    final.Width = _previewShape.Width;
                    final.Height = _previewShape.Height;
                    final.Text = _model.GenerateRandomText();

                    var command = new DrawCommand(_model, final);
                    _model.ExecuteCommand(command);

                    _previewShape = null;
                    _model.NotifyModelChanged();
                    _model.EnterPointerState();
                }
            }
        }

        public void Draw(IGraphics graphics)
        {
            if (_previewShape != null)
            {
                _previewShape.Draw(graphics);
            }
        }

        private void UpdateShapeSize(Shape shape, double currentX, double currentY)
        {
            shape.Width = Math.Abs((int)(currentX - _firstPoint.X));
            shape.Height = Math.Abs((int)(currentY - _firstPoint.Y));

            if (currentX < _firstPoint.X)
                shape.X = (int)currentX;
            if (currentY < _firstPoint.Y)
                shape.Y = (int)currentY;
        }

        private void UpdateTerminatorSize(Shape shape, double currentX, double currentY)
        {
            int width = Math.Abs((int)(currentX - _firstPoint.X));
            int height = Math.Abs((int)(currentY - _firstPoint.Y));

            height = Math.Max(height, 30);
            width = Math.Max(width, height * 3 / 2);

            shape.Width = width;
            shape.Height = height;

            if (currentX < _firstPoint.X)
                shape.X = (int)currentX;
            if (currentY < _firstPoint.Y)
                shape.Y = (int)currentY;
        }
    }
}