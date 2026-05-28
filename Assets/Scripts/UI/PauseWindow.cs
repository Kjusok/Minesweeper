using System;
using Core.UI.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseWindow : MonoBehaviour, IUiWindow<PauseWindow>
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;

        public event Action OnContinueClicked;
        public event Action OnRestartClicked;
        public event Action OnMainMenuClicked;
        
        private void Awake()
        {
            _continueButton.onClick.AddListener(HandleContinueClick);
            _restartButton.onClick.AddListener(HandleRestartClick);
            _mainMenuButton.onClick.AddListener(HandleMainMenuClick);
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(HandleContinueClick);
            _restartButton.onClick.RemoveListener(HandleRestartClick);
            _mainMenuButton.onClick.RemoveListener(HandleMainMenuClick);
        }

        private void HandleContinueClick()
        {
            OnContinueClicked?.Invoke();
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
}