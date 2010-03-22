using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace RTS
{
    public interface IShape
    {
        Matrix Position { get; set; }
        void Render(Device device);
        void Render(Device device,Matrix Offset);
    }
}
