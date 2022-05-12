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
        #region Properties

        /// <summary>
        /// Gets the parent of the current node
        /// </summary>
        public Node Parent { get; private set; }

        /// <summary>
        /// Gets the root of the tree
        /// </summary>
        public Node Root
        {
            get
            {
                if (this.Parent == null)
                    return this;

                Node current = this.Parent;

                while (current.Parent != null)
                    current = current.Parent;

                return current;
            }
        }

        /// <summary>
        /// Gets the tree of the current node
        /// </summary>
        public BehaviorTreeAgent Tree
        {
            get
            {
                Node current = this;

                while (current._tree == null && current.Parent != null)
                    current = current.Parent;

                return current._tree;
            }
        }

        #endregion

        #region Protected Fields

        /// <summary>
        /// Children node that the node will have
        /// </summary>
        protected readonly List<Node> Children = new();

        /// <summary>
        /// Current state of the node
        /// </summary>
        protected NodeState State;

        #endregion

        #region Private Fields

        /// <summary>
        /// Dictionary that will contain information stocked by the agent
        /// </summary>
        private readonly Dictionary<string, object> _dataContext = new();

        /// <summary>
        /// The tree containing the node
        /// </summary>
        private readonly BehaviorTreeAgent _tree;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a node with no parent
        /// </summary>
        /// <param name="tree">Tree containing the node</param>
        public Node(BehaviorTreeAgent tree = null)
        {
            this.Parent = null;
            this._tree = tree;
        }

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

        public T GetNode<T>() where T : Node
        {
            foreach (var node in this.Children)
                if (node is T castedNode)
                    return castedNode;

            return null;
        }

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
        public void Initialize()
        {
            this.OnInitialized();

            foreach (var child in this.Children)
                child.Initialize();
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

        #region Public Overrideable Methods

        /// <summary>
        /// Evaluate children node
        /// </summary>
        /// <returns>Return FAILURE by default</returns>
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        /// <summary>
        /// Fired on tree initialization
        /// </summary>
        public virtual void OnInitialized() { }

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