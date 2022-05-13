using BehaviorTree;
using UnityEngine;

namespace CrowdControl
{
    public class CheckIfTooMuchPeople : Node
    {
        // [ExposedInVisualEditor]
        // public float DetectionRange { get; set; }

        private int _aiLayerMask;
        private float _detectionRange;

        protected override void OnInitialize()
        {
            _aiLayerMask = Agent.GetComponent<AgentData>().aiLayerMask;
            _detectionRange = Agent.GetComponent<AgentData>().DetectionAroundRange;
        }

        protected override NodeState OnUpdate()
        {
            Vector3 transform = Agent.transform.position;
            Debug.DrawRay(transform, Agent.transform.right * 2f);
            Collider[] colliders = Physics.OverlapSphere(transform,
                _detectionRange, _aiLayerMask);

            if (colliders.Length > 4)
            {
                SetData("TooMuchPeople", true);
                return NodeState.Failure;
            }

            RemoveData("TooMuchPeople");
            return NodeState.Success;
        }
    }
}