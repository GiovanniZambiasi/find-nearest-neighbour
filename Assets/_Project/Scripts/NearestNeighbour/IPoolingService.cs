using UnityEngine;

namespace NearestNeighbour
{
    public interface IPoolingService
    {
        GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);

        void Release(GameObject instance);

        void Release(GameObject instance, float delay);
    }
}
