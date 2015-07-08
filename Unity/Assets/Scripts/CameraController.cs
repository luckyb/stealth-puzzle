using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
	[SerializeField] GameObject playerController;

	new Camera camera;

	void Start()
	{
		camera = GetComponent<Camera>();
	}

	void Update()
	{
		int height = Screen.height;
		height = height % 500;
		if (height < 250) height += 500;
		camera.orthographicSize = height;
	}
}
