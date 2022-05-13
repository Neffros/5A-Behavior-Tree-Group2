using BehaviorTree;
using NodeReflection;

namespace Infiltration
{
    [VisualNode]
    public class CheckGameState : Node
    {
        protected override NodeState OnEvaluate()
        {
            if (GameManager.StateSet)
            {
                return NodeState.FAILURE;
            }

            return NodeState.SUCCESS;
        }
    }
}