using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GameController : MonoBehaviour
{
	public static GameController main;

	[SerializeField] PlayerController playerController;
	[SerializeField] InputController inputController;
	[SerializeField] UIController uiController;

	static int currentLevel = 1;

	bool initiate = true;
	CoroutineHandler coroutineHandler;
	float elapsedTime;
	bool playingGame;

	public bool createNewLevel;

	void Awake()
	{
		main = this;

		if (!Application.isPlaying)
		{
			return;
		}

		Application.targetFrameRate = 60;

		coroutineHandler = this.AddChild<CoroutineHandler>();
		coroutineHandler.name = "CoroutineHandler";

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
			
			foreach (Key key in GetComponentsInChildren<Key>())
			{
				key.onTouchedByPlayer = level.TriggerPlayerObtainedKey;
			}
		}

		uiController.ToggleStart(true);
	}

	void Update()
	{
		if (!Application.isPlaying)
		{
			main = this;

			if (createNewLevel)
			{
				createNewLevel = false;

				Level levelResource = (Resources.Load("Level001") as GameObject).GetComponent<Level>();

				Level level = this.AddChild<Level>();
				level.name = "Level000";
				level.tilesContainer = level.gameObject.AddChild();
				level.tilesContainer.name = "Tiles";
				level.obstaclesContainer = level.gameObject.AddChild();
				level.obstaclesContainer.name = "Obstacles";
				level.itemsContainer = level.gameObject.AddChild();
				level.itemsContainer.name = "Items";
				level.tileSprite = levelResource.tileSprite;
				level.securityCameraPrefab = levelResource.securityCameraPrefab;
				level.patrolGuardPrefab = levelResource.patrolGuardPrefab;
				level.keyPrefab = levelResource.keyPrefab;
			}

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
			currentLevel--;
			Application.LoadLevel("Game");
		}

		if (Input.GetKeyDown(KeyCode.RightBracket))
		{
			currentLevel++;
			Application.LoadLevel("Game");
		}
	}

	void LoadLevel()
	{
		string levelName = string.Format("Level{0}", currentLevel.ToString("000"));
		Object levelResource = Resources.Load(levelName);

		if (levelResource == null)
		{
			currentLevel = 1;
			levelName = string.Format("Level{0}", currentLevel.ToString("000"));
			levelResource = Resources.Load(levelName);
		}

		GameObject levelObject = gameObject.AddChild(Resources.Load(levelName) as GameObject);

		Level level = levelObject.GetComponentInChildren<Level>();
		level.onPlayerReachedGoal = OnPlayerReachedGoal;

		foreach (Obstacle obstacle in levelObject.GetComponentsInChildren<Obstacle>())
		{
			obstacle.coroutineHandler = coroutineHandler;
			obstacle.onPlayerDetected = OnPlayerDetectedByObstacle;
		}
		
		foreach (Key key in GetComponentsInChildren<Key>())
		{
			key.onTouchedByPlayer = level.TriggerPlayerObtainedKey;
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
		currentLevel++;
		uiController.Success(elapsedTime);
	}
}
