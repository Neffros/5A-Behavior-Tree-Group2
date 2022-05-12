using Infiltration;
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
            if (UIManager.StateSet)
            {
                return;
            }
            _root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}