namespace Game.Timer
{
    public class GameTimerService
    {
        private bool _isRunning;

        public float ElapsedTime { get; private set; }

        public void Start()
        {
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Pause()
        {
            _isRunning = false;
        }

        public void Resume()
        {
            _isRunning = true;
        }

        public void Reset()
        {
            ElapsedTime = 0f;
            _isRunning = false;
        }

        public void Update(float deltaTime)
        {
            if (_isRunning)
            {
                ElapsedTime += deltaTime;
            }
        }
    }
}
