using UnityEngine;

namespace NearestNeighbour
{
    public class RandomMovementComponent : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;

        private Bounds _movementBounds;
        private Vector3 _movementDirection;

        public void Setup(Bounds movementBounds)
        {
            _movementBounds = movementBounds;
            RandomizeMovementDirection();
        }

        public void Tick(float deltaTime)
        {
            Vector3 nextPosition = transform.position + _movementDirection * (_speed * deltaTime);

            if (!_movementBounds.Contains(nextPosition))
            {
                nextPosition = _movementBounds.ClosestPoint(nextPosition);
                RandomizeMovementDirection();
            }

            transform.position = nextPosition;
        }

        private void RandomizeMovementDirection()
        {
            _movementDirection = Random.onUnitSphere;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, _movementDirection * 2f);
        }
    }
}
