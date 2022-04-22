using UnityEngine;

namespace NearestNeighbour
{
    public interface IPoolingService
    {
        Component Spawn(Component prefab, Vector3 position, Quaternion rotation);

        void Release(Component instance);
    }
}
