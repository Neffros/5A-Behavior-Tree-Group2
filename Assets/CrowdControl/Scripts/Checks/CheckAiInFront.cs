using BehaviorTree;
using UnityEngine;

namespace CrowdControl
{
    public class CheckAiInFront : Node
    {
        private Vector3 _boxSize;

        private int _aiLayerMask;

        protected override void OnInitialize()
        {
            _aiLayerMask = Agent.GetComponent<AgentData>().aiLayerMask;
            _boxSize = new Vector3(1f, 1f, Agent.GetComponent<AgentData>().DetectionInFrontRange);
        }

        protected override NodeState OnUpdate()
        {
            object tooMuchPeople = GetData("TooMuchPeople");
            if (tooMuchPeople != null)
            {
                if ((bool)tooMuchPeople)
                {
                    return NodeState.Failure;
                }
            }

            object aiInFront = GetData("AiInFront");
            if (aiInFront != null)
            {
                return NodeState.Success;
            }

            Vector3 transform = Agent.transform.position;
            Collider[] colliders = Physics.OverlapBox(transform, _boxSize, Agent.transform.localRotation, _aiLayerMask);

            if (colliders.Length > 0)
            {
                SetData("AiInFront", colliders[0].GetComponent<AgentData>());
                Vector3 pos = colliders[0].transform.position;
                Vector3 dir = (Agent.transform.position - pos).normalized;
                SetData("DirectionAiInFront", dir);

                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}