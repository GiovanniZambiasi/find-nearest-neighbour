using UnityEngine;

namespace NearestNeighbour.NeighbourFinder
{
    public class NeighbourView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _deathParticlesPrefab;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private MeshRenderer _renderer;

        private static readonly int LastBounceTimeHash = Shader.PropertyToID("_LastBounceTime");

        private MaterialPropertyBlock _propertyBlock;

        public void HandleInstantiated()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void ShowNearestNeighbourFeedback(Vector3 neighbourPosition)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, neighbourPosition);
        }

        public void HideNearestNeighbourFeedback()
        {
            _lineRenderer.enabled = false;
        }

        public void SpawnDeathEffect(IPoolingService poolingService)
        {
            GameObject particlesObject = poolingService.Spawn(_deathParticlesPrefab.gameObject, transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particlesObject.GetComponent<ParticleSystem>();
            particleSystem.Play(false);
            poolingService.Release(particlesObject, _deathParticlesPrefab.main.duration);
        }

        public void PlayBounceFeedback()
        {
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(LastBounceTimeHash, Time.time);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
