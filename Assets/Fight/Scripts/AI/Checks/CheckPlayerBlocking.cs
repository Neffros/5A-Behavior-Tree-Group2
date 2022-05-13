using BehaviorTree;
using Fight.AI.Agents;
using NodeReflection;

namespace Fight.AI.Checks
{
	[VisualNode]
	public class CheckPlayerBlocking : Node
	{
		private PlayerControllerScript _playerController;
		
		protected override void OnInitialize()
		{
			BossSceneData sceneData = Agent.GetComponent<BossSceneData>();
			
			_playerController = sceneData.PlayerController;
		}

		protected override NodeState OnStart()
		{
			return _playerController.IsBlocking ? NodeState.Success : NodeState.Failure;
		}
	}
}