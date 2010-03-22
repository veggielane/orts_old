using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Diagnostics;
using System.Drawing;
namespace RTS
{
    public class TestUnit : Unit
    {
        //private Triangle triangle;

        public TestUnit()
        {
            GenerateGeometry();
        }
        public override void GenerateGeometry()
        {
            shapes.Clear();
            Box box1 = new Box(Matrix.Translation(0, 0, -2f), 6f, 4f, 4f);

            Box box2 = new Box(Matrix.Translation(0, 3f, -1f), 8f, 2f, 2f);
            Material black = new Material();
            black.Ambient = Color.Black;
            box2.material = black;
            Box box3 = new Box(Matrix.Translation(0, -3f, -1f), 8f, 2f, 2f);
            box3.material = black;

            Circle test = new Circle(Matrix.Identity, 10f);

            shapes.Add(box1);
            shapes.Add(box2);
            shapes.Add(box3);
            shapes.Add(test);
            //triangle = new Triangle(Matrix.Translation(-2f, -2.5f, 0f), Matrix.Translation(2f, -2.5f, 0f) , Matrix.Translation(0f, 2.5f, 3f));
            //triangle.Colour = Color.Red;
            //triangle.Filled = Color.Blue;
            //shapes.Add(triangle);
            //shapes.Add(new Model("xwing.x"));
        }
    }
    public class Unit
    {
        protected Matrix position;
        public Matrix Position
        {
            get{return this.position;}
            set{this.position = value;}
        }
        protected List<IShape> shapes;
        public Unit()
        {
            Position = Matrix.Translation(2f, 3f, 4f);
            shapes = new List<IShape>();
        }
        public virtual void GenerateGeometry()
        {
            Debug.WriteLine("You are doing it wrong");
        }
        public void Render(Device device)
        {
            foreach (IShape Shape in shapes)
            {
                Shape.Render(device, position);
            }
        }
    }



    public static class Extensions
    {
        public static Vector3 ToVector3(this Matrix m)
        {
            return new Vector3(m.M41,m.M42,m.M43);
        }
    }
}
