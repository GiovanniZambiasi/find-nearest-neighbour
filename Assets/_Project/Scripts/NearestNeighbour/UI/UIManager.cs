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

        [SerializeField] private NeighboursPopup _neighbours;

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
    }
}
