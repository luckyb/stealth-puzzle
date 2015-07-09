using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
	[SerializeField] Transform playerController;

	Camera camera;

	void Start()
	{
		camera = GetComponent<Camera>();
	}

	void Update()
	{
		int height = Screen.height % 500;
		if (height < 250) height += 500;
		camera.orthographicSize = height;

		transform.localPosition = new Vector3(playerController.localPosition.x, playerController.localPosition.y, transform.localPosition.z);
	}
}
