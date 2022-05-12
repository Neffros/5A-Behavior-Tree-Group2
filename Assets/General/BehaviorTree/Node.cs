using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// State of the node
    /// </summary>
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    /// <summary>
    /// Node that is connected to the root, will be evaluated by the agent
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Current state of the node
        /// </summary>
        protected NodeState State;

        /// <summary>
        /// Parent of the current node
        /// </summary>
        public Node Parent { get; private set; }
        /// <summary>
        /// Children node that the node will have
        /// </summary>
        protected readonly List<Node> Children = new();

        /// <summary>
        /// Dictionary that will contain information stocked by the agent
        /// </summary>
        private readonly Dictionary<string, object> _dataContext = new();

        /// <summary>
        /// Create node with no parent
        /// </summary>
        protected Node()
        {
            Parent = null;
        }

        /// <summary>
        /// Attach child node with this current node
        /// </summary>
        /// <param name="node">Child node to be attached</param>
        /// <returns>Return current node for chaining</returns>
        public Node Attach(Node node)
        {
            node.Parent = this;
            Children.Add(node);
            return this;
        }

        /// <summary>
        /// Evaluate children node
        /// </summary>
        /// <returns>Return FAILURE by default</returns>
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        /// <summary>
        /// Add data to the dictionary
        /// </summary>
        /// <param name="key">Key to be add</param>
        /// <param name="value">Value to be add</param>
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        /// <summary>
        /// Add data to the dictionary of the root node
        /// </summary>
        /// <param name="key">Key to be add</param>
        /// <param name="value">Value to be add</param>
        public void SetDataToRoot(string key, object value)
        {
            Node node = Parent;
            while (node.Parent != null)
            {
                node = node.Parent;
            }

            node._dataContext[key] = value;
        }

        /// <summary>
        /// Retrieve value from dictionary
        /// </summary>
        /// <param name="key">Key used to look for data</param>
        /// <returns>Return value of the dictionary according to the key</returns>
        protected object GetData(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
            {
                return value;
            }

            return Parent?.GetData(key);
        }

        /// <summary>
        /// Remove data from the dictionary
        /// </summary>
        /// <param name="key">Key remove from the dictionary</param>
        /// <returns>Return true if the removal succeeded, false if not</returns>
        protected bool RemoveData(string key)
        {
            if (_dataContext.TryGetValue(key, out _))
            {
                _dataContext.Remove(key);
                return true;
            }

            var node = Parent;
            while (node != null)
            {
                var cleared = node.RemoveData(key);
                if (cleared)
                    return true;
                node = node.Parent;
            }

            return false;
        }
    }
}