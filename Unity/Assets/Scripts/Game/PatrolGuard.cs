using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolGuard : Obstacle
{
	[System.Serializable]
	class PatrolNode
	{
		public enum Type
		{
			Move,
			Rotate,
			Pause
		}
		
		public Type type = Type.Move;
		public float time = 0;
		public Vector2 moveDelta = Vector2.zero;
		public float rotateDelta = 0;
	}

	[SerializeField] VisionCone visionCone;

	[SerializeField] List<PatrolNode> nodes;
	[SerializeField] bool pingPong;

	void Awake()
	{
		visionCone.onPlayerDetected = OnPlayerDetected;
	}

	public override void Initiate()
	{
		StartObstacleCoroutine(Patrol());
	}

	IEnumerator Patrol()
	{
		int index = 0;
		float direction = 1;

		while (true)
		{
			PatrolNode node = nodes[index];

			if (node.type == PatrolNode.Type.Move)
			{
				yield return StartObstacleCoroutine(PatrolMove(node.time, node.moveDelta * direction));
			}
			else if (node.type == PatrolNode.Type.Rotate)
			{
				yield return StartObstacleCoroutine(PatrolRotate(node.time, node.rotateDelta * direction));
			}
			else
			{
				yield return new WaitForSeconds(node.time);
			}

			index = Mathf.Clamp(index + (int)direction, -1, nodes.Count);

			if (index == -1 || index == nodes.Count)
			{
				if (pingPong)
				{
					direction = -direction;
					index += (int)direction;
				}
				else if (index == nodes.Count)
				{
					index = 0;
				}
				else if (index == -1)
				{
					index = nodes.Count - 1;
				}
			}
		}
	}

	IEnumerator PatrolMove(float time, Vector2 delta)
	{
		float factor = 0;
		Vector2 from = transform.localPosition;

		while (factor < 1)
		{
			yield return new WaitForEndOfFrame();

			factor = Mathf.Clamp01(factor + Time.deltaTime / time);
			transform.localPosition = delta * factor + from;
		}
	}

	IEnumerator PatrolRotate(float time, float delta)
	{
		float factor = 0;
		float from = transform.localEulerAngles.z;

		while (factor < 1)
		{
			yield return new WaitForEndOfFrame();

			factor = Mathf.Clamp01(factor + Time.deltaTime / time);
			transform.localEulerAngles = new Vector3(0, 0, delta * factor + from);
		}
	}

	void OnPlayerDetected(PlayerController player)
	{
		StartCoroutine(LookAt(player.Position, 0.25f));
		StartCoroutine(ApproachTowards(player.Position, 0.5f, 0.25f));
		TriggerPlayerDetected();
	}
}
