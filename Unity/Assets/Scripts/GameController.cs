using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	[SerializeField] InputController inputController;
	[SerializeField] UIController uiController;
	[SerializeField] VictoryZone victoryZone;

	void Awake()
	{
		victoryZone.onPlayerEnter = OnPlayerEnteredVictoryZone;

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

	void OnPlayerEnteredVictoryZone()
	{
		Stop();
		uiController.Success();
	}
}
