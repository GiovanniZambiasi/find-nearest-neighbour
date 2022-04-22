using UnityEngine;

namespace NearestNeighbour.Player
{
    public class Weapon : MonoBehaviour
    {
        public event System.Action<IDamageable> OnTargetHit;
        public event System.Action OnFired;

        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private float _projectileSpawnForwardOffset = 3f;
        [SerializeField] private float _projectileImpulse = 10f;
        [SerializeField] private float _projectilesPerSecond = 10f;

        private readonly GameObjectComponentCache<IDamageable> _damageableCache = new GameObjectComponentCache<IDamageable>();
        private IPoolingService _poolingService;
        private float _projectileSpawnInterval;
        private float _lastFireTime;

        private Vector3 ProjectileSpawnPosition => transform.position + transform.forward * _projectileSpawnForwardOffset;
        private bool CanFire
        {
            get
            {
                float timeSinceLastFire = Time.time - _lastFireTime;
                return timeSinceLastFire >= _projectileSpawnInterval;
            }
        }

        public void Setup(IPoolingService poolingService)
        {
            _poolingService = poolingService;
            _projectileSpawnInterval = 1f / _projectilesPerSecond;
            _lastFireTime = -_projectileSpawnInterval;
        }

        public void Aim(Vector3 position, Vector3 direction)
        {
            Transform transform = this.transform;
            transform.localPosition = position;
            transform.forward = direction;
        }

        public void Fire()
        {
            if (!CanFire)
            {
                return;
            }

            Vector3 spawnPosition = ProjectileSpawnPosition;
            Projectile projectile = _poolingService.Spawn(_projectilePrefab, spawnPosition, transform.rotation) as Projectile;
            projectile.Setup(transform.forward * _projectileImpulse, HandleHit);

            _lastFireTime = Time.time;
            OnFired?.Invoke();
        }

        private void HandleHit(Projectile projectile, Collider other)
        {
            TryDamage(other);
            _poolingService.Release(projectile);
        }

        private void TryDamage(Collider other)
        {
            Rigidbody attachedRigidbody = other.attachedRigidbody;

            if (!attachedRigidbody)
            {
                return;
            }

            GameObject hitObject = attachedRigidbody.gameObject;

            if (_damageableCache.TryGetComponent(hitObject, out IDamageable damageable))
            {
                damageable.Damage();
                OnTargetHit?.Invoke(damageable);
            }
        }
    }
}
