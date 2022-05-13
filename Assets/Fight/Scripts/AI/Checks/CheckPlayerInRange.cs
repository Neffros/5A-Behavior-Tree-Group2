using BehaviorTree;
using Fight.AI.Agents;
using UnityEngine;

namespace Fight.AI.Checks
{
	public class CheckPlayerInRange : Node
	{
		private Transform _boss;
		private Transform _player;

		protected override void OnInitialize()
		{
			BossSceneData sceneData = Agent.GetComponent<BossSceneData>();
			
			_boss = sceneData.BossController.transform;
			_player = sceneData.PlayerController.transform;
		}

		protected override NodeState OnStart()
		{
			Vector3 bossToPlayer = _boss.position - _player.position;
			bossToPlayer.y = 0;
			float distance = bossToPlayer.magnitude;

			return distance < 2 ? NodeState.Success : NodeState.Failure;
		}
	}
}