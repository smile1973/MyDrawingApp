using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrawing.shapes
{
    public class Decision : Shape
    {
        public Decision()
        {
            ShapeType = "Decision";
        }

        public override void Draw(IGraphics graphics)
        {
            int centerX = X + Width / 2;
            int centerY = Y + Height / 2;

            graphics.DrawLine(centerX, Y, X + Width, centerY);
            graphics.DrawLine(X + Width, centerY, centerX, Y + Height);
            graphics.DrawLine(centerX, Y + Height, X, centerY);
            graphics.DrawLine(X, centerY, centerX, Y);
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