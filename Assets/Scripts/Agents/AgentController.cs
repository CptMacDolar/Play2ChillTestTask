using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Agents
{
    public class AgentController : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private Vector3 spawnPosition;
        [SerializeField] private float moveRange = 10;
        [Header("References")]
        [SerializeField] private GameObject agentPrefab;
        
        private List<Agent> _agents;
        
        private void OnEnable()
        {
            GameManager.Instance.TimeService.OnTick += OnTick;
            GameManager.Instance.TimeService.OnGameSpeedChange += ChangeAllAgentsSpeed;
            GameManager.Instance.TimeService.OnPauseChange += ChangeAllAgentsPaused;
            
            GameManager.Instance.AgentService.AddAgentEvent += SpawnAgent;
            GameManager.Instance.AgentService.RemoveAgentEvent += RemoveAgent;
            GameManager.Instance.AgentService.RemoveAllAgentsEvent += RemoveAllAgents;

            _agents = new();
        }
        
        private void OnTick()
        {
            foreach (var agent in _agents)
            {
                agent.OnTick();
            }
        }
        
        private void SpawnAgent()
        {
            
        }

        private void ChangeAllAgentsSpeed(float speed)
        {
           
        }

        private void ChangeAllAgentsPaused(bool paused)
        {
            
        }
        
        private void RemoveAgent()
        {
            
        }
        
        private void RemoveAllAgents()
        {
           
        }
    }
}
