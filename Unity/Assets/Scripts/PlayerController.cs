using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] Rigidbody rigidbody;
	[SerializeField] InputController inputController;
	[SerializeField] float speed;

	void FixedUpdate()
	{
		rigidbody.MovePosition(rigidbody.position + (Vector3)inputController.Input * speed * Time.fixedDeltaTime);
	}
}
