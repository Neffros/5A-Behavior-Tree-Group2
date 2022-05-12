using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState State;

        public Node Parent { get; private set; }
        protected List<Node> Children = new List<Node>();

        private readonly Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        /// <summary>
        /// it does things
        /// </summary>
        public Node()
        {
            Parent = null;
        }

        /// <summary>
        /// attaches nodes
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Node Attach(Node node)
        {
            node.Parent = this;
            Children.Add(node);
            return this;
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public void SetDataToRoot(string key, object value)
        {
            Node node = Parent;
            while (node.Parent != null)
            {
                node = node.Parent;
            }

            node._dataContext[key] = value;
        }

        protected object GetData(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
            {
                return value;
            }

            return Parent?.GetData(key);
        }

        protected bool RemoveData(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
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