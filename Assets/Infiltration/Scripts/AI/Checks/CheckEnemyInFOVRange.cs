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

        private SpriteRenderer _renderer;

        public override void OnInitialized()
        {
            this._renderer = this.Agent.GetComponentInChildren<SpriteRenderer>();
        }

        public override NodeState Evaluate()
        {
            var target = GetData<Transform>("target");

            if (target != null)
            {
                if (Vector3.Distance(this.Agent.transform.position, ((Transform)target).position) > 8f)
                {
                    RemoveData("target");
                    _renderer.color = Color.blue;
                    State = NodeState.FAILURE;
                    return State;
                }
            }

            if (target == null)
            {
                var colliders = Physics.OverlapSphere(
                    this.Agent.transform.position,
                    FovRange,
                    PlayerLayerMask
                );
                if (colliders.Length > 0)
                {
                    this.Root.SetData("target", colliders[0].transform);
                    _renderer.color = Color.red;
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