using NodeReflection;

namespace BehaviorTree
{
    [VisualNode]
    /// <summary>
    /// Inverts the result of the child node.
    /// </summary>
    public class Inverter : Node
    {
        /// <summary>
        /// Evaluate the node
        /// </summary>
        /// <returns>Return SUCCESS if a child node failed, FAILURE if a child node succeeded, or RUNNING</returns>
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