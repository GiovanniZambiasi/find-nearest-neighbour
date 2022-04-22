using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    public class NeighbourManager : MonoBehaviour, INeighbourSpawner
    {
        [SerializeField] private FindNearestNeighbour _neighbourPrefab;
        [SerializeField] private int _startingNeighbours = 10;
        [Header("Movement Settings")]
        [SerializeField] private Vector3 _boundsExtents = new Vector3(100f, 100f, 100f);

        private readonly List<FindNearestNeighbour> _neighbours = new List<FindNearestNeighbour>();
        private IPoolingService _poolingService;
        private Bounds _movementBounds;
        private int _distanceQueries = 0;

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
            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPosition = _movementBounds.GetRandomPointWithin();
                FindNearestNeighbour neighbour = _poolingService.Spawn(_neighbourPrefab, spawnPosition, Quaternion.identity) as FindNearestNeighbour;
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

        private void UpdateDistances()
        {
            for (int i = 0; i < _neighbours.Count; i++)
            {
                FindNearestNeighbour neighbour = _neighbours[i];
                neighbour.ResetNearestNeighbour();
            }

            _distanceQueries = 0;

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
            _distanceQueries++;

            float distanceSqr = Vector3.SqrMagnitude(from.transform.position - to.transform.position);

            from.UpdateNearestNeighbour(new NeighbourDistanceInfo(to.gameObject, distanceSqr));
            to.UpdateNearestNeighbour(new NeighbourDistanceInfo(from.gameObject, distanceSqr));
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
            neighbour.Setup(_movementBounds);
            _neighbours.Add(neighbour);
        }

        private void UnRegisterNeighbour(FindNearestNeighbour neighbour)
        {
            if (!_neighbours.Remove(neighbour))
            {
                Debug.LogError($"Trying to unregister already unregistered {nameof(FindNearestNeighbour)} '{neighbour.name}'");
                return;
            }

            _poolingService.Release(neighbour);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _boundsExtents * 2f);
        }

        public void DespawnRandom()     // TODO - Remove later
        {
            int index = Random.Range(0, _neighbours.Count);
            FindNearestNeighbour despawned = _neighbours[index];
            UnRegisterNeighbour(despawned);
        }
    }
}
