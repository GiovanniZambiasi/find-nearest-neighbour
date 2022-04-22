using UnityEngine;

namespace NearestNeighbour.UI
{
    public class UIManager : MonoBehaviour
    {
        public event System.Action<int> OnNeighbourSpawnRequested
        {
            add => _neighbours.OnSpawnRequested += value;
            remove => _neighbours.OnSpawnRequested -= value;
        }
        public event System.Action<int> OnNeighbourDeSpawnRequested
        {
            add => _neighbours.OnDeSpawnRequested += value;
            remove => _neighbours.OnDeSpawnRequested -= value;
        }

        [SerializeField] private NeighboursPopup _neighbours;
        [SerializeField] private PlayerStatsPopup _playerStats;

        public void Setup()
        {
            _neighbours.Setup();
        }

        public void SetNeighbourCount(int neighbourCount)
        {
            _neighbours.SetNeighbourCount(neighbourCount);
        }

        public void SetQueryCount(int distanceQueries)
        {
            _neighbours.SetQueryCount(distanceQueries);
        }

        public void SetPlayerStats(PlayerStats stats)
        {
            _playerStats.SetStats(stats);
        }
    }
}
