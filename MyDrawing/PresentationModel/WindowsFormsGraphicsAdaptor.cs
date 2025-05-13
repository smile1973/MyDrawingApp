using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyDrawing.ShapeModel;

namespace MyDrawing.PresentationModel
{
    class WindowsFormsGraphicsAdaptor : IGraphics
    {
        private readonly Graphics _graphics;
        private readonly Pen _defaultPen;
        private readonly SolidBrush _defaultBrush;
        private readonly Font _defaultFont;
        public WindowsFormsGraphicsAdaptor(Graphics graphics)
        {
            this._graphics = graphics;
            _defaultPen = new Pen(Color.Black, 2);
            _defaultBrush = new SolidBrush(Color.Black);
            _defaultFont = SystemFonts.DefaultFont;
        }
        public void ClearAll()
        {

        }
        public void DrawLine(double x1, double y1, double x2, double y2)
        {
            _graphics.DrawLine(_defaultPen, (float)x1, (float)y1, (float)x2, (float)y2);
        }
        public void DrawRectangle(double x, double y, double width, double height)
        {
            _graphics.DrawRectangle(_defaultPen, (float)x, (float)y, (float)width, (float)height);
        }
        public void DrawEllipse(double x, double y, double width, double height)
        {
            _graphics.DrawEllipse(_defaultPen, (float)x, (float)y, (float)width, (float)height);
        }
        public void DrawArc(int x, int y, int width, int height, float startAngle, float sweepAngle)
        {
            _graphics.DrawArc(_defaultPen, x, y, width, height, startAngle, sweepAngle);
        }
        public void DrawString(string text, double x, double y)
        {
            _graphics.DrawString(text, _defaultFont, _defaultBrush, (float)x, (float)y);
        }
        public void SetPenColor(string color)
        {
            _defaultPen.Color = Color.FromName(color);
        }
        public void SetPen(int x)
        {
            _defaultPen.Width = x;
        }
    }
}
