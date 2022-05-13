using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class CheckEnemyInFOVRange : Node
    {
        private const int PlayerLayerMask = 1 << 6;

        [ExposedInVisualEditor] public float FovRange { get; set; } = 6;

        private SpriteRenderer _renderer;

        protected override void OnInitialized()
        {
            this._renderer = this.Agent.GetComponent<GuardSceneData>().FieldOfView;
        }

        protected override NodeState OnEvaluate()
        {
            var target = GetData<Transform>("target");

            if (target != null)
            {
                if (Vector3.Distance(this.Agent.transform.position, target.position) > 8f)
                {
                    RemoveData("target");
                    _renderer.color = Color.blue;
                    return NodeState.FAILURE;
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
                    return NodeState.SUCCESS;
                }

                return NodeState.FAILURE;
            }

            return NodeState.SUCCESS;
        }
    }
}