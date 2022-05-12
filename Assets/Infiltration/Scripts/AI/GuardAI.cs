using BehaviorTree;
using UnityEngine;

namespace Infiltration
{
    public class GuardAI : BehaviorTreeAgent
    {
        public const float AttackRange = 2f;
        public const float FOVRange = 6f;
        public const float Speed = 6f;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private SpriteRenderer fieldOfView;

        protected override Node SetupTree()
        {
            var root = new Selector();
            var currentTransform = transform;

            var sequence1 = new Sequence();
            var checkEnemyInRange = new CheckEnemyInRange(currentTransform);
            var taskAttackPlayer = new TaskAttackPlayer(currentTransform);

            var sequence2 = new Sequence();
            var checkEnemyInFOVRange =
                new CheckEnemyInFOVRange(currentTransform, fieldOfView);
            var taskGoTowardEnemy = new TaskGoTowardEnemy(currentTransform);

            var taskGoPatrol = new TaskPatrol(currentTransform, waypoints);

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
            Gizmos.DrawWireSphere(currentPosition, AttackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(currentPosition, FOVRange);
        }
    }
}