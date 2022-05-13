using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AgentData : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> waypoints;
    [SerializeField]
    private NavMeshAgent navAgent;
    [SerializeField]
    private float minSpeed = 3.5f;
    [SerializeField]
    private float maxSpeed = 7f;
    
    private float _patience;
    private float _speed;

    public float MinSpeed => minSpeed;

    public float MaxSpeed => maxSpeed;

    public float Speed => _speed;

    public float Patience => _patience;

    public List<Vector3> Waypoints => waypoints;

    public NavMeshAgent NavAgent => navAgent;

    private void Start()
    {
        _patience = Random.Range(0f, 100f);
        _speed = Random.Range(minSpeed, maxSpeed);

    }
}
