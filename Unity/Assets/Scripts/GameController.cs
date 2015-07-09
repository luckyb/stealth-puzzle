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
	CoroutineHandler coroutineHandler;
	float elapsedTime;
	bool playingGame;

	void Awake()
	{
		main = this;

		if (!Application.isPlaying)
		{
			return;
		}

		Application.targetFrameRate = 60;

		coroutineHandler = this.AddChild<CoroutineHandler>();

		Level level = GetComponentInChildren<Level>();
		if (level == null)
		{
			LoadLevel();
		}
		else
		{
			level.onPlayerReachedGoal = OnPlayerReachedGoal;
			
			foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
			{
				obstacle.coroutineHandler = coroutineHandler;
				obstacle.onPlayerDetected = OnPlayerDetectedByObstacle;
			}
		}

		uiController.ToggleStart(true);
	}

	void Update()
	{
		if (!Application.isPlaying)
		{
			main = this;
			return;
		}

		if (initiate && inputController.Input != Vector2.zero)
		{
			initiate = false;
			uiController.ToggleStart(false);
			Initiate();
		}

		if (playingGame)
		{
			elapsedTime += Time.deltaTime;
			uiController.Time = elapsedTime;
		}
		
		if (Input.GetKeyDown(KeyCode.LeftBracket))
		{
			level--;
			Application.LoadLevel("Game");
		}

		if (Input.GetKeyDown(KeyCode.RightBracket))
		{
			level++;
			Application.LoadLevel("Game");
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

		levelObject.GetComponentInChildren<Level>().onPlayerReachedGoal = OnPlayerReachedGoal;

		foreach (Obstacle obstacle in levelObject.GetComponentsInChildren<Obstacle>())
		{
			obstacle.coroutineHandler = coroutineHandler;
			obstacle.onPlayerDetected = OnPlayerDetectedByObstacle;
		}
	}
	
	void Initiate()
	{
		foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>())
		{
			obstacle.Initiate();
		}

		playingGame = true;
	}

	void Stop()
	{
		inputController.Stop();
		coroutineHandler.StopAllCoroutines();
		playingGame = false;
	}

	public PlayerController PlayerController { get { return playerController; } }

	void OnPlayerDetectedByObstacle(Obstacle obstacle)
	{
		Stop();
		uiController.Failed(elapsedTime);
	}

	void OnPlayerReachedGoal(Tile tile)
	{
		Stop();
		playerController.MoveToPosition(tile.Position, 0.25f);
		level++;
		uiController.Success(elapsedTime);
	}
}
