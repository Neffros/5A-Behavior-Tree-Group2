﻿using BehaviorTree;

namespace CrowdControl
{
    public class TaskShiftOnTheLeft : Node
    {
        protected override NodeState OnUpdate()
        {
            return NodeState.Running;
        }
    }
}