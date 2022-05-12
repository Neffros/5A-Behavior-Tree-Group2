using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// Main class for the behavior of the agent
    /// </summary>
    public abstract class BehaviorTreeAgent : MonoBehaviour
    {
        /// <summary>
        /// A root node that will contain children node
        /// </summary>
        private Node _root;

        /// <summary>
        /// Setup the tree by adding node (selector, sequences ...)
        /// </summary>
        private void Start()
        {
            _root = SetupTree();
        }

        /// <summary>
        /// Evaluate every node from the tree by browsing every children every frame
        /// </summary>
        private void Update()
        {
            _root?.Evaluate();
        }

        /// <summary>
        /// Setup nodes for the behavior of the agent
        /// </summary>
        /// <returns></returns>
        protected abstract Node SetupTree();
    }
}