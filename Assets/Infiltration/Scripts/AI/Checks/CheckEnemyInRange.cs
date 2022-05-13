using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class CheckEnemyInRange : Node {
        [ExposedInVisualEditor] public float AttackRange { get; set; } = 1;

        protected override NodeState OnEvaluate()
        {
            var target = this.GetData<Transform>("target");

            if (target == null)
            {
                return NodeState.FAILURE;
            }

            if (Vector3.Distance(this.Agent.transform.position, target.position) <= AttackRange)
            {
                return NodeState.SUCCESS;
            }

            return NodeState.FAILURE;
        }
    }
}