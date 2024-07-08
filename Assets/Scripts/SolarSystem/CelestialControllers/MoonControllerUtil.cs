using System.Collections.Generic;
using UnityEngine;

namespace SolarSystem
{
    public class MoonControllerUtil
    {
        private static List<Vector3> cachedMoonPositions = new ();

        public static void AddCachedPosition(Vector3 position)
        {
            cachedMoonPositions.Add(position);
        }

        public static bool IsPositionValid(Vector3 position, float radius)
        {
            foreach (Vector3 cachedPosition in cachedMoonPositions)
            {
                if (Vector3.Distance(position, cachedPosition) < radius * 2)
                {
                    return false;
                }
            }
            return true;
        }
    }
}