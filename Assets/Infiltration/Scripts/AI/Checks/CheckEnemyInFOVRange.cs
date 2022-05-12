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
        public float FovRange { get; set; }

        [ExposedInVisualEditor]
        public SpriteRenderer Renderer { get; set; }

        [ExposedInVisualEditor]
        public Transform Transform { get; set; }

        public override NodeState Evaluate()
        {
            var target = GetData("target");

            if (target != null)
            {
                if (Vector3.Distance(Transform.position, ((Transform)target).position) > 8f)
                {
                    RemoveData("target");
                    Renderer.color = Color.blue;
                    State = NodeState.FAILURE;
                    return State;
                }
            }

            if (target == null)
            {
                var colliders = Physics.OverlapSphere(Transform.position, FovRange, PlayerLayerMask);
                if (colliders.Length > 0)
                {
                    Parent.Parent.SetData("target", colliders[0].transform);
                    Renderer.color = Color.red;
                    State = NodeState.SUCCESS;
                    return State;
                }

                State = NodeState.FAILURE;
                return State;
            }

            State = NodeState.SUCCESS;
            return State;
        }
    }
}