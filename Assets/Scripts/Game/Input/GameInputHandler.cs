using System;
using Game.Timer;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Input
{
    public class GameInputHandler : ITickable
    {
        private readonly GameTimerService _timerService;

        public event Action OnRestartPressed;
        public event Action OnPausePressed;

        public GameInputHandler(GameTimerService timerService)
        {
            _timerService = timerService;
        }

        public void Tick()
        {
            _timerService.Update(Time.deltaTime);

            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                OnRestartPressed?.Invoke();
                return;
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                OnPausePressed?.Invoke();
            }
        }
    }
}
