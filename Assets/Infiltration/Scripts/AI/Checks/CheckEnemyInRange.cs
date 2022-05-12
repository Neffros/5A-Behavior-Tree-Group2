using BehaviorTree;
using UnityEngine;

namespace Infiltration
{
    public class CheckEnemyInRange : Node
    {
        private readonly Transform _transform;
        private readonly float _attackRange;
        public CheckEnemyInRange(Transform transform, float attackRange)
        {
            _transform = transform;
            _attackRange = attackRange;
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

            if (Vector3.Distance(_transform.position, targetPos.position) <= _attackRange)
            {
                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}