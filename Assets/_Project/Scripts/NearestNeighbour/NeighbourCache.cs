using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour
{
    public struct NeighbourPair
    {
        public GameObject A;
        public GameObject B;
        public float SqrDistance;
        public int Frame;
    }

    [CreateAssetMenu(fileName = nameof(NeighbourCache), menuName = "Nearest Neighbour/NeighbourCache")]
    public class NeighbourCache : ScriptableObject
    {
        private Dictionary<GameObject, NeighbourPair> _neighbourInfos = new Dictionary<GameObject, NeighbourPair>();
    }
}
