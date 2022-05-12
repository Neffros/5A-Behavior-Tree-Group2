using BehaviorTree;

namespace Infiltration
{
    public class CheckEnemyInRange : Node{
        public override NodeState Evaluate()
        {
            State = NodeState.FAILURE;
            return NodeState.FAILURE;
        }
    }
}