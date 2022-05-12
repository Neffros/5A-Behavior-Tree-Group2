using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class TaskGoTowardEnemy : Node
    {
        [ExposedInVisualEditor]
        public float Speed { get; set; }

        [ExposedInVisualEditor]
        public Transform Transform { get; set; }

        public override NodeState Evaluate()
        {
            var target = (Transform)GetData("target");

            if (Vector3.Distance(Transform.position, target.position) > .1f)
            {
                Transform.position =
                    Vector3.MoveTowards(Transform.position, target.position, Speed * Time.deltaTime);
                Transform.LookAt(target);
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}