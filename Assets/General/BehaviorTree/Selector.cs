using System;
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
        protected override NodeState OnUpdate()
        {
            if (Children.Count == 0)
            {
                throw new InvalidOperationException("A Selector must have at least one child.");
            }
            
            foreach (var node in Children)
            {
                node.Update();
                switch (node.State)
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        return NodeState.Success;
                    case NodeState.Running:
                        return NodeState.Running;
                    default:
                        continue;
                }
            }

            return NodeState.Failure;
        }
    }
}