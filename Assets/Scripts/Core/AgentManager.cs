using System;
using UnityEngine;

namespace Core
{
    public class AgentManager : MonoBehaviour, IAgentService
    {
        public event Action AddAgentEvent;
        public event Action RemoveAgentEvent;
        public event Action RemoveAllAgentsEvent;
        public event Action<int> AgentsNumberChangeEvent;
        public event Action<string> AgentSendLogEvent;
        public void AddAgent()
        {
            AddAgentEvent?.Invoke();
        }

        public void RemoveAgent()
        {
            RemoveAgentEvent?.Invoke();
        }

        public void RemoveAllAgents()
        {
            RemoveAllAgentsEvent?.Invoke();
        }

        public void ChangeAgentsNumber(int agentsNumber)
        {
            AgentsNumberChangeEvent?.Invoke(agentsNumber);
        }

        public void LogAgent(string log)
        {
            AgentSendLogEvent?.Invoke(log);
        }
    }
}
