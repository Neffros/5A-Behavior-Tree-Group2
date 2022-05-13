

using System;
using BehaviorTreeSerializer.Data;
using NodeReflection;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Main class for the behavior of the agent
    /// </summary>
    public class BehaviorTreeAgent : MonoBehaviour
    {
        #region Unity Fields

        [SerializeField, Tooltip("Behavior tree of the agent")]
        private BehaviorTreeObject _behaviorTree;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root node that will contain children node
        /// </summary>
        public Node Root { get; private set; }

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Sets up this agent's tree hierarchy
        /// </summary>
        private void Awake()
        {
            this.Root = SetupTree();
            if (this.Root == null)
            {
                throw new InvalidOperationException("Tree root must not be null.");
            }
        }

        /// <summary>
        /// Initializes this agent's tree nodes
        /// </summary>
        private void Start()
        {
            this.Root.Initialize(this);
        }

        /// <summary>
        /// Evaluates every node from the tree by browsing every children every frame
        /// </summary>
        private void Update()
        {
            this.Root.Update();

            if (Root.State is NodeState.Success or NodeState.Failure)
            {
                Root.Reset();
            }
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Sets up nodes for the behavior of the agent
        /// </summary>
        /// <returns>The root node. Must not be null.</returns>
        protected virtual Node SetupTree()
        {
            if (this._behaviorTree == null)
            {
                Debug.LogError("Behavior tree not set");
                return null;
            }

            return Engine.GenerateTree(this._behaviorTree);
        }

        #endregion
    }
}