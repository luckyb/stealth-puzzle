using UnityEngine;
using System.Collections;

public abstract class Obstacle : MonoBehaviour
{
	public System.Action<Obstacle> onPlayerDetected;

	Coroutine obstacleCoroutine;

	protected Coroutine StartObstacleCoroutine(IEnumerator routine)
	{
		obstacleCoroutine = StartCoroutine(routine);
		return obstacleCoroutine;
	}

	protected void TriggerPlayerDetected()
	{
		onPlayerDetected(this);
	}

	public void Stop()
	{
		StopCoroutine(obstacleCoroutine);
	}
}
