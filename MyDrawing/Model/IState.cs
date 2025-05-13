using System;
using System.Reflection;

namespace MyDrawing
{
    public interface IState
    {
        void HandlePointerPressed(double x, double y);
        void HandlePointerMoved(double x, double y);
        void HandlePointerReleased(double x, double y);
        void Draw(IGraphics graphics);
    }
}
