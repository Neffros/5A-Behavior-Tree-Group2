using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace CrowdControl
{
	public class TaskMoveToWayPoint : Node
	{
		private float DistanceToWayPoint = 5f;

		private Vector3 _targetWayPointPosition;
		private List<Vector3> _waypoints;
		private NavMeshAgent _navMeshAgent;
		
		protected override void OnInitialize()
		{
			AgentData data = this.Agent.gameObject.GetComponent<AgentData>();
			_navMeshAgent = data.NavAgent;
			_waypoints = data.Waypoints;
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

		protected override NodeState OnUpdate()
		{
			if (Vector3.Distance(_targetWayPointPosition, this.Agent.gameObject.transform.position) <
			    DistanceToWayPoint)
				_targetWayPointPosition = UpdatePosition(_targetWayPointPosition);
				
			_navMeshAgent.SetDestination(_targetWayPointPosition);
			return NodeState.Success;
		}
	}
}