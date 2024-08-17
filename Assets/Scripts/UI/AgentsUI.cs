using Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class AgentsUI : MonoBehaviour
    {
        private Button _agentAddButton;
        private Button _agentRemoveButton;
        private Button _agentRemoveAllButton;
        
        private Label _agentsNumberText;
        private Label _logText;
    
        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            
            _agentAddButton = root.Q<Button>("AgentAddButton");
            _agentRemoveButton = root.Q<Button>("AgentRemoveButton");
            _agentRemoveAllButton = root.Q<Button>("AgentRemoveAllButton");

            _agentsNumberText = root.Q<Label>("AgentsNumberText");
            _logText = root.Q<Label>("LogText");
        }
        
        private void OnEnable()
        {
            _agentAddButton.clicked += OnAddAgentClicked;
            _agentRemoveButton.clicked += OnRemoveAgentClicked;
            _agentRemoveAllButton.clicked += OnRemoveAllAgentsClicked;
            GameManager.Instance.AgentService.AgentSendLogEvent += OnNewLog;
            GameManager.Instance.AgentService.AgentsNumberChangeEvent += UpdateAgentsNumberLabel;
        }

        private void OnDisable()
        {
            _agentAddButton.clicked -= OnAddAgentClicked;
            _agentRemoveButton.clicked -= OnRemoveAgentClicked;
            _agentRemoveAllButton.clicked -= OnRemoveAllAgentsClicked;
            GameManager.Instance.AgentService.AgentSendLogEvent -= OnNewLog;
            GameManager.Instance.AgentService.AgentsNumberChangeEvent -= UpdateAgentsNumberLabel;
        }
        
        private void OnAddAgentClicked()
        {
            GameManager.Instance.AgentService.AddAgent();
        }
        
        private void OnRemoveAgentClicked()
        {
            GameManager.Instance.AgentService.RemoveAgent();
        }
        
        private void OnRemoveAllAgentsClicked()
        {
            GameManager.Instance.AgentService.RemoveAllAgents();
        }

        private void OnNewLog(string log)
        {
            _logText.text = $"{log}\n";
        }
        
        private void UpdateAgentsNumberLabel(int agentsNumber)
        {
            _agentsNumberText.text = agentsNumber.ToString();
        }
    }
}
