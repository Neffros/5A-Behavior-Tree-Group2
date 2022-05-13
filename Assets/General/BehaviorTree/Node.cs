using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree
{
    /// <summary>
    /// State of the node : SUCCESS, FAILURE, RUNNING
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
    public abstract class Node
    {
        #region Public Properties

        /// <summary>
        /// Gets the behavior tree agent
        /// </summary>
        public BehaviorTreeAgent Agent { get; private set; }

        /// <summary>
        /// Children node that the node will have
        /// </summary>
        public List<Node> Children { get; private set; } = new();

        /// <summary>
        /// Gets the parent of the current node
        /// </summary>
        public Node Parent { get; private set; }

        /// <summary>
        /// Gets the root of the tree containing the node
        /// </summary>
        public Node Root { get; private set; }

        /// <summary>
        /// Current state of the node
        /// </summary>
        public NodeState State { get; private set; }

        #endregion

        #region Private Fields

        /// <summary>
        /// Dictionary that will contain information stocked by the agent
        /// </summary>
        private readonly Dictionary<string, object> _dataContext = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Attach child node to this instance of Node
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
        /// Gets the first found node with the given type in the tree of this node
        /// </summary>
        /// <typeparam name="T">Node type to find</typeparam>
        /// <returns>A node with the given type</returns>
        public T GetNode<T>() where T : Node
        {
            foreach (var node in this.Children)
            {
                if (node is T castedNode)
                    return castedNode;
            }

            foreach (var node in this.Children)
            {
                var found = node.GetNode<T>();

                if (found != null)
                    return found;
            }

            return null;
        }

        /// <summary>
        /// Gets nodes by type in the tree of this node
        /// </summary>
        /// <typeparam name="T">Node type to find</typeparam>
        /// <returns>An enumerable of nodes with the given type</returns>
        public IEnumerable<T> GetNodes<T>() where T : Node
        {
            return this
                .Children
                .SelectMany(child => child is T castedChild
                    ? child.GetNodes<T>().Prepend(castedChild)
                    : child.GetNodes<T>()
                );
        }

        /// <summary>
        /// Initializes the node
        /// </summary>
        public void Initialize(BehaviorTreeAgent agent, Node root = null)
        {
            this.Agent = agent;
            this.Root = root;

            this.OnInitialized();

            foreach (var child in this.Children)
                child.Initialize(agent, root ?? this);
        }

        /// <summary>
        /// Triggers the evaluation of the node
        /// </summary>
        public void Evaluate()
        {
            State = OnEvaluate();
        }

        /// <summary>
        /// Adds data to the dictionary
        /// </summary>
        /// <param name="key">Key to be added</param>
        /// <param name="value">Value to be added</param>
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        #endregion

        #region Protected Overrideable Methods

        /// <summary>
        /// Evaluate children node
        /// </summary>
        /// <returns>Return FAILURE by default</returns>
        protected virtual NodeState OnEvaluate() => NodeState.FAILURE;

        /// <summary>
        /// Fired on tree initialization
        /// </summary>
        protected virtual void OnInitialized() { }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Retrieve value from dictionary
        /// </summary>
        /// <param name="key">Key used to look for data</param>
        /// <returns>Return value of the dictionary according to the key</returns>
        protected object GetData(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
                return value;

            return Parent?.GetData(key);
        }

        /// <summary>
        /// Retrieve value from dictionary
        /// </summary>
        /// <param name="key">Key used to look for data</param>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <returns>Return value of the dictionary according to the key</returns>
        protected T GetData<T>(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
                return (T)value;

            return (T)Parent?.GetData(key);
        }

        /// <summary>
        /// Remove KeyValuePair from the dictionary
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

        #endregion
    }
}