using BehaviorTree;
using Fight.AI.Agents;
using UnityEngine;

namespace Fight.AI.Tasks
{
	public class TaskGoToPlayer : Node
	{
		private BossControllerScript _boss;
		private Transform _player;
		private float _moveSpeed;
		
		protected override void OnInitialize()
		{
			BossSceneData sceneData = Agent.GetComponent<BossSceneData>();

			_boss = sceneData.BossController;
			_player = sceneData.PlayerController.transform;
			_moveSpeed = sceneData.MoveSpeed;
		}

		protected override NodeState OnUpdate()
		{
			Vector3 moveDirection = _player.position - _boss.transform.position;
			moveDirection.y = 0;
			moveDirection.Normalize();

			moveDirection *= _moveSpeed;

			Vector3 velocity = _boss.Velocity;
			velocity.x = moveDirection.x;
			velocity.z = moveDirection.z;
			_boss.Velocity = velocity;
			
			return NodeState.Failure;
		}
	}
}