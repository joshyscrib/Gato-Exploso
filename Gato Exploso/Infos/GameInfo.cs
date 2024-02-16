using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.Infos
{
    public class GameInfo
    {
        public int GameTime { get; set; }
        public List<PlayerInfo> PlayerInfos { get; set; }
        public List<TileInfo> TileInfos { get; set; }

    }
}
