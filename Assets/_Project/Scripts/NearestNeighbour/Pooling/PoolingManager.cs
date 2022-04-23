using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour.Pooling
{
    public class PoolingManager : MonoBehaviour, IPoolingService
    {
        [SerializeField] private List<Pool> _pools = new List<Pool>();

        private readonly Dictionary<int, Pool> _spawnedObjects = new Dictionary<int, Pool>();
        private readonly Dictionary<int, Pool> _prefabPoolMap = new Dictionary<int, Pool>();
        private readonly List<ScheduledDeSpawn> _scheduledDeSpawns = new List<ScheduledDeSpawn>();

        public void Setup()
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                Pool pool = _pools[i];

                GameObject root = new GameObject(pool.Prefab.name);
                root.transform.SetParent(transform);

                pool.Setup(root.transform);

                _prefabPoolMap.Add(pool.Prefab.GetInstanceID(), pool);
            }
        }

        public void Tick(float elapsedTime)
        {
            for (int i = _scheduledDeSpawns.Count - 1; i >= 0; i--)
            {
                ScheduledDeSpawn deSpawn = _scheduledDeSpawns[i];

                if (elapsedTime >= deSpawn.DeSpawnTime)
                {
                    Release(deSpawn.Object);
                    _scheduledDeSpawns.RemoveAt(i);
                }
            }
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int prefabInstanceId = prefab.GetInstanceID();

            if (!_prefabPoolMap.ContainsKey(prefabInstanceId))
            {
                Debug.LogError($"Doesn't have registered pool for prefab '{prefab.name}'");

                return default;
            }

            Pool pool = _prefabPoolMap[prefabInstanceId];
            GameObject spawnedObject = pool.Get(position, rotation);
            _spawnedObjects.Add(spawnedObject.GetInstanceID(), pool);

            return spawnedObject;
        }

        public void Release(GameObject instance)
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

        public void Release(GameObject instance, float delay)
        {
            float deSpawnTime = Time.time + delay;

            _scheduledDeSpawns.Add(new ScheduledDeSpawn
            {
                Object = instance,
                DeSpawnTime = deSpawnTime,
            });
        }
    }
}
