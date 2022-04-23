using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    public class FindNearestNeighbour : MonoBehaviour, IDamageable, IPoolFeedbackReceiver
    {
        public event System.Action<FindNearestNeighbour> OnDamaged;

        [SerializeField] private RandomMovementComponent _movementComponent;
        [SerializeField] private NeighbourView _view;

        private NeighbourDistanceInfo _nearestNeighbour;
        private IPoolingService _poolingService;

        public Transform Transform { get; private set; }
        public Vector3 Position => _movementComponent.Position;

        public void Setup(Bounds movementBounds, IPoolingService poolingService)
        {
            Transform = transform;
            _movementComponent.Setup(movementBounds);
            _poolingService = poolingService;
        }

        public void Dispose()
        {
            _view.Dispose();
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
                _view.ShowNearestNeighbourFeedback(_nearestNeighbour.NeighbourPosition);
            }
            else
            {
                _view.HideNearestNeighbourFeedback();
            }
        }

        public void Damage()
        {
            _view.SpawnDeathEffect(_poolingService);
            OnDamaged?.Invoke(this);
        }

        public void HandleInstantiated()
        {
            _view.HandleInstantiated();
            _movementComponent.OnDirectionChanged += _view.PlayBounceFeedback;
        }
    }
}
