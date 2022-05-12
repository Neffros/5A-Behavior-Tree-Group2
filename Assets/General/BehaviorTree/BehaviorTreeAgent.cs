using UnityEngine;

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
        /// Setup the tree by adding node (selector, sequences ...)
        /// </summary>
        private void Start()
        {
            Root = SetupTree();
            Root?.Initialize();
        }

        /// <summary>
        /// Evaluate every node from the tree by browsing every children every frame
        /// </summary>
        private void Update()
        {
            Root?.Evaluate();
        }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Setup nodes for the behavior of the agent
        /// </summary>
        /// <returns></returns>
        protected abstract Node SetupTree();

        #endregion
    }
}