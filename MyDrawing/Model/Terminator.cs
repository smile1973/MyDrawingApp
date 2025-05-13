using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing.shapes
{
    public class Terminator : Shape
    {
        public Terminator()
        {
            ShapeType = "Terminator";
        }
        public override void Draw(IGraphics graphics)
        {
            // 圓弧半徑
            int radius = Height / 2;

            // 寬度是高度的1.5倍
            int minWidth = Height * 3 / 2;
            int actualWidth = Math.Max(Width, minWidth);

            // 中間寬度
            int rectWidth = actualWidth - Height;

            // 左圓
            graphics.DrawArc(X, Y, Height, Height, 90, 180);

            // 右圓
            graphics.DrawArc(X + rectWidth, Y, Height, Height, 270, 180);

            // 上下兩條
            graphics.DrawLine(
                X + radius,             // 左圓弧中心點X
                Y,                      // 上邊緣Y
                X + rectWidth + radius, // 右圓弧中心點X
                Y                       // 上邊緣Y
            );

            graphics.DrawLine(
                X + radius,             // 左圓弧中心點X
                Y + Height,             // 下邊緣Y
                X + rectWidth + radius, // 右圓弧中心點X
                Y + Height              // 下邊緣Y
            );
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
