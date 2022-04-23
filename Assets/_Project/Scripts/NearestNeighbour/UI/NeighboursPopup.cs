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
        [Header("Keyboard Shortcuts")]
        [SerializeField] private KeyCode _spawnShortcut = KeyCode.KeypadPlus;
        [SerializeField] private KeyCode _deSpawnShortcut = KeyCode.KeypadMinus;

        public void Setup()
        {
            _spawnCountInputField.onValidateInput += ValidateInput;
            _spawnCountInputField.onEndEdit.AddListener(HandleInputEditFinished);

            _spawnButton.onClick.AddListener(RequestSpawn);
            _spawnButton.interactable = false;

            _deSpawnButton.onClick.AddListener(RequestDeSpawn);
            _deSpawnButton.interactable = false;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(_spawnShortcut))
            {
                RequestSpawn();
            }

            if (Input.GetKeyDown(_deSpawnShortcut))
            {
                RequestDeSpawn();
            }
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

        private void RequestSpawn()
        {
            ParseInputAndSendEvent(OnSpawnRequested);
        }

        private void RequestDeSpawn()
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
                Debug.LogError($"Failed to parse input '{_spawnCountInputField.text}'. Did you input a valid number?");
            }
        }

        private char ValidateInput(string text, int charIndex, char addedChar)
        {
            return !char.IsNumber(addedChar) ? '\0' : addedChar;
        }
    }
}
