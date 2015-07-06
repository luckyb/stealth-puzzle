using UnityEngine;

public class InputController : MonoBehaviour
{
	[SerializeField] SpriteRenderer background;
	[SerializeField] SpriteRenderer knob;
	[SerializeField] float inputRadius;

	bool inputActive;
	Vector2 inputOrigin, inputPosition, inputValue;

	void Update()
	{
		if (inputActive)
		{
			Vector2 delta = inputPosition - inputOrigin;

			float distance = delta.magnitude;
			if (distance > 0)
			{
				Vector2 direction = delta / distance;
				if (distance > inputRadius)
				{
					inputOrigin += direction * (distance - inputRadius);
					distance = inputRadius;
				}
				
				inputValue = direction * distance / inputRadius;
			}
			else
			{
				inputValue = Vector2.zero;
			}
		}
		else
		{
			inputValue = Vector2.zero;
		}

		background.gameObject.SetActive(inputActive);
		background.transform.localPosition = inputOrigin;
		background.transform.localScale = new Vector3(inputRadius / background.sprite.bounds.size.x * 2f, inputRadius / background.sprite.bounds.size.y * 2f, 1);

		knob.gameObject.SetActive(inputActive);
		knob.transform.localPosition = inputPosition;
	}

	void OnDetectFingerDown(FingerDownEvent e)
	{
		inputActive = true;
		inputOrigin = GetScreenPosition(e.Position);
		inputPosition = inputOrigin;
	}

	void OnDetectFingerUp(FingerUpEvent e)
	{
		inputActive = false;
	}

	void OnDetectFingerMotion(FingerMotionEvent e)
	{
		inputPosition = GetScreenPosition(e.Position);
	}

	Vector2 GetScreenPosition(Vector2 mousePosition)
	{
		mousePosition -= new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
		mousePosition.x *= 640f / Screen.width;
		mousePosition.y *= 1136f / Screen.height;
		return mousePosition;
	}

	public Vector2 Input
	{
		get { return inputValue; }
	}

	public void Stop()
	{
		gameObject.SetActive(false);
		inputValue = Vector2.zero;
	}
}
