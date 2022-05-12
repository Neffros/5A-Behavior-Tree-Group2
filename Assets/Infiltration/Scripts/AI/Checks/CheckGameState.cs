using BehaviorTree;
using NodeReflection;

namespace Infiltration
{
    [VisualNode]
    public class CheckGameState : Node
    {
        public override NodeState Evaluate()
        {
            if (GameManager.StateSet)
            {
                State = NodeState.FAILURE;
                return State;
            }

            State = NodeState.SUCCESS;
            return State;
        }
    }
}