using UnityEngine;

namespace BehaviorTree
{
    public abstract class BehaviorTreeAgent : MonoBehaviour
    {
        private Node _root;

        private void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            _root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}