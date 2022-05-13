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

        public override NodeState Evaluate()
        {
            var target = this.GetData<Transform>("target");

            if (target == null)
            {
                State = NodeState.FAILURE;
                return State;
            }

            if (Vector3.Distance(this.Agent.transform.position, target.position) <= AttackRange)
            {
                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}