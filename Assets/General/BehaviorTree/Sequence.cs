using System;
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
        protected override NodeState OnUpdate()
        {
            if (Children.Count == 0)
            {
                throw new InvalidOperationException("A Sequence must have at least one child.");
            }
            
            foreach (var node in Children)
            {
                node.Update();
                switch (node.State)
                {
                    case NodeState.Failure:
                        return NodeState.Failure;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        return NodeState.Running;
                    default:
                        continue;
                }
            }

            return NodeState.Success;
        }
    }
}