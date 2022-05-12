using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AgentAi agent = other.GetComponent<AgentAi>();
        if (agent == null)
            return;
        if (agent.TargetGoal != this.gameObject)
            return;
        agent.UpdatePosition();
    }

}
