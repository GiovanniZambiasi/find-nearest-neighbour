using NearestNeighbour.NeighbourFinder;
using NearestNeighbour.Player;
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
        [SerializeField] private PlayerManager _playerManager;

        private void Start()
        {
            _poolingManager.Setup();

            _uiManager.Setup();
            _uiManager.OnNeighbourSpawnRequested += _neighbourManager.SpawnNeighbours;
            _uiManager.OnNeighbourDeSpawnRequested += _neighbourManager.DeSpawnNeighbours;

            _neighbourManager.Setup(_poolingManager);
            _neighbourManager.OnNeighboursChanged += _uiManager.SetNeighbourCount;

            _playerManager.Setup(_poolingManager);
            _playerManager.OnStatsChanged += _uiManager.SetPlayerStats;

            _uiManager.SetNeighbourCount(_neighbourManager.NeighbourCount);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            _neighbourManager.Tick(deltaTime);

            _playerManager.Tick();

            _uiManager.Tick();
            _uiManager.SetQueryCount(_neighbourManager.DistanceQueries);

            _poolingManager.Tick(Time.time);
        }
    }
}
