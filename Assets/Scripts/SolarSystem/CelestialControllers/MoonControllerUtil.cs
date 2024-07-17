using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SolarSystem
{
    public class MoonControllerUtil
    {
        public static List<Vector3> cachedMoonPositions = new ();

        public static void AddCachedPosition(Vector3 position)
        {
            cachedMoonPositions.Add(position);
        }

        public static bool IsPositionValid(Vector3 position, float radius)
        {
            return cachedMoonPositions.All(cachedPosition => !(Vector3.Distance(position, cachedPosition) < radius * 4));
        }
    }
}