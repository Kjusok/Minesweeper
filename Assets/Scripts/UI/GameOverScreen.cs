using System;
using Core.UI.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverScreen : MonoBehaviour, IUiWindow<GameOverScreen, GameOverParams>
    {
        private const string VictoryText = "Victory!";
        private const string GameOverText = "Game Over";

        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;

        public event Action OnRestartClicked;
        public event Action OnMainMenuClicked;

        public void SetParameters(GameOverParams parameters)
        {
            _resultText.text = parameters.IsWin ? VictoryText : GameOverText;
        }
        
        private void Awake()
        {
            _restartButton.onClick.AddListener(HandleRestartClick);
            _mainMenuButton.onClick.AddListener(HandleMainMenuClick);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(HandleRestartClick);
            _mainMenuButton.onClick.RemoveListener(HandleMainMenuClick);
        }

        private void HandleRestartClick()
        {
            OnRestartClicked?.Invoke();
        }

        private void HandleMainMenuClick()
        {
            OnMainMenuClicked?.Invoke();
        }
    }

    public class GameOverParams : IUiWindowParams<GameOverScreen, GameOverParams>
    {
        public bool IsWin { get; }

        public GameOverParams(bool isWin)
        {
            IsWin = isWin;
        }
    }
}