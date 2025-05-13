using MyDrawing.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDrawing.Model;
using MyDrawing.Model.command;

namespace MyDrawing
{
    public class DrawLineState : IState
    {
        private readonly ShapeModel _model;
        private ConnectPoint _startPoint;
        private bool _isDrawing = false;
        private double _currentX, _currentY;
        private Shape _hoveredShape;

        public DrawLineState(ShapeModel model)
        {
            _model = model;
        }

        public void HandlePointerPressed(double x, double y)
        {
            var shape = _model.GetShapeAt((int)x, (int)y);
            if (shape != null)
            {
                var point = GetNearestConnectPoint(shape, x, y);
                if (point != null)
                {
                    _startPoint = point;
                    _isDrawing = true;
                    _currentX = point.X;
                    _currentY = point.Y;
                }
            }
        }

        public void HandlePointerMoved(double x, double y)
        {
            _hoveredShape = _model.GetShapeAt((int)x, (int)y);

            if (_isDrawing)
            {
                _currentX = x;
                _currentY = y;
            }
            _model.NotifyModelChanged();
        }

        public void HandlePointerReleased(double x, double y)
        {
            if (_isDrawing)
            {
                var endShape = _model.GetShapeAt((int)x, (int)y);
                if (endShape != null)
                {
                    var endPoint = GetNearestConnectPoint(endShape, x, y);
                    if (endPoint != null && !(endPoint.Shape == _startPoint.Shape && endPoint.X == _startPoint.X && endPoint.Y == _startPoint.Y))
                    {
                        var line = new Line
                        {
                            ID = _model.GetNextId(),
                            StartPoint = _startPoint,
                            EndPoint = endPoint
                        };
                        line.UpdateLineProperties();
                        var command = new DrawCommand(_model, line);
                        _model.ExecuteCommand(command);
                        _model.EnterPointerState();
                    }
                }
                _isDrawing = false;
                _startPoint = null;
                _hoveredShape = null;
                _model.NotifyModelChanged();
            }
        }

        public void Draw(IGraphics graphics)
        {
            if (_hoveredShape != null && !(_hoveredShape is Line))
            {
                DrawConnectPoints(graphics, _hoveredShape);
            }
            if (_isDrawing && _startPoint != null)
            {
                DrawConnectPoints(graphics, _startPoint.Shape);
                graphics.SetPenColor("Black");
                graphics.DrawLine(_startPoint.X, _startPoint.Y, _currentX, _currentY);
            }
        }

        private void DrawConnectPoints(IGraphics graphics, Shape shape)
        {
            var points = GetConnectPoints(shape);
            graphics.SetPenColor("Blue");
            foreach (var point in points)
            {
                graphics.DrawEllipse(point.X - 3, point.Y - 3, 6, 6);
            }
        }

        private ConnectPoint GetNearestConnectPoint(Shape shape, double x, double y)
        {
            const int threshold = 10;
            var points = GetConnectPoints(shape);
            foreach (var point in points)
            {
                if (Math.Abs(point.X - x) <= threshold && Math.Abs(point.Y - y) <= threshold)
                {
                    return point;
                }
            }
            return null;
        }

        private ConnectPoint[] GetConnectPoints(Shape shape)
        {
            return new[]
            {
                new ConnectPoint(shape.X + shape.Width / 2, shape.Y, shape),
                new ConnectPoint(shape.X + shape.Width, shape.Y + shape.Height / 2, shape),
                new ConnectPoint(shape.X + shape.Width / 2, shape.Y + shape.Height, shape),
                new ConnectPoint(shape.X, shape.Y + shape.Height / 2, shape)
            };
        }
    }

    public class ConnectPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Shape Shape { get; set; }

        public ConnectPoint(double x, double y, Shape shape)
        {
            X = x;
            Y = y;
            Shape = shape;
        }
    }
}