using NodeReflection;

namespace BehaviorTree
{
    [VisualNode]
    public class Inverter : Node
    {
        public override NodeState Evaluate()
        {
            var anyChildIsRunning = false;

            foreach (var node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        State = NodeState.SUCCESS;
                        return State;
                    case NodeState.SUCCESS:
                        State = NodeState.FAILURE;
                        return State;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        break;
                    default:
                        State = NodeState.FAILURE;
                        return State;
                }
            }

            State = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return State;
        }
    }
}