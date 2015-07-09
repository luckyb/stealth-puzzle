using UnityEngine;
using System.Collections;

public abstract class Obstacle : MonoBehaviour
{
	public CoroutineHandler coroutineHandler;
	public System.Action<Obstacle> onPlayerDetected;

	protected Coroutine StartObstacleCoroutine(IEnumerator routine)
	{
		return coroutineHandler.StartCoroutine(routine);
	}

	protected void TriggerPlayerDetected()
	{
		onPlayerDetected(this);
	}

	public abstract void Initiate();

	public Vector2 Position
	{
		get { return transform.localPosition; }
		set { transform.localPosition = new Vector3(value.x, value.y, 0); }
	}
}
