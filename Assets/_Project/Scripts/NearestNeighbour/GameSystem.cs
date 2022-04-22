using NearestNeighbour.NeighbourFinder;
using NearestNeighbour.Pooling;
using NearestNeighbour.UI;
using UnityEngine;

namespace NearestNeighbour
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField] private NeighbourManager _neighbourManager;
        [SerializeField] private PoolingManager _poolingManager;
        [SerializeField] private UIManager _uiManager;

        private void Start()
        {
            _poolingManager.Setup();

            _uiManager.Setup();
            _uiManager.OnNeighbourSpawnRequested += _neighbourManager.SpawnNeighbours;

            _neighbourManager.Setup(_poolingManager);
            _neighbourManager.OnNeighboursChanged += _uiManager.SetNeighbourCount;

            _uiManager.SetNeighbourCount(_neighbourManager.NeighbourCount);
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

            _uiManager.SetQueryCount(_neighbourManager.DistanceQueries);
        }
    }
}
