using BehaviorTree;

namespace CrowdControl
{
    public class TaskShiftOnTheRight : Node
    {
        protected override NodeState OnUpdate()
        {
            // Not enough time to be done
            return NodeState.Running;
        }
    }
}