using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing
{
    public interface IGraphics
    {
        void ClearAll();
        void DrawLine(double x1, double y1, double x2, double y2);
        void DrawRectangle(double x, double y, double width, double height);
        void DrawEllipse(double x, double y, double width, double height);
        void DrawString(string text, double x, double y);
        void DrawArc(int x, int y, int width, int height, float startAngle, float sweepAngle);
        void SetPenColor(string color);
        void SetPen(int x);
    }
}
