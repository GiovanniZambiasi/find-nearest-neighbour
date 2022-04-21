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

        public void Tick(float deltaTime, int frame)
        {
            for (int i = 0; i < _neighbours.Count; i++)
            {
                FindNearestNeighbour neighbour = _neighbours[i];
                neighbour.UpdateMovement(deltaTime);
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
