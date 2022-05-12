﻿using BehaviorTree;
using NodeReflection;
using UnityEngine;

namespace Infiltration
{
    [VisualNode]
    public class TaskAttackPlayer : Node
    {
        [ExposedInVisualEditor]
        public float AttackRange { get; set; }

        public override NodeState Evaluate()
        {
            var target = this.GetData<Transform>("target");

            if (Vector3.Distance(this.Agent.transform.position, target.position) <= AttackRange)
            {
                var player = target.GetComponent<PlayerMovement>();
                player.GetHit();
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}