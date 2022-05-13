using BehaviorTree;
using UnityEngine;

namespace CrowdControl
{
    public class CheckSide : Node
    {
        private LayerMask _aiMask;

        protected override void OnInitialize()
        {
            _aiMask = Agent.GetComponent<AgentData>().aiLayerMask;
        }

        protected override NodeState OnUpdate()
        {
            int rnd = Random.Range(0, 2);
            object alreadyCheckSide = GetData("OnRight");
            if (alreadyCheckSide != null)
            {
                return NodeState.Success;
            }

            // Check left or right first
            if (rnd == 1)
            {
                if (Physics.Raycast(Agent.transform.position, Agent.transform.right * 2f, out RaycastHit _, _aiMask))
                {
                    SetDataToRoot("OnRight", true);
                    return NodeState.Success;
                }

                if (Physics.Raycast(Agent.transform.position, -Agent.transform.right * 2f, out RaycastHit _, _aiMask))
                {
                    SetDataToRoot("OnRight", false);
                    return NodeState.Success;
                }
            }

            if (Physics.Raycast(Agent.transform.position, -Agent.transform.right * 2f, out RaycastHit _, _aiMask))
            {
                SetDataToRoot("OnRight", false);
                return NodeState.Success;
            }

            if (Physics.Raycast(Agent.transform.position, Agent.transform.right * 2f, out RaycastHit _, _aiMask))
            {
                SetDataToRoot("OnRight", true);
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}