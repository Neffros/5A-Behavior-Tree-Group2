using BehaviorTree;
using UnityEngine;

namespace Infiltration
{
    public class CheckEnemyInRange : Node
    {
        private readonly Transform _transform;
        public CheckEnemyInRange(Transform transform)
        {
            _transform = transform;
        }
        public override NodeState Evaluate()
        {
            var target = GetData("target");

            if (target == null)
            {
                State = NodeState.FAILURE;
                return State;
            }

            var targetPos = (Transform)target;

            if (Vector3.Distance(_transform.position, targetPos.position) <= GuardAI.AttackRange)
            {
                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}