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
        public TestUnit()
        {
            Triangle triangle = new Triangle(new Vector3(2f, 2f, 0f), new Vector3(7f, 2f, 0f), new Vector3(4.5f, 7f, 3f));
            triangle.Colour = Color.Red;
            triangle.Filled = Color.FromArgb(94, 169, 198);
            shapes.Add(triangle);
        }
    }
    public class Unit
    {
        private Vector3 position;
        private Matrix rotation;

        public Vector3 Position
        {
            get{return this.position;}
            set{this.position = value;}
        }
        public Matrix Rotation {
            get { return this.rotation; }
            set { this.rotation = value; }
        }

        protected List<IShape> shapes;
        public Unit()
        {
            shapes = new List<IShape>();
        }
        public void Render(Device device)
        {
            foreach (IShape Shape in shapes)
            {
                Shape.Render(device);
            }
        }
    }

    public class Triangle : IShape
    {
        public Vector3 Position { get; set; }
        public Matrix Rotation { get; set; }
        protected Color colour = Color.Red;
        protected Color filled;
        public CustomVertex.PositionColored[] vertices1;
        public CustomVertex.PositionColored[] vertices2;
        public Color Colour
        {
            get
            {
                return this.colour;
            }
            set
            {
                this.colour = value;
                vertices2[0].Color = this.colour.ToArgb();
                vertices2[1].Color = this.colour.ToArgb();
                vertices2[2].Color = this.colour.ToArgb();
                vertices2[3].Color = this.colour.ToArgb();
            }
        }
        public Color Filled
        {
            get
            {
                return this.filled;
            }
            set
            {
                this.filled = value;
                vertices1 = new CustomVertex.PositionColored[3];
                vertices1[0].Position = vertices2[0].Position;
                vertices1[1].Position = vertices2[1].Position;
                vertices1[2].Position = vertices2[2].Position;
                vertices1[0].Color = this.filled.ToArgb();
                vertices1[1].Color = this.filled.ToArgb();
                vertices1[2].Color = this.filled.ToArgb();

            }
        }

        public Triangle(Vector3 vert1, Vector3 vert2, Vector3 vert3)
        {

            vertices2 = new CustomVertex.PositionColored[4];
            vertices2[0].Position = vertices2[3].Position = vert1;
            vertices2[1].Position = vert2;
            vertices2[2].Position = vert3;
            this.Colour = this.colour;
        }

        public void Render(Device device)
        {
            if (!this.filled.IsEmpty) device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, vertices1);
            device.DrawUserPrimitives(PrimitiveType.LineStrip, 3, vertices2);
        }
    }



    public class Grid : IShape
    {
        public Vector3 Position { get; set; }
        public Matrix Rotation { get; set; }
        public CustomVertex.PositionColored[] vertices;
        public Grid(float Width, float Height, int WidthNo, int HeightNo)
        {
            List<CustomVertex.PositionColored> vertlist = new List<CustomVertex.PositionColored>();
            float CellWidth = Width / WidthNo;
            float CellHeight = Height / HeightNo;

            for (int i = 0; i < WidthNo +1; i++)
            {
                Debug.WriteLine(i);
                vertlist.Add(new CustomVertex.PositionColored(i*CellWidth,0f,0f,Color.Black.ToArgb()));
                vertlist.Add(new CustomVertex.PositionColored(i * CellWidth, Height, 0f, Color.Black.ToArgb()));
            }
            for (int i = 0; i < HeightNo + 1; i++)
            {
                vertlist.Add(new CustomVertex.PositionColored(0f, i * CellHeight, 0f, Color.Black.ToArgb()));
                vertlist.Add(new CustomVertex.PositionColored(Width, i * CellHeight, 0f, Color.Black.ToArgb()));
            }
            vertices = vertlist.ToArray();
        }
        public void Render(Device device)
        {
            device.DrawUserPrimitives(PrimitiveType.LineList, 12, vertices);
        }
    }
    public interface IShape
    {
        Vector3 Position { get; set; }
        Matrix Rotation { get; set; }
        void Render(Device device);
    }

}
