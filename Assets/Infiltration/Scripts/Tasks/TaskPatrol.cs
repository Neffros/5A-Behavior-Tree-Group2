using BehaviorTree;
using Infiltration.Scripts;
using UnityEngine;

namespace Infiltration
{
    public class TaskPatrol : Node{
        private readonly Transform _transform;
        private readonly Transform[] _waypoints;

        private int _currentWaypointIndex;

        private const float WaitTime = 1f; // in seconds
        private float _waitCounter;
        private bool _waiting;

        public TaskPatrol(Transform transform, Transform[] waypoints)
        {
            _transform = transform;
            _waypoints = waypoints;
        }

        public override NodeState Evaluate()
        {
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= WaitTime)
                {
                    _waiting = false;
                }
            }
            else
            {
                var wp = _waypoints[_currentWaypointIndex];
                if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
                {
                    _transform.position = wp.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                }
                else
                {
                    var position = wp.position;
                    _transform.position = Vector3.MoveTowards(_transform.position, position, GuardAI.Speed * Time.deltaTime);
                    _transform.LookAt(position);
                }
            }


            State = NodeState.RUNNING;
            return State;
        }
    }
}