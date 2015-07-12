using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
	[SerializeField] Transform playerController;

	new Camera camera;

	void Start()
	{
		camera = GetComponent<Camera>();
	}

	void Update()
	{
		int height = Screen.height % 500;
		if (height < 250) height += 500;
		camera.orthographicSize = height;

		transform.position = new Vector3(playerController.position.x, playerController.position.y, transform.position.z);
	}
}
