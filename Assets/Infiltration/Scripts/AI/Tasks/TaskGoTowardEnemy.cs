using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class TaskGoTowardEnemy : Node {
        [ExposedInVisualEditor] public float Speed { get; set; } = 6;

        [ExposedInVisualEditor] public float UnseeRange { get; set; } = 7;
        
        private SpriteRenderer _renderer;

        protected override void OnInitialize()
        {
            this._renderer = this.Agent.GetComponent<GuardSceneData>().FieldOfView;
        }

        protected override NodeState OnStart()
        {
            _renderer.color = Color.red;

            return NodeState.Running;
        }

        protected override NodeState OnUpdate()
        {
            var target = this.GetData<Transform>("target");
            
            if (Vector3.Distance(this.Agent.transform.position, target.position) > UnseeRange || GameManager.StateSet)
            {
                RemoveData("target");
                _renderer.color = Color.blue;
                return NodeState.Failure;
            }

            if (Vector3.Distance(this.Agent.transform.position, target.position) > .1f)
            {
                this.Agent.transform.position = Vector3.MoveTowards(
                    this.Agent.transform.position,
                    target.position,
                    Speed * Time.deltaTime
                );
                this.Agent.transform.LookAt(target);
                
                return NodeState.Running;
            }
            
            return NodeState.Success;
        }
    }
}