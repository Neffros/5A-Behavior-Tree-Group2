using BehaviorTree;

namespace CrowdControl
{
	public class AgentAi : BehaviorTreeAgent
	{
		protected override Node SetupTree()
		{
			Node nodeRoot = new Selector();
			TaskMoveToWayPoint taskMove = new TaskMoveToWayPoint();
			nodeRoot.Attach(taskMove);
			
			return nodeRoot;
		}
	}
}