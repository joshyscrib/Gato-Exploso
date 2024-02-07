using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    public class PlayerActionArgs
    {
        public MoveDirection direction { get; set; }
        public bool placeBomb { get; set; }
        public string name { get; set; }
        public bool attack { get; set; }
    }
}
