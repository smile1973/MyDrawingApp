using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing.shapes
{
    public abstract class Shape
    {
        public int ID { get; set; }
        public string ShapeType { get; protected set; }
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TextX { get; set; }
        public int TextY { get; set; }
        public int DragPointX { get; set; }
        public int DragPointY { get; set; }

        public abstract void Draw(IGraphics graphics);
    }
}
