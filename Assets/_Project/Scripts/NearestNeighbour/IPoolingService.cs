using UnityEngine;

namespace NearestNeighbour
{
    public interface IPoolingService
    {
        MonoBehaviour Spawn(MonoBehaviour prefab, Vector3 position, Quaternion rotation);

        void Release(MonoBehaviour instance);
    }
}
