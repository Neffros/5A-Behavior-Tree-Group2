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

        protected override NodeState OnStart()
        {
            var target = this.GetData<Transform>("target");

            if (target == null)
            {
                return NodeState.Failure;
            }

            if (Vector3.Distance(this.Agent.transform.position, target.position) <= AttackRange)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}