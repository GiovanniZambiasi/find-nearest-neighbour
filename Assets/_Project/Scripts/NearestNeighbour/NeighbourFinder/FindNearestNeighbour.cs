using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    public class FindNearestNeighbour : MonoBehaviour, IDamageable
    {
        public event System.Action<FindNearestNeighbour> OnDamaged;

        [SerializeField] private LineRenderer _renderer;
        [SerializeField] private RandomMovementComponent _movementComponent;

        private NeighbourDistanceInfo _nearestNeighbour;

        public void Setup(Bounds movementBounds)
        {
            _movementComponent.Setup(movementBounds);
        }

        public void UpdateMovement(float deltaTime)
        {
            _movementComponent.Tick(deltaTime);
        }

        public void UpdateNearestNeighbour(NeighbourDistanceInfo distanceInfo)
        {
            if (!_nearestNeighbour.IsValid || _nearestNeighbour.DistanceSqr > distanceInfo.DistanceSqr)
            {
                _nearestNeighbour = distanceInfo;
            }
        }

        public void ResetNearestNeighbour()
        {
            _nearestNeighbour = default;
        }

        public void UpdateFeedback()
        {
            if (_nearestNeighbour.IsValid)
            {
                UpdateNeighbourFeedback(_nearestNeighbour);
            }
            else
            {
                _renderer.enabled = false;
            }
        }

        public void Damage()
        {
            OnDamaged?.Invoke(this);
        }

        private void UpdateNeighbourFeedback(NeighbourDistanceInfo distanceInfo)
        {
            _renderer.enabled = true;
            _renderer.SetPosition(0, transform.position);

            Vector3 neighbourPosition = distanceInfo.Neighbour.transform.position;
            _renderer.SetPosition(1, neighbourPosition);
        }
    }
}
