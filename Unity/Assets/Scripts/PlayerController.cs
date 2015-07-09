using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	[SerializeField] new Rigidbody rigidbody;
	[SerializeField] InputController inputController;
	[SerializeField] float speed;

	void FixedUpdate()
	{
		rigidbody.MovePosition(rigidbody.position + (Vector3)inputController.Input * speed * Time.fixedDeltaTime);
	}

	public void MoveToPosition(Vector2 position, float time)
	{
		StartCoroutine(InternalMoveToPosition(position, time));
	}

	IEnumerator InternalMoveToPosition(Vector2 to, float time)
	{
		float factor = 0;

		Vector2 from = Position;

		while (true)
		{
			Position = (to - from) * factor + from;

			if (factor == 1) break;

			yield return new WaitForEndOfFrame();

			factor = Mathf.Clamp01(factor + Time.deltaTime / time);
		}
	}

	public Vector2 Position
	{
		get { return transform.localPosition; }
		set { transform.localPosition = new Vector3(value.x, value.y, 50); }
	}
}
