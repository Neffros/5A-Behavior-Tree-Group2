using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class CheckEnemyInFOVRange : Node
    {
        private const int PlayerLayerMask = 1 << 6;

        [ExposedInVisualEditor]
        public float SeeRange { get; set; }

        protected override NodeState OnStart()
        {
            var target = GetData<Transform>("target");

            if (target == null)
            {
                var colliders = Physics.OverlapSphere(
                    this.Agent.transform.position,
                    SeeRange,
                    PlayerLayerMask
                );
                
                if (colliders.Length == 0)
                {
                    return NodeState.Failure;
                }
                
                this.Root.SetData("target", colliders[0].transform);
            }

            return NodeState.Success;
        }
    }
}