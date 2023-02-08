using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoad : MonoBehaviour {

	public GameObject player;

	//Queue with the active roads
	private Queue<GameObject> activeRoads;
	//Array with the inactive roads
	public List<GameObject> inactiveRoads = new List<GameObject>();

	//Position in Z where the road will spawn
	private float zPosition;
	//Maximum distance to the player at which the roads are allowed to spawn
	public float renderDistance = 120.0f;

	//Bool to know when the game is already spawning a road
	private bool spawningPlatform = false;

	//Variable for the start platform to be able to disable it later
	public GameObject startPlatform;

	// Use this for initialization
	void Start () {
		zPosition = startPlatform.GetComponent<MeshRenderer>().bounds.size.z;

		activeRoads = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		//Checks if player is close enough to spawn a new platform
		if (spawningPlatform == false &&  player.transform.position.z + renderDistance >= zPosition)
        {
			spawningPlatform = true;
			StartCoroutine(SpawnRoad());
        }

		//Deactivates the start platform once the player gets far enough from it to save resources
		if (startPlatform.active && player.transform.position.z >= 75)
        {
			startPlatform.SetActive(false);
        }

		//Checks if there are active roads
		if (activeRoads.Peek() != null)
        {
			//Checks if the road is far enough away from the player
			if (activeRoads.Peek().transform.position.z + renderDistance <= player.transform.position.z)
			{
				//Deactivates the road and places it back in the pool
				GameObject removeRoad = activeRoads.Dequeue();

				removeRoad.SetActive(false);

				inactiveRoads.Add(removeRoad);
			}
        }
	}

	IEnumerator SpawnRoad()
    {
		//Selects a random road platform from the roads that are inactive
		int roadPlatformNum = Random.Range(0, inactiveRoads.Count);

		GameObject newRoad = inactiveRoads[roadPlatformNum];

		//Removes the new road platform from the inactive list
		inactiveRoads.RemoveAt(roadPlatformNum);

		//Adjusts the position and activates it
		newRoad.transform.position = new Vector3(0.0f, 0.0f, zPosition);
		newRoad.SetActive(true);

		//Re-activates power-ups
		checkPowerUps(newRoad);

		//Adds the road to the active roads queue
		activeRoads.Enqueue(newRoad);

		//Adjusts the spawning position so it's ready for the next road spawn
		zPosition += newRoad.GetComponentInChildren<MeshRenderer>().bounds.size.z;

		//Returns that the spawning process is over
		yield return spawningPlatform = false;
    }

	//When the player gets a power-up, it's mesh renderer and capsule collider
	//With this method it re-activates any existing power-ups in that road
	public void checkPowerUps(GameObject newRoad)
    {
		//Checks for the mesh renderers and activates them
		foreach (var meshRenderer in newRoad.GetComponentsInChildren<MeshRenderer>())
        {
			meshRenderer.enabled = true;
        }

		//Checks for the capsule colliders and activates them
		foreach (var capsuleCollider in newRoad.GetComponentsInChildren<CapsuleCollider>())
		{
			capsuleCollider.enabled = true;
		}
	}
}
