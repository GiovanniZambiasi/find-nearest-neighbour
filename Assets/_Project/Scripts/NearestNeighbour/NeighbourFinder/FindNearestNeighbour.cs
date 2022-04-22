using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    public class FindNearestNeighbour : MonoBehaviour
    {
        [SerializeField] private LineRenderer _renderer;
        [SerializeField] private RandomMovementComponent _movementComponent;

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
            if (distanceInfo.IsValid)
            {
                UpdateNeighbourFeedback(distanceInfo);
            }
            else
            {
                _renderer.enabled = false;
            }
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
