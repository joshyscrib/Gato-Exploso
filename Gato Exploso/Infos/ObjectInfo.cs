using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso.Infos
{
    public class ObjectInfo
    {
        // type of object
        public string ObjectType { get; set; }

        public String CreateInfoString(int x, int y)
        {
            string objType = "0";
            if (ObjectType == "bomb")
            {
                objType = "b";
            }
            if (ObjectType == "rock")
            {
                objType = "r";
            }
            if (ObjectType == "tree")
            {
                objType = "t";
            }

            return String.Format("{0},{1},{2}", x, y, objType);
        }
    }
}
