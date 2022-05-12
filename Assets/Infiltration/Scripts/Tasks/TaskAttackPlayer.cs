using BehaviorTree;

namespace Infiltration
{
    public class TaskAttackPlayer : Node
    {
        public override NodeState Evaluate()
        {
            State = NodeState.RUNNING;
            return State;
        }
    }
}