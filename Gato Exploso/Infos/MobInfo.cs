using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.Infos
{
    public class MobInfo
    {
        public int Id { get; set; }
        public MobType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public MoveDirection Facing { get; set; }

        public String CreateInfoString()
        {
            return String.Format("{0},{1},{2},{3},{4}",Id,  (int)Type, X, Y, Health);
        }
    }
}
