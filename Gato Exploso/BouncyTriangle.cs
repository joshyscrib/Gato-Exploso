using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Gato_Exploso
{
  class BouncyTriangle
  {
    // variables
    int x;
    int y;
    int width = 32;
    int height = 32;
    int health = 40;
    bool good = true;
   
    // methods
    public int attack(){
       Random r = new Random();
      return r.Next()*20;
    }
    
    
  }

}
