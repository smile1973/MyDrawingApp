using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing.shapes
{
    public class Start : Shape
    {
        public Start()
        {
            ShapeType = "Start";
        }
        public override void Draw(IGraphics graphics)
        {
            graphics.DrawEllipse(X, Y, Width, Height);
            if (TextX == 0 && TextY == 0)
            {
                TextX = X + (Width / 5);
                TextY = Y + (Height / 2);
            }
            if (!string.IsNullOrEmpty(Text))
            {
                if (DragPointX == 0 && DragPointY == 0)
                {
                    DragPointX = TextX + Text.Length * 5;
                    DragPointY = TextY - 4;
                }
                graphics.DrawString(Text, TextX, TextY);
            }
            
        }
    }
}
