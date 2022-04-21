using System;
using System.Collections.Generic;
using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    public class NeighbourManager : MonoBehaviour
    {
        [SerializeField] private List<FindNearestNeighbour> _neighbours = new List<FindNearestNeighbour>();
        [SerializeField] private NeighbourCache _cache;
        [Header("Movement Settings")]
        [SerializeField] private Vector3 _boundsExtents = new Vector3(100f, 100f, 100f);

        private Bounds _movementBounds;
        private int _distanceQueries = 0;

        public void Setup()
        {
            _movementBounds = new Bounds
            {
                center = transform.position,
                extents = _boundsExtents,
            };

            for (int i = 0; i < _neighbours.Count; i++)
            {
                FindNearestNeighbour neighbour = _neighbours[i];
                SetupNeighbour(neighbour);
            }
        }

        public void Tick(float deltaTime)
        {
            UpdateMovement(deltaTime);
            UpdateDistances();
            UpdateFeedbacks();
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
            _cache.Reset();
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
            _cache.UpdateDistanceInfo(from.gameObject, to.gameObject, distanceSqr);
        }

        private void UpdateFeedbacks()
        {
            for (int i = 0; i < _neighbours.Count; i++)
            {
                FindNearestNeighbour neighbour = _neighbours[i];
                NeighbourDistanceInfo distanceInfo = _cache.GetDistanceInfo(neighbour.gameObject);
                neighbour.UpdateNearestNeighbour(distanceInfo);
            }
        }

        private void SetupNeighbour(FindNearestNeighbour neighbour)
        {
            neighbour.Setup(_movementBounds);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _boundsExtents * 2f);
        }
    }
}
