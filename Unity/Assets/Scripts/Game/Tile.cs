using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
	public enum TileType
	{
		Normal,
		Wall,
		Spawn,
		Goal,
		Door
	}

	[SerializeField] TileType type;
	[SerializeField] Color doorCode = Color.red;

	[Space(8)]

	public bool generateSecurityCamera;
	public bool generatePatrolGuard;
	public bool generateKey;
	
	new SpriteRenderer renderer;
	new BoxCollider collider;

	Level level;

	bool doorOpen;

	void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<BoxCollider>();
		collider.center = new Vector3(0, 0, -0.5f);
		level = GetComponentInParent<Level>();
		RefreshTile();
	}

	void Update()
	{
		if (!Application.isPlaying)
		{
			if (generateSecurityCamera)
			{
				generateSecurityCamera = false;
				level.GenerateSecurityCameraAtTile(this);
			}

			if (generatePatrolGuard)
			{
				generatePatrolGuard = false;
				level.GeneratePatrolGuardAtTile(this);
			}

			if (generateKey)
			{
				generateKey = false;
				level.GenerateKeyAtTile(this);
			}

			RefreshTile();

			return;
		}
	}
	
	void RefreshTile()
	{
		if (type == TileType.Normal)
		{
			renderer.color = Color.black;
			collider.enabled = false;
		}
		else if (type == TileType.Wall)
		{
			renderer.color = Color.gray;
			collider.enabled = true;
			collider.isTrigger = false;
		}
		else if (type == TileType.Spawn)
		{
			renderer.color = Color.black;
			collider.enabled = false;
			level.SetSpawnTile(this);
		}
		else if (type == TileType.Goal)
		{
			renderer.color = Color.cyan;
			collider.enabled = true;
			collider.isTrigger = true;
		}
		else if (type == TileType.Door)
		{
			renderer.color = doorOpen ? Color.black : Color.red;
			collider.enabled = !doorOpen;
			collider.isTrigger = false;
		}
	}

	public TileType Type
	{
		get { return type; }
		set { type = value; RefreshTile(); }
	}

	public Color DoorCode
	{
		get { return doorCode; }
	}

	public bool DoorOpen
	{
		get { return doorOpen; }
		set { doorOpen = value; RefreshTile(); }
	}

	public Sprite Sprite
	{
		get { return renderer.sprite; }
		set { renderer.sprite = value; }
	}

	public Vector2 Position
	{
		get { return transform.localPosition; }
		set { transform.localPosition = new Vector3(value.x, value.y, 50); }
	}

	public Vector2 Size
	{
		get { return transform.localScale; }
		set { transform.localScale = new Vector3(value.x, value.y, 100); }
	}

	void OnCollisionEnter(Collision collision)
	{

	}

	void OnTriggerEnter(Collider other)
	{
		if (type == TileType.Goal)
		{
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			if (player != null) level.TriggerPlayerReachedGoal(this);
		}
	}
}
