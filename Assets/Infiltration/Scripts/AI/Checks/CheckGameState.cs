using BehaviorTree;
using NodeReflection;

namespace Infiltration
{
    [VisualNode]
    public class CheckGameState : Node
    {
        protected override NodeState OnStart()
        {
            if (GameManager.StateSet)
            {
                return NodeState.Failure;
            }

            return NodeState.Success;
        }
    }
}