using System;
using System.Threading.Tasks;
using Core;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Agents
{
    public class Agent : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float nextWaypointDistance = 0.25f;
        [SerializeField] private float moveRange = 15f;
        [SerializeField] private float baseMoveSpeed = 5f;
        [SerializeField] private float rotationDuration = 0.5f;
        
        private Guid _guid;
        private float _gameSpeed = 1f;
        
        // Flags
        private bool _isMoving;
        private bool _paused;
        
        //DOTween
        private Tween _currentMovementTween;
        private Tween _currentRotationTween;
        
        // A* Path Finding
        private Seeker _seeker;
        private Path _path;
        private int _currentWaypoint;
        private Vector3 _destination;
        
        
        private void Awake()
        {
            _seeker = GetComponent<Seeker>();
        }

        private void OnEnable()
        {
            _seeker.pathCallback += OnPathComplete;
        }

        public void OnDisable ()
        {
            _seeker.pathCallback -= OnPathComplete;
        }
        
        private void OnPathComplete(Path path)
        {
            if (path.error)
            {
                Debug.LogError(path.errorLog);
                return;
            }

            _path = path;
            _currentWaypoint = 0;
        }
        
        public void OnSpawn()
        {
            _guid = Guid.NewGuid();
            _path = null;
            _isMoving = false;
            
            SetRandomDestination();
        }
        
        public void OnTick()
        {
            CheckWaypoints();
        }

        private void CheckWaypoints()
        {
            if(_paused)
                return;
            
            if (_path == null)
                return;

            while (true)
            {
                if(_paused)
                    return;
                
                var distanceToWaypoint = Vector3.Distance(transform.position, _path.vectorPath[_currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    if (_currentWaypoint + 1 < _path.vectorPath.Count)
                    {
                        _currentWaypoint++;
                    }
                    else
                    {
                        OnMovementComplete();
                        break;
                    }
                }
                else
                {
                    ResetMovement();
                    break;
                }
            }
        }
        
        private void SetRandomDestination()
        {
            var randomPosition = Random.insideUnitSphere * moveRange;
            randomPosition.y = transform.position.y;

            _destination = randomPosition;
            _seeker.StartPath(transform.position, _destination);
        }
        
        public void ChangeGameSpeed(float gameSpeed)
        {
            _gameSpeed = gameSpeed;
            
            if(_isMoving)
                ResetMovement();
        }
        
        private void ResetMovement()
        {
            if(_paused)
                return;
            
            var currentDestination = _path.vectorPath[_currentWaypoint];
            var currentSpeed = baseMoveSpeed * _gameSpeed;
            float distance = Vector3.Distance(transform.position, currentDestination);
            float duration = distance / currentSpeed;
            
            _currentMovementTween?.Kill();
            _currentRotationTween?.Kill();
            _isMoving = true;

            _currentRotationTween = transform.DOLookAt(currentDestination, rotationDuration / _gameSpeed)
                .SetEase(Ease.Linear);

            _currentMovementTween = transform.DOMove(currentDestination, duration)
                .SetEase(Ease.Linear);
        }

        public void ChangePaused(bool paused)
        {
            _paused = paused;
            
            if (paused)
            {
                if(_currentMovementTween.IsActive() && _currentMovementTween.IsPlaying())
                    _currentMovementTween.Pause();
                
                if(_currentRotationTween.IsActive() && _currentRotationTween.IsPlaying())
                    _currentRotationTween.Pause();
            }
            else
            {
                if(_currentMovementTween.IsActive() && !_currentMovementTween.IsPlaying())
                    _currentMovementTween.Play();
                
                if(_currentRotationTween.IsActive() && !_currentRotationTween.IsPlaying())
                    _currentRotationTween.Play();
            }
        }
        
        private void OnMovementComplete()
        {
            var log = $"Agent {_guid} arrived";
            GameManager.Instance.AgentService.LogAgent(log);
            SetRandomDestination();
            _isMoving = false;
        }

        public async void OnRemove()
        {
            gameObject.SetActive(false);
            _currentMovementTween?.Kill(true);
            _currentRotationTween?.Kill(true);
            await Task.Delay(500);
            Destroy(gameObject);
        }
    }
}
