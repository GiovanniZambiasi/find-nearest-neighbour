using UnityEngine;
using UnityEngine.UI;

namespace NearestNeighbour.UI
{
    public class PlayerStatsPopup : MonoBehaviour
    {
        [SerializeField] private Text _projectileCountLabel;
        [SerializeField] private Text _hitsLabel;
        [SerializeField] private Text _accuracyLabel;

        public void SetStats(PlayerStats stats)
        {
            _projectileCountLabel.text = $"Projectiles: {stats.FiredProjectiles.ToString()}";
            _hitsLabel.text = $"Hits: {stats.Hits.ToString()}";
            _accuracyLabel.text = $"{(stats.Accuracy * 100f).ToString("0")}%";
        }
    }
}
