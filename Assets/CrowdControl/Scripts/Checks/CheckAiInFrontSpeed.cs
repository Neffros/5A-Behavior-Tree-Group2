using BehaviorTree;

namespace CrowdControl
{
    public class CheckAiInFrontSpeed : Node
    {
        private AgentData _agentData;

        protected override void OnInitialize()
        {
            _agentData = Agent.GetComponent<AgentData>();
        }

        protected override NodeState OnStart()
        {
            AgentData aiInFront = GetData<AgentData>("AiInFront");

            if (aiInFront != null)
            {
                return aiInFront.Speed > _agentData.Speed ? NodeState.Failure : NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}