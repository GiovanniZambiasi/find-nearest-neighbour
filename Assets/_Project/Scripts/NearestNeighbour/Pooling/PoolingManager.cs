using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour.Pooling
{
    public class PoolingManager : MonoBehaviour, IPoolingService
    {
        [SerializeField] private List<Pool> _pools = new List<Pool>();

        private readonly Dictionary<int, Pool> _spawnedObjects = new Dictionary<int, Pool>();
        private readonly Dictionary<MonoBehaviour, Pool> _prefabPoolMap = new Dictionary<MonoBehaviour, Pool>();

        public void Setup()
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                Pool pool = _pools[i];

                GameObject root = new GameObject(pool.Prefab.GetType().Name);
                root.transform.SetParent(transform);

                pool.Setup(root.transform);

                _prefabPoolMap.Add(pool.Prefab, pool);
            }
        }

        public MonoBehaviour Spawn(MonoBehaviour prefab, Vector3 position, Quaternion rotation)
        {
            if (!_prefabPoolMap.ContainsKey(prefab))
            {
                Debug.LogError($"Doesn't have registered pool for prefab '{prefab.name}'");

                return null;
            }

            Pool pool = _prefabPoolMap[prefab];
            MonoBehaviour spawnedObject = pool.Get(position, rotation);
            _spawnedObjects.Add(spawnedObject.GetInstanceID(), pool);

            return spawnedObject;
        }

        public void Release(MonoBehaviour instance)
        {
            int instanceId = instance.GetInstanceID();

            if (!_spawnedObjects.ContainsKey(instanceId))
            {
                Debug.LogError($"Spawned object '{instance.name}' is not registered to any pools!");
                return;
            }

            Pool pool = _spawnedObjects[instanceId];
            pool.Release(instance);

            _spawnedObjects.Remove(instanceId);
        }
    }
}
