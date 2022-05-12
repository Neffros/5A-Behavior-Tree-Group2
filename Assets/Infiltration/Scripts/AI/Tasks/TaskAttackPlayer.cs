using BehaviorTree;
using UnityEngine;

namespace Infiltration
{
    public class TaskAttackPlayer : Node
    {
        private readonly Transform _transform;

        public TaskAttackPlayer(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            var target = (Transform)GetData("target");


            if (Vector3.Distance(_transform.position, target.position) <= GuardAI.AttackRange)
            {
                var player = target.GetComponent<PlayerMovement>();
                player.GetHit();
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}