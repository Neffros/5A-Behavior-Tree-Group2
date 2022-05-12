using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public GameObject agentPrefab;
	public GameObject buildingPrefab;
    
	public int nbAgents;
	public Vector3 minSpawnPoint;
	public Vector3 maxSpawnPoint;

	private void Start()
	{
		for (int i = 0; i < nbAgents; ++i)
		{
			Vector3 spawnPoint = new Vector3(Random.Range(minSpawnPoint.x, maxSpawnPoint.x),
				Random.Range(minSpawnPoint.y, maxSpawnPoint.y), Random.Range(minSpawnPoint.z, maxSpawnPoint.z));
            
			RaycastHit hit;
			if (Physics.Raycast(spawnPoint + Vector3.up*50, Vector3.down, out hit, 100, LayerMask.GetMask("Building")))
			{
				--i;
				continue;
                
			}

			Instantiate(agentPrefab, spawnPoint, Quaternion.identity);
		}
	}

}