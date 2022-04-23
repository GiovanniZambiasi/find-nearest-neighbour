using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    public class FindNearestNeighbour : MonoBehaviour, IDamageable, IPoolFeedbackReceiver
    {
        public event System.Action<FindNearestNeighbour> OnDamaged;

        [SerializeField] private RandomMovementComponent _movementComponent;
        [SerializeField] private NeighbourView _view;

        private NeighbourDistanceInfo _nearestNeighbourInfo;
        private IPoolingService _poolingService;

        public Vector3 Position => _movementComponent.Position;

        public void Setup(Bounds movementBounds, IPoolingService poolingService)
        {
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

        public void UpdateNearestNeighbour(FindNearestNeighbour neighbour, Vector3 neighbourPosition, float distanceSqr)
        {
            if (_nearestNeighbourInfo.IsValid && !(_nearestNeighbourInfo.DistanceSqr > distanceSqr))
            {
                return;
            }

            _nearestNeighbourInfo.Neighbour = neighbour;
            _nearestNeighbourInfo.NeighbourPosition = neighbourPosition;
            _nearestNeighbourInfo.DistanceSqr = distanceSqr;
            _nearestNeighbourInfo.IsValid = true;
        }

        public void ResetNearestNeighbour()
        {
            _nearestNeighbourInfo = default;
        }

        public void UpdateFeedback()
        {
            if (_nearestNeighbourInfo.IsValid)
            {
                _view.ShowNearestNeighbourFeedback(_nearestNeighbourInfo.NeighbourPosition);
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
