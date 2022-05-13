using BehaviorTree;
using Fight.AI.Agents;
using NodeReflection;

namespace Fight.AI.Checks
{
	[VisualNode]
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