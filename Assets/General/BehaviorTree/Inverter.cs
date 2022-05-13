using System;
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
        protected override NodeState OnUpdate()
        {
            if (Children.Count != 1)
            {
                throw new InvalidOperationException("An Inverter must have exactly one child.");
            }

            Node child = Children[0];
            
            child.Update();
            switch (child.State)
            {
                case NodeState.Failure:
                    return NodeState.Success;
                case NodeState.Success:
                    return NodeState.Failure;
                case NodeState.Running:
                    return NodeState.Running;
                default:
                    throw new InvalidOperationException("");
            }
        }
    }
}