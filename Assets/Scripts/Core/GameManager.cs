using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public ITimeService TimeService { get; private set; }
        public IAgentService AgentService { get; private set; }

        private void Awake()
        {
            if(Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
            
            TimeService = GetComponent<ITimeService>();
            AgentService = GetComponent<IAgentService>();
        }
    }
}
