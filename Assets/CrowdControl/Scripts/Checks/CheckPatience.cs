using BehaviorTree;
using UnityEngine;

namespace CrowdControl
{
    public class CheckPatience : Node
    {
        private float _patience;
        protected override void OnInitialize()
        {
            _patience = Agent.GetComponent<AgentData>().Patience;
        }

        protected override NodeState OnStart()
        {
            float getAction = Random.Range(0, 100);
            if (getAction<= _patience)
            {
                SetData("LetIsPass", true);
            }
            else
            {
                SetData("LetIsPass", false);
            }
            return NodeState.Success;
        }
    }
}