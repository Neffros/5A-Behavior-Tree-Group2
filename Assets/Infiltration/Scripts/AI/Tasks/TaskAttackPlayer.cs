using BehaviorTree;
using UnityEngine;

namespace Infiltration
{
    public class TaskAttackPlayer : Node
    {
        private readonly Transform _transform;
        private readonly float _attackRange;

        public TaskAttackPlayer(Transform transform, float attackRange)
        {
            _transform = transform;
            _attackRange = attackRange;
        }

        public override NodeState Evaluate()
        {
            var target = (Transform)GetData("target");


            if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
            {
                var player = target.GetComponent<PlayerMovement>();
                player.GetHit();
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}