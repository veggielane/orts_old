using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.GameObjects
{
    public class GOGroup
    {
        public List<IGameObject> Objects { get; private set; }

        public GOGroup(List<IGameObject> objects)
        {
            Objects = objects;
        }

        public GOGroup(IGameObject obj)
        {
            Objects = new List<IGameObject>();

            Objects.Add(obj);
        }

        public static GOGroup Empty
        {
            get { return new GOGroup(new List<IGameObject>()); }
        }
    }
}
