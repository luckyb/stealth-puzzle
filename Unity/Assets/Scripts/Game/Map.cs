using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Map : MonoBehaviour
{
	[SerializeField] PlayerController playerController;
	[SerializeField] GameObject obstaclesContainer;

	[Space(8)]

	[SerializeField] Sprite tileSprite;
	[SerializeField] GameObject securityCameraPrefab;
	[SerializeField] GameObject patrolGuardPrefab;

	[Space(8)]

	[SerializeField] Vector2 size;
	[SerializeField][HideInInspector] List<TileColumn> grid;
	public Tile spawnTile;

	[Space(8)]

	public bool reset;
	public bool generateBox;

	void Update()
	{
		if (!Application.isPlaying)
		{
			if (reset)
			{
				reset = false;
				size = Vector2.zero;
				grid = new List<TileColumn>();
				spawnTile = null;
				this.DestroyChildren();
			}

			if (generateBox)
			{
				generateBox = false;

				for (int row = 0; row < grid.Count; row++)
				{
					for (int col = 0; col < grid[row].Count; col++)
					{
						if (row == 0 || row == grid.Count - 1 || col == 0 || col == grid[row].Count - 1)
						{
							grid[row][col].Type = Tile.TileType.Wall;
						}
						else
						{
							grid[row][col].Type = Tile.TileType.Normal;
						}
					}
				}
			}

			ResizeGrid();

			return;
		}
	}

	void ResizeGrid()
	{
		size = new Vector2(Mathf.Floor(size.x), Mathf.Floor(size.y));
		
		// Add or remove columns.
		for (int row = 0; row < grid.Count; row++)
		{
			if (grid[row].Count > size.y)
			{
				for (int col = grid[row].Count - 1; col >= size.y; col--)
				{
					grid[row][col].Destroy();
					grid[row].RemoveAt(col);
				}
			}
			else if (grid[row].Count < size.y)
			{
				for (int col = grid[row].Count; col < size.y; col++)
				{
					grid[row].Add(GenerateTile(row, col));
				}
			}
		}
		
		// Add or remove rows.
		if (grid.Count > size.x)
		{
			for (int row = grid.Count - 1; row >= size.x; row--)
			{
				for (int col = 0; col < grid[row].Count; col++)
				{
					grid[row][col].Destroy();
				}
				
				grid.RemoveAt(row);
			}
		}
		else if (grid.Count < size.x)
		{
			for (int row = grid.Count; row < size.x; row++)
			{
				TileColumn newColumn = new TileColumn();
				
				for (int col = 0; col < size.y; col++)
				{
					newColumn.Add(GenerateTile(row, col));
				}
				
				grid.Add(newColumn);
			}
		}

		for (int row = 0; row < grid.Count; row++)
		{
			for (int col = 0; col < grid[row].Count; col++)
			{
				grid[row][col].transform.SetSiblingIndex(row * (int)size.y + col);
			}
		}
	}

	Tile GenerateTile(int x, int y)
	{
		Tile tile = this.AddChild<Tile>();
		tile.map = this;
		tile.name = string.Format("Tile ({0},{1})", x, y);
		tile.Type = Tile.TileType.Wall;
		tile.Sprite = tileSprite;
		tile.Position = new Vector2(x * 100, y * 100);
		tile.Size = new Vector2(100, 100);
		return tile;
	}

	public void SetSpawnTile(Tile tile)
	{
		if (spawnTile != null && spawnTile != tile) spawnTile.Type = Tile.TileType.Normal;
		spawnTile = tile;
		playerController.transform.localPosition = spawnTile.Position;
	}

	public void GenerateSecurityCameraAtTile(Tile tile)
	{
		SecurityCamera securityCamera = obstaclesContainer.AddChild<SecurityCamera>(securityCameraPrefab);
		securityCamera.name = "Security Camera";
		securityCamera.Position = tile.Position;
		securityCamera.onPlayerDetected = TriggerObstacleDetectedPlayer;
	}

	public System.Action<Tile> onPlayerReachedGoal;
	public void TriggerPlayerReachedGoal(Tile tile)
	{
		onPlayerReachedGoal(tile);
	}

	public System.Action<Obstacle> onObstacleDetectedPlayer;
	public void TriggerObstacleDetectedPlayer(Obstacle obstacle)
	{
		onObstacleDetectedPlayer(obstacle);
	}
}

[System.Serializable]
public class TileColumn
{
	[SerializeField] List<Tile> tiles = new List<Tile>();

	public Tile this[int index] { get { return tiles[index]; } }

	public int Count { get { return tiles.Count; } }

	public void Add(Tile tile) { tiles.Add(tile); }

	public void RemoveAt(int index) { tiles.RemoveAt(index); }
}
