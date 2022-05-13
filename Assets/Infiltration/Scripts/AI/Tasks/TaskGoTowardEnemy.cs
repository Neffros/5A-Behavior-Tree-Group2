using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class TaskGoTowardEnemy : Node {
        [ExposedInVisualEditor] public float Speed { get; set; } = 6;

        protected override NodeState OnEvaluate()
        {
            var target = this.GetData<Transform>("target");

            if (Vector3.Distance(this.Agent.transform.position, target.position) > .1f)
            {
                this.Agent.transform.position = Vector3.MoveTowards(
                    this.Agent.transform.position,
                    target.position,
                    Speed * Time.deltaTime
                );
                this.Agent.transform.LookAt(target);
            }

            return NodeState.RUNNING;
        }
    }
}