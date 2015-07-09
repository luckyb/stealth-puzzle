using UnityEngine;

public static class Tools
{
	public static GameObject AddChild(this GameObject parent, GameObject prefab)
	{
		GameObject gameObject = GameObject.Instantiate(prefab);
		gameObject.transform.SetParent(parent.transform);
		gameObject.transform.localPosition = prefab.transform.localPosition;
		gameObject.transform.localEulerAngles = prefab.transform.localEulerAngles;
		gameObject.transform.localScale = prefab.transform.localScale;
		return gameObject;
	}

	public static T AddChild<T>(this GameObject parent, GameObject prefab)
		where T : Component
	{
		GameObject gameObject = GameObject.Instantiate(prefab);
		gameObject.transform.SetParent(parent.transform);
		gameObject.transform.localPosition = prefab.transform.localPosition;
		gameObject.transform.localEulerAngles = prefab.transform.localEulerAngles;
		gameObject.transform.localScale = prefab.transform.localScale;
		return gameObject.GetComponent<T>();
	}

	public static T AddChild<T>(this Component parent)
		where T : Component
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.SetParent(parent.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		return gameObject.AddComponent<T>();
	}

	public static void Destroy(this Component component)
	{
		if (Application.isPlaying)
		{
			GameObject.Destroy(component.gameObject);
		}
		else
		{
			GameObject.DestroyImmediate(component.gameObject);
		}
	}
	
	public static void DestroyChildren(this GameObject parent)
	{
		for (int i = parent.transform.childCount - 1; i >= 0; i--)
		{
			parent.transform.GetChild(i).Destroy();
		}
	}

	public static void DestroyChildren(this Component parent)
	{
		for (int i = parent.transform.childCount - 1; i >= 0; i--)
		{
			parent.transform.GetChild(i).Destroy();
		}
	}
}
