using BehaviorTree;
using UnityEngine;

namespace Infiltration
{
    public class TaskGoTowardEnemy : Node
    {
        private readonly Transform _transform;

        public TaskGoTowardEnemy(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            var target = (Transform)GetData("target");

            if (Vector3.Distance(_transform.position, target.position) > .1f)
            {
                _transform.position =
                    Vector3.MoveTowards(_transform.position, target.position, GuardAI.Speed * Time.deltaTime);
                _transform.LookAt(target);
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}