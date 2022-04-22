using NearestNeighbour.NeighbourFinder;
using NearestNeighbour.Pooling;
using UnityEngine;

namespace NearestNeighbour
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField] private NeighbourManager _neighbourManager;
        [SerializeField] private PoolingManager _poolingManager;

        private void Start()
        {
            _poolingManager.Setup();
            _neighbourManager.Setup(_poolingManager);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            _neighbourManager.Tick(deltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _neighbourManager.SpawnNeighbours(3);
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                _neighbourManager.DespawnRandom();
            }
        }
    }
}
