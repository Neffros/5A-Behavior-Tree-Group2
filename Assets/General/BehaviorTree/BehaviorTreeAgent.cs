﻿using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Main class for the behavior of the agent
    /// </summary>
    public abstract class BehaviorTreeAgent : MonoBehaviour
    {
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
            Root = SetupTree();
        }

        /// <summary>
        /// Initializes this agent's tree nodes
        /// </summary>
        private void Start()
        {
            Root?.Initialize(this);
        }

        /// <summary>
        /// Evaluates every node from the tree by browsing every children every frame
        /// </summary>
        private void Update()
        {
            Root?.Evaluate();
        }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Sets up nodes for the behavior of the agent
        /// </summary>
        /// <returns></returns>
        protected abstract Node SetupTree();

        #endregion
    }
}