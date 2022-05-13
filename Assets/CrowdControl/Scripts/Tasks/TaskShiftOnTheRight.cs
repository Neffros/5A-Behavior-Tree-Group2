using BehaviorTree;

namespace CrowdControl
{
    public class TaskShiftOnTheRight : Node
    {
        protected override NodeState OnUpdate()
        {
            return NodeState.Running;
        }
    }
}