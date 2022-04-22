using UnityEngine;

namespace NearestNeighbour
{
    public static class BoundsExtensions
    {
        public static Vector3 GetRandomPointWithin(this Bounds bounds)
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            return new Vector3
            {
                x = Random.Range(min.x, max.x),
                y = Random.Range(min.y, max.y),
                z = Random.Range(min.z, max.z),
            };
        }
    }
}
