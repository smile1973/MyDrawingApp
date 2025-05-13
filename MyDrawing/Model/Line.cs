using MyDrawing.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing.Model
{
    public class Line : Shape
    {
        public ConnectPoint StartPoint { get; set; }
        public ConnectPoint EndPoint { get; set; }

        public Line()
        {
            ShapeType = "Line";
            Text = "";
        }

        public override void Draw(IGraphics graphics)
        {
            if (StartPoint != null && EndPoint != null)
            {
                UpdateLineProperties();

                double startXRatio = (StartPoint.X - StartPoint.Shape.X) / StartPoint.Shape.Width;
                double startYRatio = (StartPoint.Y - StartPoint.Shape.Y) / StartPoint.Shape.Height;
                double endXRatio = (EndPoint.X - EndPoint.Shape.X) / EndPoint.Shape.Width;
                double endYRatio = (EndPoint.Y - EndPoint.Shape.Y) / EndPoint.Shape.Height;

                double startX = StartPoint.Shape.X + (StartPoint.Shape.Width * startXRatio);
                double startY = StartPoint.Shape.Y + (StartPoint.Shape.Height * startYRatio);
                double endX = EndPoint.Shape.X + (EndPoint.Shape.Width * endXRatio);
                double endY = EndPoint.Shape.Y + (EndPoint.Shape.Height * endYRatio);

                graphics.SetPenColor("Black");
                graphics.DrawLine(startX, startY, endX, endY);
            }
        }

        public void UpdateLineProperties()
        {
            X = (int)StartPoint.X;
            Y = (int)StartPoint.Y;

            Width = Math.Abs((int)(EndPoint.X - StartPoint.X));
            Height = Math.Abs((int)(EndPoint.Y - StartPoint.Y));
        }
    }
}
