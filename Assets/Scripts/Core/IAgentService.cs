using System;

namespace Core
{
    public interface IAgentService
    {
        event Action AddAgentEvent;
        event Action RemoveAgentEvent;
        event Action RemoveAllAgentsEvent;
        event Action<int> AgentsNumberChangeEvent;
        event Action<string> AgentSendLogEvent;
        
        void AddAgent();
        void RemoveAgent();
        void RemoveAllAgents();
        void ChangeAgentsNumber(int agentsNumber);
        void LogAgent(string log);
    }
}