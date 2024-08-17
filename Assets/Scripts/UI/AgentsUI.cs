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
        }

        private void OnDisable()
        {
            _agentAddButton.clicked -= OnAddAgentClicked;
            _agentRemoveButton.clicked -= OnRemoveAgentClicked;
            _agentRemoveAllButton.clicked -= OnRemoveAllAgentsClicked;
        }
        
        private void UpdateAgentsNumberLabel(int agentsNumber)
        {
            _agentsNumberText.text = agentsNumber.ToString();
        }
        
        private void OnAddAgentClicked()
        {
           
        }
        
        private void OnRemoveAgentClicked()
        {
            
        }
        
        private void OnRemoveAllAgentsClicked()
        {
            
        }

        private void OnNewLog(string log)
        {
            _logText.text = $"{log}\n";
        }
    }
}
