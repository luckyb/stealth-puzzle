using UnityEngine;

public abstract class Item : MonoBehaviour
{
	public System.Action<Item> onTouchedByPlayer;

	public Vector2 Position
	{
		get { return transform.localPosition; }
		set { transform.localPosition = new Vector3(value.x, value.y, 0); }
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerController player = other.gameObject.GetComponent<PlayerController>();
		if (player != null) onTouchedByPlayer(this);
	}
}
