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

	public Vector2 Position
	{
		get { return transform.localPosition; }
		set { transform.localPosition = new Vector3(value.x, value.y, 0); }
	}
}
