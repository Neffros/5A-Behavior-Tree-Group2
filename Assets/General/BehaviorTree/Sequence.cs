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
        public override NodeState Evaluate()
        {
            var anyChildIsRunning = false;

            foreach (var node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        State = NodeState.FAILURE;
                        return State;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        break;
                    default:
                        State = NodeState.SUCCESS;
                        return State;
                }
            }

            State = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return State;
        }
    }
}