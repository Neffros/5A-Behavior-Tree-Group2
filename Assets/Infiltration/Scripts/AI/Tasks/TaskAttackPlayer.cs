using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class TaskAttackPlayer : Node
    {
        [ExposedInVisualEditor]
        public float AttackRange { get; set; }

        [ExposedInVisualEditor]
        public Transform Transform { get; set; }

        public override NodeState Evaluate()
        {
            var target = (Transform)GetData("target");

            if (Vector3.Distance(Transform.position, target.position) <= AttackRange)
            {
                var player = target.GetComponent<PlayerMovement>();
                player.GetHit();
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}