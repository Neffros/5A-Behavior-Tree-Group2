using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class CheckEnemyInRange : Node
    {
        [ExposedInVisualEditor]
        public float AttackRange { get; set; }

        [ExposedInVisualEditor]
        public Transform Transform { get; set; }

        public override NodeState Evaluate()
        {
            var target = GetData("target");

            if (target == null)
            {
                State = NodeState.FAILURE;
                return State;
            }

            var targetPos = (Transform)target;

            if (Vector3.Distance(Transform.position, targetPos.position) <= AttackRange)
            {
                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}