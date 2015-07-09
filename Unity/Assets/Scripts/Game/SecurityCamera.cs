using UnityEngine;
using System.Collections;

public class SecurityCamera : Obstacle
{
	[SerializeField] VisionCone visionCone;

	[SerializeField] float startRotation;
	[SerializeField] float endRotation;
	[SerializeField] float rotationPeriod;
	[SerializeField] float rotationPause;
	[SerializeField] bool clockwise;

	void Awake()
	{
		visionCone.onPlayerDetected = OnPlayerDetected;

		StartObstacleCoroutine(Rotate());
	}

	IEnumerator Rotate()
	{
		float factor = 0;
		float direction = 1;
		float delta = clockwise ? startRotation - endRotation : endRotation - startRotation;

		transform.localEulerAngles = new Vector3(0, 0, startRotation);

		while (true)
		{
			yield return new WaitForEndOfFrame();

			factor = Mathf.Clamp01(factor + Time.deltaTime / rotationPeriod * direction);
			transform.localEulerAngles = new Vector3(0, 0, delta * factor + startRotation);

			if (factor == 0 || factor == 1)
			{
				yield return new WaitForSeconds(rotationPause);

				direction = -direction;
			}
		}
	}

	void OnPlayerDetected()
	{
		TriggerPlayerDetected();
	}
}
