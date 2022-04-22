using UnityEngine;

namespace NearestNeighbour.Player
{
    public class Projectile : MonoBehaviour
    {
        public delegate void HitHandler(Projectile projectile, Collider other);

        [SerializeField] private Rigidbody _rigidbody;

        private HitHandler _hitAction;

        public void Setup(Vector3 impulse, HitHandler hitAction)
        {
            _hitAction = hitAction;

            AddImpulse(impulse);
        }

        private void OnDisable()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _hitAction = null;
        }

        private void AddImpulse(Vector3 impulse)
        {
            _rigidbody.AddForce(impulse, ForceMode.Force);
        }

        private void OnTriggerEnter(Collider other)
        {
            _hitAction?.Invoke(this, other);
        }
    }
}
