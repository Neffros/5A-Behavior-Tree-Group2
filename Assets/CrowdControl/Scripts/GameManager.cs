using UnityEngine;
using Random = UnityEngine.Random;

namespace CrowdControl
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject agentPrefab;

		[SerializeField]
		private int nbAgents;
	
		[SerializeField]
		private Vector3 minSpawnPoint;
	
		[SerializeField]
		private Vector3 maxSpawnPoint;


		private void Start()
		{
			for (int i = 0; i < nbAgents; ++i)
			{
				Vector3 spawnPoint = new(Random.Range(minSpawnPoint.x, maxSpawnPoint.x),
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
}