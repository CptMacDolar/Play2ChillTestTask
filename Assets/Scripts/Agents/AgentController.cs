using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Agents
{
    public class AgentController : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private Vector3 spawnPosition;
        [SerializeField] private int defaultPoolSize;
        [SerializeField] private int maxPoolSize;
        [Header("References")]
        [SerializeField] private GameObject agentPrefab;
        
        private List<Agent> _activeAgents;
        private ObjectPool<Agent> _agentsPool;

        private void Start()
        {
            _activeAgents = new List<Agent>();
            _agentsPool = new ObjectPool<Agent>(CreateAgent, GetAgent, ReturnAgent, DestroyAgent,
                false, defaultPoolSize, maxPoolSize);
        }

        private void OnEnable()
        {
            GameManager.Instance.TimeService.OnTick += OnTick;
            GameManager.Instance.TimeService.OnGameSpeedChange += ChangeAllAgentsGameSpeed;
            GameManager.Instance.TimeService.OnPauseChange += ChangeAllAgentsPaused;
            
            GameManager.Instance.AgentService.AddAgentEvent += AddAgent;
            GameManager.Instance.AgentService.RemoveAgentEvent += RemoveRandomAgent;
            GameManager.Instance.AgentService.RemoveAllAgentsEvent += RemoveAllAgents;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.TimeService.OnTick -= OnTick;
            GameManager.Instance.TimeService.OnGameSpeedChange -= ChangeAllAgentsGameSpeed;
            GameManager.Instance.TimeService.OnPauseChange -= ChangeAllAgentsPaused;
            
            GameManager.Instance.AgentService.AddAgentEvent -= AddAgent;
            GameManager.Instance.AgentService.RemoveAgentEvent -= RemoveRandomAgent;
            GameManager.Instance.AgentService.RemoveAllAgentsEvent -= RemoveAllAgents;
        }
        
        private void OnTick()
        {
            foreach (var agent in _activeAgents)
            {
                agent.OnTick();
            }
        }
        
        private void AddAgent()
        {
            _agentsPool.Get();
        }

        private void RemoveRandomAgent()
        {
            if(_activeAgents.Count < 1)
                return;
            
            var randomAgent = _activeAgents[Random.Range(0, _activeAgents.Count)];
            _activeAgents.Remove(randomAgent);
            _agentsPool.Release(randomAgent);
            
            GameManager.Instance.AgentService.ChangeAgentsNumber(_activeAgents.Count);
        }
        
        private void RemoveAllAgents()
        {
            foreach (var agent in _activeAgents)
            {
                _agentsPool.Release(agent);
            }
            _activeAgents.Clear();
            
            GameManager.Instance.AgentService.ChangeAgentsNumber(_activeAgents.Count);
        }
        
        private Agent CreateAgent()
        {
            var spawnedObject = Instantiate(agentPrefab, spawnPosition, Quaternion.identity, transform);
            var agent = spawnedObject.GetComponent<Agent>();
            return agent;
        }

        private void GetAgent(Agent agent)
        {
            agent.transform.position = spawnPosition;
            agent.gameObject.SetActive(true);
            _activeAgents.Add(agent);
            agent.OnSpawn();
            agent.ChangeGameSpeed(GameManager.Instance.TimeService.GetGameSpeed());
            agent.ChangePaused(GameManager.Instance.TimeService.GetGamePaused());
            GameManager.Instance.AgentService.ChangeAgentsNumber(_activeAgents.Count);
        }
        
        private void ReturnAgent(Agent agent)
        {
            agent.gameObject.SetActive(false);
        }

        private void DestroyAgent(Agent agent)
        {
            agent.OnRemove();
        }

        private void ChangeAllAgentsGameSpeed(float speed)
        {
            foreach (var agent in _activeAgents)
            {
                agent.ChangeGameSpeed(speed);
            }
        }

        private void ChangeAllAgentsPaused(bool paused)
        {
            foreach (var agent in _activeAgents)
            {
                agent.ChangePaused(paused);
            }
        }
        
        
    }
}
