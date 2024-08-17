using System;

namespace Core
{
    public interface ITimeService
    {
        event Action OnTick;
        event Action<float> OnGameSpeedChange;
        event Action<bool> OnPauseChange;

        void ChangeGameSpeed(bool increase);
        void ChangePause();
        float GetGameSpeed();
        bool GetGamePaused();
    }
}
