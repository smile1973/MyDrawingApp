using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyDrawing.ShapeModel;
using MyDrawing.shapes;

namespace MyDrawing
{
    public class ShapeFactory
    {
        public static Shape CreateShape(string shapeType)
        {
            switch (shapeType)
            {
                case "Start":
                    return new Start();
                case "Terminator":
                    return new Terminator();
                case "Process":
                    return new Processes();
                case "Decision":
                    return new Decision();
                default:
                    return null;
            }
        }
    }
}
