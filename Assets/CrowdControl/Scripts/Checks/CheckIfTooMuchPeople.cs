using BehaviorTree;
using UnityEngine;

namespace CrowdControl
{
    public class CheckIfTooMuchPeople : Node
    {
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
            Collider[] colliders = Physics.OverlapSphere(transform,
                _detectionRange, _aiLayerMask);

            if (colliders.Length > 3)
            {
                SetData("TooMuchPeople", true);
                return NodeState.Failure;
            }

            RemoveData("TooMuchPeople");
            return NodeState.Success;
        }
    }
}