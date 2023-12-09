using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gato_Exploso
{
  class BouncyTriangle
  {
  int x;
  int y;
  int width;
  int height;
  int health;
  bool good;
    public int attack(){
      return 2 * (Math.Random() * 10);
    }
    
    
  }

}
