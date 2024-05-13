using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.Infos
{
    public class PlayerInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public MoveDirection Facing { get; set; }
        public int Points { get; set; }

        public String CreateInfoString()
        {
            int facing = 0;
            if (Facing != null)
            {
                if (Facing.Up) facing = 1;
                if (Facing.Down) facing = 2;
                if (Facing.Left)
                    facing = 3;
                if (Facing.Right) facing = 4;
            }

            return String.Format("{0},{1},{2},{3},{4}", Name, X, Y, Health, facing);
        }
    }
}
