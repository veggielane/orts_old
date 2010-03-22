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
        Color Blue = Color.FromArgb(94, 169, 198);
        Color Back = Color.FromArgb(34, 30, 31);
        public List<Unit> Units;
        Grid grid;
        public Game()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            InitializeComponent();
            InitializeDevice();
            Units = new List<Unit>();
                    int row = 0;
            int col = 1;
            float spacing = 9f;

            Units.Add(new TestUnit());

            Units[0].Position = Matrix.RotationY((float)Math.PI/2)*Matrix.Translation(row++ * spacing, col * spacing, 0);
            Units.Add(new TestUnit());
            Units[1].Position = Matrix.Translation(row++ * spacing, col * spacing, 0);
            Units.Add(new TestUnit());
            Units[2].Position = Matrix.Translation(row++ * spacing, col * spacing, 0);
            Units.Add(new TestUnit());
            Units[3].Position = Matrix.Translation(row++ * spacing, col * spacing, 0);
            Units.Add(new TestUnit());
            Units[4].Position = Matrix.Translation(row++ * spacing, col * spacing, 0);
            Units.Add(new TestUnit());
            Units[5].Position = Matrix.Translation(row++ * spacing, col * spacing, 0);
            Units.Add(new TestUnit());
            Units[6].Position = Matrix.Translation(row++ * spacing, col * spacing, 0);
            Units.Add(new TestUnit());
            Units[7].Position = Matrix.Translation(row++ * spacing, col * spacing, 0);





            grid = new Grid(10f,10f,5,5);
         }

        public void InitializeDevice()
        {
            Caps caps = Manager.GetDeviceCaps(Manager.Adapters.Default.Adapter, DeviceType.Hardware);
            CreateFlags flags;
            if (caps.DeviceCaps.SupportsHardwareTransformAndLight)
                flags = CreateFlags.HardwareVertexProcessing;
            else
                flags = CreateFlags.SoftwareVertexProcessing;
            PresentParameters present_params = new PresentParameters();
            present_params.Windowed = true;
            present_params.AutoDepthStencilFormat = DepthFormat.D16;
            present_params.EnableAutoDepthStencil = true;
            //present_params.AutoDepthStencilFormat = DepthFormat.D16;
            present_params.SwapEffect = SwapEffect.Discard;
            device = new Device(0, DeviceType.Hardware, this, flags, present_params);
            device.DeviceReset += new System.EventHandler(this.OnResetDevice);
            OnResetDevice(device, null);
        }

        public void OnResetDevice(object sender, EventArgs e)
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1f, 50f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 30), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            device.RenderState.CullMode = Cull.CounterClockwise;//Should not be set
            device.RenderState.Ambient = Color.White;
            device.RenderState.Lighting = true;
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
        private float test=1;
        private float scaling = 0.05f;
        public void Render()
        {
            device.Clear(ClearFlags.Target|ClearFlags.ZBuffer, Color.WhiteSmoke.ToArgb(), 1.0f, 0);
            device.BeginScene();
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.Transform.View = Matrix.Scaling(scaling, scaling, scaling) * Matrix.RotationYawPitchRoll(Geometry.DegreeToRadian(spinX), Geometry.DegreeToRadian(spinY), 0.0f) * Matrix.Translation(0.0f, 0.0f, 5.0f);
            grid.Render(device);
            foreach (Unit unit in Units)
            {
                unit.Render(device);
            }
            device.EndScene();
            device.Present();
            Units[0].Position = Matrix.RotationZ(Geometry.DegreeToRadian(test)) * Units[0].Position;
            Units[1].Position = Matrix.RotationZ(Geometry.DegreeToRadian(-test)) * Units[1].Position;
            Units[2].Position = Matrix.RotationZ(Geometry.DegreeToRadian(test*2)) * Units[2].Position;
            Units[3].Position = Matrix.RotationZ(Geometry.DegreeToRadian(-test)) * Units[3].Position;
            Units[4].Position = Matrix.RotationZ(Geometry.DegreeToRadian(test)) * Units[4].Position;
            Units[5].Position = Matrix.RotationZ(Geometry.DegreeToRadian(-test)) * Units[5].Position;
            //Units[1].Position = Matrix.RotationZ(Geometry.DegreeToRadian(-test)) * Matrix.Translation(0f, 5f, 0f) * Matrix.RotationZ(Geometry.DegreeToRadian(-test));
            //test = test + 1f;
            //device.Transform.World = Matrix.RotationZ(Geometry.DegreeToRadian(test/2));
            //Matrix.Translation(0.005f, 0.005f, 0.005f)
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            device.Dispose();
        }
    }
}