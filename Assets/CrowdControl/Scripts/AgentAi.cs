using BehaviorTree;
using UnityEngine;

namespace CrowdControl
{
    public class AgentAi : BehaviorTreeAgent
    {
        protected override Node SetupTree()
        {
            Node nodeRoot = new Selector();
            TaskMoveToWayPoint taskMove = new();
            Sequence sequence1 = new();
            CheckIfTooMuchPeople checkIfTooMuchPeople = new();
            CheckAiInFront checkAiInFront = new();

            Sequence sequence2 = new();
            Sequence sequence3 = new();

            CheckSide checkSide = new();
            CheckPatience checkPatience = new();
            Selector selector1 = new();
            Selector selector2 = new();

            TaskLetIsPassLeft taskLetIsPassLeft = new();
            TaskLetIsPassRight taskLetIsPassRight = new();

            CheckAiInFrontSpeed checkAiInFrontSpeed = new();
            Selector selector3 = new();
            TaskShiftOnTheLeft taskShiftOnTheLeft = new();
            TaskShiftOnTheRight taskShiftOnTheRight = new();


            // Setup sequence 1
            sequence1.Attach(checkIfTooMuchPeople).Attach(checkAiInFront).Attach(taskMove);
            nodeRoot.Attach(sequence1);

            // Setup sequence 2
            sequence2.Attach(checkAiInFront).Attach(checkSide).Attach(checkPatience).Attach(selector1);
            selector1.Attach(selector2).Attach(sequence3);
            sequence3.Attach(checkAiInFrontSpeed).Attach(selector3);
            selector2.Attach(taskLetIsPassLeft).Attach(taskLetIsPassRight);
            selector3.Attach(taskShiftOnTheLeft).Attach(taskShiftOnTheRight);
            nodeRoot.Attach(sequence2);

            return nodeRoot;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red; 
            Vector3 position = transform.position;
            Gizmos.DrawWireSphere(position, 2.5f);
        }
    }
}