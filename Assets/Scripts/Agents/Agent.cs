using System;
using UnityEngine;

namespace Agents
{
    public class Agent : MonoBehaviour
    {
        private Guid _guid;
        
        public void OnSpawn(float moveRange)
        {
            _guid = Guid.NewGuid();
        }
        
        public void OnTick()
        {
            throw new System.NotImplementedException();
        }
    }
}
