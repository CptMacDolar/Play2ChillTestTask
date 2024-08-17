using System.Globalization;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class GameSpeedUI : MonoBehaviour
    {
        private Button _gameSpeedDecreaseButton;
        private Button _gameSpeedIncreaseButton;
        private Button _gamePauseButton;

        private Label _gameSpeedLabel;
        private Label _agentsNumberLabel;
        private Label _logLabel;

        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            
            _gameSpeedDecreaseButton = root.Q<Button>("GameSpeedDecreaseButton");
            _gameSpeedIncreaseButton = root.Q<Button>("GameSpeedIncreaseButton");
            _gamePauseButton = root.Q<Button>("GamePauseButton");

            _gameSpeedLabel = root.Q<Label>("GameSpeedText");
        }

        private void OnEnable()
        {
            _gameSpeedDecreaseButton.clicked += OnDecreaseSpeedClicked;
            _gameSpeedIncreaseButton.clicked += OnIncreaseSpeedClicked;
            _gamePauseButton.clicked += OnGamePauseClicked;
            GameManager.Instance.TimeService.OnGameSpeedChange += UpdateGameSpeedLabel;
        }

        private void OnDisable()
        {
            _gameSpeedDecreaseButton.clicked -= OnDecreaseSpeedClicked;
            _gameSpeedIncreaseButton.clicked -= OnIncreaseSpeedClicked;
            _gamePauseButton.clicked -= OnGamePauseClicked;
            GameManager.Instance.TimeService.OnGameSpeedChange -= UpdateGameSpeedLabel;
        }
        
        private void OnIncreaseSpeedClicked()
        {
            GameManager.Instance.TimeService.ChangeGameSpeed(true);
        }
        
        private void OnDecreaseSpeedClicked()
        {
            GameManager.Instance.TimeService.ChangeGameSpeed(false);
        }
        
        private void OnGamePauseClicked()
        {
            GameManager.Instance.TimeService.ChangePause();
        }
        
        private void UpdateGameSpeedLabel(float gameSpeed)
        {
            _gameSpeedLabel.text = gameSpeed.ToString(CultureInfo.InvariantCulture) + "x";
        }
    }
}
