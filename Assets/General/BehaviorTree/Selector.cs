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
        public override NodeState Evaluate()
        {
            foreach (var node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        State = NodeState.SUCCESS;
                        return State;
                    case NodeState.RUNNING:
                        State = NodeState.RUNNING;
                        return State;
                    default:
                        continue;
                }
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}