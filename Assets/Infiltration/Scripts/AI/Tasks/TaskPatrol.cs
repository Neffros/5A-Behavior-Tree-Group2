using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class TaskPatrol : Node
    {
        [ExposedInVisualEditor]
        public float Speed { get; set; }

        [ExposedInVisualEditor]
        public Transform Transform { get; set; }

        [ExposedInVisualEditor]
        public Transform[] Waypoints { get; set; }

        private int _currentWaypointIndex;

        private const float WaitTime = 1f; // in seconds
        private float _waitCounter;
        private bool _waiting;

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
                var wp = Waypoints[_currentWaypointIndex];
                if (Vector3.Distance(Transform.position, wp.position) < 0.01f)
                {
                    Transform.position = wp.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % Waypoints.Length;
                }
                else
                {
                    var position = wp.position;
                    Transform.position = Vector3.MoveTowards(Transform.position, position, Speed * Time.deltaTime);
                    Transform.LookAt(position);
                }
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}