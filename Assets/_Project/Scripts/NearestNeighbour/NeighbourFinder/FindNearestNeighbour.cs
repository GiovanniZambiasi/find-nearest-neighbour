using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    public class FindNearestNeighbour : MonoBehaviour
    {
        private RandomMovementComponent _movementComponent;

        public void Setup(Bounds movementBounds)
        {
            _movementComponent = GetComponent<RandomMovementComponent>();
            _movementComponent.Setup(movementBounds);
        }

        public void UpdateMovement(float deltaTime)
        {
            _movementComponent.Tick(deltaTime);
        }
    }
}
