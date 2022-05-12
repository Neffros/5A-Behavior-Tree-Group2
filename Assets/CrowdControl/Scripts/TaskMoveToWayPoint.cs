using System.Collections.Generic;
using BehaviorTree;
using NodeReflection;
using UnityEngine;
using UnityEngine.AI;

namespace CrowdControl
{
	public class TaskMoveToWayPoint : Node
	{
		[ExposedInVisualEditor]
		private float Radius { get; set; }
		
		//[ExposedInVisualEditor]
		//private float DistanceToWayPoint { get; set; } //TODO use property when visual is implemented
		private float DistanceToWayPoint = 5f;

		private Vector3 _targetWayPointPosition;
		
		private List<Vector3> _waypoints;

		private float _patience;
		private float _speed;
		private NavMeshAgent _navMeshAgent;
		// Start is called before the first frame update
		public override void OnInitialized()
		{
			AgentData data = this.Agent.gameObject.GetComponent<AgentData>();
			_navMeshAgent = data.NavAgent;
			_waypoints = data.Waypoints;
			
			_patience = Random.Range(0, 100);
			_navMeshAgent.speed = Random.Range(3.5f, 7f);
			
			_targetWayPointPosition = UpdatePosition(_targetWayPointPosition);
			_navMeshAgent.SetDestination(_targetWayPointPosition);

		}
		
		private Vector3 UpdatePosition(Vector3 previousTargetPosition)
		{
			int newPosIndex = Random.Range(0, _waypoints.Count);

			if (Vector3.Distance(previousTargetPosition, _waypoints[newPosIndex]) < 0.1f)
			{
				UpdatePosition(previousTargetPosition);
			}

			return _waypoints[newPosIndex];
		}

		public override NodeState Evaluate()
		{
			if (Vector3.Distance(_targetWayPointPosition, this.Agent.gameObject.transform.position) <
			    DistanceToWayPoint)
				_targetWayPointPosition = UpdatePosition(_targetWayPointPosition);
				
			_navMeshAgent.SetDestination(_targetWayPointPosition);
			return NodeState.RUNNING;
		}
	}
}