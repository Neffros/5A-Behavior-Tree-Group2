using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> goals;

    public GameObject agentPrefab;
    public int nbAgents;
    public Vector3 minSpawnPoint;
    public Vector3 maxSpawnPoint;

    #region Singleton

    private static GameManager _instance;
    
    public static GameManager Instance => _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

    }

    #endregion


    private void Start()
    {
        for (int i = 0; i < nbAgents; ++i)
        {
            Instantiate(agentPrefab,
                new Vector3(Random.Range(minSpawnPoint.x, maxSpawnPoint.x),
                    Random.Range(minSpawnPoint.y, maxSpawnPoint.y), Random.Range(minSpawnPoint.z, maxSpawnPoint.z)),
                Quaternion.identity);
        }
    }

    public GameObject GetNewTarget(GameObject oldTarget)
    {
        GameObject newPos = goals[Random.Range(0, goals.Count)];

        return newPos == oldTarget ? GetNewTarget(oldTarget) : newPos;
    }

}
