using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    public class PlayerInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public MoveDirection Facing { get; set; }
    }
}
