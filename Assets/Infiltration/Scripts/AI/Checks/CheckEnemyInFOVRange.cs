using BehaviorTree;
using UnityEngine;

namespace Infiltration
{
    public class CheckEnemyInFOVRange : Node
    {
        private readonly Transform _transform;
        private const int PlayerLayerMask = 1 << 6;

        private readonly SpriteRenderer _renderer;
        private readonly float _fovRange;
        public CheckEnemyInFOVRange(Transform transform, SpriteRenderer renderer, float fovRange)
        {
            _transform = transform;
            _renderer = renderer;
            _fovRange = fovRange;
        }

        public override NodeState Evaluate()
        {
            var target = GetData("target");

            if (target != null)
            {
                if (Vector3.Distance(_transform.position, ((Transform)target).position) > 8f)
                {
                    RemoveData("target");
                    _renderer.color = Color.blue;
                    State = NodeState.FAILURE;
                    return State;
                }
            }

            if (target == null)
            {
                var colliders = Physics.OverlapSphere(_transform.position, _fovRange, PlayerLayerMask);
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