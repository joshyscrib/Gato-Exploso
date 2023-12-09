using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gato_Exploso
{
  class BouncyTriangle
  {
  int x;
  int y;
  int width = 32;
  int height = 32;
  int health = 40;
  bool good = true;
    public int attack(){
      return 2 * (Math.Random() * 10);
    }
    
    
  }

}
