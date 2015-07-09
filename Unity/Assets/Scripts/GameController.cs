using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	[SerializeField] Map map;

	[Space(8)]

	[SerializeField] InputController inputController;
	[SerializeField] UIController uiController;

	void Awake()
	{
		Application.targetFrameRate = 60;

		map.onPlayerReachedGoal = OnPlayerReachedGoal;

		foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
		{
			obstacle.onPlayerDetected = OnPlayerDetectedByObstacle;
		}
	}

	void Stop()
	{
		inputController.Stop();

		foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
		{
			obstacle.Stop();
		}
	}

	void OnPlayerDetectedByObstacle(Obstacle obstacle)
	{
		Stop();
		uiController.Failed();
	}

	void OnPlayerReachedGoal(Tile tile)
	{
		Stop();
		uiController.Success();
	}
}
