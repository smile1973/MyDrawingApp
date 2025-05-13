using MyDrawing.Model;
using MyDrawing.Model.command;
using MyDrawing.shapes;
using System.Collections.Generic;
using System.Linq;

namespace MyDrawing
{
    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public class PointerState : IState
    {
        private readonly ShapeModel _model;
        private Shape _selectedShape;
        private bool _isDragging = false;
        private Coordinate _dragOffset;
        private bool _isDraggingText = false;
        private Coordinate _textDragOffset;
        private int dragPointSize = 6;
        private int _textPosX;
        private int _textPosY;
        private int deltaX, deltaY;
        private List<int> _pos; // old x, old y, old_textx, old_texty

        public PointerState(ShapeModel model)
        {
            _pos = new List<int>();
            _model = model;
        }

        public void HandlePointerPressed(double x, double y)
        {
            Shape clickedShape = _model.GetShapeAt((int)x, (int)y);
            bool neednotify = true;
            if (clickedShape == null)
            {
                _selectedShape = null;
                _model.NotifyModelChanged();
                return;
            }

            _selectedShape = clickedShape;
            _isDragging = true;
            _dragOffset = new Coordinate(
                (int)(x - _selectedShape.X),
                (int)(y - _selectedShape.Y)
            );
            _pos.Clear();
            _pos.AddRange(new List<int> { _selectedShape.X, _selectedShape.Y, _selectedShape.TextX, _selectedShape.TextY });

            if (x >=  _selectedShape.DragPointX - dragPointSize &&
                x <=  _selectedShape.DragPointX + dragPointSize &&
                y >=  _selectedShape.DragPointY - dragPointSize &&
                y <=  _selectedShape.DragPointY + dragPointSize)
            {
                _isDraggingText = true;
            }
            _textDragOffset = new Coordinate(
                (int)(x - _selectedShape.TextX),
                (int)(y - _selectedShape.TextY)
            );
            if (_isDraggingText) neednotify = false;
            if (neednotify) _model.NotifyModelChanged();
        }

        public void HandlePointerMoved(double x, double y)
        {
            if (_isDragging && _selectedShape != null)
            {
                if (_isDraggingText)
                {
                    _selectedShape.TextX = (int)x - _textDragOffset.X;
                    _selectedShape.TextY = (int)y - _textDragOffset.Y;
                    _selectedShape.DragPointX = _selectedShape.TextX + _selectedShape.Text.Length * 5;
                    _selectedShape.DragPointY = _selectedShape.TextY - 4;

                    if (_selectedShape.DragPointX < _selectedShape.X + dragPointSize) _textPosX = _selectedShape.X + 10;
                    if (_selectedShape.DragPointX > _selectedShape.X + _selectedShape.Width - dragPointSize) _textPosX = _selectedShape.X + _selectedShape.Width - 50;
                    if (_selectedShape.DragPointY < _selectedShape.Y + dragPointSize) _textPosY = _selectedShape.Y + 10;
                    if (_selectedShape.DragPointY > _selectedShape.Y + _selectedShape.Height - dragPointSize) _textPosY = _selectedShape.Y + _selectedShape.Height - 10;
                }
                else
                {
                    deltaX = (int)x - (_selectedShape.X + _dragOffset.X);
                    deltaY = (int)y - (_selectedShape.Y + _dragOffset.Y);

                    _selectedShape.X = (int)x - _dragOffset.X;
                    _selectedShape.Y = (int)y - _dragOffset.Y;

                    _selectedShape.TextX = (int)x - _textDragOffset.X;
                    _selectedShape.TextY = (int)y - _textDragOffset.Y;
                    _selectedShape.DragPointX = _selectedShape.TextX + _selectedShape.Text.Length * 5;
                    _selectedShape.DragPointY = _selectedShape.TextY - 4;

                    HandleLine();
                }
                _model.NotifyModelChanged();
            }
        }
        public void HandleLine()
        {
            foreach (Shape shape in _model.GetShapes())
            {
                if (shape is Line line)
                {
                    if (line.StartPoint.Shape == _selectedShape)
                    {
                        line.StartPoint = new ConnectPoint(
                            line.StartPoint.X + deltaX,
                            line.StartPoint.Y + deltaY,
                            _selectedShape
                        );
                    }
                    if (line.EndPoint.Shape == _selectedShape)
                    {
                        line.EndPoint = new ConnectPoint(
                            line.EndPoint.X + deltaX,
                            line.EndPoint.Y + deltaY,
                            _selectedShape
                        );
                    }
                }
            }
        }
        public void HandlePointerReleased(double x, double y)
        {
            if (_isDraggingText)
            {
                if (_textPosX != 0)
                {
                    _selectedShape.TextX = _textPosX;
                    _selectedShape.DragPointX = _selectedShape.TextX + _selectedShape.Text.Length * 5;
                    _textPosX = 0;
                }
                if (_textPosY != 0)
                {
                    _selectedShape.TextY = _textPosY;
                    _selectedShape.DragPointY = _selectedShape.TextY - 4;
                    _textPosY = 0;
                }
                if (_pos[2] != _selectedShape.TextX || _pos[3] != _selectedShape.TextY) 
                {
                    var command = new TextMoveCommand(_selectedShape, _pos);
                    _model.ExecuteCommand(command);
                }
            }

            if (_isDragging && !_isDraggingText && (_pos[0] != _selectedShape.X || _pos[1] != _selectedShape.Y))
            {
                var command = new MoveCommand(_selectedShape, _pos, _model.GetShapes());
                _model.ExecuteCommand(command);
            }

            _isDragging = false;
            _isDraggingText = false;
            _model.NotifyModelChanged();
        }
        public Shape GetSelectedShape()
        {
            return _selectedShape;
        }
        public void HandleShapeDeleted(int shapeId)
        {
            if (_selectedShape?.ID == shapeId)
            {
                _selectedShape = null;
                _isDragging = false;
            }
        }

        public void Draw(IGraphics graphics)
        {
            if (_selectedShape != null)
            {
                graphics.SetPenColor("Red");
                graphics.DrawRectangle(
                    _selectedShape.X,
                    _selectedShape.Y,
                    _selectedShape.Width,
                    _selectedShape.Height
                );
                graphics.SetPen(1);
                graphics.DrawRectangle(
                    _selectedShape.TextX,
                    _selectedShape.TextY - 4,
                    _selectedShape.Text.Length * 10,
                    20
                );
                // point
                graphics.SetPenColor("Orange");
                graphics.SetPen(4);
                graphics.DrawEllipse(
                    _selectedShape.DragPointX,
                    _selectedShape.DragPointY,
                    3,
                    3
                );
            }
        }
    }
}