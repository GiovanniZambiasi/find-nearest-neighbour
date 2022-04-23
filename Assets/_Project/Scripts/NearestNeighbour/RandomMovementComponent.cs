using UnityEngine;

namespace NearestNeighbour
{
    public class RandomMovementComponent : MonoBehaviour
    {
        public event System.Action OnDirectionChanged;

        [SerializeField] private float _speed = 3f;

        private Transform _transform;
        private Bounds _movementBounds;
        private Vector3 _movementDirection;

        public Vector3 Position { get; private set; }

        public void Setup(Bounds movementBounds)
        {
            _movementBounds = movementBounds;
            RandomizeMovementDirection();
            _transform = transform;
            Position = _transform.position;
        }

        public void Tick(float deltaTime)
        {
            Position += _movementDirection * (_speed * deltaTime);

            if (!_movementBounds.Contains(Position))
            {
                Position = _movementBounds.ClosestPoint(Position);
                RandomizeMovementDirection();
                OnDirectionChanged?.Invoke();
            }

            _transform.localPosition = Position;
        }

        private void RandomizeMovementDirection()
        {
            _movementDirection = Random.onUnitSphere;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(Position, _movementDirection * 2f);
        }
    }
}
