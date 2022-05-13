using BehaviorTree;

namespace CrowdControl
{
    public class TaskShiftOnTheLeft : Node
    {
        protected override NodeState OnUpdate()
        {
            // Not enough time to be done
            return NodeState.Running;
        }
    }
}