using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Gato_Exploso
{
  public class BouncyTriangle : Entity
  {
  protected ContentManager Content;
  protected Texture2D texture;
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
