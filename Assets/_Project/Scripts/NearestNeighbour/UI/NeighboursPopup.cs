using UnityEngine;
using UnityEngine.UI;

namespace NearestNeighbour.UI
{
    public class NeighboursPopup : MonoBehaviour
    {
        public event System.Action<int> OnSpawnRequested;

        [SerializeField] private InputField _spawnCountInputField;
        [SerializeField] private Button _spawnButton;
        [SerializeField] private Text _countLabel;
        [SerializeField] private Text _queriesLabel;

        public void Setup()
        {
            _spawnCountInputField.onValidateInput += ValidateInput;
            _spawnCountInputField.onEndEdit.AddListener(HandleInputEditFinished);
            _spawnButton.interactable = false;
            _spawnButton.onClick.AddListener(HandleSpawnButtonClicked);
        }

        public void SetQueryCount(int queries)
        {
            _queriesLabel.text = $"Distance Queries (per frame): {queries.ToString()}";
        }

        public void SetNeighbourCount(int neighbourCount)
        {
            _countLabel.text = $"Count: {neighbourCount.ToString()}";
        }

        private void HandleInputEditFinished(string input)
        {
            _spawnButton.interactable = int.TryParse(input, out int count);
        }

        private void HandleSpawnButtonClicked()
        {
            if (int.TryParse(_spawnCountInputField.text, out int amount))
            {
                OnSpawnRequested?.Invoke(amount);
            }
            else
            {
                Debug.LogError($"Failed to parse input '{_spawnCountInputField.text}'");
            }
        }

        private char ValidateInput(string text, int charIndex, char addedChar)
        {
            return !char.IsNumber(addedChar) ? '\0' : addedChar;
        }
    }
}
