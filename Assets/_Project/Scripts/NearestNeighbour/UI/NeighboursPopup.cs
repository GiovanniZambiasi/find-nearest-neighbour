using UnityEngine;
using UnityEngine.UI;

namespace NearestNeighbour.UI
{
    public class NeighboursPopup : MonoBehaviour
    {
        public event System.Action<int> OnSpawnRequested;
        public event System.Action<int> OnDeSpawnRequested;

        [SerializeField] private InputField _spawnCountInputField;
        [SerializeField] private Button _spawnButton;
        [SerializeField] private Button _deSpawnButton;
        [SerializeField] private Text _countLabel;
        [SerializeField] private Text _queriesLabel;

        public void Setup()
        {
            _spawnCountInputField.onValidateInput += ValidateInput;
            _spawnCountInputField.onEndEdit.AddListener(HandleInputEditFinished);

            _spawnButton.onClick.AddListener(HandleSpawnButtonClicked);
            _spawnButton.interactable = false;

            _deSpawnButton.onClick.AddListener(HandleDeSpawnButtonClicked);
            _deSpawnButton.interactable = false;
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
            bool isValidInput = int.TryParse(input, out int count);
            _spawnButton.interactable = isValidInput;
            _deSpawnButton.interactable = isValidInput;
        }

        private void HandleSpawnButtonClicked()
        {
            ParseInputAndSendEvent(OnSpawnRequested);
        }

        private void HandleDeSpawnButtonClicked()
        {
            ParseInputAndSendEvent(OnDeSpawnRequested);
        }

        private void ParseInputAndSendEvent(System.Action<int> @event)
        {
            if (int.TryParse(_spawnCountInputField.text, out int amount))
            {
                @event?.Invoke(amount);
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
