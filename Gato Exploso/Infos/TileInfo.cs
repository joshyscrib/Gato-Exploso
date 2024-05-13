using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.Infos
{
    public class TileInfo
    {
        // location
        public int X { get; set; }
        public int Y { get; set; }
        public List<ObjectInfo> ObjectInfos { get; set; }
        public int State { get; set; }

        public String CreateInfoString()
        {
            return String.Format("{0},{1},{2}. {3}", X, Y, State);
        }
    }
}
