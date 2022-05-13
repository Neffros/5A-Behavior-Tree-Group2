using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace CrowdControl
{
    public class AgentData : MonoBehaviour
    {
        [SerializeField] private List<Vector3> waypoints;
        [SerializeField] private NavMeshAgent navAgent;

        [SerializeField] private float radius = 5f;
        [SerializeField] private float minSpeed = 3.5f;
        [SerializeField] private float maxSpeed = 7f;

        [SerializeField] private float detectionAroundRange = 2.5f;

        [SerializeField] private float detectionInFrontRange = 4f;

        public int aiLayerMask = 1 << 6;

        private float _patience;
        private float _speed;

        public float Radius => radius;
        public float DetectionInFrontRange => detectionInFrontRange;
        public float DetectionAroundRange => detectionAroundRange;

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
}