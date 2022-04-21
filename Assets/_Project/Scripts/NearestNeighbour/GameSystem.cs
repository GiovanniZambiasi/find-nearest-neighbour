using NearestNeighbour.NeighbourFinder;
using UnityEngine;

namespace NearestNeighbour
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField] private NeighbourManager _neighbourManager;

        private void Start()
        {
            _neighbourManager.Setup();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            _neighbourManager.Tick(deltaTime);
        }
    }
}
