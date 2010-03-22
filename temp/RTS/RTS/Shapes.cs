using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace RTS
{
    public class Triangle : IShape
    {
        public Matrix Position { get; set; }
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
        public Triangle(Matrix vert1, Matrix vert2, Matrix vert3)
            : this(vert1.ToVector3(), vert2.ToVector3(), vert3.ToVector3())
        {

        }
        public void Render(Device device)
        {
            if (!this.filled.IsEmpty) device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, vertices1);
            device.DrawUserPrimitives(PrimitiveType.LineStrip, 3, vertices2);
        }
        public void Render(Device device, Matrix Offset)
        {

        }
    }

    public class Circle : IShape
    {
        protected Matrix position = Matrix.Identity;
        public Matrix Position
        {
            get { return this.position; }
            set { this.position = value; }
        }
        public CustomVertex.PositionColored[] vertices;
        public Circle(Matrix Position,float Radius)
        {
            List<CustomVertex.PositionColored> vertlist = new List<CustomVertex.PositionColored>();

            float smoothness = 0.05f;

            for(float angle= 0.0f; angle<= (2.0f * Math.PI); angle += smoothness)
            {
                vertlist.Add(new CustomVertex.PositionColored(Position.M41 + (Radius* (float)Math.Sin(angle)), Position.M42 + (Radius * (float)Math.Cos(angle)), 0f, Color.Red.ToArgb()));
            }
            vertlist.Add(new CustomVertex.PositionColored(Position.M41, Position.M42 + Radius, 0f, Color.Black.ToArgb()));
            vertices = vertlist.ToArray();
        }
        public void Render(Device device)
        {
            device.Transform.World = Matrix.Identity;
        }
        public void Render(Device device, Matrix Offset)
        {

            device.Transform.World = this.Position * Offset;
            device.DrawUserPrimitives(PrimitiveType.LineStrip, vertices.Length-1, vertices);
            //device.DrawUserPrimitives(PrimitiveType.LineList, 12, vertices);
        }
    }

    public class Box : IShape
    {
        protected Matrix position = Matrix.Identity;
        public Material material;
        public Matrix Position
        {
            get { return this.position; }
            set { this.position = value; }
        }
        private Mesh mesh = null;
        float Width, Height, Depth;
        public Box(Matrix Position, float Width,float Height,float Depth)
        {
            this.Position = Position;
            this.Width = Width;
            this.Height = Height;
            this.Depth = Depth;
            material = new Material();
            material.Ambient = Color.FromArgb(255,0,255,0);
            material.Diffuse = Color.FromArgb(255, 0, 255, 0);
            material.Emissive = Color.FromArgb(255, 0, 255, 0);
        }
        public void GenerateGeometry(Device device)
        {
            mesh = Mesh.Box(device, this.Width, this.Height, this.Depth);
        }

        public void Render(Device device)
        {
            if (mesh == null) GenerateGeometry(device);
            device.Transform.World = position;
            mesh.DrawSubset(0);
        }
        public void Render(Device device, Matrix Offset)
        {
            if (mesh == null) GenerateGeometry(device);
            device.Transform.World = this.Position * Offset;
            device.Material = material;
            mesh.DrawSubset(0);
        }
    }


    public class Model : IShape
    {
        public Matrix Position { get; set; }

        private Mesh mesh;
        private Material[] meshmaterials;
        private Texture[] meshtextures;
        private ExtendedMaterial[] materialarray;
        private Boolean Loaded = false;
        private String Name;
        public Model(String Name)
        {
            this.Name = Name;
            //return 
        }
        public void Render(Device device, Matrix Offset)
        {

        }
        public void Render(Device device)
        {
            if (!Loaded)
            {
                mesh = Mesh.FromFile(Name, MeshFlags.Managed, device, out materialarray);
                if ((materialarray != null) && (materialarray.Length > 0))
                {
                    meshmaterials = new Material[materialarray.Length];
                    meshtextures = new Texture[materialarray.Length];

                    for (int i = 0; i < materialarray.Length; i++)
                    {
                        meshmaterials[i] = materialarray[i].Material3D;
                        meshmaterials[i].Ambient = meshmaterials[i].Diffuse;

                        if ((materialarray[i].TextureFilename != null) && (materialarray[i].TextureFilename != string.Empty))
                        {
                            meshtextures[i] = TextureLoader.FromFile(device, materialarray[i].TextureFilename);
                        }
                    }
                }
                mesh = mesh.Clone(mesh.Options.Value, CustomVertex.PositionNormalTextured.Format, device);
                mesh.ComputeNormals();

                Loaded = true;
            }
            for (int i = 0; i < meshmaterials.Length; i++)
            {
                device.Material = meshmaterials[i];
                device.SetTexture(0, meshtextures[i]);
                mesh.DrawSubset(i);
            }
            //device.DrawUserPrimitives(PrimitiveType.LineList, 12, vertices);
        }
    }

    public class Grid : IShape
    {
        public Matrix Position { get; set; }
        public CustomVertex.PositionColored[] vertices;
        public Grid(float Width, float Height, int WidthNo, int HeightNo)
        {
            List<CustomVertex.PositionColored> vertlist = new List<CustomVertex.PositionColored>();
            float CellWidth = Width / WidthNo;
            float CellHeight = Height / HeightNo;

            for (int i = 0; i < WidthNo + 1; i++)
            {
                vertlist.Add(new CustomVertex.PositionColored(i * CellWidth, 0f, 0f, Color.Black.ToArgb()));
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
            device.Transform.World = Matrix.Identity;
            device.DrawUserPrimitives(PrimitiveType.LineList, 12, vertices);
        }
        public void Render(Device device,Matrix Offset)
        {
            device.Transform.World = Matrix.Identity;
            device.DrawUserPrimitives(PrimitiveType.LineList, 12, vertices);
        }
    }
}
