using NodeReflection;

namespace BehaviorTree
{
    /// <summary>
    /// Selector nodes contain one or more children. Upon execution, it executes every child until one of them succeeds, otherwise it fails
    /// </summary>
    [VisualNode]
    public class Selector : Node
    {
        /// <summary>
        /// Evaluate the node
        /// </summary>
        /// <returns>Return SUCCESS if a child node succeeded, RUNNING if a child node is running, or FAILURE after every evaluated children </returns>
        protected override NodeState OnEvaluate()
        {
            foreach (var node in Children)
            {
                node.Evaluate();
                switch (node.State)
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        return NodeState.SUCCESS;
                    case NodeState.RUNNING:
                        return NodeState.RUNNING;
                    default:
                        continue;
                }
            }

            return NodeState.FAILURE;
        }
    }
}