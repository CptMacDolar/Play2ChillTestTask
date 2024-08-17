using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Agents
{
    public class AgentController : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private Vector3 spawnPosition;
        [Header("References")]
        [SerializeField] private GameObject agentPrefab;
        
        private List<Agent> _agents;
        
        private void OnEnable()
        {
            GameManager.Instance.TimeService.OnTick += OnTick;
            GameManager.Instance.TimeService.OnGameSpeedChange += ChangeAllAgentsGameSpeed;
            GameManager.Instance.TimeService.OnPauseChange += ChangeAllAgentsPaused;
            
            GameManager.Instance.AgentService.AddAgentEvent += SpawnAgent;
            GameManager.Instance.AgentService.RemoveAgentEvent += RemoveRandomAgent;
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
            var spawnedObject = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);
            var agent = spawnedObject.GetComponent<Agent>();
            _agents.Add(agent);
            agent.OnSpawn();
            agent.ChangeGameSpeed(GameManager.Instance.TimeService.GetGameSpeed());
            agent.ChangePaused(GameManager.Instance.TimeService.GetGamePaused());
            GameManager.Instance.AgentService.ChangeAgentsNumber(_agents.Count);
        }

        private void ChangeAllAgentsGameSpeed(float speed)
        {
            foreach (var agent in _agents)
            {
                agent.ChangeGameSpeed(speed);
            }
        }

        private void ChangeAllAgentsPaused(bool paused)
        {
            foreach (var agent in _agents)
            {
                agent.ChangePaused(paused);
            }
        }
        
        private void RemoveRandomAgent()
        {
            if(_agents.Count < 1)
                return;
            
            var randomAgent = _agents[Random.Range(0, _agents.Count)];
            _agents.Remove(randomAgent);
            Destroy(randomAgent.gameObject);
            
            GameManager.Instance.AgentService.ChangeAgentsNumber(_agents.Count);
        }
        
        private void RemoveAllAgents()
        {
            foreach (var agent in _agents)
            {
                Destroy(agent.gameObject);
            }
            _agents.Clear();
            
            GameManager.Instance.AgentService.ChangeAgentsNumber(_agents.Count);
        }
    }
}
