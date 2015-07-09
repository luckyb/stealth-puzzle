using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GameController : MonoBehaviour
{
	public static GameController main;

	[SerializeField] PlayerController playerController;
	[SerializeField] InputController inputController;
	[SerializeField] UIController uiController;

	static int level = 1;

	bool initiate = true;

	void Awake()
	{
		main = this;

		if (!Application.isPlaying)
		{
			return;
		}

		Application.targetFrameRate = 60;

		Map map = GetComponentInChildren<Map>();
		if (map == null)
		{
			LoadLevel();
		}
		else
		{
			map.onPlayerReachedGoal = OnPlayerReachedGoal;
			
			foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
			{
				obstacle.onPlayerDetected = OnPlayerDetectedByObstacle;
			}
		}

		uiController.ToggleStart(true);
	}

	void Update()
	{
		if (initiate && inputController.Input != Vector2.zero)
		{
			initiate = false;
			uiController.ToggleStart(false);
			Initiate();
		}

		if (!Application.isPlaying)
		{
			main = this;
			return;
		}
	}

	void LoadLevel()
	{
		string levelName = string.Format("Level{0}", level.ToString("000"));
		Object levelResource = Resources.Load(levelName);

		if (levelResource == null)
		{
			level = 1;
			levelName = string.Format("Level{0}", level.ToString("000"));
			levelResource = Resources.Load(levelName);
		}

		GameObject levelObject = gameObject.AddChild(Resources.Load(levelName) as GameObject);

		levelObject.GetComponentInChildren<Map>().onPlayerReachedGoal = OnPlayerReachedGoal;

		foreach (Obstacle obstacle in levelObject.GetComponentsInChildren<Obstacle>())
		{
			obstacle.onPlayerDetected = OnPlayerDetectedByObstacle;
		}
	}
	
	void Initiate()
	{
		foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
		{
			obstacle.Initiate();
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

	public PlayerController PlayerController { get { return playerController; } }

	void OnPlayerDetectedByObstacle(Obstacle obstacle)
	{
		Stop();
		uiController.Failed();
	}

	void OnPlayerReachedGoal(Tile tile)
	{
		Stop();
		level++;
		uiController.Success();
	}
}
