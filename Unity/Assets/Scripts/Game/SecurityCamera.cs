using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SecurityCamera : Obstacle
{
	[SerializeField] VisionCone visionCone;

	[SerializeField] float startRotation;
	[SerializeField] float endRotation;
	[SerializeField] float rotationPeriod;
	[SerializeField] float rotationPause;
	[SerializeField] bool clockwise;

	[Space(8)]
	
	public bool snapToTopLeft;
	public bool snapToTop;
	public bool snapToTopRight;
	public bool snapToLeft;
	public bool snapToCenter;
	public bool snapToRight;
	public bool snapToBottomLeft;
	public bool snapToBottom;
	public bool snapToBottomRight;

	void Awake()
	{
		visionCone.onPlayerDetected = OnPlayerDetected;

		transform.localEulerAngles = new Vector3(0, 0, startRotation);
	}

	public override void Initiate()
	{
		StartObstacleCoroutine(Rotate());
	}

	void Update()
	{
		if (!Application.isPlaying)
		{
			if (snapToTopLeft)
			{
				snapToTopLeft = false;
				Snap(-25, 25);
			}
			
			if (snapToTop)
			{
				snapToTop = false;
				Snap(0, 25);
			}

			if (snapToTopRight)
			{
				snapToTopRight = false;
				Snap(25, 25);
			}
			
			if (snapToLeft)
			{
				snapToLeft = false;
				Snap(-25, 0);
			}
			
			if (snapToCenter)
			{
				snapToCenter = false;
				Snap(0, 0);
			}
			
			if (snapToRight)
			{
				snapToRight = false;
				Snap(25, 0);
			}
			
			if (snapToBottomLeft)
			{
				snapToBottomLeft = false;
				Snap(-25, -25);
			}
			
			if (snapToBottom)
			{
				snapToBottom = false;
				Snap(0, -25);
			}
			
			if (snapToBottomRight)
			{
				snapToBottomRight = false;
				Snap(25, -25);
			}
			
			return;
		}
	}

	void Snap(float xOffset, float yOffset)
	{
		Position = new Vector2(Mathf.Round(Position.x / 100f) * 100f, Mathf.Round(Position.y / 100f) * 100f) + new Vector2(xOffset, yOffset);
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

	void OnPlayerDetected(PlayerController player)
	{
		StartCoroutine(LookAt(player.Position, 0.25f));
		TriggerPlayerDetected();
	}
}
