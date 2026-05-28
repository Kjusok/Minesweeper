using System;
using Core.UI.Abstract;
using Game.Grid;
using Game.Timer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameHudScreen : MonoBehaviour, IUiWindow<GameHudScreen>
    {
        private const int SecondsPerMinute = 60;

        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Transform _cellsContainer;

        private GameTimerService _timerService;
        private GridService _gridService;

        public event Action OnPauseClicked;

        [Inject]
        private void Construct(GameTimerService timerService, GridService gridService)
        {
            _timerService = timerService;
            _gridService = gridService;
        }

        public void OnShow()
        {
            _gridService.CreateGrid(_cellsContainer);
        }

        private void Awake()
        {
            _pauseButton.onClick.AddListener(HandlePauseClick);
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(HandlePauseClick);
        }

        private void Update()
        {
            var totalSeconds = Mathf.FloorToInt(_timerService.ElapsedTime);
            var minutes = totalSeconds / SecondsPerMinute;
            var seconds = totalSeconds % SecondsPerMinute;
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void HandlePauseClick()
        {
            OnPauseClicked?.Invoke();
        }
    }
}