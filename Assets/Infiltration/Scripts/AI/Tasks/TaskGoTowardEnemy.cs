using BehaviorTree;
using UnityEngine;

namespace Infiltration
{
    public class TaskGoTowardEnemy : Node
    {
        private readonly Transform _transform;
        private readonly float _speed;

        public TaskGoTowardEnemy(Transform transform, float speed)
        {
            _transform = transform;
            _speed = speed;
        }

        public override NodeState Evaluate()
        {
            var target = (Transform)GetData("target");

            if (Vector3.Distance(_transform.position, target.position) > .1f)
            {
                _transform.position =
                    Vector3.MoveTowards(_transform.position, target.position, _speed * Time.deltaTime);
                _transform.LookAt(target);
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}