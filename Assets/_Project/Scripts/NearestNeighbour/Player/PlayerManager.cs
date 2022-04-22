using UnityEngine;
using UnityEngine.EventSystems;

namespace NearestNeighbour.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public event System.Action<PlayerStats> OnStatsChanged;

        [SerializeField] private Camera _camera;
        [SerializeField] private Weapon _weapon;

        private PlayerStats _stats;

        private bool ShouldFire => Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverUI();

        public void Setup(IPoolingService poolingService)
        {
            _weapon.Setup(poolingService);
            _weapon.OnFired += HandleFired;
            _weapon.OnTargetHit += HandleTargetHit;
        }

        public void Tick()
        {
            if (ShouldFire)
            {
                FireWeapon();
            }
        }

        private void HandleTargetHit(IDamageable obj)
        {
            _stats.Hits++;
            OnStatsChanged?.Invoke(_stats);
        }

        private void HandleFired()
        {
            _stats.FiredProjectiles++;
            OnStatsChanged?.Invoke(_stats);
        }

        private void FireWeapon()
        {
            Ray aimRay = _camera.ScreenPointToRay(Input.mousePosition);
            _weapon.Aim(aimRay.origin, aimRay.direction);
            _weapon.Fire();
        }
    }
}
