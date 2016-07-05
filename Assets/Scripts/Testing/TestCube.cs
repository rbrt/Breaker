using UnityEngine;
using System.Collections;

public class TestCube : MonoBehaviour
{

	Vector3 startPosition;
	Vector3 tempPosition;
	float offset = 1;

	void Awake()
	{
		startPosition = transform.position;
		offset = Random.Range(0f,10f);
	}

	void Update ()
	{
		tempPosition = startPosition;
		tempPosition.x += Mathf.Sin(Time.time + offset * 2);
		tempPosition.y += Mathf.Cos(Time.time + offset * 2);
		transform.position = tempPosition;

		transform.Rotate(Vector3.up, 5);
	}
}
