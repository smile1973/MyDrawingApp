using MyDrawing.Model;
using MyDrawing.shapes;
using MyDrawing;
using System.Collections.Generic;

public class MoveCommand : ICommand
{
    private readonly Shape _shape;
    private int _oldX, _oldY, _oldTextX, _oldTextY;
    private int _newX, _newY, _newTextX, _newTextY;
    private readonly List<Shape> _shapes;

    public MoveCommand(Shape shape, List<int> pos, List<Shape> shapes)
    {
        _shape = shape;
        _oldX = pos[0];
        _oldY = pos[1];
        _oldTextX = pos[2];
        _oldTextY = pos[3];
        _newX = _shape.X;
        _newY = _shape.Y;
        _newTextX = _shape.TextX;
        _newTextY = _shape.TextY;
        _shapes = shapes;
    }

    public void Execute()
    {
        int deltaX = _newX - _shape.X;
        int deltaY = _newY - _shape.Y;

        _shape.X = _newX;
        _shape.Y = _newY;
        _shape.TextX = _newTextX;
        _shape.TextY = _newTextY;
        _shape.DragPointX = _shape.TextX + _shape.Text.Length * 5;
        _shape.DragPointY = _shape.TextY - 4;

        foreach (Shape shape in _shapes)
        {
            if (shape is Line line)
            {
                if (line.StartPoint.Shape == _shape)
                {
                    line.StartPoint = new ConnectPoint(
                        line.StartPoint.X + deltaX,
                        line.StartPoint.Y + deltaY,
                        _shape
                    );
                }
                if (line.EndPoint.Shape == _shape)
                {
                    line.EndPoint = new ConnectPoint(
                        line.EndPoint.X + deltaX,
                        line.EndPoint.Y + deltaY,
                        _shape
                    );
                }
            }
        }
    }

    public void UnExecute()
    {
        int deltaX = _oldX - _shape.X;
        int deltaY = _oldY - _shape.Y;

        _newX = _shape.X;
        _newY = _shape.Y;
        _newTextX = _shape.TextX;
        _newTextY = _shape.TextY;
        _shape.X = _oldX;
        _shape.Y = _oldY;
        _shape.TextX = _oldTextX;
        _shape.TextY = _oldTextY;
        _shape.DragPointX = _shape.TextX + _shape.Text.Length * 5;
        _shape.DragPointY = _shape.TextY - 4;

        foreach (Shape shape in _shapes)
        {
            if (shape is Line line)
            {
                if (line.StartPoint.Shape == _shape)
                {
                    line.StartPoint = new ConnectPoint(
                        line.StartPoint.X + deltaX,
                        line.StartPoint.Y + deltaY,
                        _shape
                    );
                }
                if (line.EndPoint.Shape == _shape)
                {
                    line.EndPoint = new ConnectPoint(
                        line.EndPoint.X + deltaX,
                        line.EndPoint.Y + deltaY,
                        _shape
                    );
                }
            }
        }
    }
}