using System;
using Core.UI.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuScreen : MonoBehaviour, IUiWindow<MainMenuScreen>
    {
        [SerializeField] private Button _startButton;

        public event Action OnStartClicked;
        
        private void Awake()
        {
            _startButton.onClick.AddListener(HandleStartClick);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(HandleStartClick);
        }

        private void HandleStartClick()
        {
            OnStartClicked?.Invoke();
        }
    }
}