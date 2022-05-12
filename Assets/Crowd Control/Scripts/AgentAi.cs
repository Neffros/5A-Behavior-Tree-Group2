using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AgentAi : MonoBehaviour
{

    private float _patience; 

    private GameObject _targetGoal;
    
    public NavMeshAgent agent;
    public GameObject TargetGoal => _targetGoal;

    private void Start()
    {
        UpdatePosition();
        _patience = Random.Range(0, 100);
    }

    public void UpdatePosition()
    {
        _targetGoal = GameManager.Instance.GetNewTarget(_targetGoal);
        agent.destination = _targetGoal.transform.position;
    }
}
