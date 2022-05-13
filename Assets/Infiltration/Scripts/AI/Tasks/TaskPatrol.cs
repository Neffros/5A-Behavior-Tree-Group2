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

        private Transform[] _waypoints;

        private int _currentWaypointIndex;

        private const float WaitTime = 1f; // in seconds
        private float _waitCounter;
        private bool _waiting;

        protected override void OnInitialized()
        {
            this._waypoints = this.Agent.GetComponent<GuardSceneData>().Waypoints;
        }

        protected override NodeState OnEvaluate()
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
                if (Vector3.Distance(this.Agent.transform.position, wp.position) < 0.01f)
                {
                    this.Agent.transform.position = wp.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                }
                else
                {
                    var position = wp.position;
                    this.Agent.transform.position = Vector3.MoveTowards(this.Agent.transform.position, position, Speed * Time.deltaTime);
                    this.Agent.transform.LookAt(position);
                }
            }

            return NodeState.RUNNING;
        }
    }
}