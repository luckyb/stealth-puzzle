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

		Vector2 from = transform.localPosition;

		while (true)
		{
			transform.localPosition = (to - from) * factor + from;

			if (factor == 1) break;

			yield return new WaitForEndOfFrame();

			factor = Mathf.Clamp01(factor + Time.deltaTime / time);
		}
	}
}
