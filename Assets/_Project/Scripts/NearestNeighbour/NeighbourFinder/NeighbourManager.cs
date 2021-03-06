using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    public class NeighbourManager : MonoBehaviour
    {
        public event System.Action<int> OnNeighboursChanged;

        [SerializeField] private FindNearestNeighbour _neighbourPrefab;
        [SerializeField] private int _startingNeighbours = 10;
        [SerializeField] private int _neighbourLimit = 1000;
        [Header("Movement Settings")]
        [SerializeField] private Vector3 _boundsExtents = new Vector3(100f, 100f, 100f);

        private readonly GameObjectComponentCache<FindNearestNeighbour> _neighbourComponentCache = new GameObjectComponentCache<FindNearestNeighbour>();
        private readonly List<FindNearestNeighbour> _neighbours = new List<FindNearestNeighbour>();
        private IPoolingService _poolingService;
        private Bounds _movementBounds;

        public int NeighbourCount => _neighbours.Count;
        public int DistanceQueries { get; private set; }

        public void Setup(IPoolingService poolingService)
        {
            _poolingService = poolingService;
            _movementBounds = new Bounds
            {
                center = transform.position,
                extents = _boundsExtents,
            };

            SpawnNeighbours(_startingNeighbours);
        }

        public void Tick(float deltaTime)
        {
            UpdateMovement(deltaTime);
            UpdateDistances();
            UpdateFeedbacks();
        }

        public void SpawnNeighbours(int count)
        {
            count = Mathf.Clamp(count, 0, _neighbourLimit - NeighbourCount);

            for (int i = 0; i < count; i++)
            {
                SpawnNeighbour();
            }

            OnNeighboursChanged?.Invoke(NeighbourCount);
        }

        private void SpawnNeighbour()
        {
            Vector3 spawnPosition = _movementBounds.GetRandomPointWithin();
            GameObject neighbourObject = _poolingService.Spawn(_neighbourPrefab.gameObject, spawnPosition, Quaternion.identity);

            if (_neighbourComponentCache.TryGetComponent(neighbourObject, out FindNearestNeighbour neighbour))
            {
                RegisterNeighbour(neighbour);
            }
        }

        private void UpdateMovement(float deltaTime)
        {
            for (int i = 0; i < _neighbours.Count; i++)
            {
                FindNearestNeighbour neighbour = _neighbours[i];
                neighbour.UpdateMovement(deltaTime);
            }
        }

        public void DeSpawnNeighbours(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (NeighbourCount > 0)
                {
                    DeSpawnRandom();
                }
                else
                {
                    break;
                }
            }

            OnNeighboursChanged?.Invoke(NeighbourCount);
        }

        private void UpdateDistances()
        {
            for (int i = 0; i < _neighbours.Count; i++)
            {
                FindNearestNeighbour neighbour = _neighbours[i];
                neighbour.ResetNearestNeighbour();
            }

            DistanceQueries = 0;

            for (int i = 0; i < _neighbours.Count; i++)
            {
                FindNearestNeighbour from = _neighbours[i];

                for (int j = i + 1; j < _neighbours.Count; j++)
                {
                    FindNearestNeighbour to = _neighbours[j];
                    UpdateDistance(from, to);
                }
            }
        }

        private void UpdateDistance(FindNearestNeighbour from, FindNearestNeighbour to)
        {
            ++DistanceQueries;

            Vector3 fromPosition = from.Position;
            Vector3 toPosition = to.Position;

            float distanceSqr = Vector3.SqrMagnitude(fromPosition - toPosition);

            from.UpdateNearestNeighbour(to, toPosition, distanceSqr);
            to.UpdateNearestNeighbour(from, fromPosition, distanceSqr);
        }

        private void UpdateFeedbacks()
        {
            for (int i = 0; i < _neighbours.Count; i++)
            {
                FindNearestNeighbour neighbour = _neighbours[i];
                neighbour.UpdateFeedback();
            }
        }

        private void RegisterNeighbour(FindNearestNeighbour neighbour)
        {
            neighbour.Setup(_movementBounds, _poolingService);
            neighbour.OnDamaged += HandleNeighbourDamaged;
            _neighbours.Add(neighbour);
        }

        private void UnRegisterNeighbour(FindNearestNeighbour neighbour)
        {
            if (!_neighbours.Remove(neighbour))
            {
                Debug.LogError($"Trying to unregister already unregistered {nameof(FindNearestNeighbour)} '{neighbour.name}'");
                return;
            }

            neighbour.Dispose();
            neighbour.OnDamaged -= HandleNeighbourDamaged;
            _poolingService.Release(neighbour.gameObject);
        }

        private void DeSpawnRandom()
        {
            int index = Random.Range(0, _neighbours.Count);
            FindNearestNeighbour deSpawned = _neighbours[index];
            UnRegisterNeighbour(deSpawned);
        }

        private void HandleNeighbourDamaged(FindNearestNeighbour neighbour)
        {
            UnRegisterNeighbour(neighbour);

            OnNeighboursChanged?.Invoke(NeighbourCount);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _boundsExtents * 2f);
        }
    }
}
