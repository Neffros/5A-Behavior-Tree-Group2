using NodeReflection;

namespace BehaviorTree
{
    /// <summary>
    /// Inverts the result of the child node.
    /// </summary>
    [VisualNode]
    public class Inverter : Node
    {
        /// <summary>
        /// Evaluate the node
        /// </summary>
        /// <returns>Return SUCCESS if a child node failed, FAILURE if a child node succeeded, or RUNNING</returns>
        protected override NodeState OnEvaluate()
        {
            var anyChildIsRunning = false;

            foreach (var node in Children)
            {
                node.Evaluate();
                switch (node.State)
                {
                    case NodeState.FAILURE:
                        return NodeState.SUCCESS;
                    case NodeState.SUCCESS:
                        return NodeState.FAILURE;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        break;
                    default:
                        return NodeState.FAILURE;
                }
            }

            return anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        }
    }
}