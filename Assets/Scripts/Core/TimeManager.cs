using System;
using UnityEngine;

namespace Core
{
    public class TimeManager : MonoBehaviour, ITimeService
    {
        public event Action OnTick;
        public event Action<float> OnGameSpeedChange;
        public event Action<bool> OnPauseChange;
        
        [SerializeField] private int baseTickRate = 10;
        [SerializeField] private int baseGameSpeed = 1;
        [SerializeField] private float maxGameSpeed = 10f;
        [SerializeField, Min(0)] private float minGameSpeed = 0.125f;

        private int _tickRate;
        private float _gameSpeed;
        private float _timeSinceLastTick;
        private bool _isPaused;
        
        private void Start()
        {
            _tickRate = baseTickRate;
            _gameSpeed = baseGameSpeed;
        }
        
        void Update()
        {
            _timeSinceLastTick += Time.deltaTime;
            
            if (_timeSinceLastTick >= 1f / _tickRate)
            {
                OnTick?.Invoke();
                _timeSinceLastTick = 0f;
            }
        }
        
        public void ChangeGameSpeed(bool increase)
        {
            if (increase)
            {
                _gameSpeed *= 2;
            }
            else
            {
                _gameSpeed /= 2;
            }
            _gameSpeed = Mathf.Clamp(_gameSpeed, minGameSpeed, maxGameSpeed);
            _tickRate = (int)(baseTickRate * _gameSpeed);
            
            if(_isPaused)
                ChangePause();
            OnGameSpeedChange?.Invoke(_gameSpeed);
        }

        public void ChangePause()
        {
            _isPaused = !_isPaused;
            OnPauseChange?.Invoke(_isPaused);
        }

        public float GetGameSpeed() => _gameSpeed;
        public bool GetGamePaused() => _isPaused;
    }
}
