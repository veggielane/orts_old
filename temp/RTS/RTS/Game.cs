using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace RTS
{
    public partial class Game : Form
    {
        private Device device = null;
       
        private float angle = 0f;

        Color Blue = Color.FromArgb(94, 169, 198);
        Color Back = Color.FromArgb(34, 30, 31);
        public List<Unit> Units;
        Grid grid;
        //Triangle triangle;
        //Hexahedron hex;
        public Game()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            PresentParameters present_params = new PresentParameters();
            present_params.Windowed = true;
            present_params.SwapEffect = SwapEffect.Discard;
            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, present_params);
            device.DeviceReset += new System.EventHandler(this.OnResetDevice);

            //triangle = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(5f, 0f, 0f), new Vector3(5f, 5f, 0f));
            //triangle.Colour = Back;
            //triangle.Filled = Color.FromArgb(94, 169, 198);
            //hex = new Hexahedron(new Vector3(0f, 0f, 0f), new Vector3(5f, 0f, 0f), new Vector3(5f, 5f, 0f), new Vector3(0f, 5f, 0f), new Vector3(0f, 0f, 5f), new Vector3(5f, 0f, 5f), new Vector3(5f, 5f, 5f), new Vector3(0f, 5f, 5f));
            InitializeComponent();
            Units = new List<Unit>();
            Units.Add(new TestUnit());
            grid = new Grid(10f,10f,5,5);
        }

        public void OnResetDevice(object sender, EventArgs e)
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1f, 50f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, -30), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            device.RenderState.Lighting = false;
            device.RenderState.CullMode = Cull.None;
        }
        private void Game_Paint(object sender, PaintEventArgs e)
        {
            this.Render();
            this.Invalidate();
        }
        private bool mousing = false;
        private Point ptLastMousePosit;
        private Point ptCurrentMousePosit;
        private int spinX;
        private int spinY;

        private void Game_MouseDown(object sender, MouseEventArgs e)
        {
            ptLastMousePosit = ptCurrentMousePosit = PointToScreen(new Point(e.X, e.Y));
            mousing = true;
        }

        private void Game_MouseUp(object sender, MouseEventArgs e)
        {
            mousing = false;
        }

        private void Game_MouseMove(object sender, MouseEventArgs e)
        {
            ptCurrentMousePosit = PointToScreen(new Point(e.X, e.Y));

            if (mousing)
            {
                spinX -= (ptCurrentMousePosit.X - ptLastMousePosit.X);
                spinY -= (ptCurrentMousePosit.Y - ptLastMousePosit.Y);
            }

            ptLastMousePosit = ptCurrentMousePosit;
        }
        public void Render()
        {
            device.Clear(ClearFlags.Target, Color.White.ToArgb(), 1.0f, 0);
            device.BeginScene();
            
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.Transform.World = Matrix.RotationYawPitchRoll(Geometry.DegreeToRadian(spinX), Geometry.DegreeToRadian(spinY), 0.0f) * Matrix.Translation(0.0f, 0.0f, 5.0f);
            foreach (Unit unit in Units)
            {
                unit.Render(device);
            }
            grid.Render(device);
            device.EndScene();
            device.Present();
            angle += 0.05f;
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            device.Dispose();
        }

    }

    /*
    public class Triangle
    {
        public CustomVertex.PositionColored[] vertices;
        public Color Fill = Color.FromArgb(94, 169, 198);

        public Triangle(Vector3 vert1, Vector3 vert2, Vector3 vert3)
        {
            vertices = new CustomVertex.PositionColored[4];
            vertices[0].Position = vert1;
            vertices[0].Color = Color.White.ToArgb();
            vertices[1].Position = vert2;
            vertices[1].Color = Color.White.ToArgb();
            vertices[2].Position = vert3;
            vertices[2].Color = Color.White.ToArgb();
            vertices[3].Position = vert1;
            vertices[3].Color = Color.White.ToArgb();
        }
        public void Render(Device device)
        {
            device.DrawUserPrimitives(PrimitiveType.LineStrip, 3, vertices);  
        }
    }
     
    public class FilledTriangle
    {
        public CustomVertex.PositionColored[] vertices;
        public Color Fill = Color.FromArgb(94, 169, 198);
        public FilledTriangle(Vector3 vert1, Vector3 vert2, Vector3 vert3)
        {
            vertices = new CustomVertex.PositionColored[3];
            vertices[0].Position = vert1;
            vertices[0].Color = Fill.ToArgb();
            vertices[1].Position = vert2;
            vertices[1].Color = Fill.ToArgb();
            vertices[2].Position = vert3;
            vertices[2].Color = Fill.ToArgb();
        }
        public void Render(Device device)
        {
            device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, vertices);
        }
    }
    public class FilledQuadrilateral
    {
        private FilledTriangle[] triangles = new FilledTriangle[2];
        public FilledQuadrilateral(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            triangles[0] = new FilledTriangle(v1, v2, v3);
            triangles[1] = new FilledTriangle(v1, v3, v4);
        }
        public void Render(Device device)
        {
            foreach (FilledTriangle triangle in triangles)
            {
                triangle.Render(device);
            }
        }
    }
    public class Hexahedron
    {
        private FilledQuadrilateral[] quadrilaterals = new FilledQuadrilateral[6];
        public Hexahedron(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 v5, Vector3 v6, Vector3 v7, Vector3 v8)
        {
            quadrilaterals[0] = new FilledQuadrilateral(v1, v2, v3, v4);
            quadrilaterals[1] = new FilledQuadrilateral(v5, v6, v7, v8);
            quadrilaterals[2] = new FilledQuadrilateral(v1, v2, v6, v5);
            quadrilaterals[3] = new FilledQuadrilateral(v2, v6, v7, v3);
            quadrilaterals[4] = new FilledQuadrilateral(v4, v3, v7, v8);
            quadrilaterals[5] = new FilledQuadrilateral(v1, v5, v8, v4);
        }
        public void Render(Device device)
        {
            foreach (FilledQuadrilateral quadrilateral in quadrilaterals)
            {
               quadrilateral.Render(device);
            }
        }
    }
    */
}