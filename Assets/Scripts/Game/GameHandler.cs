using System;
using Core.UI;
using Game.Grid;
using Game.Input;
using Game.Timer;
using States;
using UI;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameHandler : MonoBehaviour
    {
        private GridService _gridService;
        private GameTimerService _timerService;
        private GameInputHandler _inputHandler;
        private UiWindowsHandler _uiHandler;

        private GameState _currentState;
        private bool _firstReveal = true;

        [Inject]
        private void Construct(
            GridService gridService,
            GameTimerService timerService,
            GameInputHandler inputHandler,
            UiWindowsHandler uiHandler)
        {
            _gridService = gridService;
            _timerService = timerService;
            _inputHandler = inputHandler;
            _uiHandler = uiHandler;
        }

        private void Start()
        {
            _gridService.OnMineHit += HandleLose;
            _gridService.OnWin += HandleWin;
            _gridService.OnCellRevealed += HandleFirstReveal;

            _inputHandler.OnRestartPressed += HandleRestart;
            _inputHandler.OnPausePressed += HandlePause;

            SetState(GameState.Menu);
        }

        private void SetState(GameState newState)
        {
            _currentState = newState;
            _gridService.SetInputEnabled(newState == GameState.Playing);

            switch (newState)
            {
                case GameState.Menu:
                    _uiHandler.HideAll();
                    _gridService.Cleanup();
                    _timerService.Reset();
                    var menu = _uiHandler.Show<MainMenuScreen>();
                    menu.OnStartClicked -= HandleStartGame;
                    menu.OnStartClicked += HandleStartGame;
                    break;

                case GameState.Playing:
                    _uiHandler.HideAll();
                    var hud = _uiHandler.Show<GameHudScreen>();
                    hud.OnPauseClicked -= HandlePause;
                    hud.OnPauseClicked += HandlePause;
                    break;

                case GameState.Paused:
                    _timerService.Pause();
                    var pause = _uiHandler.Show<PauseWindow>();
                    pause.OnContinueClicked -= HandleContinue;
                    pause.OnRestartClicked -= HandleRestart;
                    pause.OnMainMenuClicked -= HandleMainMenu;
                    pause.OnContinueClicked += HandleContinue;
                    pause.OnRestartClicked += HandleRestart;
                    pause.OnMainMenuClicked += HandleMainMenu;
                    break;

                case GameState.Win:
                case GameState.Lose:
                    _timerService.Stop();
                    var gameOver = _uiHandler.Show<GameOverScreen, GameOverParams>(
                        new GameOverParams(newState == GameState.Win));
                    gameOver.OnRestartClicked -= HandleRestart;
                    gameOver.OnMainMenuClicked -= HandleMainMenu;
                    gameOver.OnRestartClicked += HandleRestart;
                    gameOver.OnMainMenuClicked += HandleMainMenu;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void HandleFirstReveal(CellData cell)
        {
            if (!_firstReveal)
            {
                return;
            }
            
            _timerService.Start();
            _firstReveal = false;
        }

        private void HandleStartGame()
        {
            _firstReveal = true;
            SetState(GameState.Playing);
        }

        private void HandlePause()
        {
            if (_currentState == GameState.Playing)
            {
                SetState(GameState.Paused);
            }
        }

        private void HandleContinue()
        {
            _timerService.Resume();
            _uiHandler.Hide<PauseWindow>();
            _gridService.SetInputEnabled(true);
            _currentState = GameState.Playing;
        }

        private void HandleRestart()
        {
            _firstReveal = true;
            _gridService.Cleanup();
            _timerService.Reset();
            SetState(GameState.Playing);
        }

        private void HandleMainMenu()
        {
            SetState(GameState.Menu);
        }

        private void HandleWin()
        {
            SetState(GameState.Win);
        }

        private void HandleLose()
        {
            SetState(GameState.Lose);
        }
    }
}
