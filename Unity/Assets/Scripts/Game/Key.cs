using UnityEngine;

[ExecuteInEditMode]
public class Key : Item
{
	[SerializeField] Color keyCode = Color.red;

	new SpriteRenderer renderer;

	void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		RefreshKey();
	}

	void Update()
	{
		if (!Application.isPlaying)
		{
			RefreshKey();
			return;
		}
	}

	void RefreshKey()
	{
		renderer.color = keyCode;
	}

	public Color KeyCode
	{
		get { return keyCode; }
	}
}
