using NodeReflection;

namespace BehaviorTree
{
    /// <summary>
    /// Sequence nodes contain one or more children. Upon execution, it executes every child and fails when one of the children fails.
    /// </summary>
    [VisualNode(displayAsBlock: true)]
    public class Sequence : Node
    {
        /// <summary>
        /// Evaluate the node
        /// </summary>
        /// <returns>Return FAILURE if a child node failed, RUNNING if a child is running after every evaluated node, SUCCESS if none of the child is running or failing </returns>
        protected override NodeState OnEvaluate()
        {
            var anyChildIsRunning = false;

            foreach (var node in Children)
            {
                node.Evaluate();
                switch (node.State)
                {
                    case NodeState.FAILURE:
                        return NodeState.FAILURE;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        break;
                    default:
                        return NodeState.SUCCESS;
                }
            }

            return anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        }
    }
}