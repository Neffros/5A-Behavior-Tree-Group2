using BehaviorTree;
using Infiltration.Scripts;
using UnityEngine;

namespace Infiltration
{
    public class CheckEnemyInFOVRange : Node
    {
        private readonly Transform _transform;
        private const int EnemyLayerMask = 1 << 6;

        private SpriteRenderer _renderer;
        public CheckEnemyInFOVRange(Transform transform, SpriteRenderer renderer)
        {
            _transform = transform;
            _renderer = renderer;
        }

        public override NodeState Evaluate()
        {
            var target = GetData("target");

            if (target != null)
            {
                if (Vector3.Distance(_transform.position, ((Transform)target).position) > 10f)
                {
                    RemoveData("target");
                    _renderer.color = Color.blue;
                    State = NodeState.FAILURE;
                    return State;
                }
            }

            if (target == null)
            {
                var colliders = Physics.OverlapSphere(_transform.position, GuardAI.FOVRange, EnemyLayerMask);
                if (colliders.Length > 0)
                {
                    Parent.Parent.SetData("target", colliders[0].transform);
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