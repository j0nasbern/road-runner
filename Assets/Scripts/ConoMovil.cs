using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConoMovil : MonoBehaviour {

	public int speed;
	private int currentSpeed;

	// Use this for initialization
	void OnEnable () {
		speed = Random.Range(8, 16);

		//Starts the cone in a random X position on the road
		Vector3 position = transform.position;

		position.x = Random.Range(-8, 9);

		//Moves the cone in a random direction in X
		int move = Random.Range(0, 2);

		if (move == 0)
		{
			currentSpeed = -speed;
		}
		else currentSpeed = speed;

		transform.position = position;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 position = transform.position;

		//If the cone reaches the border of the road it starts moving in the other X direction
		if (position.x <= -8)
		{
			currentSpeed = speed;
		} else if (position.x >= 8)
        {
			currentSpeed = -speed;
		}

		position.x += currentSpeed * Time.deltaTime;

		transform.position = position;
	}
}
