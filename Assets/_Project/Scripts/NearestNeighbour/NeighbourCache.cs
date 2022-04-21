using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour
{
    public struct NeighbourDistanceInfo
    {
        public GameObject Neighbour;
        public float DistanceSqr;
    }

    [CreateAssetMenu(fileName = nameof(NeighbourCache), menuName = "Nearest Neighbour/NeighbourCache")]
    public class NeighbourCache : ScriptableObject
    {
        private Dictionary<GameObject, NeighbourDistanceInfo> _distanceInfoCache = new Dictionary<GameObject, NeighbourDistanceInfo>();

        public void UpdateDistanceInfo(GameObject a, GameObject b, float distanceSqr)
        {
            UpdateDistanceInfo(a, new NeighbourDistanceInfo
            {
                Neighbour = b,
                DistanceSqr = distanceSqr,
            });

            UpdateDistanceInfo(b, new NeighbourDistanceInfo
            {
                Neighbour = a,
                DistanceSqr = distanceSqr,
            });
        }

        public void Reset()
        {
            _distanceInfoCache.Clear();
        }

        private void UpdateDistanceInfo(GameObject from, NeighbourDistanceInfo distanceInfo)
        {
            if (!_distanceInfoCache.ContainsKey(from))
            {
                _distanceInfoCache.Add(from, distanceInfo);
            }
            else
            {
                NeighbourDistanceInfo currentDistanceInfo = _distanceInfoCache[from];

                if (currentDistanceInfo.DistanceSqr > distanceInfo.DistanceSqr)
                {
                    _distanceInfoCache[from] = distanceInfo;
                }
            }
        }

        public NeighbourDistanceInfo GetDistanceInfo(GameObject neighbour)
        {
            if (_distanceInfoCache.TryGetValue(neighbour, out NeighbourDistanceInfo value))
            {
                return value;
            }

            Debug.LogError($"Failed to get {nameof(NeighbourDistanceInfo)} from '{neighbour.name}'");

            return default;
        }
    }
}
