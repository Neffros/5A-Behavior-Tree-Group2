using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace CrowdControl
{
    public class TaskLetIsPassRight : Node
    {
        private NavMeshAgent _agent;
        private bool _isMoving;
        private Vector3 _sideDestination;

        protected override void OnInitialize()
        {
            _agent = Agent.GetComponent<AgentData>().NavAgent;
        }
        protected override NodeState OnUpdate()
        {
            object onRight = GetData("OnRight");
            if (onRight == null)
            {
                return NodeState.Failure;
            }

            bool move = (bool)onRight;
            if (!move)
            {
                return NodeState.Failure;
            }
            Vector3 currentPos = Agent.transform.position;
            if (!_isMoving)
            {
                _sideDestination = new Vector3(currentPos.x + 2f, currentPos.y, currentPos.z);
                _agent.SetDestination(_sideDestination);
                Debug.Log("Dodge right");
                _isMoving = true;
            }

            if (Vector3.Distance(currentPos, _sideDestination) < .1f)
            {
                _isMoving = false;
                Debug.Log("Finish right dodge");
                return NodeState.Success;
            }

            return NodeState.Running;
        }
    }
}