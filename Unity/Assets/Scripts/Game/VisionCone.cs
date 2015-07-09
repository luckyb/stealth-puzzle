using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class VisionCone : MonoBehaviour
{
	[SerializeField] float angle = 45;
	[SerializeField] float range = 200;
	[SerializeField] float width = 50;

	MeshFilter meshFilter;
	MeshCollider meshCollider;
	MeshRenderer meshRenderer;

	Mesh mesh;

	public bool regenerateMesh;

	public System.Action<PlayerController> onPlayerDetected;

	void Awake()
	{
		if (mesh == null) mesh = new Mesh();

		meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		meshCollider = GetComponent<MeshCollider>();
		meshCollider.convex = true;
		meshCollider.isTrigger = true;

		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		meshRenderer.receiveShadows = false;
		meshRenderer.useLightProbes = false;
		meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

		GenerateMesh();
	}

	void Update()
	{
		if (regenerateMesh)
		{
			regenerateMesh = false;
			GenerateMesh();
		}
	}

	void OnDestroy()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			Mesh.DestroyImmediate(mesh);
		}
		else
#endif
		{
			Mesh.Destroy(mesh);
		}
	}

	void GenerateMesh()
	{
		float x = Mathf.Tan(angle * Mathf.Deg2Rad * 0.5f) * range;

		mesh.vertices = new Vector3[]{
			new Vector3(0, 0, -width * 0.5f),
			new Vector3(-x, range, -width * 0.5f),
			new Vector3(x, range, -width * 0.5f),
			new Vector3(0, 0, width * 0.5f),
			new Vector3(-x, range, width * 0.5f),
			new Vector3(x, range, width * 0.5f)
		};
		
		mesh.uv = new Vector2[]{
			new Vector2(0.5f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(0.5f, 0.5f)
		};

		mesh.triangles = new int[]{
			0, 1, 2,
			0, 3, 4,
			0, 4, 1,
			0, 2, 5,
			0, 5, 3,
			3, 5, 4,
			2, 1, 4,
			2, 4, 5
		};
		
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = mesh;
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerController player = other.gameObject.GetComponent<PlayerController>();
		if (player != null) onPlayerDetected(player);
	}
}
