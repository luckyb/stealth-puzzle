using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
public class VictoryZone : MonoBehaviour
{
	BoxCollider boxCollider;

	public System.Action onPlayerEnter;

	void Awake()
	{
		boxCollider = GetComponent<BoxCollider>();
		boxCollider.isTrigger = true;
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerController player = other.gameObject.GetComponent<PlayerController>();
		if (player != null) onPlayerEnter();
	}
}
