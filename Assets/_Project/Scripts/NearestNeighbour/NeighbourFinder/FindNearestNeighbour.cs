﻿using System;
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

        public void Setup(Bounds movementBounds, IPoolingService poolingService)
        {
            _movementComponent.Setup(movementBounds);
            _poolingService = poolingService;
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
                _view.ShowNearestNeighbourFeedback(_nearestNeighbour.Neighbour.transform.localPosition);
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
