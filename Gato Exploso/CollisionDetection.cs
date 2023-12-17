using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gato_Exploso
{
    // static class to detect when 2 objects are colliding
    internal static class CollisionDetection
    {
        static CollisionDetection()
        {

        }
        // checks if a single point is in a rectangle
        static bool IsPointInRect(int x, int y, int width, int height, Vector2 point)
        {
            if (point.X >= x && point.X <= x + width && point.Y >= y && point.Y <= y)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        // checks if 2 rectangles are touching each other
        static bool AreRectsInEachOther(int x1, int y1, int width1, int height1, Entity entity)
        {
            Vector2 TL1 = new Vector2(x1, y1);
            Vector2 TL2 = new Vector2(entity.x, entity.y);
            // checks 1st rectangle to 2nd rectangle
            if ((IsPointInRect((int)TL2.X, (int)TL2.Y, entity.width, entity.height, TL1) ||
                IsPointInRect((int)TL2.X, (int)TL2.Y, entity.width, entity.height, new Vector2(TL1.X + width1, TL1.Y)) ||
                IsPointInRect((int)TL2.X, (int)TL2.Y, entity.width, entity.height, new Vector2(TL1.X, TL1.Y + height1)) ||
                IsPointInRect((int)TL2.X, (int)TL2.Y, entity.width, entity.height, new Vector2(TL1.X + width1, TL1.Y + height1))) ||
                // checks 2nd rectangle to first rectangle
                IsPointInRect((int)TL1.X, (int)TL1.Y, width1, height1, TL2) ||
                IsPointInRect((int)TL1.X, (int)TL1.Y, width1, height1, new Vector2(TL2.X + entity.width, TL2.Y)) ||
                IsPointInRect((int)TL1.X, (int)TL1.Y, width1, height1, new Vector2(TL2.X, TL2.Y + entity.height)) ||
                IsPointInRect((int)TL1.X, (int)TL1.Y, width1, height1, new Vector2(TL2.X + entity.width, TL2.Y + entity.height)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
