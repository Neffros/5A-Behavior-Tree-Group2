using BehaviorTree;
using Fight.AI.Agents;

namespace Fight.AI.Checks
{
	public class CheckPlayerAttacking : Node
	{
		private PlayerControllerScript _playerController;
		
		protected override void OnInitialize()
		{
			BossSceneData sceneData = Agent.GetComponent<BossSceneData>();
			
			_playerController = sceneData.PlayerController;
		}

		protected override NodeState OnStart()
		{
			return _playerController.IsAttacking ? NodeState.Success : NodeState.Failure;
		}
	}
}