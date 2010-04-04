using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;

namespace Orts.Core.Players
{
    public class PlayerController
    {
        public GOGroup SelectedObjects { get; set; }

        public PlayerController()
        {
            SelectedObjects = GOGroup.Empty;
        }
    }
}
