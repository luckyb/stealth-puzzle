using UnityEngine;
using System.Collections;

public abstract class Obstacle : MonoBehaviour
{
	[HideInInspector] public CoroutineHandler coroutineHandler;
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

	protected IEnumerator LookAt(Vector2 position, float time)
	{
		float factor = 0;
		
		float fromAngle = transform.localEulerAngles.z;
		
		Vector2 delta = position - Position;
		float toAngle = Mathf.Acos(Vector2.Dot(Vector2.up, delta) / delta.magnitude) * Mathf.Rad2Deg;
		toAngle = delta.x < 0 ? toAngle : 360f - toAngle;
		
		float turnAngle = toAngle - fromAngle;
		if (turnAngle > 180) turnAngle -= 360;
		
		while (true)
		{
			transform.localEulerAngles = new Vector3(0, 0, turnAngle * factor + fromAngle);
			
			if (factor == 1) break;
			
			yield return new WaitForEndOfFrame();
			
			factor = Mathf.Clamp01(factor + Time.deltaTime / time);
		}
	}

	protected IEnumerator ApproachTowards(Vector2 position, float approachFactor, float time)
	{
		float factor = 0;
		
		Vector2 fromPosition = Position;
		
		Vector2 delta = position - Position;
		float distance = delta.magnitude;
		Vector2 direction = delta / distance;
		Vector2 toPosition = Position + direction * distance * approachFactor;
		
		while (true)
		{
			Position = (toPosition - fromPosition) * factor + fromPosition;
			
			if (factor == 1) break;
			
			yield return new WaitForEndOfFrame();
			
			factor = Mathf.Clamp01(factor + Time.deltaTime / time);
		}
	}
}
