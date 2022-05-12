using BehaviorTree;
using UnityEngine;

namespace Infiltration
{
    public class GuardAI : BehaviorTreeAgent
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float fovRange;
        [SerializeField] private float speed;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private SpriteRenderer fieldOfView;

        protected override Node SetupTree()
        {
            var root = new Selector();
            var currentTransform = transform;

            var inverter1 = new Inverter();
            var checkGameState = new CheckGameState();
            
            var sequence1 = new Sequence();
            var checkEnemyInRange = new CheckEnemyInRange(currentTransform, attackRange);
            var taskAttackPlayer = new TaskAttackPlayer(currentTransform, attackRange);

            var sequence2 = new Sequence();
            var checkEnemyInFOVRange =
                new CheckEnemyInFOVRange(currentTransform, fieldOfView, fovRange);
            var taskGoTowardEnemy = new TaskGoTowardEnemy(currentTransform, speed);

            var taskGoPatrol = new TaskPatrol(currentTransform, waypoints, speed);

            // Setup inverter1
            root.Attach(inverter1);
            inverter1.Attach(checkGameState);
            
            // Setup sequence1
            root.Attach(sequence1);
            sequence1.Attach(checkEnemyInRange)
                .Attach(taskAttackPlayer);

            // Setup sequence2
            root.Attach(sequence2);
            sequence2.Attach(checkEnemyInFOVRange).Attach(taskGoTowardEnemy);

            // Setup default task
            root.Attach(taskGoPatrol);

            return root;
        }

        private void OnDrawGizmos()
        {
            var currentPosition = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentPosition, attackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(currentPosition, fovRange);
        }
    }
}